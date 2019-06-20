using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Poetry.UI.AspNetCore.AuthorizationSupport
{
    public class UIAuthorizationPolicyProvider : IUIAuthorizationPolicyProvider
    {
        static UIAuthorizeOptions UIAuthorizeOptions { get; set; }

        internal static void Set(UIAuthorizeOptions uiAuthorizeOptions)
        {
            UIAuthorizeOptions = uiAuthorizeOptions;
        }

        public AuthorizationPolicy AuthorizationPolicy { get; }
        
        public UIAuthorizationPolicyProvider(IAuthorizationPolicyProvider authorizationPolicyProvider)
        {
            if (UIAuthorizeOptions != null)
            {
                AuthorizationPolicy = AuthorizationPolicy.CombineAsync(authorizationPolicyProvider, new List<IAuthorizeData> { UIAuthorizeOptions }).Result;
            }
        }

    }
}
