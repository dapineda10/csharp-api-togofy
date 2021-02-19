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
    public class ReservaMenuController : Controller
    {
        private const string TABLE = "reserva_menu";

        // GET: ReservaMenu/GetAll/
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

        // GET: ReservaMenu/Get/5
        public JArray Get(string data)
        {
            JArray resp = new JArray();
            JObject respuesta = new JObject();
            try
            {
                Database db = new Database();
                ReservaMenu rm = new ReservaMenu();
                JObject json = JObject.Parse(data);
                rm.Id_reservaFK = Convert.ToInt32(json["id_reservaFK"]);
                rm.Id_salaFK = Convert.ToInt32(json["id_salaFK"]);
                rm.Id_menuFK = Convert.ToInt32(json["id_menuFK"]);
                var values = rm.SearchReservaMenu();
                JValue valores = (JValue)JsonConvert.SerializeObject(values);
                return JArray.Parse(valores.ToString());
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                resp.Add(respuesta);
                return resp;
            }
        }

        // POST: ReservaMenu/Create
        [HttpPost]
        public JObject Create(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                JObject json = JObject.Parse(data);
                ReservaMenu rm = new ReservaMenu();
                rm.Id_reservaFK = Convert.ToInt32(json["id_reservaFK"]);
                rm.Id_salaFK = Convert.ToInt32(json["id_salaFK"]);
                rm.Id_menuFK = Convert.ToInt32(json["id_menuFK"]);
                respuesta["response"] = rm.Create(data);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }
        }

        // POST: ReservaMenu/Edit
        [HttpPost]
        public JObject Edit(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                ReservaMenu reserva_menu = new ReservaMenu();
                respuesta["response"] = reserva_menu.Update(data);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["response"] = ex.Message.ToString();
                return respuesta;
            }
        }

        // POST: ReservaMenu/Delete
        [HttpPost]
        public JObject Delete(string data)
        {
            JObject respuesta = new JObject();
            try
            {
                ReservaMenu reserva_menu = new ReservaMenu();
                Database db = new Database();
                JObject json = JObject.Parse(data);
                reserva_menu.Id_menuFK = Convert.ToInt32(json["id_menuFK"]);
                reserva_menu.Id_reservaFK = Convert.ToInt32(json["id_reservaFK"]);
                reserva_menu.Id_salaFK = Convert.ToInt32(json["id_salaFK"]);
                respuesta["response"] = reserva_menu.Delete();
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