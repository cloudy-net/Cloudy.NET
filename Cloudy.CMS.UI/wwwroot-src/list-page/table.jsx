import { useEffect, useState } from '../preact-htm/standalone.module';
import SearchBox from '../components/search-box';
import ListFilter from './list-filter';
import { COLUMN_WIDTH_CSS_CLASSES, LISTING_COLUMN_WIDTHS, SORT_DIRECTIONS } from '@constants';

export default ({ entityType, columns: initialColumns, filters: listFilters, pageSize: initialPageSize, editLink, deleteLink }) => {
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
  const [orderBy, setOrderBy] = useState('');
  const [orderByDirection, setOrderByDirection] = useState(SORT_DIRECTIONS.ASCENDING);

  const columnFn = {
    isEqual: (width) => width === LISTING_COLUMN_WIDTHS.EQUAL,
    isFill: (width) => width === LISTING_COLUMN_WIDTHS.FILL,
    getColumnWidthStyle: (width) => columnFn.isEqual(width) && !columns.some(c => columnFn.isFill(c.width)) && columns.filter(c => columnFn.isEqual(c.width)).length > 1
    ? { width: `${100 / (columns.filter(c => columnFn.isEqual(c.width)).length || 1)}% ` }
    : {}
  };

  const setSorting = (newOrderBy) => {
    if (newOrderBy == orderBy) {
      setOrderByDirection(orderByDirection === SORT_DIRECTIONS.ASCENDING ? SORT_DIRECTIONS.DESCENDING : SORT_DIRECTIONS.ASCENDING);
    } else setOrderByDirection(SORT_DIRECTIONS.ASCENDING);

    setOrderBy(newOrderBy);
  };

  useEffect(function () {
    (async () => {
      setError(null);

      const response = await fetch(
        `/Admin/api/list/result?entityType=${entityType}&columns=${columns.map(c => c.name).join(',')}&${Object.entries(filters).map(([key, value]) => `filters[${key}]=${encodeURIComponent(Array.isArray(value) ? JSON.stringify(value) : value)}`).join("&")}&pageSize=${pageSize}&page=${page}&search=${search}&orderBy=${orderBy}&orderByDirection=${orderByDirection}`,
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
  }, [page, pageSize, columns, filters, retryError, search, orderBy, orderByDirection]);

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
  } else {
    content = <div class="table-responsive">
      <table class="table table--content-list">
        <thead>
          <tr className={`text-nowrap ${orderByDirection === SORT_DIRECTIONS.ASCENDING ? 'dropup' : ''}`}>
            {columns.map(c => c.sortable
              ? <th style={columnFn.getColumnWidthStyle(c.width)} className={`${COLUMN_WIDTH_CSS_CLASSES[c.width]} ${orderBy === c.name ? 'dropdown-toggle' : ''}`} role="button" onClick={() => setSorting(c.name)}>{c.label}</th>
              : <th style={columnFn.getColumnWidthStyle(c.width)} className={COLUMN_WIDTH_CSS_CLASSES[c.width]}>{c.label}</th>
            )}
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
      </table>
    </div>;
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
