import { useEffect, useState } from 'preact/hooks';
import SearchBox from '../components/search-box';
import ListFilter from './list-filter';

export default ({ contentType, columns: initialColumns, filters: listFilters, pageSize: initialPageSize, editLink, deleteLink }) => {
  const [pageSize, setPageSize] = useState(initialPageSize);
  const [page, setPage] = useState(1);
  const [pageCount, setPageCount] = useState();
  const [pages, setPages] = useState();
  const [columns, setColumns] = useState(initialColumns);
  const [filters, setFilters] = useState({});
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState();
  const [error, setError] = useState();
  const [retryError, setRetryError] = useState(0);
  const [search, setSearch] = useState('');

  useEffect(function () {
    (async () => {
      setError(null);

      const response = await fetch(
        `/Admin/api/list/result?contentType=${contentType}&columns=${columns.map(c => c.name).join(',')}&${Object.entries(filters).map(([key, value]) => `filters[${key}]=${encodeURIComponent(Array.isArray(value) ? JSON.stringify(value) : value)}`).join("&")}&pageSize=${pageSize}&page=${page}&search=${search}`,
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
      const pageCount = Math.ceil(json.totalCount / pageSize);
      setPageCount(pageCount);
      setPages([...Array(pageCount)]);
    })();
  }, [page, pageSize, columns, filters, retryError, search]);

  let content = null;

  if (error) {
    content = <>
      <div class="alert alert-primary">
        <p>There was an error (<code>{error.response.status}{error.response.statusText ? " " + error.response.statusText : ""}</code>) loading your list{error.body ? ":" : "."}</p>
        {error.body ? <small><pre>{error.body}</pre></small> : ""}
        <p class="mb-0"><button class="btn btn-primary" onClick={() => { setError(null); setTimeout(() => setRetryError(retryError + 1), 500); }}>Reload</button></p>
      </div>
    </>;
  }

  if (loading) {
    content = <>Loading ...</>;
  }

  if (!data) {
    content = <>Could not load data</>;
  }

  if (content == null) {
    content = <table class="table">
      <thead>
        <tr>
          {columns.map(c => <th>{c.label}</th>)}
          <th style="width: 1%;"></th>
        </tr>
      </thead>
      <tbody>
        {data.items.map(d => <tr>
          {columns.map((_, i) =>
            <td dangerouslySetInnerHTML={{ __html: d.values[i] }}></td>
          )}
          <td>
            <a class="me-2" href={`${editLink}${d.keys.map(k => `&keys=${k}`).join('&')}`}>Edit</a>
            <a href={`${deleteLink}${d.keys.map(k => `&keys=${k}`).join('&')}`}>Delete</a>
          </td>
        </tr>)}
        {[...new Array(pageSize - data.items.length)].map(() => <tr class="list-page-blank-row"><td>&nbsp;</td></tr>)}
      </tbody>
    </table>;
  }

  return <>
    <div class="list-page-header m-2">
      <div class="list-page-search">
        <SearchBox callback={value => setSearch(value)} floating={listFilters.length} />
      </div>
      {listFilters.map(c => <ListFilter {...c} filter={(key, value) => {
        if (!value) {
          var newFilters = { ...filters };

          delete newFilters[key];

          setFilters(newFilters);
          return;
        }

        setFilters({ ...filters, [key]: value });
      }} />)}
    </div>
    <div class="table-responsive">
      {content}
      {pages && <nav>
        <ul class="pagination justify-content-center">
          <li class="page-item"><a class={"page-link" + (page == 1 ? " disabled" : "")} onClick={() => setPage(Math.max(1, page - 1))}>Previous</a></li>
          {pages.map((_, i) => <li class={"page-item" + (page == i + 1 ? " active" : "")}><a class="page-link" onClick={() => setPage(i + 1)}>{i + 1}</a></li>)}
          <li class="page-item"><a class={"page-link" + (page == pageCount ? " disabled" : "")} onClick={() => setPage(Math.min(pageCount, page + 1))}>Next</a></li>
        </ul>
      </nav>}
    </div>
  </>;
}
