export default ({ provider, value, onSelect, dependencies }) => {
  const [pageSize] = dependencies.useState(10);
  const [page, setPage] = dependencies.useState(1);
  const [pageCount, setPageCount] = dependencies.useState();
  const [pages, setPages] = dependencies.useState();
  const [loading, setLoading] = dependencies.useState(true);
  const [data, setData] = dependencies.useState();
  const [pathSegments, setPathSegments] = dependencies.useState([]);
  const [error, setError] = dependencies.useState();
  const [retryError, setRetryError] = dependencies.useState(0);
  const [refresh, setRefresh] = dependencies.useState(0);

  dependencies.useEffect(function () {
    (async () => {
      var response = await fetch(
        `/Admin/api/controls/mediapicker/list?provider=${provider}&path=${pathSegments.length ? encodeURIComponent(pathSegments.join('/')) + '/' : ''}`,
        {
          credentials: 'include'
        }
      );

      if (!response.ok) {
        setError({ response, body: await response.text() });
        return;
      }

      var json = await response.json();

      setLoading(false);
      setData(json);
      const pageCount = Math.max(1, Math.ceil(json.items.length / pageSize));
      setPageCount(pageCount);
      setPages([...Array(pageCount)]);
      setPage(Math.min(pageCount, page)); // if filtered results have less pages than what is on the current page
    })();
  }, [pathSegments, retryError, refresh]);

  if (error) {
    return dependencies.html`
      <div class="alert alert-primary mx-2">
        <p>There was an error (<code>${error.response.status}${error.response.statusText ? " " + error.response.statusText : ""}</code>) loading your list${error.body ? ":" : "."}</p>
        ${error.body ? dependencies.html`<small><pre>${error.body}</pre></small>` : ""}
        <p class="mb-0"><button class="btn btn-primary" onClick=${() => { setError(null); setTimeout(() => setRetryError(retryError + 1), 500); }}>Reload</button></p>
      </div>
    `;
  }

  if (loading) {
    return dependencies.html`
      <div>
        ${[...new Array(pageSize)].map((_, i) => dependencies.html`<div><a class="dropdown-item disabled">${i == 0 ? 'Loading ...' : dependencies.html`<span class="nbsp" />`}</a></div>`)}
      </div>

      <ul class="pagination pagination-sm m-0 mt-2 invisible">
        <li class="page-item"><a class="page-link nbsp" /></li>
      </ul>
    `;
  }

  const skip = (page - 1) * pageSize;
  const items = data.items.slice(skip, skip + pageSize);

  const pushPathSegment = segment => {
    setPathSegments([...pathSegments, segment]);
  };

  const popPathSegment = () => {
    const segments = [...pathSegments];

    segments.pop();

    setPathSegments(segments);
  };

  const selectFile = element => {
    let input = document.createElement('input');
    input.type = 'file';
    input.onchange = async () => {
      const data = new FormData();
      data.append('file', input.files[0]);
      data.append('path', pathSegments.join('/'));

      const response = await fetch(`/Admin/api/controls/mediapicker/upload?provider=${provider}`, {
        method: 'POST',
        body: data,
        credentials: 'include'
      });

      if (!response.ok) {
        setError({ response, body: await response.text() });
        return;
      }

      const json = await response.json();

      onSelect(json.path);
      dependencies.closeDropdown(element);
    };
    input.click();
  }

  return dependencies.html`
    <div class="media-picker-header">
      <ol class="breadcrumb">
        <li class="breadcrumb-item"><a onClick=${() => setPathSegments([])}>Root</a></li>
        ${pathSegments.map((segment, i) =>
          dependencies.html`<li class=${'breadcrumb-item' + (i == pathSegments.length - 1 ? ' active' : '')}>
            ${i < pathSegments.length - 1 ?
              dependencies.html`<a onClick=${() => setPathSegments(pathSegments.slice(0, i + 1))}>${segment}</a>` :
              dependencies.html`${segment}`}
          </li>`
        )}

        <li class="media-picker-header-back"><a class=${'btn btn-sm' + (!pathSegments.length ? ' disabled' : '')} onClick=${() => popPathSegment()} title="Back up one level">Back</a></li>
      </ol>
    </div>
    <div>
      ${items.map(item =>
        dependencies.html`<div>
          ${item.type == 'folder' ?
          dependencies.html`<a class="dropdown-item" onClick=${event => { pushPathSegment(item.value); setTimeout(() => event.target.blur(), 0) }} tabIndex="0">
              <span class="media-picker-icon">📁</span>
              ${item.name}
            </a>` :
            dependencies.html`<a class=${"dropdown-item" + (item.value == value ? " active" : "")} onClick=${event => { onSelect(item.value == value ? null : item.value); dependencies.closeDropdown(event.target); }} tabIndex="0">
              <span class="media-picker-icon">📄</span>
              ${item.name}
            </a>`}
        </div>`
      )}
    </div>
    <div>
      ${[...new Array(pageSize - items.length)].map(() => dependencies.html`<div><a class="dropdown-item disabled nbsp" /></div>`)}
    </div>
    <div class="media-picker-footer">
      <ul class="pagination pagination-sm">
        <li class="page-item"><a class=${"page-link" + (page == 1 ? " disabled" : "")} onClick=${() => setPage(Math.max(1, page - 1))} title="Previous" tabindex="0" dangerouslySetInnerHTML=${{__html: '&laquo;'}} /></li>
        ${pages.map((_, i) => dependencies.html`<li class=${"page-item" + (page == i + 1 ? " active" : "")}><a class="page-link" onClick=${() => setPage(i + 1)} tabindex="0">${i + 1}</a></li>`)}
        <li class="page-item"><a class=${"page-link" + (page == pageCount ? " disabled" : "")} onClick=${() => setPage(Math.min(pageCount, page + 1))} title="Next" tabindex="0" dangerouslySetInnerHTML=${{__html: '&raquo;'}} /></li>
        <li class="ms-auto">
          <div class="btn-group">
            <button type="button" class="btn btn-sm btn-primary" onClick=${event => selectFile(event.target)}>Upload</button>
          </div>
        </li>
      </ul>
    </div>
  `;
};