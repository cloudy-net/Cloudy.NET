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
    public class CloudyUserStore :
        IUserStore<CloudyUser>,
        IUserClaimStore<CloudyUser>,
        IUserLoginStore<CloudyUser>,
        IUserPasswordStore<CloudyUser>,
        IUserEmailStore<CloudyUser>,
        IUserPhoneNumberStore<CloudyUser>
    {
        IContainerSpecificContentCreator ContainerSpecificContentCreator { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }
        IContainerSpecificContentUpdater ContainerSpecificContentUpdater { get; }
        IContainerSpecificContentDeleter ContainerSpecificContentDeleter { get; }
        IDocumentFinder DocumentFinder { get; set; }
        IContentDeserializer ContentDeserializer { get; set; }
        ContentTypeDescriptor ContentType { get; }
        string Container { get; }

        public CloudyUserStore(
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
            ContentType = contentTypeProvider.Get(typeof(CloudyUser));
            Container = ContentType.Container;
        }

        public async Task<IdentityResult> CreateAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            await ContainerSpecificContentCreator.CreateAsync(user, Container);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            await ContainerSpecificContentDeleter.DeleteAsync(user.Id, Container);

            return IdentityResult.Success;
        }

        public void Dispose() { }

        public async Task<CloudyUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await ContainerSpecificContentGetter.GetAsync<CloudyUser>(userId, null, Container);
        }

        public async Task<CloudyUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var document = (await DocumentFinder.Find(Container).WhereEquals<CloudyUser, string>(u => u.NormalizedUsername, normalizedUserName).GetResultAsync()).FirstOrDefault();

            if(document == null)
            {
                return null;
            }

            var content = ContentDeserializer.Deserialize(document, ContentType, null) as CloudyUser;

            return content;
        }

        public async Task<string> GetNormalizedUserNameAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.NormalizedUsername;
        }

        public async Task<string> GetUserIdAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.Id;
        }

        public async Task<string> GetUserNameAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.Username;
        }

        public async Task SetNormalizedUserNameAsync(CloudyUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUsername = normalizedName;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task SetUserNameAsync(CloudyUser user, string userName, CancellationToken cancellationToken)
        {
            user.Username = userName;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<IdentityResult> UpdateAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }

            return IdentityResult.Success;
        }

        public async Task AddLoginAsync(CloudyUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.Logins.Add(login);

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task RemoveLoginAsync(CloudyUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            user.Logins.Remove(user.Logins.Single(l => l.ProviderKey == providerKey));

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.Logins;
        }

        public Task<CloudyUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Claim>> GetClaimsAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.Claims ?? new List<Claim>();
        }

        public async Task AddClaimsAsync(CloudyUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
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

        public async Task ReplaceClaimAsync(CloudyUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
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

        public async Task RemoveClaimsAsync(CloudyUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
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

        public Task<IList<CloudyUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetPasswordHashAsync(CloudyUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<string> GetPasswordHashAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.PasswordHash != null;
        }

        public async Task SetEmailAsync(CloudyUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<string> GetEmailAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.EmailConfirmed;
        }

        public async Task SetEmailConfirmedAsync(CloudyUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<CloudyUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var document = (await DocumentFinder.Find(Container).WhereEquals<CloudyUser, string>(u => u.NormalizedEmail, normalizedEmail).GetResultAsync()).FirstOrDefault();

            if (document == null)
            {
                return null;
            }

            var content = ContentDeserializer.Deserialize(document, ContentType, null) as CloudyUser;

            return content;
        }

        public async Task<string> GetNormalizedEmailAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.NormalizedEmail;
        }

        public async Task SetNormalizedEmailAsync(CloudyUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;

            if(user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task SetPhoneNumberAsync(CloudyUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;

            if (user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }

        public async Task<string> GetPhoneNumberAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.PhoneNumber;
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(CloudyUser user, CancellationToken cancellationToken)
        {
            return user.PhoneNumberConfirmed;
        }

        public async Task SetPhoneNumberConfirmedAsync(CloudyUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;

            if (user.Id != null)
            {
                await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
            }
        }
    }
}
