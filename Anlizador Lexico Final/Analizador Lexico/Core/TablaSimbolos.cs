using System;
using System.Collections.Generic;

namespace Analizador_Lexico.Core
{
    public class TablaSimbolos
    {
        private readonly Dictionary<string, ElementoSimbolo> tabla =
            new Dictionary<string, ElementoSimbolo>(StringComparer.Ordinal);
        private readonly List<ElementoSimbolo> orden = new List<ElementoSimbolo>();

        public int Cantidad => orden.Count;
        public IReadOnlyList<ElementoSimbolo> Elementos => orden;

        public ElementoSimbolo RegistrarIdentificador(string lexema, int linea, int columna, string tipoDatoPrevio)
        {
            ElementoSimbolo existente;
            if (tabla.TryGetValue(lexema, out existente))
            {
                existente.Frecuencia++;
                existente.Referencias.Add(linea);
                if (existente.TipoDato == ElementoSimbolo.SinEspecificar && tipoDatoPrevio != null)
                    existente.TipoDato = tipoDatoPrevio;
                return existente;
            }

            ElementoSimbolo nuevo = new ElementoSimbolo(orden.Count + 1, lexema, tipoDatoPrevio, linea, columna);
            tabla.Add(lexema, nuevo);
            orden.Add(nuevo);
            return nuevo;
        }

        public bool Contiene(string lexema)
        {
            return tabla.ContainsKey(lexema);
        }

        public ElementoSimbolo Buscar(string lexema)
        {
            ElementoSimbolo e;
            return tabla.TryGetValue(lexema, out e) ? e : null;
        }

        public void Limpiar()
        {
            tabla.Clear();
            orden.Clear();
        }
    }
}
