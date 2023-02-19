import html from '@src/html-init.js';
import { render } from 'preact'
import Table from './list-page/table.js'
import Form from './form/form.js';

window.viteIsLoaded = true;

document.querySelectorAll('.list-page-table').forEach(element =>
  render(html`<${Table} ...${JSON.parse(element.getAttribute('settings') || '{}')} />`, element)
);

document.addEventListener('keydown', event => {
  if(event.key != 'Enter'){
    return;
  }

  if(event.target.tagName == 'A' && !event.target.getAttribute('href') && event.target.getAttribute('tabindex') == '0'){
    event.target.click();
  }
});

document.querySelectorAll('.cloudy-form').forEach(element =>
  render(html`<${Form} ...${JSON.parse(element.getAttribute('settings') || '{}')} />`, element)
);