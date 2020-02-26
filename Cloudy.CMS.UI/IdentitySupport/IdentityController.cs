using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Poetry.UI.FormSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.IdentitySupport
{
    [Area("Cloudy.CMS")]
    [Route("Identity")]
    public class IdentityController
    {
        UserManager UserManager { get; }

        public IdentityController(UserManager userManager)
        {
            UserManager = userManager;
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<object> ChangePassword([FromBody] ChangePassword input)
        {
            var user = await UserManager.FindByIdAsync(input.UserId);
            var addPasswordResult = await UserManager.AddPasswordAsync(user, input.Password);
            if (!addPasswordResult.Succeeded)
            {
                return addPasswordResult.Errors;
            }

            return new
            {
                success = true,
            };
        }
    }
}
