using ClaimHandlingApp.Controllers;
using ClaimHandlingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ClaimHandlingApp.Models;

namespace ClaimHandlingApp.UnitTest
{
    public class ClaimUnitTest_ForAdd
    {
        [Fact]
        public void Add_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<ICosmosDbService>();
         

            var controller = new ClaimController(mockRepo.Object);
            controller.ModelState.AddModelError("Name", "Required");
            var newClaim = GetTestClaim();
            newClaim.DamageCost = 300.7m;

            // Act
            var result = controller.CreateAsync(newClaim);

            // Assert
            var badRequestResult = Assert.IsType<ViewResult>(result.Result);
            Assert.True(!badRequestResult.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Add_AddsClaimAndReturnsARedirect_WhenModelStateIsValid()
        {
            // Arrange
            var mockRepo = new Mock<ICosmosDbService>();
            mockRepo.Setup(repo => repo.AddItemAsync(It.IsAny<Claim>()))
                .Verifiable();
            var controller = new ClaimController(mockRepo.Object);
            var newClaim = GetTestClaim();

            // Act
            var result = controller.CreateAsync(newClaim) as Task<ActionResult>;

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepo.Verify();
        }

        private Claim GetTestClaim()
        {
            var claim = new Claim()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "ece",
                Year = 2017,
                DamageCost = 1.7m,
                Type = TypeEnum.BadWeather
            };

            return claim;
        }

    }
}
