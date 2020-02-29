using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.AspNetCore
{
    public class AuthorizeOptions : IAuthorizeData
    {
        /// <summary>
        /// Gets or sets the policy name that determines access to the resource.
        /// </summary>
        public string Policy { get; set; }

        /// <summary>
        /// Gets or sets a comma delimited list of roles that are allowed to access the resource.
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// Gets or sets a comma delimited list of schemes from which user information is constructed.
        /// </summary>
        public string AuthenticationSchemes { get; set; }
    }
}
