using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace AcmeApi.Models
{
    public class Menu
    {
        private int id_menu;
        private string nombre;

        public int Id_menu { get => id_menu; set => id_menu = value; }
        public string Nombre { get => nombre; set => nombre = value; }

        public string Create(string data)
        {
            Database db = new Database();
            JObject json = JObject.Parse(data);
            try
            {
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);
                cmd.CommandText = $"INSERT INTO menu (nombre) VALUES('{json["nombre"].ToString()}')";
                cmd.ExecuteNonQuery();
                return "Se creó correctamente el registro";
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

                cmd.CommandText = $"select count(*) from menu where id_menu = {json["id_menu"]}";
                int rows = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (rows > 0)
                {
                    cmd.CommandText = $"UPDATE menu SET nombre = '{json["nombre"]}' where id_menu = {json["id_menu"]}";
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
    }
}