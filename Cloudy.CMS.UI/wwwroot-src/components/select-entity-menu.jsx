import { useEffect, useState } from 'preact/hooks';
import SearchBox from "./search-box";
import closeDropdown from './close-dropdown';
import DropdownItem from './dropdown-item';

export default ({ entityType, simpleKey, value, onSelect }) => {
  const [pageSize] = useState(10);
  const [page, setPage] = useState(1);
  const [pageCount, setPageCount] = useState();
  const [pages, setPages] = useState();
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState();
  const [filter, setFilter] = useState('');
  const [error, setError] = useState();
  const [retryError, setRetryError] = useState(0);

  useEffect(function () {
    (async () => {
      var response = await fetch(
        `/Admin/api/controls/select/list?entityType=${entityType}&filter=${filter}&pageSize=${pageSize}&page=${page}&simpleKey=${simpleKey}`,
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
      const pageCount = Math.max(1, Math.ceil(json.totalCount / pageSize));
      setPageCount(pageCount);
      setPages([...Array(pageCount)]);
      setPage(Math.min(pageCount, page)); // if filtered results have less pages than what is on the current page
    })();
  }, [page, pageSize, filter, retryError]);

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
    return <>Loading ...</>;
  }

  return <div style="width: 300px; max-width: 100%;">
    <SearchBox small={true} callback={value => setFilter(value)} />
    <div>
      {data.items.map(item =>
        <div><DropdownItem text={item.name} active={item.reference == value} onClick={() => onSelect(item.reference == value ? null : item)} /></div>
      )}
    </div>
    <div>
      {[...new Array(pageSize - data.items.length)].map(() => <DropdownItem disabled={true} text={<>&nbsp;</>}/>)}
    </div>
    <nav>
      <ul class="pagination center">
        <li class="page-item"><a class={"page-link" + (page == 1 ? " disabled" : "")} onClick={() => setPage(Math.max(1, page - 1))} title="Previous">&laquo;</a></li>
        {pages.map((_, i) => <li class={"page-item" + (page == i + 1 ? " active" : "")}><a class="page-link" onClick={() => setPage(i + 1)}>{i + 1}</a></li>)}
        <li class="page-item"><a class={"page-link" + (page == pageCount ? " disabled" : "")} onClick={() => setPage(Math.min(pageCount, page + 1))} title="Next">&raquo;</a></li>
      </ul>
    </nav>
  </div>;
};