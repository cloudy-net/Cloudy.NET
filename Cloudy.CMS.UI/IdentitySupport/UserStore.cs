using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.IdentitySupport
{
    public class UserStore :
        IUserStore<User>,
        IUserClaimStore<User>,
        IUserLoginStore<User>,
        IUserPasswordStore<User>,
        IUserEmailStore<User>,
        IUserPhoneNumberStore<User>
    {
        IContainerSpecificContentCreator ContainerSpecificContentCreator { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }
        IContainerSpecificContentUpdater ContainerSpecificContentUpdater { get; }
        IContainerSpecificContentDeleter ContainerSpecificContentDeleter { get; }
        IDocumentFinder DocumentFinder { get; set; }
        IContentDeserializer ContentDeserializer { get; set; }
        ContentTypeDescriptor ContentType { get; }
        string Container { get; }

        public UserStore(
            IContainerSpecificContentCreator containerSpecificContentCreator,
            IContainerSpecificContentGetter containerSpecificContentGetter,
            IContainerSpecificContentUpdater containerSpecificContentUpdater,
            IContainerSpecificContentDeleter containerSpecificContentDeleter,
            IDocumentFinder documentFinder,
            IContentDeserializer contentDeserializer,
            IContentTypeProvider contentTypeProvider
        )
        {
            ContainerSpecificContentCreator = containerSpecificContentCreator;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
            ContainerSpecificContentUpdater = containerSpecificContentUpdater;
            ContainerSpecificContentDeleter = containerSpecificContentDeleter;
            DocumentFinder = documentFinder;
            ContentDeserializer = contentDeserializer;
            ContentType = contentTypeProvider.Get(typeof(User));
            Container = ContentType.Container;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await ContainerSpecificContentCreator.CreateAsync(user, Container);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            await ContainerSpecificContentDeleter.DeleteAsync(user.Id, Container);

            return IdentityResult.Success;
        }

        public void Dispose() { }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await ContainerSpecificContentGetter.GetAsync<User>(userId, null, Container);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var document = (await DocumentFinder.Find(Container).WhereEquals<User, string>(u => u.NormalizedUsername, normalizedUserName).GetResultAsync()).FirstOrDefault();

            if(document == null)
            {
                return null;
            }

            var content = ContentDeserializer.Deserialize(document, ContentType, null) as User;

            return content;
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return user.NormalizedUsername;
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return user.Id;
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return user.Username;
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUsername = normalizedName;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.Username = userName;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }

            return IdentityResult.Success;
        }

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.Logins.Add(login);

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            user.Logins.Remove(user.Logins.Single(l => l.ProviderKey == providerKey));

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            return user.Logins;
        }

        public Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            return user.Claims ?? new List<Claim>();
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            if(user.Claims == null)
            {
                user.Claims = new List<Claim>();
            }

            user.Claims.AddRange(claims);

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            if (user.Claims == null)
            {
                user.Claims = new List<Claim>();
            }

            var existing = user.Claims.SingleOrDefault(c => c.Type == claim.Type);

            if (existing != null)
            {
                user.Claims.Remove(existing);
            }

            user.Claims.Add(newClaim);

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            if (user.Claims == null)
            {
                user.Claims = new List<Claim>();
            }

            var types = claims.Select(c => c.Type);

            user.Claims.RemoveAll(c => types.Contains(c.Type));

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return user.PasswordHash != null;
        }

        public async Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return user.EmailConfirmed;
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var document = (await DocumentFinder.Find(Container).WhereEquals<User, string>(u => u.NormalizedEmail, normalizedEmail).GetResultAsync()).FirstOrDefault();

            if (document == null)
            {
                return null;
            }

            var content = ContentDeserializer.Deserialize(document, ContentType, null) as User;

            return content;
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return user.NormalizedEmail;
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;

            if (user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            return user.PhoneNumber;
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return user.PhoneNumberConfirmed;
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;

            if (user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }
    }
}
