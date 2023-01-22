import './main.scss'

import { render } from './preact-htm/standalone.module'
import Table from './list-page/table'
import Form from './form/form';

window.viteIsLoaded = true;

document.querySelectorAll('.list-page-table').forEach(element =>
  render(<Table {...JSON.parse(element.getAttribute('settings') || '{}')} />, element)
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
  render(<Form {...JSON.parse(element.getAttribute('settings') || '{}')} />, element)
);