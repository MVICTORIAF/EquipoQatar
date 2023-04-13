using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipoQ22.Domino
{
    public class Jugador
    {
        private Persona p;
        private int cant;

        public Persona Persona { get; set; }
        public int Camiseta { get; set; }
        public string Posicion { get; set; }

        public Jugador(Persona persona, int camiseta, string posicion)
        {
            Persona = persona;
            Camiseta = camiseta;
            Posicion = posicion;
        }

        public Jugador(Persona p, int cant)
        {
            this.p = p;
            this.cant = cant;
        }

      
    }
}
