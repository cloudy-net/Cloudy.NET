import { route } from 'preact-router';
import { useContext, useEffect, useState } from 'preact/hooks';
import SearchBox from '../components/search-box';
import ListFilter from './list-filter';
import EntityListContext from './entity-list-context';
import html from '../util/html';
import Caret from "../assets/caret-horizontal.svg";

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
  const { settings, components, getResult, loadResult, parameters, updateParameter } = useContext(EntityListContext);
  const result = getResult(entityType);

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

  if (parameters.$loading) {
    return <>Loading settings</>;
  }

  const columnFn = {
    isEqual: (width) => width === LISTING_COLUMN_WIDTHS.EQUAL,
    isFill: (width) => width === LISTING_COLUMN_WIDTHS.FILL,
    getColumnWidthStyle: (width) => columnFn.isEqual(width) && !settings.columns.some(c => columnFn.isFill(c.width)) && settings.columns.filter(c => columnFn.isEqual(c.width)).length > 1
      ? { width: `${100 / (settings.columns.filter(c => columnFn.isEqual(c.width)).length || 1)}% ` }
      : {}
  };

  const setSorting = (newOrderBy) => {
    if (newOrderBy == parameters[entityType].orderBy) {
      setOrderByDirection(parameters[entityType].orderByDirection === SORT_DIRECTIONS.ASCENDING ? SORT_DIRECTIONS.DESCENDING : SORT_DIRECTIONS.ASCENDING);
    } else setOrderByDirection(SORT_DIRECTIONS.ASCENDING);

    updateParameter(entityType, { orderBy: newOrderBy });
  };

  useEffect(function () {
    if (settings.$loading) {
      return;
    }
    if (parameters.$loading) {
      return;
    }
    loadResult(entityType);
  }, [entityType, settings, parameters]);

  let content = null;

  if (result.error) {
    content = <div class="alert alert-primary">
      <p>There was an error (<code>{result.error.response.status}{result.error.response.statusText ? " " + result.error.response.statusText : ""}</code>) loading your list{result.error.body ? ":" : "."}</p>
      {result.error.body ? <small><pre>{result.error.body}</pre></small> : ""}
      {/* <p class="mb-0"><button class="btn btn-primary" onClick={() => { setError(null); setTimeout(() => setRetryError(retryError + 1), 500); }}>Reload</button></p> */}
    </div>;
  }

  if (result.$loading) {
    content = 'Loading ...';
  } else {
    content = <table class="expanded-entity-list-table">
      <thead>
        <tr className={`text-nowrap ${parameters[entityType].orderByDirection === SORT_DIRECTIONS.ASCENDING ? 'dropup' : ''}`}>
          {settings[entityType].columns.map(c =>
            c.sortable ?
              <th style={columnFn.getColumnWidthStyle(c.width)} className={`${COLUMN_WIDTH_CSS_CLASSES[c.width]} ${parameters[entityType].orderBy === c.name ? 'dropdown-toggle' : ''}`} role="button" onClick={() => setSorting(c.name)}>{c.label}</th> :
              <th style={columnFn.getColumnWidthStyle(c.width)} className={COLUMN_WIDTH_CSS_CLASSES[c.width]}>{c.label}</th>
          )}
        </tr>
      </thead>
      <tbody>
        {result.data.items.map(d => <tr>
          {settings[entityType].columns.map((column) =>
            d.value[column.name] && Object.keys(components).includes(column.partial) ?
              html`<td><${components[column.partial]} ...${{ keys: d.keys, value: d.value[column.name], settings: settings[entityType] }} dependencies=${{ html }} /></td>` :
              <td></td>
          )}
        </tr>)}
        {[...new Array(settings[entityType].pageSize - result.data.items.length)].map(() => <tr class="blank-row"><td /></tr>)}
      </tbody>
    </table>;
  }

  return <div class={"layout-navigation-panel expanded"}>
    <div class="list-page-header m-2">
      <div class="list-page-search">
        <SearchBox callback={value => updateParameter(entityType, { search: value })} floating={parameters[entityType].filters.length} autoFocus={true} />
      </div>
      {settings[entityType].filters.map(c => <ListFilter {...c} filter={(key, value) => {
        if (!value) {
          var newFilters = { ...parameters[entityType].filters };

          delete newFilters[key];

          updateParameter(entityType, { filters: newFilters });
          return;
        }

        updateParameter(entityType, { filters: { ...parameters[entityType].filters, [key]: value } });
      }} />)}
    </div>
    <div class="table-responsive">
      {content}
      {result.pages && <nav>
        <ul class="pagination expanded">
          <li class="page-item"><a class={"page-link" + (parameters[entityType].page == 1 ? " disabled" : "")} onClick={() => updateParameter(entityType, { page: Math.max(1, parameters[entityType].page - 1) })} title="Previous"><Caret class="page-previous-caret" /></a></li>
          {result.pages.map((_, i) => <li class={"page-item" + (parameters[entityType].page == i + 1 ? " active" : "")}><a class="page-link" onClick={() => updateParameter(entityType, { page: i + 1 })}>{i + 1}</a></li>)}
          <li class="page-item"><a class={"page-link" + (parameters[entityType].page == result.pageCount ? " disabled" : "")} onClick={() => updateParameter(entityType, { page: Math.min(result.pageCount, parameters[entityType].page + 1) })} title="Next"><Caret class="page-next-caret" /></a></li>
        </ul>
      </nav>}
    </div>
  </div>;
}
