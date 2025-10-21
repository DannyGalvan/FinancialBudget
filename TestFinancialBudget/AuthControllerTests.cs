using FinancialBudget.Server.Controllers;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Entities.Response;
using FinancialBudget.Server.Services.Interfaces;
using FluentValidation.Results;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestFinancialBudget
{
    public class AuthControllerTests
    {
        [Fact]
        public void Login_ReturnsOk_WhenAuthIsSuccessful()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var mockMapper = new Mock<IMapper>();
            var loginRequest = new LoginRequest { UserName = "user", Password = "pass" };
            var authResponse = new AuthResponse { UserName = "user", Token = "token" };
            var serviceResponse = new Response<AuthResponse, List<ValidationFailure>>
            {
                Success = true,
                Data = authResponse,
                Message = "OK"
            };

            mockAuthService.Setup(s => s.Auth(loginRequest)).Returns(serviceResponse);

            var controller = new AuthController(mockAuthService.Object, mockMapper.Object);

            // Act
            var result = controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<AuthResponse>>(okResult.Value);
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("user", response.Data.UserName);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenAuthFails()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var mockMapper = new Mock<IMapper>();
            var loginRequest = new LoginRequest { UserName = "user", Password = "wrong" };
            var serviceResponse = new Response<AuthResponse, List<ValidationFailure>>
            {
                Success = false,
                Errors = new List<ValidationFailure> { new ValidationFailure("Password", "Incorrect") },
                Message = "Error"
            };

            mockAuthService.Setup(s => s.Auth(loginRequest)).Returns(serviceResponse);

            var controller = new AuthController(mockAuthService.Object, mockMapper.Object);

            // Act
            var result = controller.Login(loginRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<List<ValidationFailure>>>(badRequestResult.Value);
            Assert.False(response.Success);
            Assert.NotNull(response.Data);
            Assert.Single(response.Data);
            Assert.Equal("Incorrect", response.Data[0].ErrorMessage);
        }
    }
}
