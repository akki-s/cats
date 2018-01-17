using System;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AGL.Pets.Core.Domain.ViewModels;
using AGL.Pets.Core.Interfaces;
using Moq;
using AGL.Pets.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AGL.Pets.Tests
{
    public class ApiControllerTests
    {
        [Fact]
        public async Task Api_Get_Grouped_When_No_Data_Should_Return_Null()
        {
            //mock petservice object that returns null
            var mockService = new Mock<IPetsService>();
            mockService.Setup(x => x.GetCatsGroupedByOwnerGender())
                .ReturnsAsync(() => null);

            var controller = new PetsController(mockService.Object);

            IActionResult response = await controller.Get();
            response.Should().BeOfType<OkObjectResult>();
            var resultObject = ((OkObjectResult)response).Value as GroupedByOwnerGenderViewModel;
            resultObject.Should().BeNull();
        }

        [Fact]
        public async Task Api_Get_Grouped_When_Exception_Should_Return_InternalServerError()
        {
            //mock carservice objects that throws exception
            var mockService = new Mock<IPetsService>();
            mockService.Setup(x => x.GetCatsGroupedByOwnerGender())
                .Throws(new Exception("exception occured"));

            var controller = new PetsController(mockService.Object);

            IActionResult response = await controller.Get();
            response.Should().BeOfType<ObjectResult>();
            var statusCode = ((ObjectResult)response).StatusCode;
            statusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }


        [Fact]
        public async Task Api_Get_Grouped_Results_Returns_Data_GroupedByOwnerGender_Sorted_By_Name_With_Ok_Response()
        {
            var mockDataForGrouping = new List<GroupedByOwnerGenderViewModel>
            {
                new GroupedByOwnerGenderViewModel {
                    OwnerGender = "Female",
                    Cats = new List<PetDetailsViewModel> {
                        new PetDetailsViewModel { Name = "Garfield", OwnerName = "Selina" },
                        new PetDetailsViewModel { Name = "Abe", OwnerName = "Selina" }
                    }
                },
                new GroupedByOwnerGenderViewModel {
                    OwnerGender = "Male",
                    Cats = new List<PetDetailsViewModel> {
                        new PetDetailsViewModel { Name = "Jasper", OwnerName = "Bruce" },
                        new PetDetailsViewModel { Name = "Garry", OwnerName = "Bruce" }
                    }
                }
            };

            var mockService = new Mock<IPetsService>();
            mockService.Setup(x => x.GetCatsGroupedByOwnerGender())
                .ReturnsAsync(mockDataForGrouping);

            var controller = new PetsController(mockService.Object);

            IActionResult response = await controller.Get();
            response.Should().BeOfType<OkObjectResult>();
            var resultObject = ((OkObjectResult)response).Value as List<GroupedByOwnerGenderViewModel>;

            resultObject.Should().NotBeNull();
            resultObject.Count.Should().Be(mockDataForGrouping.Count);

            JsonConvert.SerializeObject(resultObject).ToLower()
                .Should()
                .Be(JsonConvert.SerializeObject(mockDataForGrouping).ToLower());

            resultObject[0].OwnerGender.Should().Be("Female");
            resultObject[1].OwnerGender.Should().Be("Male");
        }
    }
}
