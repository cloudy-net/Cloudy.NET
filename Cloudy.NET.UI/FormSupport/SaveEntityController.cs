﻿using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.ContextSupport;
using Cloudy.NET.EntitySupport.PrimaryKey;
using Cloudy.NET.PropertyDefinitionSupport;
using Cloudy.NET.UI.FieldSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Cloudy.NET.UI.Serialization;
using Cloudy.NET.UI.FormSupport.ChangeHandlers;
using Cloudy.NET.UI.FormSupport.Changes;
using Microsoft.EntityFrameworkCore;

namespace Cloudy.NET.UI.FormSupport
{
    [Authorize("adminarea")]
    [Area("Admin")]
    [ResponseCache(NoStore = true)]
    public class SaveEntityController : Controller
    {
        IEntityTypeProvider EntityTypeProvider { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }
        IEntityNavigator EntityNavigator { get; }
        ISimpleChangeHandler SimpleChangeHandler { get; }
        IBlockTypeChangeHandler BlockTypeChangeHandler { get; }
        IEmbeddedBlockListHandler EmbeddedBlockListHandler { get; }

        public SaveEntityController(IEntityTypeProvider entityTypeProvider, IPrimaryKeyConverter primaryKeyConverter, IContextCreator contextCreator, IPrimaryKeyGetter primaryKeyGetter, IPropertyDefinitionProvider propertyDefinitionProvider, IPrimaryKeyPropertyGetter primaryKeyPropertyGetter, IEntityNavigator entityNavigator, ISimpleChangeHandler simpleChangeHandler, IBlockTypeChangeHandler blockTypeChangeHandler, IEmbeddedBlockListHandler embeddedBlockListHandler)
        {
            EntityTypeProvider = entityTypeProvider;
            PrimaryKeyConverter = primaryKeyConverter;
            ContextCreator = contextCreator;
            PrimaryKeyGetter = primaryKeyGetter;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
            EntityNavigator = entityNavigator;
            SimpleChangeHandler = simpleChangeHandler;
            BlockTypeChangeHandler = blockTypeChangeHandler;
            EmbeddedBlockListHandler = embeddedBlockListHandler;
        }

        [HttpPost]
        [Route("/{area}/api/form/entity/save")]
        public async Task<SaveEntityResponse> SaveEntity([FromBody] SaveEntityRequestBody data)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception($"Invalid state:\n{string.Join("\n", ModelState.Where(entry => entry.Value.ValidationState == ModelValidationState.Invalid).SelectMany(entry => entry.Value.Errors.Select(error => $"{entry.Key}: {error.ErrorMessage}")))}");
            }

            var contexts = new HashSet<IContextWrapper>();

            var result = new List<SaveEntityResult>();

            foreach (var changedEntity in data.Entities)
            {
                var entityType = EntityTypeProvider.Get(changedEntity.Reference.EntityType);
                var context = ContextCreator.CreateFor(entityType.Type);

                contexts.Add(context);

                var keyValues = changedEntity.Reference.KeyValues != null ? PrimaryKeyConverter.Convert(changedEntity.Reference.KeyValues.Select(k => k.ToString()), entityType.Type) : null;

                object entity;

                if (keyValues == null)
                {
                    entity = Activator.CreateInstance(entityType.Type);
                }
                else
                {
                    entity = await context.Context.FindAsync(entityType.Type, keyValues).ConfigureAwait(false);

                    if (changedEntity.Remove)
                    {
                        context.Context.Remove(entity);
                        result.Add(SaveEntityResult.SuccessResult(entityType.Name, keyValues, null));
                        continue;
                    }
                }

                var propertyDefinitions = PropertyDefinitionProvider.GetFor(entityType.Name).ToDictionary(p => p.Name, p => p);
                var idProperties = PrimaryKeyPropertyGetter.GetFor(entity.GetType());

                if (changedEntity.Changes.OfType<SimpleChange>().Any(c => c.Path.Length == 1 && idProperties.Any(p => p.Name == c.Path[0])))
                {
                    throw new Exception($"Tried to change primary key of entity {string.Join(", ", keyValues)} with type {changedEntity.Reference.EntityType}!");
                }

                var listTracker = new ListTracker();

                foreach (var change in changedEntity.Changes)
                {
                    var targetEntity = EntityNavigator.Navigate(entity, change.Path, listTracker);

                    switch (change)
                    {
                        case SimpleChange simpleChange:
                            SimpleChangeHandler.SetValue(targetEntity, simpleChange);
                            break;
                        case BlockTypeChange blockTypeChange:
                            BlockTypeChangeHandler.SetType(targetEntity, blockTypeChange);
                            break;
                        case EmbeddedBlockListAdd embeddedBlockListAdd:
                            EmbeddedBlockListHandler.Add(targetEntity, embeddedBlockListAdd, listTracker);
                            break;
                        case EmbeddedBlockListRemove embeddedBlockListRemove:
                            EmbeddedBlockListHandler.Remove(targetEntity, embeddedBlockListRemove, listTracker);
                            break;
                        default:
                            throw new Exception($"Unsupported change type: {change.GetType().Name}");
                    }
                }

                if (!TryValidateModel(entity))
                {
                    result.Add(SaveEntityResult.ValidationFailureResult(entityType.Name, keyValues, ModelState.ToDictionary(i => i.Key, i => i.Value.Errors.Select(e => e.ErrorMessage))));
                    continue;
                }

                if (keyValues == null)
                {
                    await context.Context.AddAsync(entity).ConfigureAwait(false);
                }
                else
                {
                    context.Context.Entry(entity).State = EntityState.Modified;
                }

                result.Add(SaveEntityResult.SuccessResult(entityType.Name, PrimaryKeyGetter.Get(entity), changedEntity.Reference.NewContentKey));
            }

            foreach (var context in contexts)
            {
                await context.Context.SaveChangesAsync().ConfigureAwait(false);
            }

            return new SaveEntityResponse(result);
        }

        public class SaveEntityResponse
        {
            public IEnumerable<SaveEntityResult> Results { get; }

            public SaveEntityResponse(IEnumerable<SaveEntityResult> result)
            {
                Results = result.ToList().AsReadOnly();
            }
        }

        public class SaveEntityResult
        {
            public bool Success { get; private set; }
            public EntityReference EntityReference { get; private set; }
            public IDictionary<string, IEnumerable<string>> ValidationErrors { get; private set; }

            public static SaveEntityResult SuccessResult(string entityType, IEnumerable<object> keyValues, string newContentKey)
            {
                return new SaveEntityResult
                {
                    Success = true,
                    EntityReference = new EntityReference
                    {
                        NewContentKey = newContentKey,
                        EntityType = entityType,
                        KeyValues = keyValues?.Select(k => JsonSerializer.SerializeToElement(k)).ToArray(),
                    },
                };
            }

            public static SaveEntityResult ValidationFailureResult(string entityTypeId, IEnumerable<object> keyValues, IDictionary<string, IEnumerable<string>> validationErrors)
            {
                return new SaveEntityResult
                {
                    Success = false,
                    EntityReference = new EntityReference
                    {
                        EntityType = entityTypeId,
                        KeyValues = keyValues?.Select(k => JsonSerializer.SerializeToElement(k)).ToArray(),
                    },
                    ValidationErrors = validationErrors.ToDictionary(e => e.Key, e => (IEnumerable<string>)e.Value.ToList().AsReadOnly()),
                };
            }
        }

        public class SaveEntityRequestBody
        {
            [Required]
            public IEnumerable<ChangedEntity> Entities { get; set; }
        }
    }
}
