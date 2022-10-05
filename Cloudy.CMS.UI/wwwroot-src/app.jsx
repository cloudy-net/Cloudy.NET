import { useEffect, useState } from 'preact/hooks';

export function App({ ContentType, Columns }) {
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState();
  
  useEffect(function () {
    fetch(`/Admin/api/list/result?contentType=${ContentType}&columns=${Columns.map(c => c.Name).join(',')}`)
    .then(response => response.json())
    .then(response => {
      console.log(response);
      setLoading(false);
      setData(response);
    });
  }, []);
  
  if(loading){
    return <>Loading ...</>;
  }
  
  if(!data){
    return <>Could not load data</>;
  }

  return (
    <>
      {data.items.map(d => <tr>{Columns.map(c => <td>{d[c.Name]}</td>)}<td></td></tr>)}
    </>
  );
}
