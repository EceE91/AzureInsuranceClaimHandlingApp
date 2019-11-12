using ClaimHandlingApp.Controllers;
using ClaimHandlingApp.Models;
using ClaimHandlingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace ClaimHandlingApp.UnitTest
{
    public class ClaimUnitTest_ForIndex
    {
        [Fact]

        public void Index_ReturnsAViewResult_WithAListOfClaims()
        {
            // Arrange
            var mockRepo = new Mock<ICosmosDbService>();
            mockRepo.Setup(repo => repo.GetItemsAsync("SELECT * FROM c"))
                .Returns(GetTestClaims());
            var controller = new ClaimController(mockRepo.Object);

            // Act
            var result = controller.Index() as Task<IActionResult>;

            // Assert
            var viewResult = Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<Claim>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        private async Task<IEnumerable<Claim>> GetTestClaims()
        {       
            List<Claim> results = new List<Claim>() {
                    new Claim()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "John",
                        Year = 2017,
                        DamageCost=1.2m,
                        Type= TypeEnum.BadWeather
                    },
                    new Claim()
                    {
                        Id =  Guid.NewGuid().ToString(),
                        Name = "Doe",
                        Year = 1960,
                        DamageCost=33.889m,
                        Type= TypeEnum.Collision
                    }
            };

            return await Task<List<Claim>>.FromResult(results);
        }
    }
}
