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

  const {
    html,
    DropdownItem,
  } = dependencies;

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
    return html`
      <div class="alert alert-primary mx-2">
        <p>There was an error (<code>${error.response.status}${error.response.statusText ? " " + error.response.statusText : ""}</code>) loading your list${error.body ? ":" : "."}</p>
        ${error.body ? html`<small><pre>${error.body}</pre></small>` : ""}
        <p class="mb-0"><button class="btn btn-primary" onClick=${() => { setError(null); setTimeout(() => setRetryError(retryError + 1), 500); }}>Reload</button></p>
      </div>
    `;
  }

  if (loading) {
    return html`
      <div>
        ${[...new Array(pageSize)].map((_, i) => html`<${DropdownItem} disabled=${true} text=${i == 0 ? 'Loading ...' : html`<span class="nbsp" />`} />`)}
      </div>

      <ul class="pagination">
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

  const caret = html`<svg xmlns="http://www.w3.org/2000/svg" width="7" height="12" fill="none" viewBox="0 0 7 12" class="breadcrumb-caret"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m1 11 5-5-5-5"></path></svg>`;

  return html`
    <div class="media-picker-header">
      <div class="breadcrumb">
        <a class="breadcrumb-item" onClick=${() => setPathSegments([])}>Root ${caret}</a>
        ${pathSegments.map((segment, i) => html`<a class=${'breadcrumb-item' + (i == pathSegments.length - 1 ? ' active' : '')} onClick=${() => { if (i < pathSegments.length - 1) { setPathSegments(pathSegments.slice(0, i + 1)); } }}>
        ${segment} ${caret}
        </a>`)}
        <a class=${'breadcrumb-item' + (!pathSegments.length ? ' disabled' : '')} style="float: right;" onClick=${() => popPathSegment()} title="Back up one level">Back</a>
      </div>
    </div>
    <div>
      ${items.map(item =>
    html`<div>
          ${item.type == 'folder' ?
        html`<${DropdownItem} text=${html`<span class="media-picker-icon">📁</span> ${item.name}`} ellipsis=${true} onClick=${event => { pushPathSegment(item.value); setTimeout(() => event.target.blur(), 0) }} keepOpen=${true}/>` :
        html`<${DropdownItem} text=${html`<span class="media-picker-icon">📄</span> ${item.name}`} ellipsis=${true} active=${item.value == value} onClick=${event => { onSelect(item.value == value ? null : item.value); dependencies.closeDropdown(event.target); }} />`}
        </div>`
  )}
    </div>
    <div>
      ${[...new Array(pageSize - items.length)].map(() => html`<${DropdownItem} disabled=${true} nbsp=${true} />`)}
    </div>
    <div class="media-picker-footer">
      <ul class="pagination">
        <li class="page-item"><a class=${"page-link" + (page == 1 ? " disabled" : "")} onClick=${() => setPage(Math.max(1, page - 1))} title="Previous" tabindex="0" dangerouslySetInnerHTML=${{ __html: '&laquo;' }} /></li>
        ${pages.map((_, i) => html`<li class=${"page-item" + (page == i + 1 ? " active" : "")}><a class="page-link" onClick=${() => setPage(i + 1)} tabindex="0">${i + 1}</a></li>`)}
        <li class="page-item"><a class=${"page-link" + (page == pageCount ? " disabled" : "")} onClick=${() => setPage(Math.min(pageCount, page + 1))} title="Next" tabindex="0" dangerouslySetInnerHTML=${{ __html: '&raquo;' }} /></li>
        <li class="ms-auto">
          <div class="btn-group">
            <button type="button" class="button primary" onClick=${event => selectFile(event.target)}>Upload</button>
          </div>
        </li>
      </ul>
    </div>
`;
};