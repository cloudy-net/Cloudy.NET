using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cloudy.CMS.Licensing
{
    internal class LicenseValidator : ILicenseValidator
    {
        private bool? validationResult;
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LicenseValidator(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> IsValidAsync(string licenseKey)
        {
            if (validationResult.HasValue) return validationResult.Value;

            validationResult = ShouldValidate()
                ? await ValidateLicenseKey(licenseKey)
                : true;

            return validationResult.Value;
        }

        private async Task<bool> ValidateLicenseKey(string licenseKey) 
        {
            if (string.IsNullOrEmpty(licenseKey)) return false;

            try
            {
                var response = await httpClient.PostAsync($"/api/Validate?key={licenseKey}", null);
                
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private bool ShouldValidate()
        {
            return !httpContextAccessor.HttpContext?.Request?.Host.Host?.Equals("localhost") ?? true;
        }
    }
}
