using System;
using System.Collections.Generic;
using System.Linq;

namespace Analizador_Lexico.Core
{
    public sealed class PasoHilo
    {
        public string Desde;
        public string Simbolo;
        public string Hasta;
    }
    public sealed class Hilo
    {
        public string Inicio;
        public List<PasoHilo> Pasos = new List<PasoHilo>();
        public bool Acepta;
        public string Resultado;
        public int Consumidos;

        public string Estado(int paso)
        {
            return paso <= 0 ? Inicio : Pasos[Math.Min(paso, Pasos.Count) - 1].Hasta;
        }

        public override string ToString()
        {
            string s = Inicio;
            foreach (PasoHilo p in Pasos) s += " -" + p.Simbolo + "-> " + p.Hasta;
            return s;
        }
    }
    public sealed class ResultadoCadena
    {
        public string Cadena;
        public bool Acepta;
        public string Error;
        public bool Truncado;
        public List<Hilo> Hilos = new List<Hilo>();
        public List<List<string>> ConjuntosPorPaso = new List<List<string>>();
        public List<string> SimbolosPorPaso = new List<string>();
    }
    public sealed class Automata
    {
        public const string Epsilon = "ε";
        public const int MaximoHilos = 300;

        public bool EsAFND;
        public bool UsaEpsilon;
        public List<string> Estados = new List<string>();
        public List<string> Alfabeto = new List<string>();
        public string Inicial;
        public HashSet<string> Finales = new HashSet<string>(StringComparer.Ordinal);
        private readonly Dictionary<string, Dictionary<string, List<string>>> tabla =
            new Dictionary<string, Dictionary<string, List<string>>>(StringComparer.Ordinal);
        public void FijarTransicion(string estado, string simbolo, IEnumerable<string> destinos)
        {
            Dictionary<string, List<string>> fila;
            if (!tabla.TryGetValue(estado, out fila))
            {
                fila = new Dictionary<string, List<string>>(StringComparer.Ordinal);
                tabla[estado] = fila;
            }
            List<string> lista = destinos.Distinct(StringComparer.Ordinal).ToList();
            if (lista.Count == 0) fila.Remove(simbolo); else fila[simbolo] = lista;
        }

        public List<string> Destinos(string estado, string simbolo)
        {
            Dictionary<string, List<string>> fila;
            List<string> lista;
            if (tabla.TryGetValue(estado, out fila) && fila.TryGetValue(simbolo, out lista)) return lista;
            return new List<string>();
        }
        public List<KeyValuePair<KeyValuePair<string, string>, List<string>>> Aristas()
        {
            Dictionary<string, KeyValuePair<KeyValuePair<string, string>, List<string>>> mapa =
                new Dictionary<string, KeyValuePair<KeyValuePair<string, string>, List<string>>>();
            List<string> orden = new List<string>();
            List<string> simbolos = new List<string>(Alfabeto);
            if (UsaEpsilon) simbolos.Add(Epsilon);
            foreach (string e in Estados)
                foreach (string s in simbolos)
                    foreach (string d in Destinos(e, s))
                    {
                        string clave = e + "\u0001" + d;
                        if (!mapa.ContainsKey(clave))
                        {
                            mapa[clave] = new KeyValuePair<KeyValuePair<string, string>, List<string>>(
                                new KeyValuePair<string, string>(e, d), new List<string>());
                            orden.Add(clave);
                        }
                        mapa[clave].Value.Add(s);
                    }
            return orden.Select(c => mapa[c]).ToList();
        }
        public List<string> Validar()
        {
            List<string> problemas = new List<string>();
            if (Estados.Count == 0) problemas.Add("Debe definir al menos un estado.");
            if (Alfabeto.Count == 0) problemas.Add("Debe definir al menos un símbolo de entrada.");
            if (string.IsNullOrEmpty(Inicial) || !Estados.Contains(Inicial))
                problemas.Add("Debe elegir el estado inicial.");
            foreach (string f in Finales)
                if (!Estados.Contains(f)) problemas.Add("El estado final '" + f + "' no existe.");
            foreach (string e in Estados)
            {
                List<string> simbolos = new List<string>(Alfabeto);
                if (UsaEpsilon) simbolos.Add(Epsilon);
                foreach (string s in simbolos)
                {
                    List<string> d = Destinos(e, s);
                    if (!EsAFND && d.Count > 1)
                        problemas.Add("AFD: la celda (" + e + ", " + s + ") tiene " + d.Count + " destinos; un AFD solo permite uno.");
                    foreach (string x in d)
                        if (!Estados.Contains(x))
                            problemas.Add("La celda (" + e + ", " + s + ") menciona el estado inexistente '" + x + "'.");
                }
            }
            return problemas;
        }
        public List<string> CierreEpsilon(IEnumerable<string> conjunto)
        {
            List<string> resultado = new List<string>();
            Stack<string> pendientes = new Stack<string>();
            foreach (string e in conjunto)
                if (!resultado.Contains(e)) { resultado.Add(e); pendientes.Push(e); }
            if (!UsaEpsilon) return resultado;
            while (pendientes.Count > 0)
            {
                string actual = pendientes.Pop();
                foreach (string d in Destinos(actual, Epsilon))
                    if (!resultado.Contains(d)) { resultado.Add(d); pendientes.Push(d); }
            }
            return resultado;
        }
        public ResultadoCadena Analizar(string cadena)
        {
            ResultadoCadena r = new ResultadoCadena { Cadena = cadena ?? "" };
            List<string> problemas = Validar();
            if (problemas.Count > 0) { r.Error = problemas[0]; return r; }

            for (int i = 0; i < r.Cadena.Length; i++)
            {
                string s = r.Cadena[i].ToString();
                if (!Alfabeto.Contains(s))
                {
                    r.Error = "El símbolo '" + s + "' (posición " + (i + 1) + ") no pertenece al alfabeto {" + string.Join(", ", Alfabeto) + "}.";
                    return r;
                }
            }
            List<string> actuales = CierreEpsilon(new[] { Inicial });
            r.ConjuntosPorPaso.Add(new List<string>(actuales));
            foreach (char c in r.Cadena)
            {
                string s = c.ToString();
                List<string> mover = new List<string>();
                foreach (string e in actuales)
                    foreach (string d in Destinos(e, s))
                        if (!mover.Contains(d)) mover.Add(d);
                actuales = CierreEpsilon(mover);
                r.SimbolosPorPaso.Add(s);
                r.ConjuntosPorPaso.Add(new List<string>(actuales));
            }
            r.Acepta = actuales.Any(e => Finales.Contains(e));

            Hilo actual = new Hilo { Inicio = Inicial };
            Explorar(Inicial, 0, r.Cadena, actual, new HashSet<string>(StringComparer.Ordinal), r);
            return r;
        }

        private void Explorar(string estado, int pos, string cadena, Hilo camino, HashSet<string> epsVisitados, ResultadoCadena r)
        {
            if (r.Hilos.Count >= MaximoHilos) { r.Truncado = true; return; }

            bool alFinal = pos == cadena.Length;
            if (alFinal && Finales.Contains(estado))
            {
                Cerrar(camino, r, true, "Aceptado", pos);
                return;
            }

            bool hayMovimiento = false;

            if (!alFinal)
            {
                string s = cadena[pos].ToString();
                foreach (string d in Destinos(estado, s))
                {
                    hayMovimiento = true;
                    camino.Pasos.Add(new PasoHilo { Desde = estado, Simbolo = s, Hasta = d });
                    Explorar(d, pos + 1, cadena, camino, new HashSet<string>(StringComparer.Ordinal), r);
                    camino.Pasos.RemoveAt(camino.Pasos.Count - 1);
                    if (r.Truncado) return;
                }
            }

            if (UsaEpsilon)
            {
                string clave = estado;
                if (!epsVisitados.Contains(clave))
                {
                    HashSet<string> nuevos = new HashSet<string>(epsVisitados, StringComparer.Ordinal) { clave };
                    foreach (string d in Destinos(estado, Epsilon))
                    {
                        hayMovimiento = true;
                        camino.Pasos.Add(new PasoHilo { Desde = estado, Simbolo = Epsilon, Hasta = d });
                        Explorar(d, pos, cadena, camino, nuevos, r);
                        camino.Pasos.RemoveAt(camino.Pasos.Count - 1);
                        if (r.Truncado) return;
                    }
                }
            }

            if (!hayMovimiento)
                Cerrar(camino, r, false, alFinal ? "Rechazado" : "Bloqueado", pos);
        }

        private static void Cerrar(Hilo camino, ResultadoCadena r, bool acepta, string resultado, int consumidos)
        {
            Hilo copia = new Hilo { Inicio = camino.Inicio, Acepta = acepta, Resultado = resultado, Consumidos = consumidos };
            foreach (PasoHilo p in camino.Pasos)
                copia.Pasos.Add(new PasoHilo { Desde = p.Desde, Simbolo = p.Simbolo, Hasta = p.Hasta });
            r.Hilos.Add(copia);
        }
    }
}
