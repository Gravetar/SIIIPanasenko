using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SIIILab2.Models;
using SIIILab2.Models.ModelViews;

namespace SIIILab2.Controllers
{
    public class RequestRoadController : Controller
    {
        ApiController apiController;
        private readonly ApiLogger _logger;

        public RequestRoadController(ApplicationContext context, ILogger<ApiController> logger)
        {
            apiController = new ApiController(context, logger);
            _logger = new ApiLogger(logger);
        }

        // GET: RequestRoadController
        public ActionResult Index()
        {
            try
            {
                _logger.LogInformation("RequestRoadController", "Index action called.");

                IActionResult? rr = apiController.GetRequests();
                var content = rr as ContentResult;
                var json = content.Content;
                var data = JsonConvert.DeserializeObject<IEnumerable<RequestRoad>>(json);

                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestRoadController", $"Error in Index: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult GetRequest(int id)
        {
            try
            {
                _logger.LogInformation("RequestRoadController", $"GetRequest action called with Id: {id}");

                IActionResult? rr = apiController.GetRequestById(id);

                if (rr != null)
                {
                    var content = rr as ContentResult;
                    var json = content.Content;
                    var data = JsonConvert.DeserializeObject<RequestRoad>(json);

                    return View("Read", data);
                }

                _logger.LogWarning("RequestRoadController", $"RequestRoad with Id {id} not found. Returning to Index.");
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestRoadController", $"Error in GetRequest: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult UpdateRequestRoad(int id)
        {
            try
            {
                _logger.LogInformation("RequestRoadController", $"UpdateRequestRoad action called with Id: {id}");

                IActionResult? rr = apiController.GetRequestById(id);

                if (rr != null)
                {
                    var content = rr as ContentResult;
                    var json = content.Content;
                    var data = JsonConvert.DeserializeObject<RequestRoad>(json);

                    return View("Update", data);
                }

                _logger.LogWarning("RequestRoadController", $"RequestRoad with Id {id} not found. Returning to Index.");
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestRoadController", $"Error in UpdateRequestRoad: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRequestRoad(RequestRoad rr)
        {
            try
            {
                _logger.LogInformation("RequestRoadController", $"UpdateRequestRoad POST action called for RequestRoad Id: {rr.Id}");

                RequestRoad_EditView rr_av = new RequestRoad_EditView();
                rr_av.costumer = rr.Costumer;
                rr_av.status = rr.Status;
                rr_av.roadid = rr.roadid;
                rr_av.result = rr.Result;
                rr_av.request_date = rr.Reques_date;

                apiController.EditRequest(rr_av, rr.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestRoadController", $"Error in UpdateRequestRoad POST: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult DeleteRequestRoad(int id)
        {
            try
            {
                _logger.LogInformation("RequestRoadController", $"DeleteRequestRoad action called with Id: {id}");

                IActionResult? rr = apiController.GetRequestById(id);

                if (rr != null)
                {
                    var content = rr as ContentResult;
                    var json = content.Content;
                    var data = JsonConvert.DeserializeObject<RequestRoad>(json);

                    return View("Delete", data);
                }

                _logger.LogWarning("RequestRoadController", $"RequestRoad with Id {id} not found. Returning to Index.");
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestRoadController", $"Error in DeleteRequestRoad: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRequestRoad(RequestRoad rr)
        {
            try
            {
                _logger.LogInformation("RequestRoadController", $"DeleteRequestRoad POST action called for RequestRoad Id: {rr.Id}");

                if (rr != null)
                {
                    apiController.DeleteRequest(rr.Id);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestRoadController", $"Error in DeleteRequestRoad POST: {ex.Message}");
                return View("Error");
            }
        }

        public ActionResult CreateRequestRoad()
        {
            try
            {
                _logger.LogInformation("RequestRoadController", "CreateRequestRoad action called.");

                return PartialView("Create");
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestRoadController", $"Error in CreateRequestRoad: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRequestRoad(RequestRoad rr)
        {
            try
            {
                _logger.LogInformation("RequestRoadController", "CreateRequestRoad POST action called.");

                RequestRoad_AddView rr_av = new RequestRoad_AddView();
                rr_av.costumer = rr.Costumer;
                rr_av.status = rr.Status;
                rr_av.roadid = rr.roadid;
                rr_av.result = rr.Result;

                apiController.AddRequest(rr_av);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestRoadController", $"Error in CreateRequestRoad POST: {ex.Message}");
                return View("Error");
            }
        }
    }
}
