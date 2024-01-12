using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public float Total { get; set; }
        public DateTime Fecha { get; set; }
        public ML.Usuario Usuario { get; set; }
        public List<object> ListaProductos { get; set; }
    }
}
