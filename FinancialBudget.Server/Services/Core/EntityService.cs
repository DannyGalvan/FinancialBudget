﻿using FinancialBudget.Server.Context;
using FinancialBudget.Server.Entities.Interfaces;
using FinancialBudget.Server.Entities.Response;
using FinancialBudget.Server.Services.Interfaces;
using FinancialBudget.Server.Utils;
using FluentValidation.Results;
using Lombok.NET;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace FinancialBudget.Server.Services.Core
{
    /// <summary>
    /// Defines the <see cref="EntityService{TEntity, TRequest, TId}" />
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TId"></typeparam>
    [AllArgsConstructor]
    public partial class EntityService<TEntity, TRequest, TId> : IEntityService<TEntity, TRequest, TId> where TEntity : class, IEntity<TId>
    {
        /// <summary>
        /// Defines the _mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<EntityService<TEntity, TRequest, TId>> _logger;

        /// <summary>
        /// Defines the _db
        /// </summary>
        private readonly DataContext _db;

        /// <summary>
        /// Defines the _filterTranslator
        /// </summary>
        private readonly IFilterTranslator _filterTranslator;

        /// <summary>
        /// Defines the _entitySupportService
        /// </summary>
        private readonly IEntitySupportService _entitySupportService;

        /// <summary>
        /// The GetAll
        /// </summary>
        /// <param name="filters">The filters<see cref="string"/></param>
        /// <param name="includes">The includes<see cref="string[]?"/></param>
        /// <param name="pageNumber">The pageNumber<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <param name="includeTotal">The pageSize<see cref="bool"/></param>
        /// <returns>The <see cref="Response{List{TEntity}, List{ValidationFailure}}"/></returns>
        public Response<List<TEntity>, List<ValidationFailure>> GetAll(string? filters, string[]? includes = null, int pageNumber = 1, int pageSize = 30, bool includeTotal = false)
        {
            Response<List<TEntity>, List<ValidationFailure>> response = new();

            try
            {
                IQueryable<TEntity> query = _db.Set<TEntity>();

                // 🔒 Filtrar automáticamente registros eliminados lógicamente
                var parameter = Expression.Parameter(typeof(TEntity), "e");
                var stateProp = Expression.Property(parameter, "State");
                var condition = Expression.NotEqual(stateProp, Expression.Constant(0));
                var lambda = Expression.Lambda<Func<TEntity, bool>>(condition, parameter);
                query = query.Where(lambda);

                if (!string.IsNullOrEmpty(filters))
                {
                    var filterExpression = _filterTranslator.TranslateToEfFilter<TEntity>(filters);
                    query = query.Where(filterExpression);
                }

                if (includes is { Length: > 0 })
                {
                    try
                    {
                        query = query.ApplyIncludes(includes);
                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Message = $"Error en Include: {ex.Message}";
                        response.Errors = [new ValidationFailure("Include", ex.Message)];
                        return response;
                    }
                }

                query = query.OrderByDescending(e => e.CreatedAt);

                int skip = (pageNumber - 1) * pageSize;
                var pagedData = query.Skip(skip).Take(pageSize + 1).AsNoTracking().ToList();

                response.Data = pagedData.Take(pageSize).ToList();

                // Si el cliente quiere saber el total real
                if (includeTotal)
                {
                    response.TotalResults = query.Count(); // costo adicional
                }
                else
                {
                    response.TotalResults = skip + response.Data.Count + (pagedData.Count > pageSize ? 1 : 0); // estimado
                }

                response.Success = true;
                response.Message = $"Entities {typeof(TEntity).Name} retrieved successfully";

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Errors = [new ValidationFailure("Id", ex.Message)];
                response.Data = null;

                _logger.LogError(ex, "Error al obtener {entity} : {message}", typeof(TEntity).Name, ex.Message);

                return response;
            }
        }

        /// <summary>
        /// The GetAll
        /// </summary>
        /// <param name="filters">The filters<see cref="string"/></param>
        /// <param name="includes">The includes<see cref="string[]?"/></param>
        /// <param name="pageNumber">The pageNumber<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <param name="includeTotal">The pageSize<see cref="bool"/></param>
        /// <returns>The <see cref="Response{List{TEntity}, List{ValidationFailure}}"/></returns>
        public Response<List<TEntity>, List<ValidationFailure>> GetAllWhitOutMetadata(string? filters, string[]? includes = null, int pageNumber = 1, int pageSize = 30, bool includeTotal = false)
        {
            Response<List<TEntity>, List<ValidationFailure>> response = new();

            try
            {
                IQueryable<TEntity> query = _db.Set<TEntity>();

                if (!string.IsNullOrEmpty(filters))
                {
                    var filterExpression = _filterTranslator.TranslateToEfFilter<TEntity>(filters);
                    query = query.Where(filterExpression);
                }

                if (includes is { Length: > 0 })
                {
                    try
                    {
                        query = query.ApplyIncludes(includes);
                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Message = $"Error en Include: {ex.Message}";
                        response.Errors = [new ValidationFailure("Include", ex.Message)];
                        return response;
                    }
                }

                query = query.OrderByDescending(e => e.CreatedAt);

                int skip = (pageNumber - 1) * pageSize;
                var pagedData = query.Skip(skip).Take(pageSize + 1).AsNoTracking().ToList();

                response.Data = pagedData.Take(pageSize).ToList();

                // Si el cliente quiere saber el total real
                if (includeTotal)
                {
                    response.TotalResults = query.Count(); // costo adicional
                }
                else
                {
                    response.TotalResults = skip + response.Data.Count + (pagedData.Count > pageSize ? 1 : 0); // estimado
                }

                response.Success = true;
                response.Message = $"Entities {typeof(TEntity).Name} retrieved successfully";

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Errors = [new ValidationFailure("Id", ex.Message)];
                response.Data = null;

                _logger.LogError(ex, "Error al obtener {entity} : {message}", typeof(TEntity).Name, ex.Message);

                return response;
            }
        }

        /// <summary>
        /// The GetById
        /// </summary>
        /// <param name="id">The id<see cref="TId"/></param>
        /// <param name="includes">The includes<see cref="string[]?"/></param>
        /// <returns>The <see cref="Response{TEntity, List{ValidationFailure}}"/></returns>
        public Response<TEntity, List<ValidationFailure>> GetById(TId id, string[]? includes = null)
        {
            Response<TEntity, List<ValidationFailure>> response = new();

            try
            {
                IQueryable<TEntity> query = _db.Set<TEntity>();

                if (includes is { Length: > 0 })
                {
                    try
                    {
                        query = query.ApplyIncludes(includes);
                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Message = $"Error en Include: {ex.Message}";
                        response.Errors = [new ValidationFailure("Include", ex.Message)];
                        return response;
                    }
                }

                // 🔒 Filtrar automáticamente registros eliminados lógicamente
                var parameter = Expression.Parameter(typeof(TEntity), "e");
                var idProp = Expression.Property(parameter, "Id");
                var condition = Expression.Equal(idProp, Expression.Constant(id));
                var lambda = Expression.Lambda<Func<TEntity, bool>>(condition, parameter);

                var entity = query.FirstOrDefault(lambda);

                if (entity == null)
                {
                    response.Success = false;
                    response.Message = $"Entity {typeof(TEntity).Name} not found";
                    response.Errors = [new ValidationFailure("Id", "Entity not found")];
                    response.Data = null;

                    return response;
                }

                response.Errors = null;
                response.Data = entity;
                response.Success = true;
                response.Message = $"Entity {typeof(TEntity).Name} retrieved successfully";

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Errors = [new ValidationFailure("Id", ex.Message)];
                response.Data = null;

                _logger.LogError(ex, "Error al obtener {entity} : {message}", typeof(TEntity).Name, ex.Message);

                return response;
            }
        }

        /// <summary>
        /// The Creation
        /// </summary>
        /// <param name="model">The model<see cref="TRequest"/></param>
        /// <returns>The <see cref="Response{TEntity, List{ValidationFailure}}"/></returns>
        public Response<TEntity, List<ValidationFailure>> Create(TRequest model)
        {
            Response<TEntity, List<ValidationFailure>> response = new();

            string userId = string.Empty;

            try
            {
                var results = _entitySupportService.GetValidator<TRequest>("Create").Validate(model);

                if (!results.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation failed";
                    response.Errors = results.Errors;
                    response.Data = null;

                    return response;
                }

                var entity = _mapper.Map<TEntity>(model!);
                var database = _db.Set<TEntity>();
                // Solo usar transacción si la base no es InMemory
                IDbContextTransaction? transaction = null;
                if (!IsInMemoryDatabase())
                {
                    transaction = _db.Database.BeginTransaction();
                }

                foreach (var interceptor in _entitySupportService.GetBeforeCreateInterceptors<TEntity, TRequest>())
                {
                    if (!response.Success) return response;

                    response.Data = entity;

                    response = interceptor.Execute(response, model);

                    entity = response.Data!;
                }

                if (!response.Success) return response;

                if (response.Message != "return")
                {
                    userId = entity.CreatedBy.ToString();

                    entity.CreatedAt = DateTimeOffset.UtcNow;
                    entity.UpdatedAt = null;
                    entity.UpdatedBy = null;

                    database.Add(entity);
                    _db.SaveChanges();
                }

                response.Errors = null;
                response.Data = entity;
                response.Success = true;
                response.Message = $"Entity {typeof(TEntity).Name} created successfully";

                if (!response.Success) return response;

                foreach (var interceptor in _entitySupportService.GetAfterCreateInterceptors<TEntity, TRequest>())
                {
                    if (!response.Success)
                    {
                        throw new Exception(response.Message);
                    }

                    response = interceptor.Execute(response, model);
                }

                // Commit solo si hay transacción
                transaction?.Commit();
                transaction?.Dispose();

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Errors = [new ValidationFailure("Id", ex.Message)];
                response.Data = null;

                _logger.LogError(ex, "Error al crear {entity} : usuario {user} : {message}", typeof(TEntity).Name, userId, ex.Message);

                return response;
            }
        }

        /// <summary>
        /// The Update
        /// </summary>
        /// <param name="model">The model<see cref="TRequest"/></param>
        /// <returns>The <see cref="Response{TEntity, List{ValidationFailure}}"/></returns>
        public Response<TEntity, List<ValidationFailure>> Update(TRequest model)
        {
            Response<TEntity, List<ValidationFailure>> response = new();

            string userId = string.Empty;

            try
            {
                var results = _entitySupportService.GetValidator<TRequest>("Update").Validate(model);

                if (!results.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation failed";
                    response.Errors = results.Errors;
                    response.Data = null;

                    return response;
                }

                TEntity entity = _mapper.Map<TEntity>(model!);
                var database = _db.Set<TEntity>();

                IDbContextTransaction? transaction = null;
                if (!IsInMemoryDatabase())
                {
                    transaction = _db.Database.BeginTransaction();
                }

                var parameter = Expression.Parameter(typeof(TEntity), "x");
                var member = Expression.PropertyOrField(parameter, "Id");
                var constant = Expression.Constant(entity.Id);
                var condition = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(member, constant), parameter);

                TEntity? prevState = database.AsNoTracking().FirstOrDefault(condition);
               

                if (prevState == null)
                {
                    response.Success = false;
                    response.Message = $"Entity {typeof(TEntity).Name} not found";
                    response.Errors = [new ValidationFailure("Id", $"Entity {typeof(TEntity).Name} not found")];
                    response.Data = null;

                    return response;
                }

                TEntity entityToUpdate = _mapper.Map<TEntity>(prevState);

                foreach (var interceptor in _entitySupportService.GetBeforeUpdateInterceptors<TEntity, TRequest>())
                {
                    if (!response.Success) return response;

                    response.Data = entity;

                    response = interceptor.Execute(response, model);

                    entity = response.Data!;
                }

                if (!response.Success) return response;

                userId = entity.CreatedBy.ToString();

                DateTimeOffset createdAt = entityToUpdate.CreatedAt;

                Util.UpdateProperties(entityToUpdate, entity);

                entityToUpdate.UpdatedAt = DateTimeOffset.UtcNow;
                entityToUpdate.CreatedAt = createdAt;

                DetachLocalIfTracked(database, entityToUpdate.Id);
                database.Update(entityToUpdate);
                _db.SaveChanges();

                response.Errors = null;
                response.Data = entityToUpdate;
                response.Success = true;
                response.Message = $"Entity {typeof(TEntity).Name} updated successfully";

                if (!response.Success) return response;

                foreach (var interceptor in _entitySupportService.GetAfterUpdateInterceptors<TEntity, TRequest>())
                {
                    if (!response.Success)
                    {
                        throw new Exception(response.Message);
                    }

                    response = interceptor.Execute(response, model, prevState);
                }

                transaction?.Commit();
                transaction?.Dispose();

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Errors = [new ValidationFailure("Id", ex.Message)];
                response.Data = null;

                _logger.LogError(ex, "Error al actualizar {entity} : usuario {user} : {message}", typeof(TEntity).Name, userId, ex.Message);

                return response;
            }
        }

        /// <summary>
        /// The PartialUpdate
        /// </summary>
        /// <param name="model">The model<see cref="TRequest"/></param>
        /// <returns>The <see cref="Response{TEntity, List{ValidationFailure}}"/></returns>
        public Response<TEntity, List<ValidationFailure>> PartialUpdate(TRequest model)
        {
            Response<TEntity, List<ValidationFailure>> response = new();

            string userId = string.Empty;

            try
            {
                var results = _entitySupportService.GetValidator<TRequest>("Partial").Validate(model);

                if (!results.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation failed";
                    response.Errors = results.Errors;
                    response.Data = null;

                    return response;
                }

                TEntity entity = _mapper.Map<TEntity>(model!);

                var database = _db.Set<TEntity>();

                IDbContextTransaction? transaction = null;
                if (!IsInMemoryDatabase())
                {
                    transaction = _db.Database.BeginTransaction();
                }

                var parameter = Expression.Parameter(typeof(TEntity), "x");
                var member = Expression.PropertyOrField(parameter, "Id");
                var constant = Expression.Constant(entity.Id);
                var condition = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(member, constant), parameter);

                TEntity? prevState = database.AsNoTracking().FirstOrDefault(condition);

                if (prevState == null)
                {
                    response.Success = false;
                    response.Message = $"Entity {typeof(TEntity).Name} not found";
                    response.Errors = [new ValidationFailure("Id", $"Entity {typeof(TEntity).Name} not found")];
                    response.Data = null;

                    return response;
                }

                TEntity entityToUpdate = _mapper.Map<TEntity>(prevState);

                userId = entity.CreatedBy.ToString();

                DateTimeOffset createdAt = entityToUpdate.CreatedAt;
                Util.UpdateProperties(entityToUpdate, entity);
                entityToUpdate.UpdatedAt = DateTimeOffset.UtcNow;
                entityToUpdate.CreatedAt = createdAt;

                DetachLocalIfTracked(database, entityToUpdate.Id);
                database.Update(entityToUpdate);
                _db.SaveChanges();

                response.Errors = null;
                response.Data = entityToUpdate;
                response.Success = true;
                response.Message = $"Entity {typeof(TEntity).Name} updated successfully";
                response.Errors = results.Errors;

                if (!response.Success) return response;

                foreach (var interceptor in _entitySupportService.GetAfterPartialUpdateInterceptors<TEntity, TRequest>())
                {
                    if (!response.Success)
                    {
                        throw new Exception(response.Message);
                    }

                    response = interceptor.Execute(response, model, prevState);
                }

               

                transaction?.Commit();
                transaction?.Dispose();

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Errors = [new ValidationFailure("Id", ex.Message)];
                response.Data = null;

                _logger.LogError(ex, "Error al actualizar parcial {entity} : usuario {user} : {message}", typeof(TEntity).Name, userId, ex.Message);

                return response;
            }
        }

        /// <summary>
        /// The Delete
        /// </summary>
        /// <param name="id">The id<see cref="TId"/></param>
        /// <param name="deletedBy">The deletedBy<see cref="long"/></param>
        /// <returns>The <see cref="Response{TEntity, List{ValidationFailure}}"/></returns>
        public Response<TEntity, List<ValidationFailure>> Delete(TId id, long deletedBy)
        {
            Response<TEntity, List<ValidationFailure>> response = new();

            string userId = string.Empty;

            try
            {
                response.Success = false;
                response.Message = "Invalid Id";
                response.Errors = [new ValidationFailure("Id", "Invalid Id")];
                response.Data = null;

                if (!Util.HasValidId(id)) return response;

                var parameter = Expression.Parameter(typeof(TEntity), "x");
                var member = Expression.PropertyOrField(parameter, "Id");
                var constant = Expression.Constant(id);
                var condition = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(member, constant), parameter);

                var database = _db.Set<TEntity>();

                TEntity? entity = database.AsNoTracking().FirstOrDefault(condition);

                IDbContextTransaction? transaction = null;
                if (!IsInMemoryDatabase())
                {
                    transaction = _db.Database.BeginTransaction();
                }

                if (entity == null)
                {
                    response.Success = false;
                    response.Message = $"Entity {typeof(TEntity).Name} not found";
                    response.Errors = [new ValidationFailure("Id", $"Entity {typeof(TEntity).Name} not found")];
                    response.Data = null;

                    return response;
                }

                userId = entity.CreatedBy.ToString();

                entity.UpdatedAt = DateTimeOffset.UtcNow;
                entity.State = 0;
                entity.UpdatedBy = deletedBy;

                DetachLocalIfTracked(database, entity.Id);
                database.Update(entity);
                _db.SaveChanges();

                response.Errors = null;
                response.Data = entity;
                response.Success = true;
                response.Message = $"Entity {typeof(TEntity).Name} deleted successfully";

                transaction?.Commit();
                transaction?.Dispose();

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Errors = [new ValidationFailure("Id", ex.Message)];
                response.Data = null;

                _logger.LogError(ex, "Error al eliminar {entity} : usuario {user} : {message}", typeof(TEntity).Name, userId, ex.Message);

                return response;
            }
        }

        private bool IsInMemoryDatabase()
        {
            return _db.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";
        }

        private void DetachLocalIfTracked<T>(DbSet<T> dbSet, TId id) where T : class, IEntity<TId>
        {
            var local = dbSet.Local.FirstOrDefault(e => e.Id!.Equals(id));
            if (local != null)
            {
                _db.Entry(local).State = EntityState.Detached;
            }
        }
    }
}