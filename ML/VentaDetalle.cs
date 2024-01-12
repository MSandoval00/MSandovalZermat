using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class VentaDetalle
    {
        public int IdVentaDetalle { get; set; }
        public ML.Venta Venta  { get; set; }
        public int Cantidad  { get; set; }
        public ML.Producto Producto  { get; set; }
    }
}
