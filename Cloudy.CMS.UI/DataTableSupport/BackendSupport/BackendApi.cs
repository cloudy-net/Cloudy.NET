using Poetry.UI.ApiSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.DataTableSupport.BackendSupport
{
    [Api("Backend")]
    public class BackendApi
    {
        IBackendProvider BackendProvider { get; }

        public BackendApi(IBackendProvider backendProvider)
        {
            BackendProvider = backendProvider;
        }

        [Endpoint("GetAll")]
        public Result GetAll(string provider, int page, string sortBy, string sortDirection)
        {
            var direction = sortDirection == "ascending" ? (SortDirection?)SortDirection.Ascending : sortDirection == "descending" ? (SortDirection?)SortDirection.Descending : null;

            var backend = BackendProvider.GetFor(provider);

            if(backend == null)
            {
                throw new BackendNotFoundException(provider);
            }

            return backend.Load(new Query(page, sortBy, direction));
        }
    }
}
