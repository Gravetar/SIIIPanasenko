using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Common;
using Moq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SIIILab2.Controllers;
using SIIILab2.Models;
using SIIILab2.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SIIILab2.Tests
{
    public class ApiControllerTests
    {
        private readonly Mock<ILogger<ApiController>> _mock = new();
        private readonly IServiceProvider _serviceProvider;

        public ApiControllerTests()
        {
            _serviceProvider = DependencyInjection.InitilizeServices().BuildServiceProvider();
        }

        #region RequestsRoad

        [Fact]
        public void GetRequestsSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            ContentResult result = controller.GetRequests() as ContentResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.StatusCode, 200);
        }

        [Fact]
        public void GetRequestByIdSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            RequestRoad testrr = new RequestRoad
            {
                Id = 1,
                Costumer = "ООО ДОРОГА",
                Reques_date = new DateTime(2023, 11, 12, 17, 54, 11),
                Status = "В работе",
                Result = "NONE",
                roadid = 1,
            };

            // Act
            ContentResult result = controller.GetRequestById(1) as ContentResult;
            var rr = JsonConvert.DeserializeObject<RequestRoad>(result.Content);

            bool eq =
            (
            testrr.Id == rr.Id &&
            testrr.Reques_date == rr.Reques_date &&
            testrr.Status == rr.Status &&
            testrr.Result == rr.Result &&
            testrr.Costumer == rr.Costumer
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eq, true);
            Assert.Equal(result.StatusCode, 200);
        }

        [Fact]
        public void AddRequestSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            RequestRoad_AddView newitem = new RequestRoad_AddView();
            newitem.costumer = "ООО \"Новая дорога\"";
            newitem.status = "В работе";
            newitem.roadid = 1;
            newitem.result = "\\1.xml";

            ContentResult result = controller.AddRequest(newitem) as ContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.StatusCode, 201);
        }

        [Fact]
        public void EditRequestSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            RequestRoad_EditView newitem = new RequestRoad_EditView();
            newitem.costumer = "ООО \"Новая дорога\"";
            newitem.status = "В работе";
            newitem.roadid = 1;
            newitem.result = "\\1.xml";
            newitem.request_date = new DateTime(2023, 11, 12, 17, 54, 11);

            ContentResult result = controller.EditRequest(newitem, 1) as ContentResult;

            var rr = JsonConvert.DeserializeObject<RequestRoad>(result.Content);

            bool eq =
            (
            newitem.costumer == rr.Costumer &&
            newitem.status == rr.Status &&
            newitem.roadid == rr.roadid &&
            newitem.result == rr.Result &&
            newitem.request_date == rr.Reques_date
            );
            // Assert
            Assert.NotNull(result);
            Assert.Equal(eq, true);
            Assert.Equal(result.StatusCode, 200);
        }

        [Fact]
        public void DeleteRequestSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            ContentResult result = controller.DeleteRequest(1) as ContentResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.StatusCode, 200);
        }
        #endregion

        #region Road

        [Fact]
        public void GetRoadsSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            ContentResult result = controller.GetRoads() as ContentResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.StatusCode, 200);
        }

        [Fact]
        public void GetRoadByIdSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            Road testrr = new Road
            {
                Id = 1,
                Address = "г. Волгоград пр. Ленина 1",
                PathFile = "\\Roads\\1.xml",
                CoorLatitude1 = 48.7194,
                CoorLongitude1 = 44.5018,
                CoorLatitude2 = 49.7194,
                CoorLongitude2 = 49.5018,

                trafficid = 1,
            };

            // Act
            ContentResult result = controller.GetRoadById(1) as ContentResult;
            var rr = JsonConvert.DeserializeObject<Road>(result.Content);

            bool eq =
            (
            testrr.Id == rr.Id &&
            testrr.Address == rr.Address &&
            testrr.PathFile == rr.PathFile &&
            testrr.CoorLatitude1 == rr.CoorLatitude1 &&
            testrr.CoorLongitude1 == rr.CoorLongitude1 &&
            testrr.CoorLatitude2 == rr.CoorLatitude2 &&
            testrr.CoorLongitude2 == rr.CoorLongitude2 &&
            testrr.trafficid == rr.trafficid
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eq, true);
            Assert.Equal(result.StatusCode, 200);
        }

        [Fact]
        public void AddRoadSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            Road_AddView newitem = new Road_AddView();
            newitem.coorLatitude1 = 1;
            newitem.coorLatitude2 = 1;
            newitem.coorLongitude1 = 1;
            newitem.coorLongitude2 = 1;
            newitem.pathFile = "/Roads/1.xml";
            newitem.address = "г. Новый город, ул. Новая д1";
            newitem.trafficid = 1;

            ContentResult result = controller.AddRoad(newitem) as ContentResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.StatusCode, 201);
        }

        [Fact]
        public void EditRoadSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            Road_EditView newitem = new Road_EditView();
            newitem.coorLatitude1 = 1;
            newitem.coorLatitude2 = 1;
            newitem.coorLongitude1 = 1;
            newitem.coorLongitude2 = 1;
            newitem.pathFile = "/Roads/1.xml";
            newitem.address = "г. Новый город, ул. Новая д1";
            newitem.trafficid = 1;

            ContentResult result = controller.EditRoad(newitem, 1) as ContentResult;

            var rr = JsonConvert.DeserializeObject<Road>(result.Content);

            bool eq =
            (
            newitem.coorLatitude1 == rr.CoorLatitude1 &&
            newitem.coorLatitude2 == rr.CoorLatitude2 &&
            newitem.coorLongitude1 == rr.CoorLongitude1 &&
            newitem.coorLongitude2 == rr.CoorLongitude2 &&
            newitem.pathFile == rr.PathFile &&
            newitem.address == rr.Address &&
            newitem.trafficid == rr.trafficid
            );
            // Assert
            Assert.NotNull(result);
            Assert.Equal(eq, true);
            Assert.Equal(result.StatusCode, 200);
        }

        [Fact]
        public void DeleteRoadSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            ContentResult result = controller.DeleteRoad(1) as ContentResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.StatusCode, 200);
        }
        #endregion

        #region Road

        [Fact]
        public void GetTrafficsSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            ContentResult result = controller.GetTraffics() as ContentResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.StatusCode, 200);
        }

        [Fact]
        public void GetTrafficByIdSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            Traffic testrr = new Traffic
            {
                Id = 1,
                Density = 1,
                PathFile = "\\Traffics\\1.xml"
            };

            // Act
            ContentResult result = controller.GetTrafficById(1) as ContentResult;
            var rr = JsonConvert.DeserializeObject<Traffic>(result.Content);

            bool eq =
            (
            testrr.Id == rr.Id &&
            testrr.Density == rr.Density &&
            testrr.PathFile == rr.PathFile
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eq, true);
            Assert.Equal(result.StatusCode, 200);
        }

        [Fact]
        public void AddTrafficSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            Traffic_AddView newitem = new Traffic_AddView();
            newitem.pathFile = "Traffics\\1.xml";
            newitem.density = 1;

            ContentResult result = controller.AddTraffic(newitem) as ContentResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.StatusCode, 201);
        }
        [Fact]
        public void EditTrafficSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            Traffic_EditView newitem = new Traffic_EditView();
            newitem.pathFile = "Traffics\\1new.xml";
            newitem.density = 1;

            ContentResult result = controller.EditTraffic(newitem, 1) as ContentResult;

            var rr = JsonConvert.DeserializeObject<Traffic>(result.Content);

            bool eq =
            (
            newitem.pathFile == rr.PathFile &&
            newitem.density == rr.Density
            );
            // Assert
            Assert.NotNull(result);
            Assert.Equal(eq, true);
            Assert.Equal(result.StatusCode, 200);
        }

        [Fact]
        public void DeleteTrafficSuccessTest()
        {
            var db = _serviceProvider.GetRequiredService<ApplicationContext>();
            // Arrange
            ApiController controller = new ApiController(db, _mock.Object);
            // Act
            ContentResult result = controller.DeleteTraffic(1) as ContentResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.StatusCode, 200);
        }
        #endregion
    }
}
