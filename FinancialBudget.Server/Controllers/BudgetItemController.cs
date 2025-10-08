using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Entities.Response;
using FinancialBudget.Server.Services.Interfaces;
using FluentValidation.Results;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBudget.Server.Controllers
{
    /// <summary>
    /// Defines the <see cref="BudgetItemController" />
    /// </summary>
    [Route("api/v1/[controller]")]
    public class BudgetItemController(IEntityService<BudgetItem, BudgetItemRequest, long> service, IMapper mapper)
        : CrudController<BudgetItem, BudgetItemRequest, BudgetItemResponse, long>(service, mapper)
    {
        /// <summary>
        /// Obtener todos las partidas presupuestarias
        /// </summary>
        /// <param name="query">The query<see cref="QueryParamsRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet]
        [Authorize(Policy = "BudgetItem.List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<BudgetItemResponse>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult GetAll([FromQuery] QueryParamsRequest query)
        {
            return base.GetAll(query);
        }

        /// <summary>
        /// Obtener una partida presupuestaria por id
        /// </summary>
        /// <param name="id">The id<see cref="long"/></param>
        /// <param name="include">The include<see cref="string?"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet("{id:long}")]
        [Authorize(Policy = "BudgetItem.List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<BudgetItemResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Get(long id, [FromQuery] string? include = null)
        {
            return base.Get(id, include);
        }

        /// <summary>
        /// Crear un nueva partida presupuestaria
        /// </summary>
        /// <param name="request">The request<see cref="BudgetItemRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpPost]
        [Authorize(Policy = "BudgetItem.Create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<BudgetItemResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Create([FromBody] BudgetItemRequest request)
        {
            return base.Create(request);
        }

        /// <summary>
        /// Actualizar una partida presupuestaria
        /// </summary>
        /// <param name="request">The request<see cref="BudgetItemRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpPut]
        [Authorize(Policy = "BudgetItem.Update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<BudgetItemResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Update([FromBody] BudgetItemRequest request)
        {
            return base.Update(request);
        }

        /// <summary>
        /// Actualizar parcialmente una partida presupuestaria
        /// </summary>
        /// <param name="request">The request<see cref="BudgetItemRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpPatch]
        [Authorize(Policy = "BudgetItem.Update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<BudgetItemResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult PartialUpdate([FromBody] BudgetItemRequest request)
        {
            return base.PartialUpdate(request);
        }

        /// <summary>
        /// Borrar una partida presupuestaria
        /// </summary>
        /// <param name="id">The id<see cref="long"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpDelete]
        [Authorize(Policy = "BudgetItem.Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<BudgetItemResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Delete([FromQuery] long id)
        {
            return base.Delete(id);
        }
    }
}
