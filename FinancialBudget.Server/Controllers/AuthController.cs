using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Entities.Response;
using FinancialBudget.Server.Services.Interfaces;
using FluentValidation.Results;
using Lombok.NET;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBudget.Server.Controllers
{
    /// <summary>
    /// Defines the <see cref="AuthController" />
    /// </summary> 
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllArgsConstructor]
    public partial class AuthController : CommonController
    {
        /// <summary>
        /// Defines the _authService
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// Defines the _authService
        /// </summary>
        private readonly IMapper _mapper;


        /// <summary>
        /// The Login
        /// </summary>
        /// <param name="model">The model<see cref="LoginRequest"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginRequest model)
        {
            var response = _authService.Auth(model);

            if (response.Success)
            {
                Response<AuthResponse> authResponse = new()
                {
                    Data = response.Data,
                    Success = response.Success,
                    Message = response.Message
                };

                return Ok(authResponse);
            }

            Response<List<ValidationFailure>> errorResponse = new()
            {
                Data = response.Errors,
                Success = response.Success,
                Message = response.Message
            };

            return BadRequest(errorResponse);
        }
    }
}