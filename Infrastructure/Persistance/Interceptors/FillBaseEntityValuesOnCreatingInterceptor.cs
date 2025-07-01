using Microsoft.EntityFrameworkCore.Diagnostics;
using BaseEntity = Core.Domain.Entities.Base.BaseEntity;

namespace Infrastructure.Persistence.Interceptors;

public sealed class FillBaseEntityValuesOnCreatingInterceptor : SaveChangesInterceptor
{
	private readonly HttpContext? _httpContext;

	public FillBaseEntityValuesOnCreatingInterceptor(IHttpContextAccessor httpContextAccessor)
	{
		_httpContext = httpContextAccessor.HttpContext;
	}

	public FillBaseEntityValuesOnCreatingInterceptor()
	{
	}

	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
		InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		if (eventData.Context is null)
			return result;

		foreach (var entry in eventData.Context.ChangeTracker.Entries())
		{
			if (entry is not { State: EntityState.Added, Entity: BaseEntity entity })
				continue;

			SetBaseEntityValues(entity);
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
			if (entry is not { State: EntityState.Added, Entity: BaseEntity entity })
				continue;

			SetBaseEntityValues(entity);
		}

		return result;
	}

	private void SetBaseEntityValues(BaseEntity entity)
	{
		if (_httpContext?.User.Identity is null || !_httpContext.User.Identity.IsAuthenticated)
		{
			return;
		}

	}
}
