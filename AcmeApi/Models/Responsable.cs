using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace AcmeApi.Models
{
    public class Responsable
    {
        private double id_responsable;
        private int id_documentoFK;
        private string nombre;
        private string apellido;
        private string cargo;
        private DateTime fecha_nacimiento;

        public double Id_responsable { get => id_responsable; set => id_responsable = value; }
        public int Id_documentoFK { get => id_documentoFK; set => id_documentoFK = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string Cargo { get => cargo; set => cargo = value; }
        public DateTime Fecha_nacimiento { get => fecha_nacimiento; set => fecha_nacimiento = value; }

        public string Create(string data)
        {
            Database db = new Database();
            JObject json = JObject.Parse(data);
            try
            {
                DateTime fechNacimiento = DateTime.ParseExact(json["fecha_nacimiento"].ToString(), "yyyy-MM-dd", null);
                string date = fechNacimiento.ToString("yyyy-MM-dd");
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);
                cmd.CommandText = $"INSERT INTO responsable (id_responsable,id_documentoFK,nombre,apellido,cargo,fecha_nacimiento) " +
                    $"VALUES({json["id_responsable"]},'{json["id_documentoFK"]}', '{json["nombre"]}','{json["apellido"]}','{json["cargo"]}','{date}')";
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
                DateTime fechNacimiento = DateTime.ParseExact(json["fecha_nacimiento"].ToString(), "yyyy-MM-dd", null);
                string date = fechNacimiento.ToString("yyyy-MM-dd");
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);

                cmd.CommandText = $"select count(*) from responsable where id_responsable = {json["id_responsable"]}";
                int rows = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (rows > 0)
                {
                    cmd.CommandText = $"UPDATE responsable SET id_responsable= {json["id_responsable"]},id_documentoFK = '{json["id_documentoFK"]}',nombre ='{json["nombre"]}',apellido='{json["apellido"]}',cargo = '{json["cargo"]}',fecha_nacimiento = '{date}' where id_responsable = {json["id_responsable"]}";
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