using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    public class ContentResponseMessage
    {
        public bool Success { get; }
        public string Message { get; }
        public IDictionary<string, IEnumerable<string>> ValidationErrors { get; }

        public ContentResponseMessage(bool success)
        {
            Success = success;
            ValidationErrors = new ReadOnlyDictionary<string, IEnumerable<string>>(new Dictionary<string, IEnumerable<string>>());
        }

        public ContentResponseMessage(bool success, string message)
        {
            Success = success;
            Message = message;
            ValidationErrors = new ReadOnlyDictionary<string, IEnumerable<string>>(new Dictionary<string, IEnumerable<string>>());
        }

        public ContentResponseMessage(IDictionary<string, IEnumerable<string>> validationErrors)
        {
            Success = false;
            Message = "Validation failed";
            ValidationErrors = new ReadOnlyDictionary<string, IEnumerable<string>>(new Dictionary<string, IEnumerable<string>>(validationErrors));
        }

        public static ContentResponseMessage CreateFrom(ModelStateDictionary modelState)
        {
            return new ContentResponseMessage(modelState.ToDictionary(i => i.Key, i => i.Value.Errors.Select(e => e.ErrorMessage)));
        }
    }
}
