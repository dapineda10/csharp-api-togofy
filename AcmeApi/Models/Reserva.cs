using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace AcmeApi.Models
{
    public class Reserva
    {
        private int id_reserva;
        private int id_salaFK;
        private double id_reservaFK;
        private int cantidad_personas;
        private DateTime fecha_inicio;
        private DateTime fecha_fin;
        private bool catering;

        public int Id_reserva { get => id_reserva; set => id_reserva = value; }
        public int Id_salaFK { get => id_salaFK; set => id_salaFK = value; }
        public double Id_reservaFK { get => id_reservaFK; set => id_reservaFK = value; }
        public int Cantidad_personas { get => cantidad_personas; set => cantidad_personas = value; }
        public DateTime Fecha_inicio { get => fecha_inicio; set => fecha_inicio = value; }
        public DateTime Fecha_fin { get => fecha_fin; set => fecha_fin = value; }
        public bool Catering { get => catering; set => catering = value; }

        public string Create(string data)
        {
            Database db = new Database();
            JObject json = JObject.Parse(data);
            Sala sala = new Sala();
            //Crear el registro en bd
            db.OpenConnection();
            try
            {

                if (Convert.ToString(json["cantidad_personas"]) == "0" || json["cantidad_personas"].ToString() == "")
                {
                    return "No puede dejar una sala con cero personas";
                }

                sala.Id_sala = Convert.ToInt32(json["id_salaFK"]);

                if (db.CheckIfValueExist("sala", "id_sala", sala.Id_sala))
                {
                    if (sala.CheckPeopleAmount(Convert.ToInt32(json["cantidad_personas"])))
                    {
                        //Verificar disponibilidad de la sala
                        if (sala.CheckAvailability())
                        {
                            //Dar formato a la fecha
                            DateTime fechaInicio = DateTime.ParseExact(json["fecha_inicio"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                            string inicio = fechaInicio.ToString("yyyy-MM-dd HH:mm:ss");
                            DateTime fechaFin = DateTime.ParseExact(json["fecha_fin"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                            string fin = fechaFin.ToString("yyyy-MM-dd HH:mm:ss");


                            var cmd = new SQLiteCommand(db.Cadenaconexion);
                            cmd.CommandText = $"INSERT INTO reserva (id_salaFK,id_responsableFK,cantidad_personas,fecha_inicio,fecha_fin,catering) " +
                                $"VALUES({json["id_salaFK"]},{json["id_responsableFK"]}, {json["cantidad_personas"]},'{inicio}','{fin}',{json["catering"]})";
                            cmd.ExecuteNonQuery();

                            //Actualizar disponilidad de la sala
                            sala.UpdateAvailability();
                            return "Se creó correctamente el registro";
                        }
                        else
                        {
                            return "La sala ya se ocupo, no hay disponibilidad";
                        }
                    }
                    else
                    {
                        return "La sala no cuenta con la capacidad para tantas personas";
                    }
                }
                else
                {
                    return "La sala no existe";
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
                DateTime fechaInicio = DateTime.ParseExact(json["fecha_inicio"].ToString(), "yyyy-MM-dd HH:mm", null);
                DateTime fechaFin = DateTime.ParseExact(json["fecha_fin"].ToString(), "yyyy-MM-dd HH:mm", null);
                db.OpenConnection();
                var cmd = new SQLiteCommand(db.Cadenaconexion);

                cmd.CommandText = $"select count(*) from reserva where id_reserva = {json["id_reserva"]}";
                int rows = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (rows > 0)
                {
                    cmd.CommandText = $"UPDATE reserva SET id_salaFK = {json["id_salaFK"]},id_responsableFK = {json["id_responsableFK"]},cantidad_personas = {json["cantidad_personas"]},fecha_inicio = '{fecha_inicio}',fecha_fin = '{fecha_fin}',catering = {json["catering"]} where id_reserva = {json["id_reserva"]}";
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