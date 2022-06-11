using System.Text.Json;
using Awarean.Sdk.SharedKernel;
using Awarean.Sdk.SharedKernel.Delegates;
using Microsoft.Extensions.Logging;

namespace TechTest.BancoMaster.Travels.Infra.Shared;
public abstract class BaseInMemoryRepository<TEntity, TId>
    : ICommandRepository<TEntity, TId>, IQueryRepository<TEntity, TId>
    where TEntity : Entity<TId>
{
    protected abstract Dictionary<TId, TEntity> Data { get; }
    protected abstract TEntity NullValue { get; }

    private readonly string entityType = typeof(TEntity).Name;
    private readonly ILogger _logger;

    public BaseInMemoryRepository(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task DeleteAsync(TId id)
    {
        _logger.LogInformation("Removing {id} from {type} records.", id, entityType);
        Data.Remove(id);
        return Task.CompletedTask;
    }

    public async Task<TEntity> GetByIdAsync(TId id)
    {
        var exists = Data.TryGetValue(id, out var queried);

        if (exists is false)
        {
            _logger.LogInformation("Not found any record of type {type} with id {id}", entityType, id);
            return NullValue;
        }

        return queried;
    }

    public async Task<IEnumerable<TEntity>> GetWhereAsync(GetWhereSelector<TEntity> filter)
    {
        return Data.Where(x => filter(x.Value)).Select(x => x.Value);
    }

    public Task<TId> SaveAsync(TEntity entity)
    {
        var id = entity.Id;

        _logger.LogInformation("Saving new {type} record for id {id} with data: {data}", entityType, id, JsonSerializer.Serialize(entity));
        Data.Add(id, entity);
        return Task.FromResult(id);
    }

    public Task UpdateAsync(TId id, TEntity updatedEntity)
    {
        _logger.LogInformation("Updating {type} record for id {id} with data: {data}", entityType, id, JsonSerializer.Serialize(updatedEntity));
        Data[id] = updatedEntity;
        return Task.CompletedTask;
    }

    protected KeyValuePair<TId?, TEntity> GetUsing(Func<KeyValuePair<TId?, TEntity>, bool> predicate)
        => Data.DefaultIfEmpty(KeyValuePair.Create(NullValue.Id ?? default, NullValue))
            .SingleOrDefault(predicate);
}