using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = context.UsuarioGetAll().ToList();
                    result.Objects = new List<object>();
                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            ML.Usuario usuario = new ML.Usuario();
                            usuario.IdUsuario = int.Parse(row.IdUsuario.ToString());
                            usuario.Nombre = row.NombreUsuario;
                            usuario.ApellidoPaterno = row.ApellidoPaterno;
                            usuario.ApellidoMaterno = row.ApellidoMaterno;
                            usuario.Email = row.Email;
                            usuario.Rol = new ML.Rol();
                            usuario.Rol.IdRol = row.IdRol;
                            usuario.Rol.Nombre = row.NombreRol;


                            result.Objects.Add(usuario);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pueden mostrar los registros del usuario";
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
        public static ML.Result GetById(int IdUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = context.UsuarioGetById(IdUsuario).First();
                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Nombre = query.NombreUsuario;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.Email = query.Email;

                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = query.IdRol;
                        usuario.Rol.Nombre = query.NombreRol;

                        result.Object = usuario;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error no se puede mostrar el usuario";
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
        public static ML.Result Add(ML.Usuario usuario)
        {
            byte[] password = Encoding.UTF8.GetBytes(usuario.Password);
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = context.UsuarioAdd(usuario.Nombre,
                               usuario.ApellidoPaterno,
                               usuario.ApellidoMaterno,
                               usuario.Email,
                               password,
                               usuario.Rol.IdRol);
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se inserto el usuario";
                    }
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
        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = context.UsuarioUpdate(usuario.IdUsuario,
                                usuario.Nombre,
                                usuario.ApellidoPaterno,
                                usuario.ApellidoMaterno,
                                usuario.Email,
                                usuario.Rol.IdRol
                                );
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se modificaron los datos del usuario";
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
        public static ML.Result Delete(int IdUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = context.UsuarioDelete(IdUsuario);
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Usuario no eliminado";
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
        public static ML.Result UsuarioGetByEmail(string Email)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    var query = context.UsuarioGetByEmail(Email).First();
                    result.Objects = new List<object>();
                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.Rol = new ML.Rol();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Nombre = query.NombreUsuario;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.Email = query.Email;
                        usuario.Rol.IdRol = query.IdRol;
                        usuario.Rol.Nombre = query.NombreRol;
                        usuario.Password = Encoding.UTF8.GetString(query.Password);
                        result.Object = usuario;
                        result.Correct = true;
                        //usuario.Password = query.Password;
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
        public static ML.Result Registro(ML.Usuario usuario)
        {
            byte[] password = Encoding.UTF8.GetBytes(usuario.Password);
            ML.Result result = new ML.Result();
            try
            {
                using(DL.MSandovalZermatEntities1 context = new DL.MSandovalZermatEntities1())
                {
                    DL.Usuario usuarioDL = new DL.Usuario();
                    usuarioDL.Nombre = usuario.Nombre;
                    usuarioDL.ApellidoPaterno = usuario.ApellidoPaterno;
                    usuarioDL.ApellidoMaterno = usuario.ApellidoMaterno;
                    usuarioDL.Email = usuario.Email;
                    usuarioDL.Password = password;
                    usuario.Rol = new ML.Rol();
                    usuario.Rol.IdRol = 2;
                    usuarioDL.IdRol = usuario.Rol.IdRol = 2;

                    context.Usuarios.Add(usuarioDL);
                    int query = context.SaveChanges();
                    if(query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error al registrar el usuario";
                    }
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}