using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinancialBudget.Server.Configs.Models;
using FinancialBudget.Server.Context;
using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Entities.Response;
using FinancialBudget.Server.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Lombok.NET;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace FinancialBudget.Server.Services.Core
{
    /// <summary>
    /// Defines the <see cref="AuthService" />
    /// </summary>
    [AllArgsConstructor]
    public partial class AuthService : IAuthService
    {
        /// <summary>
        /// Defines the _bd
        /// </summary>
        private readonly DataContext _bd;

        /// <summary>
        /// Defines the _appSettings
        /// </summary>
        private readonly IOptions<AppSettings> _appSettings;

        /// <summary>
        /// Defines the _loginValidations
        /// </summary>
        private readonly IValidator<LoginRequest> _loginValidations;

        /// <summary>
        /// Defines the _sendMail
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// The Auth
        /// </summary>
        /// <param name="model">The model<see cref="LoginRequest"/></param>
        /// <returns>The <see>
        ///         <cref>Response{AuthResponse, List{ValidationFailure}}</cref>
        ///     </see>
        /// </returns>
        public Response<AuthResponse, List<ValidationFailure>> Auth(LoginRequest model)
        {
            Response<AuthResponse, List<ValidationFailure>> userResponse = new();
            try
            {
                ValidationResult results = _loginValidations.Validate(model);

                if (!results.IsValid)
                {
                    userResponse.Success = false;
                    userResponse.Message = "Usuario y/o contraseña invalidos";
                    userResponse.Data = null;
                    userResponse.Errors = results.Errors;

                    return userResponse;
                }

                User? oUser = _bd.Users.Include(user => user.Rol!).FirstOrDefault(u =>
                    u.Email == model.UserName);

                if (oUser == null)
                {
                    userResponse.Success = false;
                    userResponse.Message = "Usuario y/o contraseña invalidos";
                    userResponse.Data = null;
                    userResponse.Errors = results.Errors;

                    return userResponse;
                }

                if (!BC.BCrypt.Verify(model.Password, oUser.Password))
                {
                    userResponse.Success = false;
                    userResponse.Message = "Usuario y/o contraseña invalidos";
                    userResponse.Data = null;
                    userResponse.Errors = results.Errors;

                    return userResponse;
                }

                List<RolOperation> rolOperations = _bd.RolOperations.Include(r => r.Operation)
                    .Where(r => r.RolId == oUser.RolId && r.State == 1).ToList();

                List<Operation> operationsRol = _mapper.Map<List<RolOperation>, List<Operation>>(rolOperations);

                var modules =
                    _bd.Modules
                        .FromSql($"""
                                  select Id,Name,Description,Image,Path,State,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy from (select ModuleId from RolOperations ro 
                                                                                                      inner join Operations o on o.Id = ro.OperationId
                                                                                                      inner join Modules m on o.ModuleId = m.Id
                                                                                                      where ro.RolId = {oUser.RolId} and ro.state = 1                
                                                                                                      group by ModuleId) as mod
                                                                                                      inner join Modules mo on mod.ModuleId = mo.Id
                                  """).ToList();


                List<Authorizations> authorizations = modules
                    .Select(module => new Authorizations
                        { 
                            Module = _mapper.Map<Module,ModuleResponse>(module), 
                            Operations = _mapper.Map<List<Operation>,List<OperationResponse>>(operationsRol.Where(o => o.ModuleId == module.Id).ToList())
                        })
                    .ToList();

                oUser.Rol!.RolOperations = rolOperations;

                string jwt = GetToken(oUser);

                AuthResponse auth = _mapper.Map<User, AuthResponse>(oUser);
                auth.Token = jwt;
                auth.Operations = authorizations;

                userResponse.Success = true;
                userResponse.Message = "Inicio de sesión exitosa";
                userResponse.Data = auth;
                userResponse.Errors = null;

                return userResponse;
            }
            catch (Exception ex)
            {
                userResponse.Success = false;
                userResponse.Message = "Upss hubo un error";
                userResponse.Data = null;
                userResponse.Errors = [new("Exception", ex.Message)];

                _logger.LogError(ex, "Error al autenticar usuario path: /api/Auth");

                return userResponse;
            }
        }

       
        /// <summary>
        /// The GetToken
        /// </summary>
        /// <param name="user">The user<see cref="User"/></param>
        /// <returns>The <see cref="string"/></returns>
        private string GetToken(User user)
        {
            try
            {
                AppSettings appSettings = _appSettings.Value;
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                var claims = new List<Claim>()
                             {
                                 new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                                 new (ClaimTypes.Email, user.Email),
                                 new (ClaimTypes.Name, user.Name),
                                 new (ClaimTypes.Hash, Guid.NewGuid().ToString()),
                                 new ("Operator", user.RolId.ToString()),
                             };

                if (user.Rol!.RolOperations.Count != 0)
                {
                    claims.AddRange(user.Rol!.RolOperations.Select(item => new Claim(ClaimTypes.Role, item.OperationId.ToString())));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims.ToArray()),
                    NotBefore = DateTime.UtcNow.AddMinutes(appSettings.NotBefore),
                    Expires = DateTime.UtcNow.AddHours(appSettings.TokenExpirationHrs),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error al Generar el jwt");

                return string.Empty;
            }
        }
    }
}
