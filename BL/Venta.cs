using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Venta
    {
        public static ML.Result Add(ML.Venta venta)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context=new DL.MSandovalZermatEntities1())
                {
                    DateTime servidorFecha = DateTime.Now;

                    DL.Venta nuevaVenta = new DL.Venta
                    {
                        Total =(decimal?)venta.Total,
                        Fecha=servidorFecha,
                        Usuario=context.Usuarios.Find(venta.Usuario.IdUsuario)
                    };
                    context.Ventas.Add(nuevaVenta);
                    int query=context.SaveChanges();
                    
                    venta.IdVenta = nuevaVenta.IdVenta;

                    if (query>0)
                    {
                        foreach (ML.Producto producto in venta.ListaProductos)
                        {
                            DL.VentaDetalle nuevaDetalleVenta = new DL.VentaDetalle
                            {
                                Venta = context.Ventas.Find(venta.IdVenta),
                                Cantidad = producto.Piezas,
                                Producto = context.Productoes.Find(producto.Sku)
                            };
                            context.VentaDetalles.Add(nuevaDetalleVenta);
                            context.SaveChanges();
                        }   
                    }
                                       
                    result.Correct=true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex=ex;
            }
            return result;
        }
    }
}
