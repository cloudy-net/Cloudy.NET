import { route } from 'preact-router';
import { useContext, useEffect, useState } from 'preact/hooks';
import SearchBox from '../components/search-box';
import ListFilter from './list-filter';
import EntityListContext from './entity-list-context';
import html from '@src/util/html.js';
import { ReactComponent as Caret } from "../assets/caret-horizontal.svg";
import { ReactComponent as VerticalCaret } from "../assets/caret-vertical.svg";
import { ReactComponent as Kebab } from "../assets/kebab.svg";
import { ReactComponent as Edit } from "../assets/icon-edit.svg";
import { ReactComponent as Trash } from "../assets/icon-trash.svg";
import { ReactComponent as Filter } from "../assets/icon-filter.svg";
import { ReactComponent as FilterActive } from "../assets/icon-filter-active.svg";
import Dropdown from '../components/dropdown';
import DropdownItem from '../components/dropdown-item';
import arrayEquals from '../util/array-equals';

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

export default ({ entityType, keyValues }) => {
  const { settings, components, getResult, loadResult, parameters, updateParameter } = useContext(EntityListContext);
  const result = getResult(entityType);

  const [filtersOpen, setFiltersOpen] = useState();

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

  const activeFilters = Object.keys(parameters[entityType].filters).length;

  return <div class="layout-navigation-panel">
    <div className="compact-entity-list-header">
      <SearchBox callback={value => updateParameter(entityType, { search: value })} />
      <div class="compact-list-filter-panel">
        <a className="compact-list-filter-button" tabIndex="1" onClick={() => setFiltersOpen(!filtersOpen)}>
          {activeFilters ? <FilterActive className="compact-list-filter-button-icon" /> : <Filter className="compact-list-filter-button-icon" />}
          <span className="compact-list-filter-button-text">Filters {activeFilters ? `(${activeFilters} active)` : ''}</span>
          <VerticalCaret className={"compact-list-filter-button-caret" + (filtersOpen ? " open" : "")} />
        </a>
        {filtersOpen &&
          <div className="compact-list-filters">
            {settings[entityType].filters.map(c => <ListFilter {...c} filter={(key, value) => {
              if (!value) {
                var newFilters = { ...parameters[entityType].filters };

                delete newFilters[key];

                updateParameter(entityType, { filters: newFilters });
                return;
              }

              updateParameter(entityType, { filters: { ...parameters[entityType].filters, [key]: value } });
            }} />)}
          </div>}
      </div>
    </div>
    {
      result.$loading ?
        'Loading ...' :
        <div className="compact-entity-list-items">
          {result.data.items.map(d =>
            settings[entityType].columns.filter(column => column.showInCompactView).map((column) =>
              d.value[column.name] && Object.keys(components).includes(column.partial) ?
                <div class={"compact-entity-list-item" + (arrayEquals(keyValues, d.keys) ? " active" : "")}>
                  {html`<${components[column.partial]} ...${{ keys: d.keys, value: d.value[column.name], settings: settings[entityType] }} dependencies=${{ html }} />`}
                  <Dropdown className="compact-entity-list-item-menu" contents={<Kebab />}>
                    <DropdownItem href={`${settings[entityType].editLink}?${d.keys.map(k => `keys=${k}`).join('&')}`} text="Edit" icon={<Edit />} />
                    <DropdownItem text="Delete" icon={<Trash />} />
                  </Dropdown>
                </div> :
                <></>
            )
          )}
        </div>
    }
    {result.pages && <nav>
      <ul class="pagination center">
        <li class="page-item"><a class={"page-link" + (parameters[entityType].page == 1 ? " disabled" : "")} onClick={() => updateParameter(entityType, { page: Math.max(1, parameters[entityType].page - 1) })} title="Previous"><Caret class="page-previous-caret" /></a></li>
        {result.pages.map((_, i) => <li class={"page-item" + (parameters[entityType].page == i + 1 ? " active" : "")}><a class="page-link" onClick={() => updateParameter(entityType, { page: i + 1 })}>{i + 1}</a></li>)}
        <li class="page-item"><a class={"page-link" + (parameters[entityType].page == result.pageCount ? " disabled" : "")} onClick={() => updateParameter(entityType, { page: Math.min(result.pageCount, parameters[entityType].page + 1) })} title="Next"><Caret class="page-next-caret" /></a></li>
      </ul>
    </nav>}
  </div>;
}
