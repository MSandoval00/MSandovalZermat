using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Producto
    {
        public int Sku { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public float Precio { get; set; }
        public ML.Categoria Categoria { get; set; }
        public float Precioventa { get; set; }
        public int Piezas { get; set; }
        public ML.Descuento Descuento { get; set; }
        public string Imagen { get; set; }
        public List<object> Productos { get; set; }
    }
}
