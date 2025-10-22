using FinancialBudget.Server.Configs.Models;
using FinancialBudget.Server.Controllers;
using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Entities.Response;
using FinancialBudget.Server.Services.Core;
using FinancialBudget.Server.Services.Interfaces;
using FluentValidation.Results;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

        [Fact]
        public void GetToken_Returns_Valid_JWT_For_User()
        {
            // Arrange
            var appSettings = new AppSettings
            {
                Secret = "supersecretkey1234567890ADFFFDASDF7845D4F6A5SD4F3A2SD1FASDFASDF",
                TokenExpirationHrs = 1,
                NotBefore = 0
            };
            var optionsMock = new Mock<IOptions<AppSettings>>();
            optionsMock.Setup(x => x.Value).Returns(appSettings);

            var loggerMock = new Mock<ILogger<AuthService>>();

            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                Name = "Test User",
                RolId = 2,
                Password = "hashedpassword",
                Rol = new Rol
                {
                    Id = 2,
                    State = 1,
                    RolOperations = new List<RolOperation>
                    {
                        new RolOperation { OperationId = 10, State = 1 },
                        new RolOperation { OperationId = 20, State = 1 }
                    }
                }
            };

            // AuthService requiere otros parámetros, pero para GetToken solo necesitamos _appSettings y _logger
            var authService = (AuthService)Activator.CreateInstance(
                typeof(AuthService),
                null, // _bd
                optionsMock.Object,
                null, // _loginValidations
                null, // _mapper
                loggerMock.Object
            );

            // Act
            var token = typeof(AuthService)
                .GetMethod("GetToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(authService, new object[] { user }) as string;

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
            Assert.Contains(".", token); // JWT tiene al menos dos puntos
        }
    }
}
