import { useEffect, useState } from 'preact/hooks';

export default ({ ContentType, Columns, PageSize, EditLink }) => {
  const [pageSize, setPageSize] = useState(PageSize);
  const [page, setPage] = useState(1);
  const [pageCount, setPageCount] = useState();
  const [pages, setPages] = useState();
  const [columns, setColumns] = useState(Columns);
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState();
  
  useEffect(function () {
    fetch(`/Admin/api/list/result?contentType=${ContentType}&columns=${columns.map(c => c.Name).join(',')}&pageSize=${pageSize}&page=${page}`)
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
    <table class="table">
      <thead>
        <tr>
          {columns.map(c => <th>{c.Name}</th>)}
          <th></th>
        </tr>
      </thead>
      <tbody>
        {data.items.map(d => <tr>
          {columns.map((_, i) => 
            <td>{d.values[i]}</td>
          )}
          <td><a href={`${EditLink}${d.keys.map(k => `&keys=${k}`)}`}>Edit</a></td>
        </tr>)}
      </tbody>
      <tfoot>
        <tr>
          <td>
            <nav>
              <ul class="pagination justify-content-center">
                <li class="page-item"><a class={"page-link" + (page == 1 ? " disabled" : "")}>Previous</a></li>
                {pages.map((_, i) => <li class={"page-item" + (page == i + 1 ? " active" : "")}><a class="page-link" onClick={() => setPage(i + 1)}>{i + 1}</a></li>)}
                <li class="page-item"><a class={"page-link" + (page == pageCount ? " disabled" : "")}>Next</a></li>
              </ul>
            </nav>
          </td>
        </tr>
      </tfoot>
    </table>
  );
}
