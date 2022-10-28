import { useEffect, useState } from "preact/hooks";

export default ({ provider, value, onSelect }) => {
  const [pageSize] = useState(10);
  const [page, setPage] = useState(1);
  const [pageCount, setPageCount] = useState();
  const [pages, setPages] = useState();
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState();
  const [path, setPath] = useState('');
  const [error, setError] = useState();
  const [retryError, setRetryError] = useState(0);

  useEffect(function () {
    (async () => {
      var response = await fetch(`/Admin/api/controls/mediapicker/list?provider=${provider}&path=${encodeURIComponent(path)}`);

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
  }, [path, retryError]);

  if (error) {
    return <>
      <div class="alert alert-primary mx-2">
        <p>There was an error (<code>{error.response.status}{error.response.statusText ? " " + error.response.statusText : ""}</code>) loading your list{error.body ? ":" : "."}</p>
        {error.body ? <small><pre>{error.body}</pre></small> : ""}
        <p class="mb-0"><button class="btn btn-primary" onClick={() => { setError(null); setTimeout(() => setRetryError(retryError + 1), 500); }}>Reload</button></p>
      </div>
    </>;
  }

  if (loading) {
    return <>
      <div>
        {[...new Array(pageSize)].map((_, i) => <div><a class="dropdown-item disabled">{i == 0 ? 'Loading ...' : <>&nbsp;</>}</a></div>)}
      </div>

      <ul class="pagination pagination-sm m-0 mt-2 invisible">
        <li class="page-item"><a class="page-link">&nbsp;</a></li>
      </ul>
    </>;
  }

  const skip = (page - 1) * pageSize;
  const items = data.items.slice(skip, skip + pageSize);

  const getPreviousPath = path => {
    const segments = path.split("/");
    segments.splice(segments.length - 2, 2);
    const result = segments.join("/")

    return result == '' ? '' : result + '/';
  };

  return <>
    <div>
      {items.map(item =>
        <div>
          {item.type == 'folder' ?
            <a class="dropdown-item" onClick={event => { setPath(item.value); setTimeout(() => event.target.blur(), 0) }} tabIndex="0">
              <span class="list-icon">📁</span>
              {item.name}
            </a> :
            <a class={"dropdown-item" + (item.value == value ? " active" : "")} onClick={() => { onSelect(item.value == value ? null : item.value); }} tabIndex="0">
              <span class="list-icon">📄</span>
              {item.name}
            </a>}
        </div>
      )}
    </div>
    <div>
      {[...new Array(pageSize - items.length)].map(() => <div><a class="dropdown-item disabled">&nbsp;</a></div>)}
    </div>
    <nav>
      <ul class="pagination pagination-sm justify-content-center m-0 mt-2">
        <li class="page-item"><a class={"page-link" + (page == 1 ? " disabled" : "")} onClick={() => setPage(Math.max(1, page - 1))} title="Previous" tabindex="0">&laquo;</a></li>
        {path && <li class="page-item"><a class="page-link" onClick={() => setPath(getPreviousPath(path))} title="Back up one level" tabindex="0">🔙</a></li>}
        {pages.map((_, i) => <li class={"page-item" + (page == i + 1 ? " active" : "")}><a class="page-link" onClick={() => setPage(i + 1)} tabindex="0">{i + 1}</a></li>)}
        <li class="page-item"><a class={"page-link" + (page == pageCount ? " disabled" : "")} onClick={() => setPage(Math.min(pageCount, page + 1))} title="Next" tabindex="0">&raquo;</a></li>
      </ul>
    </nav>
  </>;
};