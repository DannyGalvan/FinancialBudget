using Microsoft.AspNetCore.Mvc;
using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Entities.Response;
using FinancialBudget.Server.Services.Interfaces;
using Mapster;

namespace FinancialBudget.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetItemController : ControllerBase
    {
        private readonly IEntityService<BudgetItem, BudgetItemRequest, long> _service;

        public BudgetItemController(IEntityService<BudgetItem, BudgetItemRequest, long> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public ActionResult<BudgetItemResponse> Get(long id)
        {
            var result = _service.GetById(id);
            if (!result.Success || result.Data == null)
                return NotFound(result);

            return Ok(result.Data.Adapt<BudgetItemResponse>());
        }

        [HttpGet]
        public ActionResult<IEnumerable<BudgetItemResponse>> GetAll(string? filters = null, int pageNumber = 1, int pageSize = 30, bool includeTotal = false)
        {
            var result = _service.GetAll(filters, null, pageNumber, pageSize, includeTotal);
            if (!result.Success || result.Data == null)
                return BadRequest(result);

            return Ok(result.Data.Adapt<List<BudgetItemResponse>>());
        }

        [HttpPost]
        public ActionResult<BudgetItemResponse> Create([FromBody] BudgetItemRequest request)
        {
            var result = _service.Create(request);
            if (!result.Success) return BadRequest(result.Errors);

            return CreatedAtAction(nameof(Get), new { id = result.Data!.Id }, result.Data.Adapt<BudgetItemResponse>());
        }

        [HttpPut("{id}")]
        public ActionResult<BudgetItemResponse> Update(long id, [FromBody] BudgetItemRequest request)
        {
            if (id != request.Id) return BadRequest("El Id no coincide");

            var result = _service.Update(request);
            if (!result.Success) return BadRequest(result.Errors);

            return Ok(result.Data!.Adapt<BudgetItemResponse>());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id, long deletedBy)
        {
            var result = _service.Delete(id, deletedBy);
            if (!result.Success) return NotFound(result.Errors);

            return NoContent();
        }
    }
}
