import { render } from 'preact'
import Table from './list-page/table'
import SelectOne from './controls/select-one'

import './main.scss'

window.viteIsLoaded = true;

document.querySelectorAll('.list-page-table').forEach(element =>
  render(<Table {...JSON.parse(element.getAttribute('settings') || '{}')} />, element)
);
document.querySelectorAll('.select-one-control').forEach(element =>
  render(<SelectOne {...JSON.parse(element.getAttribute('settings') || '{}')} />, element)
);
document.querySelectorAll('.html-control').forEach(element => {
  var input = document.querySelector(element.getAttribute('for'));
  var quill = new Quill(element, {
    modules: {
      // toolbar: '#toolbar'
    },
    theme: 'snow'
  });
  quill.clipboard.dangerouslyPasteHTML(input.value)
  quill.on('text-change', () => input.value = quill.root.innerHTML);
});