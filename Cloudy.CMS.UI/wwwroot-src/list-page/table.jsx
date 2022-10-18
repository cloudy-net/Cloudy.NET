import { useEffect, useState } from 'preact/hooks';
import SearchBox from '../components/search-box';

export default ({ ContentType, Columns, PageSize, EditLink, DeleteLink }) => {
  const [pageSize, setPageSize] = useState(PageSize);
  const [page, setPage] = useState(1);
  const [pageCount, setPageCount] = useState();
  const [pages, setPages] = useState();
  const [columns, setColumns] = useState(Columns);
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState();
  const [error, setError] = useState();
  const [retryError, setRetryError] = useState(0);
  const [search, setSearch] = useState('');

  useEffect(function () {
    (async () => {
      const response = await fetch(`/Admin/api/list/result?contentType=${ContentType}&columns=${columns.map(c => c.Name).join(',')}&pageSize=${pageSize}&page=${page}&search=${search}`);

      if (!response.ok) {
        setError({ response, body: await response.text() });
        return;
      }

      var json = await response.json();

      setLoading(false);
      setData(json);
      const pageCount = Math.ceil(json.totalCount / pageSize);
      setPageCount(pageCount);
      setPages([...Array(pageCount)]);
    })();
  }, [page, pageSize, Columns, retryError, search]);

  if (error) {
    return <>
      <div class="alert alert-primary">
        <p>There was an error (<code>{error.response.status}{error.response.statusText ? " " + error.response.statusText : ""}</code>) loading your list{error.body ? ":" : "."}</p>
        {error.body ? <pre>{error.body}</pre> : ""}
        <p class="mb-0"><button class="btn btn-primary" onClick={() => { setError(null); setTimeout(() => setRetryError(retryError + 1), 500); }}>Reload</button></p>
      </div>
    </>;
  }

  if (loading) {
    return <>Loading ...</>;
  }

  if (!data) {
    return <>Could not load data</>;
  }

  return <>
    <SearchBox className="list-page-search" callback={value => setSearch(value)} />
    <div class="table-responsive">
      <table class="table">
        <thead>
          <tr>
            {columns.map(c => <th>{c.Label}</th>)}
            <th style="width: 1%;"></th>
          </tr>
        </thead>
        <tbody>
          {data.items.map(d => <tr>
            {columns.map((_, i) =>
              <td dangerouslySetInnerHTML={{__html:d.values[i]}}></td>
            )}
            <td>
              <a class="me-2" href={`${EditLink}${d.keys.map(k => `&keys=${k}`).join('&')}`}>Edit</a>
              <a href={`${DeleteLink}${d.keys.map(k => `&keys=${k}`).join('&')}`}>Delete</a>
            </td>
          </tr>)}
          {[...new Array(pageSize - data.items.length)].map(() => <tr class="list-page-blank-row"><td>&nbsp;</td></tr>)}
        </tbody>
      </table>
      <nav>
        <ul class="pagination justify-content-center">
          <li class="page-item"><a class={"page-link" + (page == 1 ? " disabled" : "")} onClick={() => setPage(Math.max(1, page - 1))}>Previous</a></li>
          {pages.map((_, i) => <li class={"page-item" + (page == i + 1 ? " active" : "")}><a class="page-link" onClick={() => setPage(i + 1)}>{i + 1}</a></li>)}
          <li class="page-item"><a class={"page-link" + (page == pageCount ? " disabled" : "")} onClick={() => setPage(Math.min(pageCount, page + 1))}>Next</a></li>
        </ul>
      </nav>
    </div>
  </>;
}
