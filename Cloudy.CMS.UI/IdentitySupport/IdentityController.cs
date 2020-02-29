using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS.UI.FormSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.IdentitySupport
{
    [Area("Cloudy.CMS")]
    [Route("Identity")]
    public class IdentityController : Controller
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
            if (!ModelState.IsValid)
            {
                return new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { description = e.ErrorMessage }),
                };
            }

            var user = await UserManager.FindByIdAsync(input.UserId);

            if (await UserManager.HasPasswordAsync(user))
            {
                var removePasswordResult = await UserManager.RemovePasswordAsync(user);

                if (!removePasswordResult.Succeeded)
                {
                    return new
                    {
                        success = false,
                        errors = removePasswordResult.Errors,
                    };
                }
            }

            var addPasswordResult = await UserManager.AddPasswordAsync(user, input.Password);
            if (!addPasswordResult.Succeeded)
            {
                return new
                {
                    success = false,
                    errors = addPasswordResult.Errors,
                };
            }

            return new
            {
                success = true,
            };
        }
    }
}
