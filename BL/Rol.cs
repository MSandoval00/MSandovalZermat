using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Rol
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = context.RolGetAll().ToList();
                    result.Objects = new List<object>();
                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            ML.Rol rol = new ML.Rol();
                            rol.IdRol = int.Parse(row.IdRol.ToString());
                            rol.Nombre = row.Nombre;

                            result.Objects.Add(rol);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pueden mostrar los registros de los roles";
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
