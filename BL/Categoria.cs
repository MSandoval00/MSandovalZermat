using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Categoria
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = (from tablaCategoria in context.Categorias
                                 select new
                                 {
                                     IdCategoria=tablaCategoria.IdCategoria,
                                     Nombre=tablaCategoria.Nombre,
                                 }).ToList(); 
                    result.Objects = new List<object>();
                    if (query != null && query.Count()>0)
                    {
                        foreach (var row in query)
                        {
                            ML.Categoria categoria = new ML.Categoria();
                            categoria.IdCategoria = int.Parse(row.IdCategoria.ToString());
                            categoria.Nombre = row.Nombre;

                            result.Objects.Add(categoria);
                            result.Correct=true;
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pueden mostrar los registros de las categorias";
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
    }
}
