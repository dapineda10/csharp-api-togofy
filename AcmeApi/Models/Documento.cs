using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace AcmeApi.Models
{
    public class Documento
    {
        private int id_documento;
        private string tipo;

        public int Id_documento { get => id_documento; set => id_documento = value; }
        public string Tipo { get => tipo; set => tipo = value; }

        public string Create(string data)
        {
            Database db = new Database();
            JObject json = JObject.Parse(data);
            try
            {
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);
                cmd.CommandText = $"INSERT INTO documento (tipo) VALUES('{json["tipo"]}')";
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

                cmd.CommandText = $"select count(*) from documento where id_documento = {json["id_documento"]}";
                int rows = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (rows > 0)
                {
                    cmd.CommandText = $"UPDATE documento SET tipo='{json["tipo"]}' where id_documento = {json["id_documento"]}";
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