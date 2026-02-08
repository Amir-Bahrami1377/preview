using FormerUrban_Afta.DataAccess.Model.BaseEntity;
using FormerUrban_Afta.DataAccess.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace FormerUrban_Afta.DataAccess.Configurations.Interceptors
{
    public class AddAuditDataInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddAuditDataInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // متد همگام
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ProcessEntities(eventData);
            return base.SavingChanges(eventData, result);
        }

        // متد ناهمگام
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ProcessEntities(eventData);

            var res = await base.SavingChangesAsync(eventData, result, cancellationToken);

            return res;
        }

        private void ProcessEntities(DbContextEventData eventData)
        {
            if (eventData.Context == null) return;

            var userId = GetCurrentUserId();
            var changeTracker = eventData.Context.ChangeTracker;
            ProcessAddedEntities(changeTracker, userId);
            ProcessModifiedEntities(changeTracker, userId);
            ProcessHashedProperties(changeTracker);
        }

        private Guid GetCurrentUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdString, out Guid userId) ? userId : Guid.Empty;
        }

        private void ProcessAddedEntities(ChangeTracker changeTracker, Guid userId)
        {
            var addedEntries = changeTracker.Entries<BaseModel>().Where(e => e.State == EntityState.Added);
            foreach (var entry in addedEntries)
            {
                entry.Property(c => c.CreateDateTime).CurrentValue = DateTime.UtcNow.AddHours(3.5);
                entry.Property(c => c.CreateUser).CurrentValue = userId;
            }
        }

        private void ProcessModifiedEntities(ChangeTracker changeTracker, Guid userId)
        {
            var modifiedEntries = changeTracker.Entries<BaseModel>().Where(e => e.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                var databaseValue = entry.GetDatabaseValues();

                entry.Property(c => c.ModifiedDate).CurrentValue = DateTime.UtcNow.AddHours(3.5);
                entry.Property(c => c.ModifiedUser).CurrentValue = userId;

                entry.Property(c => c.CreateDateTime).CurrentValue = Convert.ToDateTime(databaseValue?["CreateDateTime"]?.ToString() ?? "null");
                entry.Property(c => c.CreateUser).CurrentValue = new Guid(databaseValue?["CreateUser"]?.ToString() ?? "null");
            }
        }

        private void ProcessHashedProperties(ChangeTracker changeTracker)
        {
            var modifiedEntities = changeTracker.Entries()
                .Where(e => e.State is EntityState.Modified or EntityState.Added);

            foreach (var entry in modifiedEntities)
            {
                var hashProperty = entry.Entity.GetType().GetProperty("Hashed");
                if (hashProperty != null)
                {
                    hashProperty.SetValue(entry.Entity, CipherService.Hash(entry.Entity.ToString()));
                }
            }
        }
    }
}
