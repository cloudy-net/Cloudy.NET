import html from '@src/html-init.js';
import { useContext } from 'preact/hooks';
import ColumnComponentContext from './column-component-context.jsx';

const TableBody = ({ items, columns, pageSize, settings }) => {
    const components = useContext(ColumnComponentContext);

    return components && <tbody>
      {items.map(d => <tr>
        <td><input type="checkbox"/></td>
        {columns.map((_, i) =>
          d.values[i]
            && Object.keys(components).includes(d.values[i].partial)
            && html`<td><${components[d.values[i].partial]} ...${{ keys: d.keys, ...d.values[i], settings }} dependencies=${{ html }} /></td>`
        )}
      </tr>)}
      {[...new Array(pageSize - items.length)].map(() => <tr class="list-page-blank-row"><td class="nbsp" /></tr>)}
    </tbody>
  };

export default TableBody;