import './main.scss'

import { render } from 'preact'
import { Router } from 'preact-router';

import Table from './list-page/table'
import Form from './form/form';
import Dashboard from './layout/dashboard';
import Delete from './layout/delete'

import Navbar from './layout/navbar';

import EntityTypesProvider from './form/entity-types-provider';
import LayoutLeftPanel from './layout/layout-left-panel';
import { useState } from 'preact/hooks';

window.viteIsLoaded = true;

if (document.getElementById('app')) {
  const Main = () => {
    const [keyValues, setKeyValues] = useState(null);
    
    return <EntityTypesProvider>
      <div class="layout">
        <Router>
          <LayoutLeftPanel path="/Admin/List/:entityTypeName" mode="new" />
          <LayoutLeftPanel path="/Admin/New/:entityTypeName" mode="new" />
          <LayoutLeftPanel path="/Admin/Edit/:entityTypeName" mode="edit" />
          <LayoutLeftPanel path="/Admin/Delete/:entityTypeName" />
        </Router>
        <div className="layout-main-panel">
          <Navbar />
          <Router onChange={() => setKeyValues(new URL(document.location).searchParams.getAll('keys'))}>
            <Dashboard path="/Admin/" />
            <Dashboard path="/Admin/List/:entityTypeName" />
            <Form key={'form-new'} path="/Admin/New/:entityTypeName" mode="new" keyValues={keyValues} />
            <Form key={'form-edit'} path="/Admin/Edit/:entityTypeName" mode="edit" keyValues={keyValues} />
            <Delete path="/Admin/Delete/:entityTypeName" />
          </Router>
        </div>
      </div>
    </EntityTypesProvider>
  };

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