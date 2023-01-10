using Cloudy.CMS.EntityTypeSupport.Naming;
using Cloudy.CMS.EntityTypeSupport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using Cloudy.CMS.UI.FormSupport;
using Cloudy.CMS.ContextSupport;
using System.Threading.Tasks;
using System.Linq;
using Cloudy.CMS.EntitySupport.PrimaryKey;

namespace Cloudy.CMS.UI.Areas.Admin.Pages
{
    [Authorize("adminarea")]
    public class NewModel : PageModel
    {
        IEntityTypeProvider EntityTypeProvider { get; }
        IEntityTypeNameProvider EntityTypeNameProvider { get; }

        public NewModel(IEntityTypeProvider entityTypeProvider, IEntityTypeNameProvider entityTypeNameProvider)
        {
            EntityTypeProvider = entityTypeProvider;
            EntityTypeNameProvider = entityTypeNameProvider;
        }

        public EntityTypeDescriptor EntityType { get; set; }
        public EntityTypeName EntityTypeName { get; set; }

        void BindData(string entityType)
        {
            EntityType = EntityTypeProvider.Get(entityType);
            EntityTypeName = EntityTypeNameProvider.Get(EntityType.Type);
        }

        public void OnGet(string entityType)
        {
            BindData(entityType);
        }
    }
}
