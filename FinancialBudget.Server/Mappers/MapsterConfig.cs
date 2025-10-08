using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Entities.Response;
using Mapster;

namespace FinancialBudget.Server.Mappers
{
    public abstract class MapsterConfig
    {

        public static void RegisterMappings()
        {

            MapperBudget();
            MapperModules();
            MapperUser();
            MapperCatalogues();
            MapperRequest();
            MapperBudgetItem();
        }

        private static void MapperBudgetItem()
        {
            TypeAdapterConfig<BudgetItemRequest, BudgetItem>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.BudgetId, src => src.BudgetId)
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.OriginId, src => src.OriginId)
                .Map(dest => dest.SplitTypeId, src => src.SplitTypeId)
                .Map(dest => dest.RequestId, src => src.RequestId)
                .Map(dest => dest.State, src => 1)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy);

            TypeAdapterConfig<BudgetItem, BudgetItemResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.BudgetId, src => src.BudgetId)
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.OriginId, src => src.OriginId)
                .Map(dest => dest.SplitTypeId, src => src.SplitTypeId)
                .Map(dest => dest.RequestId, src => src.RequestId)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : null)
                .Map(dest => dest.Budget, src => src.Budget != null ? src.Budget.Adapt<BudgetResponse>() : null)
                .Map(dest => dest.Origin, src => src.Origin != null ? src.Origin.Adapt<CatalogueResponse>() : null)
                .Map(dest => dest.SplitType, src => src.SplitType != null ? src.SplitType.Adapt<CatalogueResponse>() : null)
                .Map(dest => dest.Request, src => src.Request != null ? src.Request.Adapt<RequestResponse>() : null);

            TypeAdapterConfig<BudgetItem, BudgetItem>.NewConfig();
        }

        private static void MapperRequest()
        {
            TypeAdapterConfig<RequestRequest, Request>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Reason, src => src.Reason)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.OriginId, src => src.OriginId)
                .Map(dest => dest.RequestAmount, src => src.RequestAmount)
                .Map(dest => dest.RequestStatusId, src => src.RequestStatusId ?? 1)
                .Map(dest => dest.PriorityId, src => src.PriorityId)
                .Map(dest => dest.RequestDate, src => string.IsNullOrEmpty(src.RequestDate) ? DateTime.Now : DateTime.ParseExact(src.RequestDate, "yyyy-MM-dd", null))
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.State, src => 1)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy);

            TypeAdapterConfig<Request, RequestResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Reason, src => src.Reason)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.OriginId, src => src.OriginId)
                .Map(dest => dest.RequestAmount, src => src.RequestAmount)
                .Map(dest => dest.RequestStatusId, src => src.RequestStatusId)
                .Map(dest => dest.PriorityId, src => src.PriorityId)
                .Map(dest => dest.RequestDate, src => src.RequestDate.ToString("dd/MM/yyyy"))
                .Map(dest => dest.ApprovedDate, src => src.ApprovedDate.HasValue ? src.ApprovedDate.Value.ToString("dd/MM/yyyy") : null)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.RejectionReason, src => src.RejectionReason)
                .Map(dest => dest.AuthorizedReason, src => src.AuthorizedReason)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : null)
                .Map(dest => dest.Origin, src => src.Origin != null ? src.Origin.Adapt<CatalogueResponse>() : null)
                .Map(dest => dest.RequestStatus, src => src.RequestStatus != null ? src.RequestStatus.Adapt<CatalogueResponse>() : null)
                .Map(dest => dest.Priority, src => src.Priority != null ? src.Priority.Adapt<CatalogueResponse>() : null);

            TypeAdapterConfig<Request, Request>.NewConfig();

        }

        private static void MapperCatalogues()
        {
            TypeAdapterConfig<CatalogueRequest, Origin>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.State, src => 1)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy);

            TypeAdapterConfig<Origin, CatalogueResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : null);

            TypeAdapterConfig<Origin, Origin>.NewConfig();

            TypeAdapterConfig<CatalogueRequest, SplitType>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.State, src => 1)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy);

            TypeAdapterConfig<SplitType, CatalogueResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : null);

            TypeAdapterConfig<SplitType, SplitType>.NewConfig();

            TypeAdapterConfig<CatalogueRequest, RequestStatus>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.State, src => 1)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy);

            TypeAdapterConfig<RequestStatus, CatalogueResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : null);

            TypeAdapterConfig<RequestStatus, RequestStatus>.NewConfig();

            TypeAdapterConfig<CatalogueRequest, Priority>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.State, src => 1)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy);

            TypeAdapterConfig<Priority, CatalogueResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : null);

            TypeAdapterConfig<Priority, Priority>.NewConfig();
        }

        private static void MapperBudget()
        {
            TypeAdapterConfig<BudgetRequest, Budget>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.AuthorizedAmount, src => src.AuthorizedAmount)
                .Map(dest => dest.AvailableAmount, src => src.AvailableAmount)
                .Map(dest => dest.CommittedAmount, src => 0)
                .Map(dest => dest.Period, src => src.Period)
                .Map(dest => dest.State, src => 1)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy);

            TypeAdapterConfig<Budget, BudgetResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.AuthorizedAmount, src => src.AuthorizedAmount)
                .Map(dest => dest.AvailableAmount, src => src.AvailableAmount)
                .Map(dest => dest.CommittedAmount, src => src.CommittedAmount)
                .Map(dest => dest.Period, src => src.Period)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : null);

            TypeAdapterConfig<Budget, Budget>.NewConfig();
        }

        private static void MapperUser()
        {
            //Mapper User
            TypeAdapterConfig<RegisterRequest, User>.NewConfig()
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.Password, src => src.Password)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.UpdatedAt!);

            TypeAdapterConfig<User, UserResponse>.NewConfig()
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.RolId, src => src.RolId)
                .Map(dest => dest.Rol, src => src.Rol)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy " +
                    "HH:mm:ss") : null);

            TypeAdapterConfig<User, AuthResponse>.NewConfig()
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.UserId, src => src.Id)
                .Map(dest => dest.Rol, src => src.RolId)
                .Ignore(dest => dest.Token)
                .Ignore(dest => dest.Operations);

            TypeAdapterConfig<User, User>.NewConfig();
        }

        private static void MapperModules()
        {
            //Mapper Rol
            TypeAdapterConfig<Rol, RolResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Users, src => src.Users)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.RolOperations, src => src.RolOperations)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : null);

            //Mapper RolOperation
            TypeAdapterConfig<RolOperation, RolOperationResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.OperationId, src => src.OperationId)
                .Map(dest => dest.Operation, src => src.Operation)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.RolId, src => src.RolId)
                .Map(dest => dest.Rol, src => src.Rol)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : null);

            TypeAdapterConfig<RolOperation, Operation>.NewConfig()
                .Map(dest => dest.Id, src => src.OperationId)
                .Map(dest => dest.Name, src => src.Operation!.Name)
                .Map(dest => dest.Policy, src => src.Operation!.Policy)
                .Map(dest => dest.Icon, src => src.Operation!.Icon)
                .Map(dest => dest.Path, src => src.Operation!.Path)
                .Map(dest => dest.ModuleId, src => src.Operation!.ModuleId)
                .Map(dest => dest.IsVisible, src => src.Operation!.IsVisible)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.CreatedBy, src => src.Operation!.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.Operation!.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.Operation!.CreatedAt)
                .Map(dest => dest.UpdatedAt, src => src.Operation!.UpdatedAt)
                .Ignore(dest => dest.RolOperations);

            //Mapper Operation
            TypeAdapterConfig<Operation, OperationResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Policy, src => src.Policy)
                .Map(dest => dest.Icon, src => src.Icon)
                .Map(dest => dest.Path, src => src.Path)
                .Map(dest => dest.ModuleId, src => src.ModuleId)
                .Map(dest => dest.IsVisible, src => src.IsVisible)
                .Map(dest => dest.RolOperations, src => src.RolOperations)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : null);

            //Mapper Module
            TypeAdapterConfig<Module, ModuleResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Image, src => src.Image)
                .Map(dest => dest.Path, src => src.Path)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.Operations, src => src.Operations)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : null);
        }


    }
}