using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Entities.Response;
using FinancialBudget.Server.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBudget.Server.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class BudgetController(IEntityService<Budget, BudgetRequest, long> service, IMapper mapper)
        : CrudController<Budget, BudgetRequest, BudgetResponse, long>(service, mapper);
}
