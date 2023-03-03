import html from '@src/html-init.js';
import { useContext } from 'preact/hooks';
import ColumnComponentContext from './column-component-context.jsx';

const TableBody = ({ items, columns, pageSize, settings }) => {
    const components = useContext(ColumnComponentContext);

    return components && <tbody>
      {items.map(d => <tr>
        {columns.map((_, i) =>
          d.values[i]
            && Object.keys(components).includes(d.values[i].partial)
            && html`<td><${components[d.values[i].partial]} ...${{ keys: d.keys, ...d.values[i], settings }} dependencies=${{ html }} /></td>`
        )}
        <td>
          <a class="me-2" href={`${settings.editLink}?${d.keys.map(k => `keys=${k}`).join('&')}`}>Edit</a>
          <a href={`${settings.deleteLink}?${d.keys.map(k => `keys=${k}`).join('&')}`}>Delete</a>
        </td>
      </tr>)}
      {[...new Array(pageSize - items.length)].map(() => <tr class="list-page-blank-row"><td class="nbsp" /></tr>)}
    </tbody>
  };

export default TableBody;