using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace AcmeApi.Models
{
    public class ReservaMenu
    {
        private int id_reservaFK;
        private int id_salaFK;
        private int id_menuFK;
        private int cantidad_personas;

        public int Id_reservaFK { get => id_reservaFK; set => id_reservaFK = value; }
        public int Id_salaFK { get => id_salaFK; set => id_salaFK = value; }
        public int Id_menuFK { get => id_menuFK; set => id_menuFK = value; }
        public int Cantidad_personas { get => cantidad_personas; set => cantidad_personas = value; }

        public string Create(string data)
        {
            Database db = new Database();
            JObject json = JObject.Parse(data);
            db.OpenConnection();
            try
            {
                
                if (!CheckIfValueExist())
                {
                    var cmd = new SQLiteCommand(db.Cadenaconexion);
                    cmd.CommandText = $"INSERT INTO reserva_menu (id_reservaFK,id_salaFK,id_menuFK,cantidad_personas) " +
                        $"VALUES({json["id_reservaFK"]}, {json["id_salaFK"]},{json["id_menuFK"]},{json["cantidad_personas"]})";
                    cmd.ExecuteNonQuery();
                    return "Se creó correctamente el registro";
                }
                else
                {
                    return "Lo sentimos,ya existe un registro en base de datos con la misma sala y el mismo menú, no se puede duplicar";
                }
                
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No fue posible crear el registro, generó el siguiente error {ex.Message}");
            }
            finally
            {
                db.CloseConnection();
            }
        }

        public string Update(string data)
        {
            Database db = new Database();
            JObject json = JObject.Parse(data);
            try
            {
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);

                //Si el valor existe
                if (!CheckIfValueExist())
                {
                    cmd.CommandText = $"UPDATE reserva_menu SET cantidad_personas = {json["cantidad_personas"]} where id_reservaFK = {json["id_reservaFK"]} and id_salaFK = {json["id_salaFK"]} and id_menuFK = {json["id_menuFK"]}";
                    cmd.ExecuteNonQuery();
                    return "Se Actualizó correctamente el registro";
                }
                else
                {
                    return "No existe un registro con ese valor";
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No fue posible actualizar el registro, la actualización generó el siguiente error {ex.Message}");
            }
            finally
            {
                db.CloseConnection();
            }
        }


        /// <summary>
        /// Verificar si el valor que se está enviando a eliminar, editar o buscar existe en bd.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfValueExist()
        {
            Database db = new Database();
            try
            {
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);

                //Consulta la sala y si no está ocupada
                cmd.CommandText = $"select count(*) from reserva_menu where id_reservaFK = {this.Id_reservaFK} and id_salaFK = {this.Id_salaFK} and id_menuFK = {this.Id_menuFK}";
                int rows = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("No fue posible si el valor existe");
            }
            finally
            {
                db.Cadenaconexion.Close();
            }
        }

        public IEnumerable<Dictionary<string, object>> SearchReservaMenu()
        {
            Database db = new Database();
            var results = new List<Dictionary<string, object>>();
            db.OpenConnection();
            try
            {
                if (CheckIfValueExist())
                {
                    string consulta = $"select * from reserva_menu where id_menuFK = {this.Id_menuFK} and id_reservaFK = {this.Id_reservaFK} and id_salaFK = {this.Id_salaFK}";
                    var cols = new List<string>();
                    SQLiteCommand comando = new SQLiteCommand(consulta, db.Cadenaconexion);
                    SQLiteDataReader datos = comando.ExecuteReader();

                    string ss = datos.GetName(1);
                    for (var i = 0; i < datos.FieldCount; i++)
                        cols.Add(datos.GetName(i));

                    while (datos.Read())
                        results.Add(db.SerializeRow(cols, datos));

                    return results;
                }
                else
                {
                    Dictionary<string,object> respuesta = new Dictionary<string,object>();
                    respuesta.Add("response", "No hay información en base de datos");
                    results.Add(respuesta);
                    return results;
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("No fue posible consultar la base de datos");
            }
            finally
            {
                db.Cadenaconexion.Close();
            }
        }

        public string Delete()
        {
            Database db = new Database();
            try
            {
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);

                //Si el valor existe
                if (CheckIfValueExist())
                {
                    cmd.CommandText = $"DELETE FROM reserva_menu where id_reservaFK = {this.Id_reservaFK} and id_salaFK = {this.Id_salaFK} and id_menuFK = {this.Id_menuFK};";
                    cmd.ExecuteNonQuery();
                    return "Se Eliminó correctamente el registro";
                }
                else
                {
                    return "No existe un registro con ese valor";
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No fue posible Eliminar el registro, la actualización generó el siguiente error {ex.Message}");
            }
            finally
            {
                db.CloseConnection();
            }
        }
    }
}