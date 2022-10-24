import './main.scss'

import { render } from 'preact'
import Table from './list-page/table'
import SelectOne from './select-one/select-one'
import EnumDropdown from './enum-dropdown/enum-dropdown';
import MediaPicker from './media-picker/media-picker';

window.viteIsLoaded = true;

document.querySelectorAll('.list-page-table').forEach(element =>
  render(<Table {...JSON.parse(element.getAttribute('settings') || '{}')} />, element)
);

document.querySelectorAll('.select-one-control').forEach(element =>
  render(<SelectOne {...JSON.parse(element.getAttribute('settings') || '{}')} />, element)
);

document.querySelectorAll('.enum-dropdown-control').forEach(element =>
  render(<EnumDropdown {...JSON.parse(element.getAttribute('settings') || '{}')} />, element)
);

document.querySelectorAll('.media-picker-control').forEach(element =>
  render(<MediaPicker {...JSON.parse(element.getAttribute('settings') || '{}')} />, element)
);

document.querySelectorAll('.html-control').forEach(element => {
  var input = document.querySelector(element.getAttribute('for'));
  var quill = new Quill(element, {
    modules: {
    },
    theme: 'snow'
  });
  quill.clipboard.dangerouslyPasteHTML(input.value)
  quill.on('text-change', () => input.value = quill.root.innerHTML);
});

document.addEventListener('keydown', event => {
  if(event.key != 'Enter'){
    return;
  }

  if(event.target.tagName == 'A' && !event.target.getAttribute('href') && event.target.getAttribute('tabindex') == '0'){
    event.target.click();
  }
});