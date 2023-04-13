using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipoQ22.Domino
{
    public class Equipo
    {
        public int idEquipo { get; set; }
        public string pais { get; set; }
        public string director { get; set; }

        public List<Jugador> Detalles { get; set; }

        public Equipo()
        {
            Detalles = new List<Jugador>();
        }

        public void AgregarDetalle(Jugador detalle)
        {
            Detalles.Add(detalle);
        }

        public void QuitarDetalle(int indice)
        {
            Detalles.RemoveAt(indice);
        }

        public double CalcularTotal()
        {
            double total = 0;
            foreach (Jugador item in Detalles)
            {
                total = total + 1;
            }

            return total;
        }

    }
}
