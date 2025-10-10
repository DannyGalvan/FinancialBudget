using FinancialBudget.Server.Context;
using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Interceptors.Interfaces;
using FinancialBudget.Server.Services.Core;
using FinancialBudget.Server.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestFinancialBudget
{
    public class EntityServiceIntegrationTests
    {
        [Fact]
        public void CreateBudget_ShouldPersistEntity_WhenValid()
        {
            // Arrange: contexto en memoria y dependencias simuladas
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var dbContext = new DataContext(options);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<EntityService<Budget, BudgetRequest, long>>>();
            var mockFilterTranslator = new Mock<IFilterTranslator>();
            var mockSupportService = new Mock<IEntitySupportService>();

            // Simular validación exitosa
            var mockValidator = new Mock<IValidator<BudgetRequest>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<BudgetRequest>()))
                .Returns(new ValidationResult());

            mockSupportService.Setup(s => s.GetValidator<BudgetRequest>("Create"))
                .Returns(mockValidator.Object);

            mockSupportService.Setup(s => s.GetBeforeCreateInterceptors<Budget, BudgetRequest>())
                .Returns(new List<IEntityBeforeCreateInterceptor<Budget, BudgetRequest>>());
            mockSupportService.Setup(s => s.GetAfterCreateInterceptors<Budget, BudgetRequest>())
                .Returns(new List<IEntityAfterCreateInterceptor<Budget, BudgetRequest>>());

            // Simular mapeo de BudgetRequest a Budget
            mockMapper.Setup(m => m.Map<Budget>(It.IsAny<BudgetRequest>()))
                .Returns((BudgetRequest req) => new Budget
                {
                    AuthorizedAmount = req.AuthorizedAmount ?? 0,
                    AvailableAmount = req.AvailableAmount ?? 0,
                    Period = req.Period ?? 0,
                    CreatedBy = req.CreatedBy ?? 1,
                    State = 1,
                    CommittedAmount = 0
                });

            var service = new EntityService<Budget, BudgetRequest, long>(
                mockMapper.Object,
                mockLogger.Object,
                dbContext,
                mockFilterTranslator.Object,
                mockSupportService.Object
            );

            var request = new BudgetRequest
            {
                AuthorizedAmount = 1000,
                AvailableAmount = 1000,
                Period = 2025,
                CreatedBy = 1
            };

            // Act
            var response = service.Create(request);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(1000, response.Data.AuthorizedAmount);
            Assert.Equal(2025, response.Data.Period);

            // Verifica que se guardó en la base de datos
            var dbBudget = dbContext.Budgets.Find(response.Data.Id);
            Assert.NotNull(dbBudget);
            Assert.Equal(1000, dbBudget.AuthorizedAmount);
            Assert.Equal(1000, dbBudget.AvailableAmount);
            Assert.Equal(2025, dbBudget.Period);
            Assert.Equal(1, dbBudget.State);
        }

        [Fact]
        public void UpdateBudget_ShouldModifyEntity_WhenValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Update")
                .Options;
            var dbContext = new DataContext(options);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<EntityService<Budget, BudgetRequest, long>>>();
            var mockFilterTranslator = new Mock<IFilterTranslator>();
            var mockSupportService = new Mock<IEntitySupportService>();

            var mockValidator = new Mock<IValidator<BudgetRequest>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<BudgetRequest>()))
                .Returns(new ValidationResult());

            mockSupportService.Setup(s => s.GetValidator<BudgetRequest>("Update"))
                .Returns(mockValidator.Object);

            mockSupportService.Setup(s => s.GetBeforeUpdateInterceptors<Budget, BudgetRequest>())
                .Returns(new List<IEntityBeforeUpdateInterceptor<Budget, BudgetRequest>>());
            mockSupportService.Setup(s => s.GetAfterUpdateInterceptors<Budget, BudgetRequest>())
                .Returns(new List<IEntityAfterUpdateInterceptor<Budget, BudgetRequest>>());

            // Insertar entidad inicial
            var initialBudget = new Budget
            {
                AuthorizedAmount = 500,
                AvailableAmount = 500,
                Period = 2024,
                CreatedBy = 1,
                State = 1,
                CommittedAmount = 0
            };
            dbContext.Budgets.Add(initialBudget);
            dbContext.SaveChanges();

            // Simular mapeo para Update
            mockMapper.Setup(m => m.Map<Budget>(It.IsAny<BudgetRequest>()))
                .Returns((BudgetRequest req) => new Budget
                {
                    Id = initialBudget.Id,
                    AuthorizedAmount = req.AuthorizedAmount ?? 0,
                    AvailableAmount = req.AvailableAmount ?? 0,
                    Period = req.Period ?? 0,
                    CreatedBy = req.CreatedBy ?? 1,
                    State = 1,
                    CommittedAmount = 0
                });
            mockMapper.Setup(m => m.Map<Budget>(It.IsAny<Budget>()))
                .Returns((Budget b) => b);

            var service = new EntityService<Budget, BudgetRequest, long>(
                mockMapper.Object,
                mockLogger.Object,
                dbContext,
                mockFilterTranslator.Object,
                mockSupportService.Object
            );

            var updateRequest = new BudgetRequest
            {
                Id = initialBudget.Id,
                AuthorizedAmount = 800,
                AvailableAmount = 800,
                Period = 2025,
                CreatedBy = 1
            };

            // Act
            var response = service.Update(updateRequest);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(800, response.Data.AuthorizedAmount);
            Assert.Equal(2025, response.Data.Period);

            var dbBudget = dbContext.Budgets.Find(initialBudget.Id);
            Assert.NotNull(dbBudget);
            Assert.Equal(800, dbBudget.AuthorizedAmount);
            Assert.Equal(2025, dbBudget.Period);
        }

        [Fact]
        public void PartialUpdateBudget_ShouldModifyEntity_WhenValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_PartialUpdate")
                .Options;
            var dbContext = new DataContext(options);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<EntityService<Budget, BudgetRequest, long>>>();
            var mockFilterTranslator = new Mock<IFilterTranslator>();
            var mockSupportService = new Mock<IEntitySupportService>();

            var mockValidator = new Mock<IValidator<BudgetRequest>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<BudgetRequest>()))
                .Returns(new ValidationResult());

            mockSupportService.Setup(s => s.GetValidator<BudgetRequest>("Partial"))
                .Returns(mockValidator.Object);

            mockSupportService.Setup(s => s.GetAfterPartialUpdateInterceptors<Budget, BudgetRequest>())
                .Returns(new List<IEntityAfterPartialUpdateInterceptor<Budget, BudgetRequest>>());

            // Insertar entidad inicial
            var initialBudget = new Budget
            {
                AuthorizedAmount = 500,
                AvailableAmount = 500,
                Period = 2024,
                CreatedBy = 1,
                State = 1,
                CommittedAmount = 0
            };
            dbContext.Budgets.Add(initialBudget);
            dbContext.SaveChanges();

            // Simular mapeo para PartialUpdate
            mockMapper.Setup(m => m.Map<Budget>(It.IsAny<BudgetRequest>()))
                .Returns((BudgetRequest req) => new Budget
                {
                    Id = initialBudget.Id,
                    AuthorizedAmount = req.AuthorizedAmount ?? initialBudget.AuthorizedAmount,
                    AvailableAmount = req.AvailableAmount ?? initialBudget.AvailableAmount,
                    Period = req.Period ?? initialBudget.Period,
                    CreatedBy = req.CreatedBy ?? initialBudget.CreatedBy,
                    State = 1,
                    CommittedAmount = 0
                });
            mockMapper.Setup(m => m.Map<Budget>(It.IsAny<Budget>()))
                .Returns((Budget b) => b);

            var service = new EntityService<Budget, BudgetRequest, long>(
                mockMapper.Object,
                mockLogger.Object,
                dbContext,
                mockFilterTranslator.Object,
                mockSupportService.Object
            );

            var partialUpdateRequest = new BudgetRequest
            {
                Id = initialBudget.Id,
                AuthorizedAmount = 900 // Solo actualiza este campo
            };

            // Act
            var response = service.PartialUpdate(partialUpdateRequest);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(900, response.Data.AuthorizedAmount);
            Assert.Equal(500, response.Data.AvailableAmount); // No se actualizó
            Assert.Equal(2024, response.Data.Period);

            var dbBudget = dbContext.Budgets.Find(initialBudget.Id);
            Assert.NotNull(dbBudget);
            Assert.Equal(900, dbBudget.AuthorizedAmount);
        }

        [Fact]
        public void DeleteBudget_ShouldSetStateToZero_WhenValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Delete")
                .Options;
            var dbContext = new DataContext(options);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<EntityService<Budget, BudgetRequest, long>>>();
            var mockFilterTranslator = new Mock<IFilterTranslator>();
            var mockSupportService = new Mock<IEntitySupportService>();

            // Insertar entidad inicial
            var initialBudget = new Budget
            {
                AuthorizedAmount = 500,
                AvailableAmount = 500,
                Period = 2024,
                CreatedBy = 1,
                State = 1,
                CommittedAmount = 0
            };
            dbContext.Budgets.Add(initialBudget);
            dbContext.SaveChanges();

            var service = new EntityService<Budget, BudgetRequest, long>(
                mockMapper.Object,
                mockLogger.Object,
                dbContext,
                mockFilterTranslator.Object,
                mockSupportService.Object
            );

            // Act
            var response = service.Delete(initialBudget.Id, 99);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(0, response.Data.State);

            var dbBudget = dbContext.Budgets.Find(initialBudget.Id);
            Assert.NotNull(dbBudget);
            Assert.Equal(0, dbBudget.State);
            Assert.Equal(99, dbBudget.UpdatedBy);
        }

        [Fact]
        public void GetByIdBudget_ShouldReturnEntity_WhenExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_GetById")
                .Options;
            var dbContext = new DataContext(options);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<EntityService<Budget, BudgetRequest, long>>>();
            var mockFilterTranslator = new Mock<IFilterTranslator>();
            var mockSupportService = new Mock<IEntitySupportService>();

            // Insertar entidad inicial
            var initialBudget = new Budget
            {
                AuthorizedAmount = 500,
                AvailableAmount = 500,
                Period = 2024,
                CreatedBy = 1,
                State = 1,
                CommittedAmount = 0
            };
            dbContext.Budgets.Add(initialBudget);
            dbContext.SaveChanges();

            var service = new EntityService<Budget, BudgetRequest, long>(
                mockMapper.Object,
                mockLogger.Object,
                dbContext,
                mockFilterTranslator.Object,
                mockSupportService.Object
            );

            // Act
            var response = service.GetById(initialBudget.Id);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(initialBudget.Id, response.Data.Id);
            Assert.Equal(500, response.Data.AuthorizedAmount);
        }
    }
}