using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport.ListenerSupport;
using System;

namespace Cloudy.CMS.UI.IdentitySupport
{
    public class UserSaveListener : ISaveListener<User>
    {
        INormalizer Normalizer { get; }

        public UserSaveListener(INormalizer normalizer)
        {
            Normalizer = normalizer;
        }

        public void BeforeSave(object content)
        {
            var user = (User)content;
            user.NormalizedEmail = Normalizer.NormalizeEmail(user.Email);
            user.NormalizedUsername = Normalizer.NormalizeName(user.Username);
        }
    }
}
