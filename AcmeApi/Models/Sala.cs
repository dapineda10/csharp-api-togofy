using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace AcmeApi.Models
{
    public class Sala
    {
        private int id_sala;
        private string nombre;
        private string ciudad;
        private string pais;
        private string tipo;
        private string capacidad;
        private bool disponible;

        public int Id_sala { get => id_sala; set => id_sala = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Ciudad { get => ciudad; set => ciudad = value; }
        public string Pais { get => pais; set => pais = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public string Capacidad { get => capacidad; set => capacidad = value; }
        public bool Disponible { get => disponible; set => disponible = value; }


        public string Create(string data)
        {
            Database db = new Database();
            JObject json = JObject.Parse(data);
            try
            {
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);
                cmd.CommandText = $"INSERT INTO sala (nombre, ciudad, pais, tipo, capacidad, disponible) " +
                    $"VALUES('{json["nombre"]}', '{json["ciudad"]}','{json["pais"]}','{json["tipo"]}',{json["capacidad"]},{json["disponible"]})";
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

                cmd.CommandText = $"select count(*) from sala where id_sala = {json["id_sala"]}";
                int rows = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (rows > 0)
                {
                    cmd.CommandText = $"UPDATE sala SET nombre='{json["nombre"]}', ciudad = '{json["ciudad"]}', pais = '{json["pais"]}', tipo = '{json["tipo"]}', capacidad = {json["capacidad"]}, disponible = {json["disponible"]} where id_sala = {json["id_sala"]}";
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

        public bool CheckAvailability()
        {
            try
            {
                Database db = new Database();
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);

                //Consulta la sala y si no está ocupada
                cmd.CommandText = $"select count(*) from sala where id_sala = {this.id_sala} and disponible = 1";
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
            catch (Exception ex)
            {

                throw new ArgumentException("No fue posible consultar disponibilidad " + ex.Message);
            }
        }

        public bool CheckPeopleAmount(int cantidad_personas)
        {
            try
            {
                Database db = new Database();
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);

                //Consulta la sala y si no está ocupada
                cmd.CommandText = $"select count(*) from sala where id_sala = {this.id_sala} and capacidad <= {cantidad_personas}";
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
            catch (Exception ex)
            {

                throw new ArgumentException("No fue posible consultar disponibilidad " + ex.Message);
            }
        }

        public void UpdateAvailability()
        {
            try
            {
                Database db = new Database();
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);

                //Consulta la sala y si no está ocupada
                cmd.CommandText = $"update sala set disponible = 0 where id_sala = {this.id_sala}";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new ArgumentException("No fue posible actualizar disponibilidad " + ex.Message);
            }
        }
    }
}