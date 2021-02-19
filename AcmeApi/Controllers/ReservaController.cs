using AcmeApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcmeApi.Controllers
{
    public class ReservaController : Controller
    {

        private const string TABLE = "reserva";
        private const string ID = "id_reserva";

        // GET: Reserva/GetAll/
        public JArray GetAll()
        {
            JArray resp = new JArray();
            JObject respuesta = new JObject();
            try
            {
                Database db = new Database();
                var values = db.GetAllValues(TABLE);
                JValue valores = (JValue)JsonConvert.SerializeObject(values);

                if (valores.Value.ToString() == "[]")
                {
                    respuesta["response"] = "No hay información en base de datos";
                    resp.Add(respuesta);
                    return resp;
                }
                else
                {
                    return JArray.Parse(valores.ToString());
                }
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                resp.Add(respuesta);
                return resp;
            }
        }

        // GET: Reserva/Get/5
        public JArray Get(int id)
        {
            JArray resp = new JArray();
            JObject respuesta = new JObject();
            try
            {
                Database db = new Database();
                var values = db.SearchById(TABLE, ID, id);
                JValue valores = (JValue)JsonConvert.SerializeObject(values);
                if (valores.Value.ToString() == "[]")
                {
                    respuesta["response"] = "No hay información en base de datos";
                    resp.Add(respuesta);
                    return resp;
                }
                else
                {
                    return JArray.Parse(valores.ToString());
                }
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                resp.Add(respuesta);
                return resp;
            }
        }

        // POST: Reserva/Create
        [HttpPost]
        public JObject Create(string data)
        {
            JObject respuesta = new JObject();
            Database db = new Database();
            try
            {
                Reserva reserva = new Reserva();
                respuesta["response"] = reserva.Create(data);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }
        }

        // POST: Reserva/Edit
        [HttpPost]
        public JObject Edit(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                Reserva reserva = new Reserva();
                respuesta["response"] = reserva.Update(data);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }
        }

        // POST: Reserva/Delete
        [HttpPost]
        public JObject Delete(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                Reserva reserva = new Reserva();
                Database db = new Database();
                JObject json = JObject.Parse(data);
                Sala sala = new Sala();
                respuesta["response"] = db.Delete(TABLE, ID, Convert.ToInt32(json[$"{ID}"]));
                sala.Id_sala = 0;
                sala.UpdateAvailability();
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }

        }

    }
}
