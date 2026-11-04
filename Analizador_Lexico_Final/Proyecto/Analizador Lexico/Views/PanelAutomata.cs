using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Analizador_Lexico.Core;

namespace Analizador_Lexico.Views
{
    /// <summary>
    /// Control que dibuja un autómata (estados, transiciones, estado inicial y finales) con GDI+.
    /// No contiene lógica del autómata: recibe el modelo y solo lo pinta. También resalta un hilo
    /// de ejecución (aristas recorridas y estado actual) y un conjunto de estados activos.
    /// </summary>
    public class PanelAutomata : Panel
    {
        private Automata modelo;
        private Hilo hiloActivo;
        private int pasoActual;
        private HashSet<string> estadosActivos = new HashSet<string>(StringComparer.Ordinal);

        private static readonly Color ColorNodo = Color.FromArgb(235, 242, 255);
        private static readonly Color ColorBorde = Color.FromArgb(55, 75, 160);
        private static readonly Color ColorActivo = Color.FromArgb(170, 230, 170);
        private static readonly Color ColorRecorrido = Color.FromArgb(235, 120, 0);

        public PanelAutomata()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.White;
        }

        /// <summary>Autómata a dibujar (null = panel vacío).</summary>
        public Automata Modelo
        {
            get { return modelo; }
            set { modelo = value; Invalidate(); }
        }

        /// <summary>Hilo cuyo recorrido se resalta en naranja.</summary>
        public Hilo HiloActivo
        {
            get { return hiloActivo; }
            set { hiloActivo = value; Invalidate(); }
        }

        /// <summary>Cuántos pasos del hilo activo se muestran (0 = solo el estado inicial).</summary>
        public int PasoActual
        {
            get { return pasoActual; }
            set { pasoActual = value; Invalidate(); }
        }

        /// <summary>Estados pintados en verde (conjunto activo en un paso de la simulación).</summary>
        public HashSet<string> EstadosActivos
        {
            get { return estadosActivos; }
            set { estadosActivos = value ?? new HashSet<string>(StringComparer.Ordinal); Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Dibujar(e.Graphics, ClientRectangle);
        }

        /// <summary>Genera una imagen del dibujo actual (para exportar a PNG).</summary>
        public Bitmap Capturar(int ancho, int alto)
        {
            Bitmap bmp = new Bitmap(ancho, alto);
            using (Graphics g = Graphics.FromImage(bmp))
                Dibujar(g, new Rectangle(0, 0, ancho, alto));
            return bmp;
        }

        // ------------------------------------------------------------------ dibujo

        public void Dibujar(Graphics g, Rectangle area)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(Color.White);

            if (modelo == null || modelo.Estados.Count == 0)
            {
                using (Font f = new Font("Segoe UI", 11F))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    g.DrawString("Defina los estados y la tabla de transiciones\ny pulse «Dibujar autómata».", f, Brushes.Gray, area, sf);
                return;
            }

            // Orden de colocación: el estado inicial primero (a la izquierda), luego los demás.
            List<string> orden = new List<string>(modelo.Estados);
            if (!string.IsNullOrEmpty(modelo.Inicial) && orden.Remove(modelo.Inicial)) orden.Insert(0, modelo.Inicial);

            int n = orden.Count;
            float margen = 78;
            float r = Math.Max(15, Math.Min(27, Math.Min(area.Width, area.Height) / (2.2f + n * 0.45f)));
            PointF centro = new PointF(area.Left + area.Width / 2f, area.Top + area.Height / 2f);
            float rx = Math.Max(30, area.Width / 2f - margen - r);
            float ry = Math.Max(30, area.Height / 2f - margen - r);

            Dictionary<string, PointF> pos = new Dictionary<string, PointF>(StringComparer.Ordinal);
            for (int i = 0; i < n; i++)
            {
                if (n == 1) { pos[orden[i]] = centro; continue; }
                double ang = Math.PI + i * 2 * Math.PI / n;
                pos[orden[i]] = new PointF(centro.X + rx * (float)Math.Cos(ang), centro.Y + ry * (float)Math.Sin(ang));
            }

            using (Font fuenteEstado = new Font("Segoe UI", 10F, FontStyle.Bold))
            using (Font fuenteEtiqueta = new Font("Segoe UI", 9.5F, FontStyle.Bold))
            using (Font fuenteTitulo = new Font("Segoe UI", 9F, FontStyle.Italic))
            {
                g.DrawString(modelo.EsAFND ? "AFND" + (modelo.UsaEpsilon ? " (con transiciones ε)" : "") : "AFD",
                    fuenteTitulo, Brushes.DimGray, area.Left + 8, area.Top + 6);

                List<KeyValuePair<KeyValuePair<string, string>, List<string>>> aristas = modelo.Aristas();
                HashSet<string> existentes = new HashSet<string>(aristas.Select(a => a.Key.Key + "\u0001" + a.Key.Value));
                Dictionary<string, List<int>> usadas = AristasUsadas();

                foreach (KeyValuePair<KeyValuePair<string, string>, List<string>> a in aristas)
                {
                    string u = a.Key.Key, v = a.Key.Value;
                    if (!pos.ContainsKey(u) || !pos.ContainsKey(v)) continue;
                    List<int> pasos;
                    usadas.TryGetValue(u + "\u0001" + v, out pasos);
                    bool resaltada = pasos != null && pasos.Count > 0;
                    bool soloEpsilon = a.Value.All(s => s == Automata.Epsilon);
                    string etiqueta = string.Join(",", a.Value);
                    if (resaltada) etiqueta += "  #" + string.Join(",", pasos);

                    if (u == v)
                        DibujarBucle(g, pos[u], r, centro, etiqueta, resaltada, soloEpsilon, fuenteEtiqueta);
                    else
                    {
                        bool hayInversa = existentes.Contains(v + "\u0001" + u);
                        DibujarArista(g, pos[u], pos[v], r, hayInversa, etiqueta, resaltada, soloEpsilon, fuenteEtiqueta);
                    }
                }

                // Estado inicial: flecha entrante con la palabra "inicio".
                if (!string.IsNullOrEmpty(modelo.Inicial) && pos.ContainsKey(modelo.Inicial))
                {
                    PointF c = pos[modelo.Inicial];
                    PointF fuera = Normalizar(n == 1 ? new PointF(-1, 0) : new PointF(c.X - centro.X, c.Y - centro.Y));
                    if (n > 1 && TieneBucle(modelo.Inicial))
                        fuera = Rotar(fuera, 80);        // evita que la flecha de inicio choque con el bucle
                    PointF desde = new PointF(c.X + fuera.X * (r + 46), c.Y + fuera.Y * (r + 46));
                    PointF hasta = new PointF(c.X + fuera.X * r, c.Y + fuera.Y * r);
                    using (Pen p = new Pen(Color.FromArgb(30, 30, 30), 2f))
                        g.DrawLine(p, desde, hasta);
                    Flecha(g, desde, hasta, Color.FromArgb(30, 30, 30), 2f);
                    g.DrawString("inicio", fuenteTitulo, Brushes.DimGray, desde.X - 16, desde.Y - (fuera.Y > 0 ? -2 : 18));
                }

                // Estados
                string actual = hiloActivo != null ? hiloActivo.Estado(pasoActual) : null;
                foreach (string e in orden)
                {
                    PointF c = pos[e];
                    RectangleF rect = new RectangleF(c.X - r, c.Y - r, 2 * r, 2 * r);
                    Color relleno = estadosActivos.Contains(e) ? ColorActivo : ColorNodo;
                    using (Brush b = new SolidBrush(relleno)) g.FillEllipse(b, rect);
                    bool esActual = actual != null && actual == e;
                    using (Pen p = new Pen(esActual ? ColorRecorrido : ColorBorde, esActual ? 3.5f : 2f))
                        g.DrawEllipse(p, rect);
                    if (modelo.Finales.Contains(e))
                    {
                        RectangleF interno = RectangleF.Inflate(rect, -5, -5);
                        using (Pen p = new Pen(esActual ? ColorRecorrido : ColorBorde, 2f)) g.DrawEllipse(p, interno);
                    }
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                        g.DrawString(e, fuenteEstado, Brushes.Black, rect, sf);
                }
            }
        }

        private bool TieneBucle(string estado)
        {
            List<string> simbolos = new List<string>(modelo.Alfabeto);
            if (modelo.UsaEpsilon) simbolos.Add(Automata.Epsilon);
            return simbolos.Any(s => modelo.Destinos(estado, s).Contains(estado));
        }

        /// <summary>Aristas recorridas por el hilo activo hasta el paso actual, con el número de paso.</summary>
        private Dictionary<string, List<int>> AristasUsadas()
        {
            Dictionary<string, List<int>> d = new Dictionary<string, List<int>>();
            if (hiloActivo == null) return d;
            for (int i = 0; i < hiloActivo.Pasos.Count && i < pasoActual; i++)
            {
                string k = hiloActivo.Pasos[i].Desde + "\u0001" + hiloActivo.Pasos[i].Hasta;
                if (!d.ContainsKey(k)) d[k] = new List<int>();
                d[k].Add(i + 1);
            }
            return d;
        }

        private static void DibujarArista(Graphics g, PointF c1, PointF c2, float r, bool curva,
                                          string etiqueta, bool resaltada, bool punteada, Font fuente)
        {
            PointF dir = Normalizar(new PointF(c2.X - c1.X, c2.Y - c1.Y));
            PointF perp = new PointF(-dir.Y, dir.X);
            float dist = Distancia(c1, c2);
            float desplazamiento = curva ? Math.Max(24, dist * 0.2f) : 0;
            PointF medio = new PointF((c1.X + c2.X) / 2 + perp.X * desplazamiento, (c1.Y + c2.Y) / 2 + perp.Y * desplazamiento);

            PointF ini = Sumar(c1, Normalizar(new PointF(medio.X - c1.X, medio.Y - c1.Y)), r);
            PointF fin = Sumar(c2, Normalizar(new PointF(medio.X - c2.X, medio.Y - c2.Y)), r);
            PointF cp1 = new PointF(ini.X + 2f / 3f * (medio.X - ini.X), ini.Y + 2f / 3f * (medio.Y - ini.Y));
            PointF cp2 = new PointF(fin.X + 2f / 3f * (medio.X - fin.X), fin.Y + 2f / 3f * (medio.Y - fin.Y));

            Color color = resaltada ? ColorRecorrido : Color.FromArgb(70, 70, 70);
            float ancho = resaltada ? 3.2f : 1.8f;
            using (Pen p = new Pen(color, ancho))
            {
                if (punteada) p.DashStyle = DashStyle.Dash;
                g.DrawBezier(p, ini, cp1, cp2, fin);
            }
            Flecha(g, medio, fin, color, ancho);

            PointF mitad = new PointF(0.25f * ini.X + 0.5f * medio.X + 0.25f * fin.X, 0.25f * ini.Y + 0.5f * medio.Y + 0.25f * fin.Y);
            if (!curva) mitad = new PointF(mitad.X + perp.X * 11, mitad.Y + perp.Y * 11);
            else mitad = new PointF(mitad.X + (desplazamiento >= 0 ? perp.X : -perp.X) * 10, mitad.Y + (desplazamiento >= 0 ? perp.Y : -perp.Y) * 10);
            Etiqueta(g, mitad, etiqueta, resaltada ? ColorRecorrido : Color.FromArgb(20, 20, 120), fuente);
        }

        private static void DibujarBucle(Graphics g, PointF c, float r, PointF centroLayout, string etiqueta,
                                         bool resaltada, bool punteada, Font fuente)
        {
            PointF fuera = Normalizar(new PointF(c.X - centroLayout.X, c.Y - centroLayout.Y));
            if (Distancia(c, centroLayout) < 1) fuera = new PointF(0, -1);
            PointF d1 = Rotar(fuera, -32), d2 = Rotar(fuera, 32);
            PointF ini = Sumar(c, d1, r), fin = Sumar(c, d2, r);
            PointF cp1 = Sumar(c, Rotar(fuera, -22), r * 3.1f), cp2 = Sumar(c, Rotar(fuera, 22), r * 3.1f);

            Color color = resaltada ? ColorRecorrido : Color.FromArgb(70, 70, 70);
            float ancho = resaltada ? 3.2f : 1.8f;
            using (Pen p = new Pen(color, ancho))
            {
                if (punteada) p.DashStyle = DashStyle.Dash;
                g.DrawBezier(p, ini, cp1, cp2, fin);
            }
            Flecha(g, cp2, fin, color, ancho);

            PointF mitad = new PointF((ini.X + 3 * cp1.X + 3 * cp2.X + fin.X) / 8, (ini.Y + 3 * cp1.Y + 3 * cp2.Y + fin.Y) / 8);
            mitad = new PointF(mitad.X + fuera.X * 12, mitad.Y + fuera.Y * 12);
            Etiqueta(g, mitad, etiqueta, resaltada ? ColorRecorrido : Color.FromArgb(20, 20, 120), fuente);
        }

        private static void Etiqueta(Graphics g, PointF centro, string texto, Color color, Font fuente)
        {
            SizeF tam = g.MeasureString(texto, fuente);
            RectangleF caja = new RectangleF(centro.X - tam.Width / 2, centro.Y - tam.Height / 2, tam.Width, tam.Height);
            using (Brush fondo = new SolidBrush(Color.FromArgb(215, 255, 255, 255))) g.FillRectangle(fondo, caja);
            using (Brush b = new SolidBrush(color)) g.DrawString(texto, fuente, b, caja.Location);
        }

        private static void Flecha(Graphics g, PointF desde, PointF punta, Color color, float grosor)
        {
            PointF dir = Normalizar(new PointF(punta.X - desde.X, punta.Y - desde.Y));
            float largo = 9 + grosor * 1.2f;
            PointF baseP = new PointF(punta.X - dir.X * largo, punta.Y - dir.Y * largo);
            PointF perp = new PointF(-dir.Y, dir.X);
            PointF[] tri =
            {
                punta,
                new PointF(baseP.X + perp.X * largo * 0.42f, baseP.Y + perp.Y * largo * 0.42f),
                new PointF(baseP.X - perp.X * largo * 0.42f, baseP.Y - perp.Y * largo * 0.42f)
            };
            using (Brush b = new SolidBrush(color)) g.FillPolygon(b, tri);
        }

        // ------------------------------------------------------------------ geometría

        private static float Distancia(PointF a, PointF b)
        {
            return (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        private static PointF Normalizar(PointF v)
        {
            float l = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
            return l < 1e-6f ? new PointF(1, 0) : new PointF(v.X / l, v.Y / l);
        }

        private static PointF Sumar(PointF p, PointF dir, float k)
        {
            return new PointF(p.X + dir.X * k, p.Y + dir.Y * k);
        }

        private static PointF Rotar(PointF v, double grados)
        {
            double a = grados * Math.PI / 180.0;
            double c = Math.Cos(a), s = Math.Sin(a);
            return new PointF((float)(v.X * c - v.Y * s), (float)(v.X * s + v.Y * c));
        }
    }
}
