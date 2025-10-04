namespace FinancialBudget.Server.Controllers
{
    using FinancialBudget.Server.Entities.Models;
    using FinancialBudget.Server.Entities.Request;
    using FinancialBudget.Server.Entities.Response;
    using FinancialBudget.Server.Services.Interfaces;
    using FluentValidation.Results;
    using MapsterMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Defines the <see cref="RequestController" />
    /// </summary>
    [Authorize]
    [Route("api/v1/[controller]")]
    public class RequestController(IEntityService<Request, RequestRequest, long> service, IMapper mapper)
        : CrudController<Request, RequestRequest, RequestResponse, long>(service, mapper)
    {
        /// <summary>
        ///  Obtener todas las solicitudes
        /// </summary>
        /// <param name="query">The query<see cref="QueryParamsRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<RequestResponse>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult GetAll([FromQuery] QueryParamsRequest query)
        {
            return base.GetAll(query);
        }

        /// <summary>
        /// Obtener una solicitud por id
        /// </summary>
        /// <param name="id">The id<see cref="long"/></param>
        /// <param name="include">The include<see cref="string?"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<RequestResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Get(long id, [FromQuery] string? include = null)
        {
            return base.Get(id, include);
        }

        /// <summary>
        /// Crear una nueva solicitud
        /// </summary>
        /// <param name="request">The request<see cref="RequestRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<RequestResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Create([FromBody] RequestRequest request)
        {
            return base.Create(request);
        }

        /// <summary>
        /// Actualizar una solicitud
        /// </summary>
        /// <param name="request">The request<see cref="RequestRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<RequestResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Update([FromBody] RequestRequest request)
        {
            return base.Update(request);
        }

        /// <summary>
        /// Realizar una actualización parcial de una solicitud
        /// </summary>
        /// <param name="request">The request<see cref="RequestRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<RequestResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult PartialUpdate([FromBody] RequestRequest request)
        {
            return base.PartialUpdate(request);
        }

        /// <summary>
        /// Borrar una solicitud
        /// </summary>
        /// <param name="id">The id<see cref="long"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<RequestResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Delete([FromQuery] long id)
        {
            return base.Delete(id);
        }
    }
}
