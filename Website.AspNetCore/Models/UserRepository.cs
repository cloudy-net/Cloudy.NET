using Cloudy.CMS.ContainerSpecificContentSupport.FinderSupport;
using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models
{
    public class UserRepository :
        IUserStore<User>
    {
        IContainerSpecificContentCreator ContainerSpecificContentCreator { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }
        IContainerSpecificContentUpdater ContainerSpecificContentUpdater { get; }
        IContainerSpecificContentDeleter ContainerSpecificContentDeleter { get; }
        IContainerSpecificContentFinder ContainerSpecificContentFinder { get; }
        string Container { get; }

        public UserRepository(
            IContainerSpecificContentCreator containerSpecificContentCreator,
            IContainerSpecificContentGetter containerSpecificContentGetter,
            IContainerSpecificContentUpdater containerSpecificContentUpdater,
            IContainerSpecificContentDeleter containerSpecificContentDeleter,
            IContainerSpecificContentFinder containerSpecificContentFinder,
            IContentTypeProvider contentTypeProvider
        )
        {
            ContainerSpecificContentCreator = containerSpecificContentCreator;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
            ContainerSpecificContentUpdater = containerSpecificContentUpdater;
            ContainerSpecificContentDeleter = containerSpecificContentDeleter;
            ContainerSpecificContentFinder = containerSpecificContentFinder;
            Container = contentTypeProvider.Get(typeof(User)).Container;
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
            return ContainerSpecificContentFinder.Find<User>(Container).Where(u => u.NormalizedUsername).EqualTo(normalizedUserName) as object as User;
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.Username = userName;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);

            return IdentityResult.Success;
        }

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.Logins.Add(login);

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            user.Logins.Remove(user.Logins.Single(l => l.ProviderKey == providerKey));

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
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
            return user.Claims;
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            user.Claims.AddRange(claims);

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            var existing = user.Claims.SingleOrDefault(c => c.Type == claim.Type);

            if (existing != null)
            {
                user.Claims.Remove(existing);
            }

            user.Claims.Add(newClaim);
            
            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var types = claims.Select(c => c.Type);

            user.Claims.RemoveAll(c => types.Contains(c.Type));

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return user.PasswordHash != null;
        }

        public async Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            return user.SecurityStamp;
        }

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        {
            return user.TwoFactorEnabled;
        }

        public async Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
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

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return user.NormalizedEmail;
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        {
            return user.LockoutEndDate;
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEndDate = lockoutEnd;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount++;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);

            return user.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return user.AccessFailedCount;
        }

        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        {
            return user.LockoutEnabled;
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
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

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken)
        {
            user.AuthenticatorKey = key;

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task<string> GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken)
        {
            return user.AuthenticatorKey;
        }

        public async Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            user.TwoFactorRecoveryCodes = recoveryCodes.ToList();

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);
        }

        public async Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken)
        {
            if (!user.TwoFactorRecoveryCodes.Remove(code))
            {
                return false;
            }

            await ContainerSpecificContentUpdater.UpdateAsync(user, Container);

            return true;
        }

        public async Task<int> CountCodesAsync(User user, CancellationToken cancellationToken)
        {
            return user.TwoFactorRecoveryCodes.Count;
        }
    }
}
