import './main.scss'

import { render } from 'preact'
import { Router } from 'preact-router';

import Form from './form/form';
import Dashboard from './layout/dashboard';
import Delete from './layout/delete'

import Navbar from './layout/navbar';

import EntityTypesProvider from './form/contexts/entity-types-provider';
import LayoutLeftPanel from './layout/layout-left-panel';
import { useState } from 'preact/hooks';

window.viteIsLoaded = true;

if (document.getElementById('app')) {
  const Main = () => {
    const [keyValues, setKeyValues] = useState(new URL(document.location).searchParams.getAll('keys'));
    
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
            <Form key={'form-new'} path="/Admin/New/:entityTypeName" mode="new" />
            <Form key={'form-edit'} path="/Admin/Edit/:entityTypeName" mode="edit" keyValues={keyValues} />
            <Delete path="/Admin/Delete/:entityTypeName" />
          </Router>
        </div>
      </div>
    </EntityTypesProvider>
  };

  render(<Main />, document.getElementById('app'));
}

document.addEventListener('keydown', event => {
  if (event.key != 'Enter') {
    return;
  }

  if (event.target.tagName == 'A' && !event.target.getAttribute('href') && event.target.getAttribute('tabindex') == '0') {
    event.target.click();
  }
});