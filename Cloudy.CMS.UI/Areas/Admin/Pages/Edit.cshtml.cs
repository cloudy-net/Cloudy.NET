using Cloudy.CMS.EntityTypeSupport.Naming;
using Cloudy.CMS.EntityTypeSupport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.UI.FormSupport;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.Areas.Admin.Pages
{
    [Authorize("adminarea")]
    public class EditModel : PageModel
    {
        IEntityTypeProvider EntityTypeProvider { get; }
        IEntityTypeNameProvider EntityTypeNameProvider { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }

        public EditModel(IEntityTypeProvider entityTypeProvider, IEntityTypeNameProvider entityTypeNameProvider, IContextCreator contextCreator, IPrimaryKeyConverter primaryKeyConverter)
        {
            EntityTypeProvider = entityTypeProvider;
            EntityTypeNameProvider = entityTypeNameProvider;
            ContextCreator = contextCreator;
            PrimaryKeyConverter = primaryKeyConverter;
        }

        public EntityTypeDescriptor EntityType { get; set; }
        public EntityTypeName EntityTypeName { get; set; }
        public IEnumerable<string> Keys { get; set; }
        public object Instance { get; set; }

        async Task BindData(string entityType, string[] keys)
        {
            EntityType = EntityTypeProvider.Get(entityType);
            EntityTypeName = EntityTypeNameProvider.Get(EntityType.Type);
            Keys = keys;
            var keyValues = PrimaryKeyConverter.Convert(keys, EntityType.Type);
            var context = ContextCreator.CreateFor(EntityType.Type);
            Instance = await context.Context.FindAsync(EntityType.Type, keyValues).ConfigureAwait(false);
        }

        public async Task<IActionResult> OnGet(string entityType, string[] keys)
        {
            await BindData(entityType, keys).ConfigureAwait(false);

            if (Instance == null)
            {
                return NotFound($"Could not find instance of type {entityType} and key{(keys.Length > 1 ? "s" : null)} {string.Join(", ", keys)}");
            }

            return Page();
        }
    }
}
