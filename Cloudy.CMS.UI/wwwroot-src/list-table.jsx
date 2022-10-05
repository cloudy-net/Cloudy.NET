import { useEffect, useState } from 'preact/hooks';

export function ListTable({ ContentType, Columns, PageSize }) {
  const [pageSize, setPageSize] = useState(20);
  const [page, setPage] = useState(1);
  const [pageCount, setPageCount] = useState();
  const [pages, setPages] = useState();
  const [columns, setColumns] = useState(Columns);
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState();
  
  useEffect(function () {
    fetch(`/Admin/api/list/result?contentType=${ContentType}&columns=${columns.map(c => c.Name).join(',')}&pageSize=${PageSize}&page=${page}`)
    .then(response => response.json())
    .then(response => {
      setLoading(false);
      setData(response);
      const pageCount = Math.ceil(response.totalCount/pageSize);
      setPageCount(pageCount);
      setPages([...Array(pageCount)]);
    });
  }, [page, pageSize, Columns]);
  
  if(loading){
    return <>Loading ...</>;
  }
  
  if(!data){
    return <>Could not load data</>;
  }

  return (
    <>
      <thead>
        <tr>
          {columns.map(c => <th>{c.Name}</th>)}
        </tr>
      </thead>
      <tbody>
        {data.items.map(d => <tr>{columns.map(c => <td>{d[c.Name]}</td>)}<td></td></tr>)}
      </tbody>
      <tfoot>
        <tr>
          <td>
            <nav>
              <ul class="pagination justify-content-center">
                <li class="page-item"><a class={"page-link" + (page == 1 ? " disabled" : "")}>Previous</a></li>
                {pages.map((_, i) => <li class="page-item"><a class="page-link">{i + 1}</a></li>)}
                <li class="page-item"><a class={"page-link" + (page == pageCount ? " disabled" : "")}>Next</a></li>
              </ul>
            </nav>
          </td>
        </tr>
      </tfoot>
    </>
  );
}
