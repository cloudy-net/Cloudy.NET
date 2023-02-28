import './main.scss'

import html from '@src/html-init.js';
import { render } from 'preact'
import { Router, Route } from 'preact-router';

import Table from './list-page/table.js'
import Form from './form/form.js';
import EntityTypeList from './entity-type-list/entity-type-list';

import Header from './layout/header';
import Footer from './layout/footer';

import EntityTypesProvider from './form/entity-types-provider';

window.viteIsLoaded = true;

if (document.getElementById('app')) {
  const Main = () => (
    <EntityTypesProvider>
      <Header />
      <Router>
        <EntityTypeList path="/Admin/" />
        <Table path="/Admin/List/:entityType" />
        <Form path="/Admin/New/:entityType" mode="new" />
        <Form path="/Admin/Edit/:entityType" mode="edit"  />
      </Router>
      <Footer />
    </EntityTypesProvider>
  );

  render(<Main />, document.getElementById('app'));
}

document.querySelectorAll('.entity-type-list').forEach(element =>
  render(html`<${EntityTypeList} />`, element)
);

document.querySelectorAll('.list-page-table').forEach(element =>
  render(html`<${Table} ...${JSON.parse(element.getAttribute('settings') || '{}')} />`, element)
);

document.addEventListener('keydown', event => {
  if (event.key != 'Enter') {
    return;
  }

  if (event.target.tagName == 'A' && !event.target.getAttribute('href') && event.target.getAttribute('tabindex') == '0') {
    event.target.click();
  }
});

document.querySelectorAll('.cloudy-form').forEach(element =>
  render(html`<${Form} ...${JSON.parse(element.getAttribute('settings') || '{}')} />`, element)
);