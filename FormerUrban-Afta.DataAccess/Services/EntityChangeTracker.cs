//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace FormerUrban_Afta.DataAccess.Services;
//public class EntityChangeTracker
//{
//    private readonly Dictionary<Type, EnumFormName> _entityTypeMap;
//    private readonly JsonSerializerOptions _jsonOptions;

//    public EntityChangeTracker()
//    {
//        // Map your entity types to audit forms
//        _entityTypeMap = new Dictionary<Type, EnumFormName>
//        {
//            { typeof(Apartman), EnumFormName.Apartman},
//            { typeof(AllowedIPRange), EnumFormName.AllowedIPRange },
//            { typeof(Baygani), EnumFormName.Baygani },
//            { typeof(BlockedIPRange), EnumFormName.BlockedIPRange},
//            { typeof(ControlMap), EnumFormName.ControlMap},
//            { typeof(Darkhast), EnumFormName.Darkhast},
//            { typeof(Dv_karbari), EnumFormName.Dv_karbari},
//            { typeof(Dv_malekin), EnumFormName.Dv_malekin},
//            { typeof(Dv_savabegh), EnumFormName.Dv_savabegh},
//            { typeof(Erja), EnumFormName.Erja},
//            { typeof(Estelam), EnumFormName.Estelam},
//            { typeof(Expert), EnumFormName.Expert},
//            { typeof(Melk), EnumFormName.Melk},
//            { typeof(Parvandeh), EnumFormName.Parvandeh},
//            { typeof(Parvaneh), EnumFormName.Parvaneh},
//            { typeof(RolePermission), EnumFormName.RolePermission},
//            { typeof(Sakhteman), EnumFormName.Sakhteman},
//            { typeof(Tarifha), EnumFormName.Tarifha},
//            { typeof(UserPermission), EnumFormName.UserPermission},
//            { typeof(EventLogThreshold), EnumFormName.EventLogThreshold},
//            { typeof(RoleRestriction), EnumFormName.RoleRestriction},
//            { typeof(EventLogFilter), EnumFormName.EventLogFilter},
//            // Add your other entities here
//        };

//        _jsonOptions = new JsonSerializerOptions
//        {
//            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//            WriteIndented = false,
//            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
//        };
//    }

//    public async Task<string> GetEntityChangesAsync(EntityEntry entry)
//    {
//        var changes = new StringBuilder();

//        switch (entry.State)
//        {
//            case EntityState.Added:
//                changes.Append("Created: ");
//                foreach (var property in entry.CurrentValues.Properties)
//                {
//                    var currentValue = entry.CurrentValues[property];
//                    if (currentValue != null)
//                    {
//                        changes.Append($"{property.Name}='{currentValue}'; ");
//                    }
//                }
//                break;

//            case EntityState.Modified:
//                changes.Append("Modified: ");
//                var databaseValue = await entry.GetDatabaseValuesAsync();
//                foreach (var property in entry.Properties.Where(p => p.IsModified))
//                {
//                    var propertyName = property.Metadata.Name;
//                    var originalValue = databaseValue[propertyName]?.ToString() ?? "null";
//                    var currentValue = property.CurrentValue?.ToString() ?? "null";
//                    if (originalValue != currentValue && propertyName != "Hashed")
//                        changes.Append($"{property.Metadata.Name}: '{originalValue}' → '{currentValue}'; ");
//                }
//                break;

//            case EntityState.Deleted:
//                changes.Append("Deleted: ");
//                foreach (var property in entry.OriginalValues.Properties)
//                {
//                    var originalValue = entry.OriginalValues[property];
//                    if (originalValue != null)
//                    {
//                        changes.Append($"{property.Name}='{originalValue}'; ");
//                    }
//                }
//                break;
//        }

//        return changes.ToString().TrimEnd(' ', ';');
//    }

//    public string GetEntityChanges(EntityEntry entry)
//    {
//        var changes = new StringBuilder();

//        switch (entry.State)
//        {
//            case EntityState.Added:
//                changes.Append("Created: ");
//                foreach (var property in entry.CurrentValues.Properties)
//                {
//                    var currentValue = entry.CurrentValues[property];
//                    if (currentValue != null)
//                    {
//                        changes.Append($"{property.Name}='{currentValue}'; ");
//                    }
//                }
//                break;

//            case EntityState.Modified:
//                changes.Append("Modified: ");
//                var databaseValue = entry.GetDatabaseValues();
//                foreach (var property in entry.Properties.Where(p => p.IsModified))
//                {
//                    var propertyName = property.Metadata.Name;
//                    var originalValue = databaseValue[propertyName]?.ToString() ?? "null";
//                    var currentValue = property.CurrentValue?.ToString() ?? "null";
//                    if (originalValue != currentValue && propertyName != "Hashed")
//                        changes.Append($"{property.Metadata.Name}: '{originalValue}' → '{currentValue}'; ");
//                }
//                break;

//            case EntityState.Deleted:
//                changes.Append("Deleted: ");
//                foreach (var property in entry.OriginalValues.Properties)
//                {
//                    var originalValue = entry.OriginalValues[property];
//                    if (originalValue != null)
//                    {
//                        changes.Append($"{property.Name}='{originalValue}'; ");
//                    }
//                }
//                break;
//        }

//        return changes.ToString().TrimEnd(' ', ';');
//    }

//    public async Task<List<AuditEntry>> GetChangesAsync(DbContext context)
//    {
//        var auditEntries = new List<AuditEntry>();

//        foreach (var entry in context.ChangeTracker.Entries()
//            .Where(e => e.State == EntityState.Added ||
//                       e.State == EntityState.Modified ||
//                       e.State == EntityState.Deleted))
//        {
//            var entityType = entry.Entity.GetType();

//            // Skip if entity type is not mapped for auditing
//            if (!_entityTypeMap.ContainsKey(entityType))
//                continue;

//            var change =await GetEntityChangesAsync(entry);

//            var auditForm = _entityTypeMap[entityType];
//            var entityId = GetEntityId(entry.Entity);

//            var auditEntry = new AuditEntry
//            {
//                Form = auditForm,
//                EntityId = entityId,
//                Action = entry.State switch
//                {
//                    EntityState.Added => EnumOperation.Post,
//                    EntityState.Modified => EnumOperation.Update,
//                    EntityState.Deleted => EnumOperation.Delete,
//                    _ => throw new ArgumentOutOfRangeException()
//                },
//                Changes = change
//            };

//            auditEntries.Add(auditEntry);
//        }

//        return auditEntries;
//    }

//    public List<AuditEntry> GetChanges(DbContext context)
//    {
//        var auditEntries = new List<AuditEntry>();

//        foreach (var entry in context.ChangeTracker.Entries()
//            .Where(e => e.State == EntityState.Added ||
//                       e.State == EntityState.Modified ||
//                       e.State == EntityState.Deleted))
//        {
//            var entityType = entry.Entity.GetType();

//            // Skip if entity type is not mapped for auditing
//            if (!_entityTypeMap.ContainsKey(entityType))
//                continue;

//            var change = GetEntityChanges(entry);

//            var auditForm = _entityTypeMap[entityType];
//            var entityId = GetEntityId(entry.Entity);

//            var auditEntry = new AuditEntry
//            {
//                Form = auditForm,
//                EntityId = entityId,
//                Action = entry.State switch
//                {
//                    EntityState.Added => EnumOperation.Post,
//                    EntityState.Modified => EnumOperation.Update,
//                    EntityState.Deleted => EnumOperation.Delete,
//                    _ => throw new ArgumentOutOfRangeException()
//                },
//                Changes = change
//            };

//            auditEntries.Add(auditEntry);
//        }

//        return auditEntries;
//    }

//    private string GetEntityId(object entity)
//    {
//        // Try to get Id property using reflection
//        var idProperty = entity.GetType().GetProperty("Identity");
//        if (idProperty != null)
//        {
//            return idProperty.GetValue(entity)?.ToString() ?? "0";
//        }

//        // Fallback for composite keys or different naming
//        var keyProperties = entity.GetType().GetProperties()
//            .Where(p => p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
//            .ToList();

//        if (keyProperties.Count == 1)
//        {
//            return keyProperties[0].GetValue(entity)?.ToString() ?? "0";
//        }

//        // For composite keys, concatenate with underscore
//        if (keyProperties.Count > 1)
//        {
//            return string.Join("_", keyProperties.Select(p => p.GetValue(entity)?.ToString() ?? "0"));
//        }

//        return "0";
//    }

//    private async Task<object> GetOriginalValues(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
//    {
//        var originalValues = new Dictionary<string, object?>();

//        var databaseValue = await entry.GetDatabaseValuesAsync();

//        foreach (var property in entry.OriginalValues.Properties)
//        {
//            originalValues[property.Name] = databaseValue[property.Name];
//        }

//        return originalValues;
//    }
//}

