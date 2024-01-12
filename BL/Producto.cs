using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Producto
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = (from tablaProducto in context.Productoes
                                 join tablaCategoria  in context.Categorias on tablaProducto.Categoria.IdCategoria equals tablaCategoria.IdCategoria
                                 join tablaDescuento in context.Descuentoes on tablaProducto.Descuento.IdDescuento equals tablaDescuento.IdDescuento
                                 select new
                                 {
                                     Sku=tablaProducto.Sku,
                                     NombreProducto=tablaProducto.Nombre,
                                     Descripcion=tablaProducto.Descripcion,
                                     Precio=tablaProducto.Precio,
                                     IdCategoria=tablaCategoria.IdCategoria,
                                     NombreCategoria=tablaCategoria.Nombre,
                                     PrecioVenta=tablaProducto.PrecioVenta,
                                     IdDescuento=tablaDescuento.IdDescuento,
                                     Valor=tablaDescuento.Valor,
                                     Imagen=tablaProducto.Imagen

                                 }).ToList(); 
                    
                    result.Objects = new List<object>();
                    if (query != null && query.Count()>0)
                    {
                        foreach (var row in query)
                        {
                            ML.Producto producto = new ML.Producto();
                            producto.Sku = int.Parse(row.Sku.ToString());
                            producto.Nombre = row.NombreProducto;
                            producto.Descripcion = row.Descripcion;
                            producto.Precio = float.Parse(row.Precio.ToString());
                            producto.Categoria = new ML.Categoria();
                            producto.Categoria.IdCategoria = row.IdCategoria;
                            producto.Categoria.Nombre = row.NombreCategoria;
                            producto.Precioventa=float.Parse(row.PrecioVenta.ToString());
                            producto.Descuento = new ML.Descuento();
                            producto.Descuento.IdDescuento=row.IdDescuento;
                            producto.Descuento.Valor = int.Parse(row.Valor.ToString());
                            producto.Imagen = row.Imagen;

                            result.Objects.Add(producto);
                            result.Correct = true;
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pueden mostrar los registros de los productos";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result GetById(int Sku)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = (from tablaProducto in context.Productoes
                                 join tablaCategoria in context.Categorias on tablaProducto.Categoria.IdCategoria equals tablaCategoria.IdCategoria
                                 join tablaDescuento in context.Descuentoes on tablaProducto.Descuento.IdDescuento equals tablaDescuento.IdDescuento
                                 where tablaProducto.Sku == Sku
                                 select new
                                 {
                                     Sku = tablaProducto.Sku,
                                     NombreProducto = tablaProducto.Nombre,
                                     Descripcion = tablaProducto.Descripcion,
                                     Precio = tablaProducto.Precio,
                                     IdCategoria = tablaCategoria.IdCategoria,
                                     NombreCategoria = tablaCategoria.Nombre,
                                     PrecioVenta = tablaProducto.PrecioVenta,
                                     IdDescuento = tablaDescuento.IdDescuento,
                                     Valor = tablaDescuento.Valor,
                                     Imagen = tablaProducto.Imagen

                                 });
                    if (query != null && query.Count()>0)
                    {
                        ML.Producto producto = new ML.Producto();
                        var queryDatos =query.ToList().Single();
                        producto.Sku = queryDatos.Sku;
                        producto.Nombre = queryDatos.NombreProducto;
                        producto.Descripcion = queryDatos.Descripcion;
                        producto.Precio = float.Parse(queryDatos.Precio.ToString());
                        producto.Categoria = new ML.Categoria();
                        producto.Categoria.IdCategoria = queryDatos.IdCategoria;
                        producto.Categoria.Nombre = queryDatos.NombreCategoria;
                        producto.Precioventa = float.Parse(queryDatos.PrecioVenta.ToString());
                        producto.Descuento = new ML.Descuento();
                        producto.Descuento.IdDescuento = queryDatos.IdDescuento;
                        producto.Descuento.Valor = int.Parse(queryDatos.Valor.ToString());
                        producto.Imagen= queryDatos.Imagen;


                        result.Object = producto;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error no se puede mostrar el producto";
                    }
                }
            }
            catch (Exception ex)
            {

                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result Add(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    DL.Producto nuevoProducto = new DL.Producto
                    {
                        Categoria = context.Categorias.Find(producto.Categoria.IdCategoria),
                        Descuento = context.Descuentoes.Find(producto.Descuento.IdDescuento),
                        Nombre = producto.Nombre,
                        Descripcion = producto.Descripcion,
                        Precio = (decimal?)producto.Precio,
                        PrecioVenta = (decimal?)producto.Precioventa,
                        Imagen = producto.Imagen
                    };
                                        
                        context.Productoes.Add(nuevoProducto);
                        context.SaveChanges();
                        result.Correct = true; 
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result Update(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = (from tablaProducto in context.Productoes
                                 where tablaProducto.Sku == producto.Sku
                                 select tablaProducto).SingleOrDefault(); 
                                
                    if (query !=null)
                    {
                        query.Nombre = producto.Nombre;
                        query.Descripcion = producto.Descripcion;
                        query.Precio = (decimal)producto.Precio;
                        query.Categoria = context.Categorias.Find(producto.Categoria.IdCategoria);
                        query.PrecioVenta = (decimal?)producto.Precioventa;
                        query.Descuento=context.Descuentoes.Find(producto.Descuento.IdDescuento);
                        query.Imagen=producto.Imagen;

                        context.SaveChanges();
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se modificaron los datos del producto";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result Delete(int Sku)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = (from tablaProducto in context.Productoes
                                 where tablaProducto.Sku == Sku
                                 select tablaProducto).First();
                    
                        context.Productoes.Remove(query);
                        context.SaveChanges();
                        result.Correct = true; 
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}
