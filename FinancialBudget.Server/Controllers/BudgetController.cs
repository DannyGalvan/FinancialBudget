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
    /// Defines the <see cref="BudgetController" />
    /// </summary>
    [Route("api/v1/[controller]")]
    public class BudgetController(IEntityService<Budget, BudgetRequest, long> service, IMapper mapper)
        : CrudController<Budget, BudgetRequest, BudgetResponse, long>(service, mapper)
    {
        /// <summary>
        /// Obtener todos los presupuestos
        /// </summary>
        /// <param name="query">The query<see cref="QueryParamsRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet]
        [Authorize(Policy = "Budget.List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<BudgetResponse>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult GetAll([FromQuery] QueryParamsRequest query)
        {
            return base.GetAll(query);
        }

        /// <summary>
        /// Obtener un presupuesto por id
        /// </summary>
        /// <param name="id">The id<see cref="long"/></param>
        /// <param name="include">The include<see cref="string?"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet("{id:long}")]
        [Authorize(Policy = "Budget.List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<BudgetResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Get(long id, [FromQuery] string? include = null)
        {
            return base.Get(id, include);
        }

        /// <summary>
        /// Crear un nuevo presupuesto
        /// </summary>
        /// <param name="request">The request<see cref="BudgetRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpPost]
        [Authorize(Policy = "Budget.Create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<BudgetResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Create([FromBody] BudgetRequest request)
        {
            return base.Create(request);
        }

        /// <summary>
        /// Actualizar un presupuesto
        /// </summary>
        /// <param name="request">The request<see cref="BudgetRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpPut]
        [Authorize(Policy = "Budget.Update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<BudgetResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Update([FromBody] BudgetRequest request)
        {
            return base.Update(request);
        }

        /// <summary>
        /// Actualizar parcialmente un presupuesto
        /// </summary>
        /// <param name="request">The request<see cref="BudgetRequest"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpPatch]
        [Authorize(Policy = "Budget.Update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<BudgetResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult PartialUpdate([FromBody] BudgetRequest request)
        {
            return base.PartialUpdate(request);
        }

        /// <summary>
        /// Borrar un presupuesto
        /// </summary>
        /// <param name="id">The id<see cref="long"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpDelete]
        [Authorize(Policy = "Budget.Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<BudgetResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<List<ValidationFailure>>))]
        public override IActionResult Delete([FromQuery] long id)
        {
            return base.Delete(id);
        }
    }
}
