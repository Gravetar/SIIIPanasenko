using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIIILab2.Models;
using SIIILab2.Models.ModelViews;

using Microsoft.Extensions.Logging;

namespace SIIILab2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApiLogger _logger;

        ApplicationContext db;

        public ApiController(ApplicationContext db, ILogger<ApiController> logger)
        {
            _logger = new ApiLogger(logger);
            this.db = db;
        }
        #region Requests

        /// <summary>
        /// Получить записи запросов
        /// </summary>
        [HttpGet("Requests")]
        public IActionResult GetRequests()
        {
            try
            {
                _logger.LogInformation(nameof(ApiController), $"{nameof(GetRequests)} called");

                dynamic rds = new JArray();

                var rds_l = db.RequestRoads?.ToList();

                foreach (var rd in rds_l)
                {
                    rds.Add(new JObject(
                            new JProperty("Id", rd.Id),
                            new JProperty("Costumer", rd.Costumer),
                            new JProperty("RoadId", rd.roadid),
                            new JProperty("Reques_date", rd.Reques_date),
                            new JProperty("Status", rd.Status),
                            new JProperty("Result", rd.Result)
                            ));
                }
                string respStr = rds.ToString();
                return new ContentResult() { Content = respStr, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(ApiController), $"{nameof(GetRequests)} Error: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }

        }

        /// <summary>
        /// Получить запись запроса по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        [HttpGet("Request/{id}")]
        public IActionResult GetRequestById(int id)
        {
            try
            {
                _logger.LogInformation("RequestController", $"GetRequestById action called with Id: {id}");
                RequestRoad? requestRoad = db.RequestRoads?.FirstOrDefault(r => r.Id == id);

                if (requestRoad != null)
                {

                    requestRoad.road = db.Roads.FirstOrDefault(r => r.Id == requestRoad.roadid);

                    requestRoad.road.traffic = db.Traffics.FirstOrDefault(t => t.Id == requestRoad.road.trafficid);

                    string respStr = JsonConvert.SerializeObject(requestRoad, Formatting.Indented);

                    return new ContentResult() { Content = respStr, StatusCode = 200 };
                }
                {
                    return new ContentResult() { Content = "Object Not Found", StatusCode = 404 };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestController", $"Error in GetRequestById: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        /// <summary>
        /// Добавить запись запроса
        /// </summary>
        /// <remarks>
        /// Пример:
        /// 
        ///     POST api/Request
        ///     {        
        ///       "costumer": "ООО ДОРОГА",
        ///       "status": "В работе",
        ///       "result": "NONE" 
        ///       "roadid": "1" 
        ///     }
        /// </remarks>
        /// <param name="data">Данные новой записи запроса</param>
        [HttpPost("Request")]
        public IActionResult AddRequest([FromBody] RequestRoad_AddView data)
        {
            try
            {
                _logger.LogInformation("RequestController", "AddRequest POST action called.");
                dynamic resp = new JObject();

                string Costumer = data.costumer;
                DateTime Request_date = DateTime.Now;
                string Status = data.status;
                string Result = data.result;
                int RoadId = data.roadid;

                RequestRoad request_road = new RequestRoad();

                request_road.Costumer = Costumer;
                request_road.Reques_date = Request_date;
                request_road.Status = Status;
                request_road.Result = Result;
                request_road.roadid = RoadId;

                request_road.road = db.Roads.FirstOrDefault(r => r.Id == request_road.roadid);

                request_road.road.traffic = db.Traffics.FirstOrDefault(t => t.Id == request_road.road.trafficid);


                db.RequestRoads.Add(request_road);
                db.SaveChanges();
                resp = JsonConvert.SerializeObject(request_road, Formatting.Indented);

                string respStr = resp.ToString();

                return new ContentResult() { Content = respStr, StatusCode = 201 };
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestController", $"Error in AddRequest POST: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        /// <summary>
        /// Изменить запись запроса
        /// </summary>
        /// <remarks>
        /// Пример:
        /// 
        ///     PUT api/Request/1
        ///     {        
        ///       "costumer": "ООО ДОРОГА",
        ///       "request_date": "01.01.2000 00:00",
        ///       "status": "В работе",
        ///       "result": "NONE" 
        ///       "roadid": "1" 
        ///     }
        /// </remarks>
        /// <param name="data">Данные для изменения</param>
        /// <param name="id">Идентификатор изменяемой записи</param>
        [HttpPut("Request/{id}")]
        public IActionResult EditRequest([FromBody] RequestRoad_EditView data, int id)
        {
            try
            {
                _logger.LogInformation("RequestController", $"EditRequest PUT action called with Id: {id}");
                dynamic resp = new JObject();

                string Costumer = data.costumer;
                DateTime Request_date = data.request_date;
                string Status = data.status;
                string Result = data.result;
                int RoadId = data.roadid;

                RequestRoad request_road = db.RequestRoads.FirstOrDefault(r => r.Id == id);

                request_road.Costumer = Costumer;
                request_road.Reques_date = Request_date;
                request_road.Status = Status;
                request_road.Result = Result;
                request_road.roadid = RoadId;

                request_road.road = db.Roads.FirstOrDefault(r => r.Id == request_road.roadid);

                request_road.road.traffic = db.Traffics.FirstOrDefault(t => t.Id == request_road.road.trafficid);

                db.RequestRoads.Update(request_road);
                db.SaveChanges();
                resp = JsonConvert.SerializeObject(request_road, Formatting.Indented);
                string respStr = resp.ToString();

                return new ContentResult() { Content = respStr, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestController", $"Error in EditRequest PUT: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
                throw;
            }
        }

        /// <summary>
        /// Удалить запись запроса по идентификатору записи
        /// </summary>
        /// <param name="id">Идентификатор записи для удаления</param>
        [HttpDelete("Request/{id}")]
        public IActionResult DeleteRequest(int id)
        {
            try
            {
                _logger.LogInformation("RequestController", $"DeleteRequest DELETE action called with Id: {id}");
                dynamic resp = new JObject();
                RequestRoad rr = db.RequestRoads.FirstOrDefault(rr => rr.Id == id);

                db.RequestRoads.Remove(rr);
                db.SaveChanges();
                resp.id = rr.Id;
                resp.status = "DELETED";

                string respStr = resp.ToString();

                return new ContentResult() { Content = respStr, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestController", $"Error in DeleteRequest DELETE: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 200 };
            }
        }

        #endregion

        #region Road

        /// <summary>
        /// Получить записи дорог
        /// </summary>
        [HttpGet("Roads")]
        public IActionResult GetRoads()
        {
            try
            {
                _logger.LogInformation("RoadController", "GetRoads action called.");
                dynamic r = new JArray();

                var r_l = db.Roads?.ToList();

                foreach (Road r_r in r_l)
                {
                    r.Add(new JObject(
                        new JProperty("Id", r_r.Id),
                        new JProperty("Address", r_r.Address),
                        new JProperty("PathFile", r_r.PathFile),
                        new JProperty("CoorLatitude1", r_r.CoorLatitude1),
                        new JProperty("CoorLongitude1", r_r.CoorLongitude1),
                        new JProperty("CoorLatitude2", r_r.CoorLatitude2),
                        new JProperty("CoorLongitude2", r_r.CoorLongitude2),
                        new JProperty("TrafficId", r_r.trafficid)
                        ));
                }
                string respStr = r.ToString();

                return new ContentResult() { Content = respStr, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in GetRoads: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        /// <summary>
        /// Получить запись дороги по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        [HttpGet("Road/{id}")]
        public IActionResult GetRoadById(int id)
        {
            try
            {
                _logger.LogInformation("RoadController", $"GetRoadById action called with Id: {id}");
                Road? road = db.Roads?.FirstOrDefault(r => r.Id == id);

                if (road != null)
                {
                    road = db.Roads?.FirstOrDefault(r => r.Id == id);
                    road.traffic = db.Traffics.FirstOrDefault(t => t.Id == road.trafficid);

                    string respStr = JsonConvert.SerializeObject(road, Formatting.Indented);

                    return new ContentResult() { Content = respStr, StatusCode = 200 };
                }
                {
                    return new ContentResult() { Content = "FAIL", StatusCode = 404 };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in GetRoadById: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        /// <summary>
        /// Добавить запись дороги
        /// </summary>
        /// <remarks>
        /// Пример:
        /// 
        ///     POST api/Road
        ///     {        
        ///       "address": "ООО ДОРОГА",
        ///       "pathFile": "\\Roads\\1.xml",
        ///       "coorLatitude1": 48.4454 
        ///       "coorLongitude1": 48.6454
        ///       "CoorLatitude2": 48.1454
        ///       "CoorLongitude2": 48.3454
        ///       "trafficid": 1
        ///     }
        /// </remarks>
        /// <param name="data">Данные новой записи запроса</param>
        [HttpPost("Road")]
        public IActionResult AddRoad([FromBody] Road_AddView data)
        {
            try
            {
                _logger.LogInformation("RoadController", "AddRoad POST action called.");
                dynamic resp = new JObject();

                string Address = data.address;
                string PathFile = data.pathFile;
                double CoorLatitude1 = data.coorLatitude1;
                double CoorLongitude1 = data.coorLongitude1;
                double CoorLatitude2 = data.coorLatitude2;
                double CoorLongitude2 = data.coorLongitude2;
                int trafficid = data.trafficid;

                Road road = new Road();

                road.Address = Address;
                road.PathFile = PathFile;
                road.CoorLatitude1 = CoorLatitude1;
                road.CoorLongitude1 = CoorLongitude1;
                road.CoorLatitude2 = CoorLatitude2;
                road.CoorLongitude2 = CoorLongitude2;
                road.trafficid = trafficid;

                road.traffic = db.Traffics.FirstOrDefault(t => t.Id == road.trafficid);

                db.Roads.Add(road);
                db.SaveChanges();
                resp = JsonConvert.SerializeObject(road, Formatting.Indented);

                string respStr = resp.ToString();

                return new ContentResult() { Content = respStr, StatusCode = 201 };
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in AddRoad POST: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        /// <summary>
        /// Изменить запись дороги
        /// </summary>
        /// <remarks>
        /// Пример:
        /// 
        ///     PUT api/Road
        ///     {        
        ///       "address": "ООО Новая ДОРОГА",
        ///       "pathFile": "\\Roads\\1.xml",
        ///       "coorLatitude1": 48.4454 
        ///       "coorLongitude1": 48.6454
        ///       "CoorLatitude2": 48.1454
        ///       "CoorLongitude2": 48.3454
        ///       "trafficid": 1
        ///     }
        /// </remarks>
        /// <param name="data">Данные для изменения</param>
        /// <param name="id">Идентификатор изменяемой записи</param>
        [HttpPut("Road/{id}")]
        public IActionResult EditRoad([FromBody] Road_EditView data, int id)
        {
            try
            {
                _logger.LogInformation("RoadController", $"EditRoad PUT action called with Id: {id}");
                dynamic resp = new JObject();

                string Address = data.address;
                string PathFile = data.pathFile;
                double CoorLatitude1 = data.coorLatitude1;
                double CoorLongitude1 = data.coorLongitude1;
                double CoorLatitude2 = data.coorLatitude2;
                double CoorLongitude2 = data.coorLongitude2;
                int trafficid = data.trafficid;

                Road road = db.Roads.FirstOrDefault(r => r.Id == id);

                road.Address = Address;
                road.PathFile = PathFile;
                road.CoorLatitude1 = CoorLatitude1;
                road.CoorLongitude1 = CoorLongitude1;
                road.CoorLatitude2 = CoorLatitude2;
                road.CoorLongitude2 = CoorLongitude2;
                road.trafficid = trafficid;

                road.traffic = db.Traffics.FirstOrDefault(t => t.Id == road.trafficid);

                db.Roads.Update(road);
                db.SaveChanges();
                resp = JsonConvert.SerializeObject(road, Formatting.Indented);

                string respStr = resp.ToString();

                return new ContentResult() { Content = respStr, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in EditRoad PUT: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        /// <summary>
        /// Удалить запись дороги по идентификатору записи
        /// </summary>
        /// <param name="id">Идентификатор записи для удаления</param>
        [HttpDelete("Road/{id}")]
        public IActionResult DeleteRoad(int id)
        {
            try
            {
                _logger.LogInformation("RoadController", $"DeleteRoad DELETE action called with Id: {id}");
                dynamic resp = new JObject();
                Road r = db.Roads.FirstOrDefault(r => r.Id == id);

                db.Roads.Remove(r);
                db.SaveChanges();
                resp.id = r.Id;
                resp.status = "DELETED";

                string respStr = resp.ToString();

                return new ContentResult() { Content = respStr, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in DeleteRoad DELETE: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        #endregion

        #region Traffic

        /// <summary>
        /// Получить записи трафиков
        /// </summary>
        [HttpGet("Traffics")]
        public IActionResult GetTraffics()
        {
            try
            {
                _logger.LogInformation("TrafficController", "GetTraffics action called.");
                dynamic t = new JArray();

                var t_l = db.Traffics?.ToList();

                foreach (Traffic t_t in t_l)
                {
                    t.Add(new JObject(
                        new JProperty("Id", t_t.Id),
                        new JProperty("Density", t_t.Density),
                        new JProperty("PathFile", t_t.PathFile)
                        ));
                }
                string respStr = t.ToString();

                return new ContentResult() { Content = respStr, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in GetTraffics: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        /// <summary>
        /// Получить запись трафика по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        [HttpGet("Traffic/{id}")]
        public IActionResult GetTrafficById(int id)
        {
            try
            {
                _logger.LogInformation("TrafficController", $"GetTrafficById action called with Id: {id}");
                Traffic? traffic = db.Traffics?.FirstOrDefault(t => t.Id == id);

                if (traffic != null)
                {

                    traffic = db.Traffics?.FirstOrDefault(t => t.Id == id);

                    string respStr = JsonConvert.SerializeObject(traffic, Formatting.Indented);

                    return new ContentResult() { Content = respStr, StatusCode = 200 };
                }
                {
                    return new ContentResult() { Content = "Not Found", StatusCode = 404 };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in GetTrafficById: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        /// <summary>
        /// Добавить запись трафика
        /// </summary>
        /// <remarks>
        /// Пример:
        /// 
        ///     POST api/Traffic
        ///     {        
        ///       "density": 5,
        ///       "pathFile": "\\Traffics\\1.xml"
        ///     }
        /// </remarks>
        /// <param name="data">Данные новой записи трафика</param>
        [HttpPost("Traffic")]
        public IActionResult AddTraffic([FromBody] Traffic_AddView data)
        {
            try
            {
                _logger.LogInformation("TrafficController", "AddTraffic POST action called.");
                dynamic resp = new JObject();

                int Density = data.density;
                string PathFile = data.pathFile;

                Traffic traffic = new Traffic();

                traffic.Density = Density;
                traffic.PathFile = PathFile;

                db.Traffics.Add(traffic);
                db.SaveChanges();
                resp = JsonConvert.SerializeObject(traffic, Formatting.Indented);

                string respStr = resp.ToString();

                return new ContentResult() { Content = respStr, StatusCode = 201 };
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in AddTraffic POST: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        /// <summary>
        /// Изменить запись трафика
        /// </summary>
        /// Пример:
        /// 
        ///     PUT api/Traffic
        ///     {        
        ///       "density": 2,
        ///       "pathFile": "\\Traffics\\1.xml"
        ///     }
        /// </remarks>
        /// <param name="data">Данные для изменения</param>
        /// <param name="id">Идентификатор изменяемой записи</param>
        [HttpPut("Traffic/{id}")]
        public IActionResult EditTraffic([FromBody] Traffic_EditView data, int id)
        {
            try
            {
                _logger.LogInformation("TrafficController", $"EditTraffic PUT action called with Id: {id}");
                dynamic resp = new JObject();

                int Density = data.density;
                string PathFile = data.pathFile;

                Traffic traffic = db.Traffics.FirstOrDefault(t => t.Id == id);

                traffic.Density = Density;
                traffic.PathFile = PathFile;

                db.Traffics.Update(traffic);
                db.SaveChanges();
                resp = JsonConvert.SerializeObject(traffic, Formatting.Indented);

                string respStr = resp.ToString();

                return new ContentResult() { Content = respStr, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in EditTraffic PUT: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        /// <summary>
        /// Удалить запись трафика по идентификатору записи
        /// </summary>
        /// <param name="id">Идентификатор записи для удаления</param>
        [HttpDelete("Traffic/{id}")]
        public IActionResult DeleteTraffic(int id)
        {
            try
            {
                _logger.LogInformation("TrafficController", $"DeleteTraffic DELETE action called with Id: {id}");
                dynamic resp = new JObject();
                Traffic t = db.Traffics.FirstOrDefault(t => t.Id == id);

                db.Traffics.Remove(t);
                db.SaveChanges();
                resp.id = t.Id;
                resp.status = "DELETED";

                string respStr = resp.ToString();

                return new ContentResult() { Content = respStr, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in DeleteTraffic DELETE: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 400 };
            }
        }

        #endregion
    }
}
