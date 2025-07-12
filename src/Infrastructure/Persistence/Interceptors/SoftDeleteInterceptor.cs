using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using BaseEntity = Core.Domain.Entities.Base.BaseEntity;

namespace Infrastructure.Persistence.Interceptors;


public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
	private readonly HttpContext? _httpContext;
	public SoftDeleteInterceptor(IHttpContextAccessor httpContextAccessor)
	{
		_httpContext = httpContextAccessor.HttpContext;
	}

	public SoftDeleteInterceptor()
	{

	}

	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
		InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		if (eventData.Context is null) return result;

		foreach (var entry in eventData.Context.ChangeTracker.Entries())
		{
			if (entry is not { State: EntityState.Deleted, Entity: BaseEntity entity })
				continue;

			DeActiveEntity(entity, entry);
		}

		await ValueTask.CompletedTask;
		return result;
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		if (eventData.Context is null)
			return result;

		foreach (var entry in eventData.Context.ChangeTracker.Entries())
		{
			if (entry is not { State: EntityState.Deleted, Entity: BaseEntity entity })
				continue;

			DeActiveEntity(entity, entry);
		}

		return result;
	}

	private void DeActiveEntity(BaseEntity entity, EntityEntry entry)
	{
		entity.IsActive = false;
		entry.State = EntityState.Modified;
	}
}