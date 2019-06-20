using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.AspNetCore.AuthorizationSupport
{
    public interface IUIAuthorizationPolicyProvider
    {
        AuthorizationPolicy AuthorizationPolicy { get; }
    }
}
