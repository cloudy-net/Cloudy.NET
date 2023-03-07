import { route } from 'preact-router';
import html from '@src/html-init.js';
import { useEffect, useState } from 'preact/hooks';
import SearchBox from '../components/search-box.js';
import ListFilter from './list-filter.js';
import Card from '@src/layout/card.jsx';
import ColumnComponentProvider from './column-component-provider';
import TableBody from './table-body';

export const SORT_DIRECTIONS = {
  ASCENDING: 'asc',
  DESCENDING: 'desc',
};

export const COLUMN_WIDTH_CSS_CLASSES = {
  'Default': '',
  'Equal': '',
  'Fill': 'w-100',
};

export const LISTING_COLUMN_WIDTHS = {
  DEFAULT: 'Default',
  FILL: 'Fill',
  EQUAL: 'Equal',
};

export default ({ entityType }) => {

  const [settings, setSettings] = useState({ loading: true });
  const [pageSize, setPageSize] = useState();
  const [page, setPage] = useState(1);
  const [pageCount, setPageCount] = useState();
  const [pages, setPages] = useState();
  const [columns, setColumns] = useState();
  const [filterOptions, setFilterOptions] = useState([]);
  const [filters, setFilters] = useState([]);
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
    setSettings({ loading: true });
    (async () => {
      await fetch(`/Admin/api/list/settings?entityTypeName=${entityType}`, { credentials: 'include' })
        .then(r => r.json())
        .then(r => {
          if (r.redirectUrl) {
            route(r.redirectUrl);
            return;
          }
          setPageSize(r.pageSize);
          setColumns(r.columns);
          setFilterOptions(r.filters);
          setSettings(r);
        });
    })();
  }, [entityType]);

  useEffect(function () {
    if (settings.loading) return;
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
  }, [settings, page, pageSize, columns, filters, retryError, search, orderBy, orderByDirection]);

  let content = null;

  if (error) {
    content = <div class="alert alert-primary">
      <p>There was an error (<code>{error.response.status}{error.response.statusText ? " " + error.response.statusText : ""}</code>) loading your list{error.body ? ":" : "."}</p>
      {error.body ? <small><pre>{error.body}</pre></small> : ""}
      <p class="mb-0"><button class="btn btn-primary" onClick={() => { setError(null); setTimeout(() => setRetryError(retryError + 1), 500); }}>Reload</button></p>
    </div>;
  }

  if (loading) {
    content = 'Loading ...';
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
        {html`<${ColumnComponentProvider} componentPartials=${[... new Set(data.items.map(i => i.values.map(v => v.partial)).flat(1))]}>
          <${TableBody} ...${{ items: data.items, columns, pageSize, settings }} />
        </>`}
      </table>
    </div>;
  }

  return <>
    <div class="list-page-header m-2">
      <div class="list-page-search">
        <SearchBox callback={value => setSearch(value)} floating={filters.length} />
      </div>
      {filterOptions.map(c => html`<${ListFilter} ...${c} filter=${(key, value) => {
        if (!value) {
          var newFilters = { ...filters };

          delete newFilters[key];

          setFilters(newFilters);
          return;
        }

        setFilters({ ...filters, [key]: value });
      }} />`)}
    </div>
    <div class="table-responsive">
      {content}
      {pages && html`<nav>
          <ul class="pagination justify-content-center">
            <li class="page-item"><a class=${"page-link" + (page == 1 ? " disabled" : "")} onClick=${() => setPage(Math.max(1, page - 1))}>Previous</a></li>
            ${pages.map((_, i) => html`<li class=${"page-item" + (page == i + 1 ? " active" : "")}><a class="page-link" onClick=${() => setPage(i + 1)}>${i + 1}</a></li>`)}
            <li class="page-item"><a class=${"page-link" + (page == pageCount ? " disabled" : "")} onClick=${() => setPage(Math.min(pageCount, page + 1))}>Next</a></li>
          </ul>
        </nav>`}
    </div>
  </>
    ;
}
