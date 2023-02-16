import { html, useRef, useEffect, useState } from '../../preact-htm/standalone.module.js';
import SearchBox from "../../components/search-box.js";

export default ({ entityType, pageSize: initialPageSize, value, onSelect, simpleKey, imageable }) => {
  const [pageSize, setPageSize] = useState(initialPageSize);
  const [page, setPage] = useState(1);
  const [pageCount, setPageCount] = useState();
  const [pages, setPages] = useState();
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState();
  const [filter, setFilter] = useState('');
  const [open, setOpen] = useState();
  const ref = useRef(null);

  useEffect(function () {
    if (!open) {
      return;
    }

    fetch(
      `/Admin/api/controls/select/list?entityType=${entityType}&filter=${filter}&pageSize=${pageSize}&page=${page}&simpleKey=${simpleKey}`,
      {
        credentials: 'include'
      }
    )
      .then(response => response.json())
      .then(response => {
        setLoading(false);
        setData(response);
        const pageCount = Math.max(1, Math.ceil(response.totalCount / pageSize));
        setPageCount(pageCount);
        setPages([...Array(pageCount)]);
        setPage(Math.min(pageCount, page)); // if filtered results have less pages than what is on the current page
      });
  }, [page, pageSize, open, filter]);

  useEffect(() => {
    const callback = event => {
      if (!ref.current) {
        return;
      }
      if (ref.current == event.target || ref.current.contains(event.target)) {
        return;
      }
      setOpen(false);
    };
    document.addEventListener('click', callback);
    return () => document.removeEventListener('click', callback);
  }, []);

  const render = () => {
    if (loading) {
      return html`Loading ...`;
    }

    if (!data) {
      return html`Could not load data`;
    }

    return html`
      <div class="mx-2 mb-2">
        <${SearchBox} small=${true} callback=${value => setFilter(value)} />
      </div>
      ${data.items.map(item =>
        html`<div class="dropdown-item-outer">
            ${imageable ? html`<div class="dropdown-image-outer">
              ${item.image ? html`<img class="dropdown-image" src=${item.image} />` : null}
            </div>` : null}

            <a class=${`dropdown-item ${item.reference == value ? 'active' : ''}`} onClick=${() => { onSelect(item); setOpen(false); }}>
              ${item.name}
            </a>
          </div>`
      )}
      ${[...new Array(pageSize - data.items.length)].map(() => html`<div><a class="dropdown-item disabled nbsp"></a></div>`)}
      <nav>
        <ul class="pagination pagination-sm justify-content-center m-0 mt-2">
          <li class="page-item"><a class=${`page-link ${page == 1 ? 'disabled' : ''}`} onClick=${() => setPage(Math.max(1, page - 1))} title="Previous" dangerouslySetInnerHTML=${{__html: '&laquo;'}}></a></li>
          ${pages.map((_, i) => html`<li class=${`page-item ${page == i + 1 ? 'active' : ''}`}><a class="page-link" onClick=${() => setPage(i + 1)}>${i + 1}</a></li>`)}
          <li class="page-item"><a class=${`page-link ${page == pageCount ? 'disabled' : ''}`} onClick=${() => setPage(Math.min(pageCount, page + 1))} title="Next" dangerouslySetInnerHTML=${{__html: '&raquo;'}}></a></li>
        </ul>
      </nav>
      `;
  };

  return html`<div class="dropdown d-inline-block" ref=${ref}>
    <button class="btn btn-beta btn-sm dropdown-toggle" type="button" aria-expanded=${open} onClick=${() => setOpen(!open)}>
      Add
    </button>
    <div class=${ `dropdown-menu ${open ? 'show' : ''}`}>
      ${render()}
    </div>
  </div>`;
};