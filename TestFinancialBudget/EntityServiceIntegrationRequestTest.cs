using FinancialBudget.Server.Context;
using FinancialBudget.Server.Entities.Models;
using FinancialBudget.Server.Entities.Request;
using FinancialBudget.Server.Entities.Response;
using FinancialBudget.Server.Interceptors.Interfaces;
using FinancialBudget.Server.Interceptors.RequestInterceptors;
using FinancialBudget.Server.Services.Business;
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
    public class EntityServiceIntegrationRequestTests
    {
        [Fact]
        public void CreateRequest_ShouldPersistEntity_AndRunInterceptors()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Request_Create")
                .Options;
            var dbContext = new DataContext(options);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<EntityService<Request, RequestRequest, long>>>();
            var mockFilterTranslator = new Mock<IFilterTranslator>();
            var mockSupportService = new Mock<IEntitySupportService>();

            // Mock validator
            var mockValidator = new Mock<IValidator<RequestRequest>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<RequestRequest>()))
                .Returns(new ValidationResult());

            mockSupportService.Setup(s => s.GetValidator<RequestRequest>("Create"))
                .Returns(mockValidator.Object);

            // Mock interceptors
            var beforeCreateInterceptor = new Mock<IEntityBeforeCreateInterceptor<Request, RequestRequest>>();
            beforeCreateInterceptor.Setup(i => i.Execute(It.IsAny<Response<Request, List<ValidationFailure>>>(), It.IsAny<RequestRequest>()))
                .Returns((Response<Request, List<ValidationFailure>> resp, RequestRequest req) => resp);

            var afterCreateInterceptor = new Mock<IEntityAfterCreateInterceptor<Request, RequestRequest>>();
            afterCreateInterceptor.Setup(i => i.Execute(It.IsAny<Response<Request, List<ValidationFailure>>>(), It.IsAny<RequestRequest>()))
                .Returns((Response<Request, List<ValidationFailure>> resp, RequestRequest req) => resp);

            mockSupportService.Setup(s => s.GetBeforeCreateInterceptors<Request, RequestRequest>())
                .Returns(new List<IEntityBeforeCreateInterceptor<Request, RequestRequest>> { beforeCreateInterceptor.Object });
            mockSupportService.Setup(s => s.GetAfterCreateInterceptors<Request, RequestRequest>())
                .Returns(new List<IEntityAfterCreateInterceptor<Request, RequestRequest>> { afterCreateInterceptor.Object });

            // Mock mapeo
            mockMapper.Setup(m => m.Map<Request>(It.IsAny<RequestRequest>()))
                .Returns((RequestRequest req) => new Request
                {
                    OriginId = req.OriginId ?? 1,
                    RequestAmount = req.RequestAmount ?? 0,
                    Name = req.Name ?? "Test",
                    Reason = req.Reason ?? "Test Reason",
                    RequestDate = DateTimeOffset.UtcNow,
                    RequestStatusId = req.RequestStatusId ?? 1,
                    Email = req.Email ?? "test@email.com",
                    PriorityId = req.PriorityId ?? 1,
                    State = 1,
                    CreatedBy = req.CreatedBy ?? 1
                });

            var service = new EntityService<Request, RequestRequest, long>(
                mockMapper.Object,
                mockLogger.Object,
                dbContext,
                mockFilterTranslator.Object,
                mockSupportService.Object
            );

            var request = new RequestRequest
            {
                OriginId = 1,
                RequestAmount = 500,
                Name = "Solicitud Test",
                Reason = "Motivo Test",
                RequestStatusId = 1,
                Email = "test@email.com",
                PriorityId = 1,
                CreatedBy = 1
            };

            // Act
            var response = service.Create(request);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("Solicitud Test", response.Data.Name);
            Assert.Equal(500, response.Data.RequestAmount);

            var dbRequest = dbContext.Requests.Find(response.Data.Id);
            Assert.NotNull(dbRequest);
            Assert.Equal("Solicitud Test", dbRequest.Name);
        }

        [Fact]
        public void AuthorizeRequest_ShouldUpdateStatus_AndRunInterceptors()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Request_Authorize")
                .Options;
            var dbContext = new DataContext(options);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<EntityService<Request, RequestRequest, long>>>();
            var mockFilterTranslator = new Mock<IFilterTranslator>();
            var mockSupportService = new Mock<IEntitySupportService>();

            var mockValidator = new Mock<IValidator<RequestRequest>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<RequestRequest>()))
                .Returns(new ValidationResult());

            mockSupportService.Setup(s => s.GetValidator<RequestRequest>("Update"))
                .Returns(mockValidator.Object);

            var beforeUpdateInterceptor = new Mock<IEntityBeforeUpdateInterceptor<Request, RequestRequest>>();
            beforeUpdateInterceptor.Setup(i => i.Execute(It.IsAny<Response<Request, List<ValidationFailure>>>(), It.IsAny<RequestRequest>()))
                .Returns((Response<Request, List<ValidationFailure>> resp, RequestRequest req) => resp);

            var afterUpdateInterceptor = new Mock<IEntityAfterUpdateInterceptor<Request, RequestRequest>>();
            afterUpdateInterceptor.Setup(i => i.Execute(It.IsAny<Response<Request, List<ValidationFailure>>>(), It.IsAny<RequestRequest>(), It.IsAny<Request>()))
                .Returns((Response<Request, List<ValidationFailure>> resp, RequestRequest req, Request prev) => resp);

            mockSupportService.Setup(s => s.GetBeforeUpdateInterceptors<Request, RequestRequest>())
                .Returns(new List<IEntityBeforeUpdateInterceptor<Request, RequestRequest>> { beforeUpdateInterceptor.Object });
            mockSupportService.Setup(s => s.GetAfterUpdateInterceptors<Request, RequestRequest>())
                .Returns(new List<IEntityAfterUpdateInterceptor<Request, RequestRequest>> { afterUpdateInterceptor.Object });

            // Insertar entidad inicial
            var initialRequest = new Request
            {
                OriginId = 1,
                RequestAmount = 500,
                Name = "Solicitud Test",
                Reason = "Motivo Test",
                RequestDate = DateTimeOffset.UtcNow,
                RequestStatusId = 1,
                Email = "test@email.com",
                PriorityId = 1,
                State = 1,
                CreatedBy = 1
            };
            dbContext.Requests.Add(initialRequest);
            dbContext.SaveChanges();

            // Mock mapeo para Update
            mockMapper.Setup(m => m.Map<Request>(It.IsAny<RequestRequest>()))
                .Returns((RequestRequest req) => new Request
                {
                    Id = initialRequest.Id,
                    OriginId = req.OriginId ?? initialRequest.OriginId,
                    RequestAmount = req.RequestAmount ?? initialRequest.RequestAmount,
                    Name = req.Name ?? initialRequest.Name,
                    Reason = req.Reason ?? initialRequest.Reason,
                    RequestDate = initialRequest.RequestDate,
                    RequestStatusId = req.RequestStatusId ?? 1, // Simula autorización
                    Email = req.Email ?? initialRequest.Email,
                    PriorityId = req.PriorityId ?? initialRequest.PriorityId,
                    State = 1,
                    CreatedBy = initialRequest.CreatedBy
                });
            mockMapper.Setup(m => m.Map<Request>(It.IsAny<Request>()))
                .Returns((Request r) => r);

            var service = new EntityService<Request, RequestRequest, long>(
                mockMapper.Object,
                mockLogger.Object,
                dbContext,
                mockFilterTranslator.Object,
                mockSupportService.Object
            );

            var authorizeRequest = new RequestRequest
            {
                Id = initialRequest.Id,
                RequestStatusId = 2, // Autorizado
                Comments = "Autorizada por gerente",
            };

            // Act
            var response = service.Update(authorizeRequest);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data.RequestStatusId);

            var dbRequest = dbContext.Requests.Find(initialRequest.Id);
            Assert.NotNull(dbRequest);
            Assert.Equal(2, dbRequest.RequestStatusId);
        }

        [Fact]
        public void AuthorizeRequest_ShouldCreateTraceability_AndSendMail()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Request_Authorize_Interceptor")
                .Options;
            var dbContext = new DataContext(options);

            // Simula el servicio de correo si el interceptor lo requiere
            var mockMailService = new Mock<ISendMail>();
            var mockIAuthorizationFlow = new Mock<IAuthorizationRequestFlow>();
            var mockLogger = new Mock<ILogger<EntityService<Request, RequestRequest, long>>>();
            mockMailService.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            var authorizationFlow = new AuthorizationRequestFlow(dbContext, mockMailService.Object);
            var interceptor = new UpdateFlowTraceabilityAndSendMail(mockLogger.Object, authorizationFlow);

            var mockMapper = new Mock<IMapper>();
            var mockFilterTranslator = new Mock<IFilterTranslator>();
            var mockSupportService = new Mock<IEntitySupportService>();

            var mockValidator = new Mock<IValidator<RequestRequest>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<RequestRequest>()))
                .Returns(new ValidationResult());

            mockSupportService.Setup(s => s.GetValidator<RequestRequest>("Update"))
                .Returns(mockValidator.Object);

            // Inyecta el interceptor real en la lista
            mockSupportService.Setup(s => s.GetBeforeUpdateInterceptors<Request, RequestRequest>())
                .Returns(new List<IEntityBeforeUpdateInterceptor<Request, RequestRequest>>());
            mockSupportService.Setup(s => s.GetAfterUpdateInterceptors<Request, RequestRequest>())
                .Returns(new List<IEntityAfterUpdateInterceptor<Request, RequestRequest>> { interceptor });


            // Insertar entidad inicial
            var initialRequest = new Request
            {
                OriginId = 1,
                RequestAmount = 500,
                Name = "Solicitud Test",
                Reason = "Motivo Test",
                RequestDate = DateTimeOffset.UtcNow,
                RequestStatusId = 1,
                Email = "test@email.com",
                PriorityId = 1,
                State = 1,
                CreatedBy = 1
            };
            dbContext.Requests.Add(initialRequest);
            dbContext.SaveChanges();

            // Agrega presupuesto disponible para el año actual
            dbContext.Budgets.Add(new Budget
            {
                AuthorizedAmount = 1000,
                AvailableAmount = 1000,
                Period = DateTimeOffset.UtcNow.Year,
                State = 1,
                CreatedBy = 1,
                CreatedAt = DateTimeOffset.UtcNow
            });

            // Agrega traza activa para la solicitud
            dbContext.Traceabilities.Add(new Traceability
            {
                RequestId = initialRequest.Id,
                State = 1,
                CreatedAt = DateTimeOffset.UtcNow,
                Comments = "Inicial",
                CreateUserId = initialRequest.CreatedBy,
                AuthorizeUserId = initialRequest.CreatedBy,
                RequestStatusId = 1
            });

            dbContext.SaveChanges();


            // Mock mapeo para Update
            mockMapper.Setup(m => m.Map<Request>(It.IsAny<RequestRequest>()))
                .Returns((RequestRequest req) => new Request
                {
                    Id = initialRequest.Id,
                    OriginId = req.OriginId ?? initialRequest.OriginId,
                    RequestAmount = req.RequestAmount ?? initialRequest.RequestAmount,
                    Name = req.Name ?? initialRequest.Name,
                    Reason = req.Reason ?? initialRequest.Reason,
                    RequestDate = initialRequest.RequestDate,
                    RequestStatusId = 1, // Simula autorización
                    Email = req.Email ?? initialRequest.Email,
                    PriorityId = req.PriorityId ?? initialRequest.PriorityId,
                    State = 1,
                    CreatedBy = initialRequest.CreatedBy
                });
            mockMapper.Setup(m => m.Map<Request>(It.IsAny<Request>()))
                .Returns((Request r) => r);

            var service = new EntityService<Request, RequestRequest, long>(
                mockMapper.Object,
                mockLogger.Object,
                dbContext,
                mockFilterTranslator.Object,
                mockSupportService.Object
            );

            var authorizeRequest = new RequestRequest
            {
                Id = initialRequest.Id,
                RequestStatusId = 2, // Autorizado
                Comments = "Autorizada por gerente",
            };

            // Act
            var response = service.Update(authorizeRequest);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(1, response.Data.RequestStatusId);

            // Verifica que se creó la trazabilidad
            var traceabilities = dbContext.Traceabilities.Where(t => t.RequestId == initialRequest.Id).ToList();
            Assert.True(traceabilities.Count > 0);

            // Verifica que se llamó al servicio de correo
            mockMailService.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void PartialUpdateRequest_ShouldModifyEntity_AndRunInterceptors()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Request_PartialUpdate")
                .Options;
            var dbContext = new DataContext(options);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<EntityService<Request, RequestRequest, long>>>();
            var mockFilterTranslator = new Mock<IFilterTranslator>();
            var mockSupportService = new Mock<IEntitySupportService>();

            var mockValidator = new Mock<IValidator<RequestRequest>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<RequestRequest>()))
                .Returns(new ValidationResult());

            mockSupportService.Setup(s => s.GetValidator<RequestRequest>("Partial"))
                .Returns(mockValidator.Object);

            var afterPartialUpdateInterceptor = new Mock<IEntityAfterPartialUpdateInterceptor<Request, RequestRequest>>();
            afterPartialUpdateInterceptor.Setup(i => i.Execute(It.IsAny<Response<Request, List<ValidationFailure>>>(), It.IsAny<RequestRequest>(), It.IsAny<Request>()))
                .Returns((Response<Request, List<ValidationFailure>> resp, RequestRequest req, Request prev) => resp);

            mockSupportService.Setup(s => s.GetAfterPartialUpdateInterceptors<Request, RequestRequest>())
                .Returns(new List<IEntityAfterPartialUpdateInterceptor<Request, RequestRequest>> { afterPartialUpdateInterceptor.Object });

            // Insertar entidad inicial
            var initialRequest = new Request
            {
                OriginId = 1,
                RequestAmount = 500,
                Name = "Solicitud Test",
                Reason = "Motivo Test",
                RequestDate = DateTimeOffset.UtcNow,
                RequestStatusId = 1,
                Email = "test@email.com",
                PriorityId = 1,
                State = 1,
                CreatedBy = 1
            };
            dbContext.Requests.Add(initialRequest);
            dbContext.SaveChanges();

            // Mock mapeo para PartialUpdate
            mockMapper.Setup(m => m.Map<Request>(It.IsAny<RequestRequest>()))
                .Returns((RequestRequest req) => new Request
                {
                    Id = initialRequest.Id,
                    OriginId = req.OriginId ?? initialRequest.OriginId,
                    RequestAmount = req.RequestAmount ?? initialRequest.RequestAmount,
                    Name = req.Name ?? initialRequest.Name,
                    Reason = req.Reason ?? initialRequest.Reason,
                    RequestDate = initialRequest.RequestDate,
                    RequestStatusId = req.RequestStatusId ?? initialRequest.RequestStatusId,
                    Email = req.Email ?? initialRequest.Email,
                    PriorityId = req.PriorityId ?? initialRequest.PriorityId,
                    State = 1,
                    CreatedBy = initialRequest.CreatedBy
                });
            mockMapper.Setup(m => m.Map<Request>(It.IsAny<Request>()))
                .Returns((Request r) => r);

            var service = new EntityService<Request, RequestRequest, long>(
                mockMapper.Object,
                mockLogger.Object,
                dbContext,
                mockFilterTranslator.Object,
                mockSupportService.Object
            );

            var partialUpdateRequest = new RequestRequest
            {
                Id = initialRequest.Id,
                Reason = "Motivo Modificado"
            };

            // Act
            var response = service.PartialUpdate(partialUpdateRequest);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("Motivo Modificado", response.Data.Reason);

            var dbRequest = dbContext.Requests.Find(initialRequest.Id);
            Assert.NotNull(dbRequest);
            Assert.Equal("Motivo Modificado", dbRequest.Reason);
        }

        [Fact]
        public void DeleteRequest_ShouldSetStateToZero()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Request_Delete")
                .Options;
            var dbContext = new DataContext(options);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<EntityService<Request, RequestRequest, long>>>();
            var mockFilterTranslator = new Mock<IFilterTranslator>();
            var mockSupportService = new Mock<IEntitySupportService>();

            // Insertar entidad inicial
            var initialRequest = new Request
            {
                OriginId = 1,
                RequestAmount = 500,
                Name = "Solicitud Test",
                Reason = "Motivo Test",
                RequestDate = DateTimeOffset.UtcNow,
                RequestStatusId = 1,
                Email = "test@email.com",
                PriorityId = 1,
                State = 1,
                CreatedBy = 1
            };
            dbContext.Requests.Add(initialRequest);
            dbContext.SaveChanges();

            var service = new EntityService<Request, RequestRequest, long>(
                mockMapper.Object,
                mockLogger.Object,
                dbContext,
                mockFilterTranslator.Object,
                mockSupportService.Object
            );

            // Act
            var response = service.Delete(initialRequest.Id, 99);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(0, response.Data.State);

            var dbRequest = dbContext.Requests.Find(initialRequest.Id);
            Assert.NotNull(dbRequest);
            Assert.Equal(0, dbRequest.State);
            Assert.Equal(99, dbRequest.UpdatedBy);
        }

        [Fact]
        public void GetByIdRequest_ShouldReturnEntity_WhenExists()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Request_GetById")
                .Options;
            var dbContext = new DataContext(options);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<EntityService<Request, RequestRequest, long>>>();
            var mockFilterTranslator = new Mock<IFilterTranslator>();
            var mockSupportService = new Mock<IEntitySupportService>();

            // Insertar entidad inicial
            var initialRequest = new Request
            {
                OriginId = 1,
                RequestAmount = 500,
                Name = "Solicitud Test",
                Reason = "Motivo Test",
                RequestDate = DateTimeOffset.UtcNow,
                RequestStatusId = 1,
                Email = "test@email.com",
                PriorityId = 1,
                State = 1,
                CreatedBy = 1
            };
            dbContext.Requests.Add(initialRequest);
            dbContext.SaveChanges();

            var service = new EntityService<Request, RequestRequest, long>(
                mockMapper.Object,
                mockLogger.Object,
                dbContext,
                mockFilterTranslator.Object,
                mockSupportService.Object
            );

            // Act
            var response = service.GetById(initialRequest.Id);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(initialRequest.Id, response.Data.Id);
            Assert.Equal("Solicitud Test", response.Data.Name);
        }
    }
}