using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Services.Interfaces;
using Lombok.NET;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Reflection;
using FinancialBudget.Server.Entities.Response;
using FluentValidation.Results;

namespace FinancialBudget.Server.Controllers
{
    [Route("api/v1/[controller]/{catalogue}")]
    [ApiController]
    [AllArgsConstructor]
    public partial class CatalogueController : CommonController
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        // ReSharper disable once UnusedMember.Local
        private IEntityService<TEntity, TRequest, TId> GetService<TEntity, TRequest, TId>(string catalogue)
        {
            return _serviceProvider.GetRequiredKeyedService<IEntityService<TEntity, TRequest, TId>>(catalogue);
        }

        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult GetAll(string catalogue, [FromQuery] QueryParamsCatalogueRequest query)
        {
            try
            {
                // Resuelve el tipo dinámico desde el string
                Type entityType = Type.GetType($"FinancialBudget.Server.Entities.Models.{catalogue}", throwOnError: false)
                                  ?? throw new InvalidOperationException($"El Catalogo {catalogue} no existe");

                var idProperty = entityType.GetProperty("Id")
                                 ?? throw new InvalidOperationException($"El tipo {entityType.Name} no tiene una propiedad 'Id'.");

                Type idType = idProperty.PropertyType;

                Type requestType = Type.GetType($"FinancialBudget.Server.Entities.Request.Catalogue.{catalogue}Request")
                                   ?? typeof(CatalogueRequest);
                Type responseType = Type.GetType($"FinancialBudget.Server.Entities.Response.CatalogueResponse", throwOnError: true)!;

                // Invoca this.GetService<TEntity, TRequest, TId>(catalogue) usando reflexión
                var getServiceMethod = typeof(CatalogueController).GetMethod("GetService", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(entityType, requestType, idType);

                var serviceInstance = getServiceMethod.Invoke(this, [catalogue]);

                // Llama al método GetAll en el servicio
                var methodGetAll = serviceInstance!.GetType().GetMethod("GetAll")!;
                var response = methodGetAll.Invoke(serviceInstance, [query.Filters!, null, query.PageNumber, query.PageSize, query.IncludeTotal]);

                dynamic dynResponse = response!;
                var data = dynResponse.Data;

                // Mapear dinámicamente
                var mapListMethod = typeof(IMapper).GetMethod("Map", [entityType])!
                    .MakeGenericMethod(typeof(List<>).MakeGenericType(responseType));
                var mapped = mapListMethod.Invoke(_mapper, [data]);

                return Ok(new
                {
                    dynResponse.Success,
                    dynResponse.Message,
                    dynResponse.TotalResults,
                    Data = mapped
                });
            }
            catch (Exception ex)
            {
                Response<List<ValidationFailure>> errorResponse = new()
                {
                    Data = [new ValidationFailure("Catalogue", ex.Message)],
                    Success = false,
                    Message = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(string catalogue, string id)
        {
            try
            {
                var entityType = Type.GetType($"FinancialBudget.Server.Entities.Models.{catalogue}", throwOnError: true)!;
                var requestType = Type.GetType($"FinancialBudget.Server.Entities.Request.Catalogue.{catalogue}Request")
                                  ?? typeof(CatalogueRequest);
                var idProperty = entityType.GetProperty("Id")
                                 ?? throw new InvalidOperationException($"El tipo {entityType.Name} no tiene una propiedad 'Id'.");
                Type idType = idProperty.PropertyType;
                var responseType = Type.GetType($"FinancialBudget.Server.Entities.Response.CatalogueResponse", throwOnError: true)!;

                // 👉 Convertir el string 'id' al tipo real de ID
                var converter = TypeDescriptor.GetConverter(idType);
                var convertedId = converter.ConvertFromString(id);

                var getServiceMethod = typeof(CatalogueController)
                    .GetMethod("GetService", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(entityType, requestType, idType);

                var service = getServiceMethod.Invoke(this, [catalogue]);
                var response = service!.GetType().GetMethod("GetById")!.Invoke(service, [convertedId]);

                dynamic dynResponse = response!;


                if (!dynResponse.Success)
                    return BadRequest(new { dynResponse.Success, dynResponse.Message, dynResponse.Errors });

                var mapped = _mapper.Map(dynResponse.Data, dynResponse.Data.GetType(), responseType);

                return Ok(new { dynResponse.Success, dynResponse.Message, Data = mapped });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, ex.Message });
            }
        }

        [Authorize]
        [HttpPost("")]
        public IActionResult Create(string catalogue, [FromBody] CatalogueRequest request)
        {
            try
            {
                var entityType = Type.GetType($"FinancialBudget.Server.Entities.Models.{catalogue}", throwOnError: true)!;
                var requestType = Type.GetType($"FinancialBudget.Server.Entities.Request.Catalogue.{catalogue}Request")
                                  ?? typeof(CatalogueRequest);
                var idProperty = entityType.GetProperty("Id")
                                 ?? throw new InvalidOperationException($"El tipo {entityType.Name} no tiene una propiedad 'Id'.");
                Type idType = idProperty.PropertyType;
                var responseType = Type.GetType($"FinancialBudget.Server.Entities.Response.CatalogueResponse", throwOnError: true)!;

                request.CreatedBy = GetUserId();

                object model = request;
                if (requestType != typeof(CatalogueRequest))
                {
                    model = _mapper.Map(request, typeof(CatalogueRequest), requestType);
                }

                var service = typeof(CatalogueController)
                    .GetMethod("GetService", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(entityType, requestType, idType)
                    .Invoke(this, [catalogue]);

                var response = service!.GetType().GetMethod("Create")!.Invoke(service, [model]);
                dynamic dynResponse = response!;
                var mapped = _mapper.Map(dynResponse.Data, entityType, responseType);

                return dynResponse.Success
                    ? Ok(new { dynResponse.Success, dynResponse.Message, Data = mapped })
                    : BadRequest(new { dynResponse.Success, dynResponse.Message, dynResponse.Errors });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, ex.Message });
            }
        }

        [Authorize]
        [HttpPut("")]
        public IActionResult Update(string catalogue, [FromBody] CatalogueRequest request)
        {
            return HandleUpdateInternal(catalogue, request, "Update");
        }

        [Authorize]
        [HttpPatch("")]
        public IActionResult PartialUpdate(string catalogue, [FromBody] CatalogueRequest request)
        {
            return HandleUpdateInternal(catalogue, request, "PartialUpdate");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(string catalogue, long id)
        {
            try
            {
                var entityType = Type.GetType($"FinancialBudget.Server.Entities.Models.{catalogue}", throwOnError: true)!;
                var requestType = Type.GetType($"FinancialBudget.Server.Entities.Request.Catalogue.{catalogue}Request")
                                  ?? typeof(CatalogueRequest);
                var idProperty = entityType.GetProperty("Id")
                                 ?? throw new InvalidOperationException($"El tipo {entityType.Name} no tiene una propiedad 'Id'.");
                Type idType = idProperty.PropertyType;
                var responseType = Type.GetType($"FinancialBudget.Server.Entities.Response.CatalogueResponse", throwOnError: true)!;

                var getServiceMethod = typeof(CatalogueController)
                    .GetMethod("GetService", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(entityType, requestType, idType);

                var service = getServiceMethod.Invoke(this, [catalogue]);

                var deleteMethod = service!.GetType().GetMethod("Delete")!;
                var response = deleteMethod.Invoke(service, [id, GetUserId()]);

                dynamic dynResponse = response!;
                var mapped = _mapper.Map(dynResponse.Data, entityType, responseType);

                return dynResponse.Success
                    ? Ok(new { dynResponse.Success, dynResponse.Message, Data = mapped })
                    : BadRequest(new { dynResponse.Success, dynResponse.Message, dynResponse.Errors });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, ex.Message });
            }
        }

        private IActionResult HandleUpdateInternal(string catalogue, CatalogueRequest request, string methodName)
        {
            try
            {
                var entityType = Type.GetType($"FinancialBudget.Server.Entities.Models.{catalogue}", throwOnError: true)!;
                var requestType = Type.GetType($"FinancialBudget.Server.Entities.Request.Catalogue.{catalogue}Request")
                                  ?? typeof(CatalogueRequest);
                var idProperty = entityType.GetProperty("Id")
                                 ?? throw new InvalidOperationException($"El tipo {entityType.Name} no tiene una propiedad 'Id'.");
                Type idType = idProperty.PropertyType;
                var responseType = Type.GetType($"FinancialBudget.Server.Entities.Response.CatalogueResponse", throwOnError: true)!;

                request.UpdatedBy = GetUserId();

                object model = request;
                if (requestType != typeof(CatalogueRequest))
                {
                    model = _mapper.Map(request, typeof(CatalogueRequest), requestType);
                }

                var service = typeof(CatalogueController)
                    .GetMethod("GetService", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(entityType, requestType, idType)
                    .Invoke(this, [catalogue]);

                var response = service!.GetType().GetMethod(methodName)!.Invoke(service, [model]);
                dynamic dynResponse = response!;
                var mapped = _mapper.Map(dynResponse.Data, entityType, responseType);

                return dynResponse.Success
                    ? Ok(new { dynResponse.Success, dynResponse.Message, Data = mapped })
                    : BadRequest(new { dynResponse.Success, dynResponse.Message, dynResponse.Errors });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, ex.Message });
            }
        }
    }
}
