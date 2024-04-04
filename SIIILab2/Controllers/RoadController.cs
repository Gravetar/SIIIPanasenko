using Microsoft.AspNetCore.Mvc;
using SIIILab2.Models.ModelViews;
using SIIILab2.Models;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Newtonsoft.Json;

namespace SIIILab2.Controllers
{
    public class RoadController : Controller
    {
        ApiController apiController;
        private readonly ApiLogger _logger;

        public RoadController(ApplicationContext context, ILogger<ApiController> logger)
        {
            apiController = new ApiController(context, logger);
            _logger = new ApiLogger(logger);
        }

        public ActionResult Index()
        {
            try
            {
                _logger.LogInformation("RoadController", "Index action called.");

                IActionResult? r = apiController.GetRoads();
                var content = r as ContentResult;
                var json = content.Content;
                var data = JsonConvert.DeserializeObject<IEnumerable<Road>>(json);

                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in Index: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult GetRoad(int id)
        {
            try
            {
                _logger.LogInformation("RoadController", $"GetRoad action called with Id: {id}");

                IActionResult? r = apiController.GetRoadById(id);

                if (r != null)
                {
                    var content = r as ContentResult;
                    var json = content.Content;
                    var data = JsonConvert.DeserializeObject<Road>(json);

                    return View("Read", data);
                }

                _logger.LogWarning("RoadController", $"Road with Id {id} not found. Returning to Index.");
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in GetRoad: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult UpdateRoad(int id)
        {
            try
            {
                _logger.LogInformation("RoadController", $"UpdateRoad action called with Id: {id}");

                IActionResult? r = apiController.GetRoadById(id);

                if (r != null)
                {
                    var content = r as ContentResult;
                    var json = content.Content;
                    var data = JsonConvert.DeserializeObject<Road>(json);

                    return View("Update", data);
                }

                _logger.LogWarning("RoadController", $"Road with Id {id} not found. Returning to Index.");
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in UpdateRoad: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRoad(Road r)
        {
            try
            {
                _logger.LogInformation("RoadController", $"UpdateRoad POST action called for Road Id: {r.Id}");

                Road_EditView r_av = new Road_EditView();
                r_av.address = r.Address;
                r_av.pathFile = r.PathFile;
                r_av.coorLatitude1 = r.CoorLatitude1;
                r_av.coorLongitude1 = r.CoorLongitude1;
                r_av.coorLatitude2 = r.CoorLatitude2;
                r_av.coorLongitude2 = r.CoorLongitude2;
                r_av.trafficid = r.trafficid;

                apiController.EditRoad(r_av, r.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in UpdateRoad POST: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult DeleteRoad(int id)
        {
            try
            {
                _logger.LogInformation("RoadController", $"DeleteRoad action called with Id: {id}");

                IActionResult? r = apiController.GetRoadById(id);

                if (r != null)
                {
                    var content = r as ContentResult;
                    var json = content.Content;
                    var data = JsonConvert.DeserializeObject<Road>(json);

                    return View("Delete", data);
                }

                _logger.LogWarning("RoadController", $"Road with Id {id} not found. Returning to Index.");
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in DeleteRoad: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoad(Road r)
        {
            try
            {
                _logger.LogInformation("RoadController", $"DeleteRoad POST action called for Road Id: {r.Id}");

                if (r != null)
                {
                    apiController.DeleteRoad(r.Id);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in DeleteRoad POST: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult CreateRoad()
        {
            try
            {
                _logger.LogInformation("RoadController", "CreateRoad action called.");

                return PartialView("Create");
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in CreateRoad: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRoad(Road r)
        {
            try
            {
                _logger.LogInformation("RoadController", "CreateRoad POST action called.");

                Road_AddView r_av = new Road_AddView();
                r_av.address = r.Address;
                r_av.pathFile = r.PathFile;
                r_av.coorLatitude1 = r.CoorLatitude1;
                r_av.coorLongitude1 = r.CoorLongitude1;
                r_av.coorLatitude2 = r.CoorLatitude2;
                r_av.coorLongitude2 = r.CoorLongitude2;
                r_av.trafficid = r.trafficid;

                apiController.AddRoad(r_av);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RoadController", $"Error in CreateRoad POST: {ex.Message}");
                return View("Error");
            }
        }
    }
}