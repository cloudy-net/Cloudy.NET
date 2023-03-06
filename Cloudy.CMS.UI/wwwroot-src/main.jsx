import './main.scss'

import { render } from 'preact'
import { Router } from 'preact-router';

import Table from './list-page/table'
import Form from './form/form';
import EntityTypeList from './entity-type-list/entity-type-list';
import Delete from './layout/delete'

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
        <Form key={'form-new'} path="/Admin/New/:entityTypeName" mode="new" />
        <Form key={'form-edit'} path="/Admin/Edit/:entityTypeName" mode="edit" />
        <Delete path="/Admin/Delete/:entityTypeName" />
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