using Microsoft.AspNetCore.Mvc;
using SIIILab2.Models.ModelViews;
using SIIILab2.Models;
using Newtonsoft.Json;

namespace SIIILab2.Controllers
{
    public class TrafficController : Controller
    {
        ApiController apiController;
        private readonly ApiLogger _logger;

        public TrafficController(ApplicationContext context, ILogger<ApiController> logger)
        {
            apiController = new ApiController(context, logger);
            _logger = new ApiLogger(logger);
        }

        public ActionResult Index()
        {
            try
            {
                _logger.LogInformation("TrafficController", "Index action called.");

                IActionResult? t = apiController.GetTraffics();
                var content = t as ContentResult;
                var json = content.Content;
                var data = JsonConvert.DeserializeObject<IEnumerable<Traffic>>(json);

                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in Index: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult GetTraffic(int id)
        {
            try
            {
                _logger.LogInformation("TrafficController", $"GetTraffic action called with Id: {id}");

                IActionResult? t = apiController.GetTrafficById(id);

                if (t != null)
                {
                    var content = t as ContentResult;
                    var json = content.Content;
                    var data = JsonConvert.DeserializeObject<Traffic>(json);

                    return View("Read", data);
                }

                _logger.LogWarning("TrafficController", $"Traffic with Id {id} not found. Returning to Index.");
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in GetTraffic: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult UpdateTraffic(int id)
        {
            try
            {
                _logger.LogInformation("TrafficController", $"UpdateTraffic action called with Id: {id}");

                IActionResult? t = apiController.GetTrafficById(id);

                if (t != null)
                {
                    var content = t as ContentResult;
                    var json = content.Content;
                    var data = JsonConvert.DeserializeObject<Traffic>(json);

                    return View("Update", data);
                }

                _logger.LogWarning("TrafficController", $"Traffic with Id {id} not found. Returning to Index.");
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in UpdateTraffic: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTraffic(Traffic t)
        {
            try
            {
                _logger.LogInformation("TrafficController", $"UpdateTraffic POST action called for Traffic Id: {t.Id}");

                Traffic_EditView t_av = new Traffic_EditView();
                t_av.density = t.Density;
                t_av.pathFile = t.PathFile;

                apiController.EditTraffic(t_av, t.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in UpdateTraffic POST: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult DeleteTraffic(int id)
        {
            try
            {
                _logger.LogInformation("TrafficController", $"DeleteTraffic action called with Id: {id}");

                IActionResult? t = apiController.GetTrafficById(id);

                if (t != null)
                {
                    var content = t as ContentResult;
                    var json = content.Content;
                    var data = JsonConvert.DeserializeObject<Traffic>(json);

                    return View("Delete", data);
                }

                _logger.LogWarning("TrafficController", $"Traffic with Id {id} not found. Returning to Index.");
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in DeleteTraffic: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTraffic(Traffic t)
        {
            try
            {
                _logger.LogInformation("TrafficController", $"DeleteTraffic POST action called for Traffic Id: {t.Id}");

                if (t != null)
                {
                    apiController.DeleteTraffic(t.Id);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in DeleteTraffic POST: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult CreateTraffic()
        {
            try
            {
                _logger.LogInformation("TrafficController", "CreateTraffic action called.");

                return PartialView("Create");
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in CreateTraffic: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTraffic(Traffic t)
        {
            try
            {
                _logger.LogInformation("TrafficController", "CreateTraffic POST action called.");

                Traffic_AddView t_av = new Traffic_AddView();
                t_av.density = t.Density;
                t_av.pathFile = t.PathFile;

                apiController.AddTraffic(t_av);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("TrafficController", $"Error in CreateTraffic POST: {ex.Message}");
                return View("Error");
            }
        }
    }
}