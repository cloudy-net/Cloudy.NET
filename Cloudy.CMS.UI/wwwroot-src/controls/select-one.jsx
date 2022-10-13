import { useEffect, useState } from 'preact/hooks';
import Filter from './select-one-filter'

export default ({ ControlName, ContentType, Columns, PageSize }) => {
  const [pageSize, setPageSize] = useState(PageSize);
  const [page, setPage] = useState(1);
  const [pageCount, setPageCount] = useState();
  const [pages, setPages] = useState();
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState();
  const [open, setOpen] = useState(false);
  const [value, setValue] = useState();
  const [filter, setFilter] = useState('');

  useEffect(function () {
    if (!open) {
      return;
    }

    fetch(`/Admin/api/controls/select/list?contentType=${ContentType}&filter=${filter}&pageSize=${pageSize}&page=${page}`)
      .then(response => response.json())
      .then(response => {
        setLoading(false);
        setData(response);
        const pageCount = Math.max(1, Math.ceil(response.totalCount / pageSize));
        setPageCount(pageCount);
        setPages([...Array(pageCount)]);
        setPage(Math.min(pageCount, page)); // if filtered results have less pages than what is on the current page
      });
  }, [page, pageSize, Columns, open, filter]);

  const render = () => {
    if (loading) {
      return <>Loading ...</>;
    }

    if (!data) {
      return <>Could not load data</>;
    }

    return <>
      <Filter callback={value => setFilter(value)} />
      {data.items.map(item =>
        <div><a class={"dropdown-item" + (item.reference == value ? " active" : "")} onClick={() => setValue(item.reference)}>{item.name}</a></div>
      )}
      {[...new Array(PageSize - data.items.length)].map(() => <div><a class="dropdown-item disabled">&nbsp;</a></div>)}
      <nav>
        <ul class="pagination pagination-sm justify-content-center m-0 mt-2">
          <li class="page-item"><a class={"page-link" + (page == 1 ? " disabled" : "")} onClick={() => setPage(Math.max(1, page - 1))} title="Previous">&laquo;</a></li>
          {pages.map((_, i) => <li class={"page-item" + (page == i + 1 ? " active" : "")}><a class="page-link" onClick={() => setPage(i + 1)}>{i + 1}</a></li>)}
          <li class="page-item"><a class={"page-link" + (page == pageCount ? " disabled" : "")} onClick={() => setPage(Math.min(pageCount, page + 1))} title="Next">&raquo;</a></li>
        </ul>
      </nav>
    </>;
  };

  return <>
    <input type="hidden" class="form-control" name={ControlName} value={value} />
    <div class="dropdown">
      <button class="btn btn-beta dropdown-toggle" type="button" aria-expanded={open} onClick={() => setOpen(!open)}>
        Add
      </button>
      <div class={"dropdown-menu" + (open ? " show" : "")}>
        {render()}
      </div>
    </div>
  </>;
}
