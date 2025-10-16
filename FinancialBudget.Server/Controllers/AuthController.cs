namespace FinancialBudget.Server.Controllers
{
    using FinancialBudget.Server.Entities.Request;
    using FinancialBudget.Server.Entities.Response;
    using FinancialBudget.Server.Services.Interfaces;
    using FluentValidation.Results;
    using Lombok.NET;
    using MapsterMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
        /// Defines the _mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Autenticar un usuario, obtener un token JWT, es necesario para acceder a los endpoints protegidos
        /// </summary>
        /// <param name="model">The model<see cref="LoginRequest"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<AuthResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
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
