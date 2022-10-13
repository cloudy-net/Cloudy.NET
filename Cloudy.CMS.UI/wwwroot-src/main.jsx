import { render } from 'preact'
import Table from './list-page/table'
import SelectOne from './controls/select-one'

import './main.scss'

document.querySelectorAll('.list-page-table').forEach(element =>
    render(<Table {...JSON.parse(element.getAttribute('settings') || '{}')} />, element)
);
document.querySelectorAll('.select-one-control').forEach(element =>
    render(<SelectOne {...JSON.parse(element.getAttribute('settings') || '{}')} />, element)
);