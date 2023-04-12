import { route } from 'preact-router';
import { useContext, useEffect, useState } from 'preact/hooks';
import SearchBox from '../components/search-box';
import ListFilter from './list-filter';
import EntityListContext from './entity-list-context';
import html from '@src/util/html.js';

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
  const { settings, components, getResult, loadResult } = useContext(EntityListContext);
  const result = getResult(entityType);

  const [page, setPage] = useState(result.page);
  const [filters, setFilters] = useState(result.filters);
  const [search, setSearch] = useState(result.search);
  const [orderBy, setOrderBy] = useState(result.orderBy);
  const [orderByDirection, setOrderByDirection] = useState(result.orderByDirection);

  if (settings.$loading) {
    return <>Loading settings</>;
  }
  if (!settings[entityType]) {
    return <>No such entity type found</>;
  }
  if (settings[entityType].redirectUrl) {
    route(settings[entityType].redirectUrl);
    return;
  }

  const columnFn = {
    isEqual: (width) => width === LISTING_COLUMN_WIDTHS.EQUAL,
    isFill: (width) => width === LISTING_COLUMN_WIDTHS.FILL,
    getColumnWidthStyle: (width) => columnFn.isEqual(width) && !settings.columns.some(c => columnFn.isFill(c.width)) && settings.columns.filter(c => columnFn.isEqual(c.width)).length > 1
      ? { width: `${100 / (settings.columns.filter(c => columnFn.isEqual(c.width)).length || 1)}% ` }
      : {}
  };

  const setSorting = (newOrderBy) => {
    if (newOrderBy == orderBy) {
      setOrderByDirection(orderByDirection === SORT_DIRECTIONS.ASCENDING ? SORT_DIRECTIONS.DESCENDING : SORT_DIRECTIONS.ASCENDING);
    } else setOrderByDirection(SORT_DIRECTIONS.ASCENDING);

    setOrderBy(newOrderBy);
  };

  useEffect(function () {
    if(settings.$loading) {
      return;
    }
    loadResult(entityType, page, filters, search, orderBy, orderByDirection);
  }, [entityType, settings, page, filters, search, orderBy, orderByDirection]);

  let content = null;

  if (result.error) {
    content = <div class="alert alert-primary">
      <p>There was an error (<code>{result.error.response.status}{result.error.response.statusText ? " " + result.error.response.statusText : ""}</code>) loading your list{result.error.body ? ":" : "."}</p>
      {result.error.body ? <small><pre>{result.error.body}</pre></small> : ""}
      <p class="mb-0"><button class="btn btn-primary" onClick={() => { setError(null); setTimeout(() => setRetryError(retryError + 1), 500); }}>Reload</button></p>
    </div>;
  }

  if (result.$loading) {
    content = 'Loading ...';
  } else {
    content = <div class="table-responsive">
      <table class="table table--content-list">
        <thead>
          <tr className={`text-nowrap ${orderByDirection === SORT_DIRECTIONS.ASCENDING ? 'dropup' : ''}`}>
            {settings[entityType].columns.map(c => c.sortable
              ? <th style={columnFn.getColumnWidthStyle(c.width)} className={`${COLUMN_WIDTH_CSS_CLASSES[c.width]} ${orderBy === c.name ? 'dropdown-toggle' : ''}`} role="button" onClick={() => setSorting(c.name)}>{c.label}</th>
              : <th style={columnFn.getColumnWidthStyle(c.width)} className={COLUMN_WIDTH_CSS_CLASSES[c.width]}>{c.label}</th>
            )}
          </tr>
        </thead>
        <tbody>
          {result.data.items.map(d => <tr>
            {settings[entityType].columns.map((column) =>
              d.value[column.name]
              && Object.keys(components).includes(column.partial)
              && html`<td><${components[column.partial]} ...${{ keys: d.keys, value: d.value[column.name], settings: settings[entityType] }} dependencies=${{ html }} /></td>`
            )}
          </tr>)}
          {[...new Array(settings[entityType].pageSize - result.data.items.length)].map(() => <tr class="list-page-blank-row"><td class="nbsp" /></tr>)}
        </tbody>
      </table>
    </div>;
  }

  return <>
    <div class="list-page-header m-2">
      <div class="list-page-search">
        <SearchBox callback={value => setSearch(value)} floating={filters.length} />
      </div>
      {settings[entityType].filters.map(c => <ListFilter {...c} filter={(key, value) => {
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
      {result.pages && <nav>
        <ul class="pagination justify-content-center">
          <li class="page-item"><a class={"page-link" + (result.page == 1 ? " disabled" : "")} onClick={() => setPage(Math.max(1, result.page - 1))}>Previous</a></li>
          {result.pages.map((_, i) => <li class={"page-item" + (result.page == i + 1 ? " active" : "")}><a class="page-link" onClick={() => setPage(i + 1)}>{i + 1}</a></li>)}
          <li class="page-item"><a class={"page-link" + (result.page == result.pageCount ? " disabled" : "")} onClick={() => setPage(Math.min(result.pageCount, result.page + 1))}>Next</a></li>
        </ul>
      </nav>}
    </div>
  </>;
}
