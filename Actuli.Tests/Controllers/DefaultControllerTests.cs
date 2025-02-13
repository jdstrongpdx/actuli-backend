using Actuli.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Actuli.Tests.Controllers
{
    public class DefaultControllerTests
    {
        [Fact]
        public void Index_ShouldReturnWelcomeMessage()
        {
            // Arrange
            var controller = new DefaultController();

            // Act
            var result = controller.Index() as OkObjectResult;

            // Assert
            Assert.NotNull(result); // Ensure the action does not return null
            Assert.Equal(200, result.StatusCode); // Ensure the status code is OK (200)
            Assert.Equal("Welcome to the Actuli Backend API!", result.Value); // Verify the response content
        }

        [Fact]
        public void HelloWorld_ShouldReturnHelloWorldMessage()
        {
            // Arrange
            var controller = new DefaultController();

            // Act
            var result = controller.HelloWorld() as OkObjectResult;

            // Assert
            Assert.NotNull(result); // Ensure the action does not return null
            Assert.Equal(200, result.StatusCode); // Ensure the status code is OK (200)
            Assert.Equal("Hello World!", result.Value); // Verify the response content
        }
    }
}