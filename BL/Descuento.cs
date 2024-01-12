using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Descuento
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = (from tablaDescuento in context.Descuentoes
                                 select new
                                 {
                                     IdDescuento=tablaDescuento.IdDescuento,
                                     Valor=tablaDescuento.Valor,
                                 }).ToList();
                    result.Objects = new List<object>();
                    if (query != null && query.Count()>0)
                    {
                        foreach (var row in query)
                        {
                            ML.Descuento descuento = new ML.Descuento();
                            descuento.IdDescuento = int.Parse(row.IdDescuento.ToString());
                            descuento.Valor = int.Parse(row.Valor.ToString());

                            result.Objects.Add(descuento);
                            result.Correct = true;
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pueden mostrar los registros de los descuentos";
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
