import { useEffect, useState } from 'preact/hooks';
import SearchBox from "./search-box";
import DropdownItem from './dropdown-item';

export default ({ entityType, simpleKey, value, onSelect }: { entityType: string, simpleKey: boolean, value: string, onSelect: (value: any) => void }) => {
  const [pageSize] = useState(10);
  const [page, setPage] = useState(1);
  const [pageCount, setPageCount] = useState(0);
  const [pages, setPages] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState<any>();
  const [filter, setFilter] = useState('');

  useEffect(function () {
    (async () => {
      var response = await fetch(
        `/Admin/api/controls/select/list?entityType=${entityType}&filter=${filter}&pageSize=${pageSize}&page=${page}&simpleKey=${simpleKey}`,
        {
          credentials: 'include'
        }
      );

      if (!response.ok) {
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
  }, [page, pageSize, filter]);

  if (!data) {
    return;
  }

  return <div style="width: 300px; max-width: 100%;">
    <SearchBox callback={value => setFilter(value)} />
    <div>
      {!loading && data.items.map((item: any) =>
        <div><DropdownItem text={item.name} ellipsis={true} active={item.reference == value} onClick={() => onSelect(item.reference == value ? null : item)} /></div>
      )}
    </div>
    <div>
      {[...new Array(pageSize - (loading ? 0 : data.items.length))].map(() => <DropdownItem disabled={true} nbsp={true} />)}
    </div>
    <nav>
      <ul class="pagination center">
        <li class="page-item"><a class={"page-link" + (page == 1 ? " disabled" : "")} onClick={() => setPage(Math.max(1, page - 1))} title="Previous">&laquo;</a></li>
        {!loading && pages.map((_, i) => <li class={"page-item" + (page == i + 1 ? " active" : "")}><a class="page-link" onClick={() => setPage(i + 1)}>{i + 1}</a></li>)}
        <li class="page-item"><a class={"page-link" + (page == pageCount ? " disabled" : "")} onClick={() => setPage(Math.min(pageCount, page + 1))} title="Next">&raquo;</a></li>
      </ul>
    </nav>
  </div>;
};