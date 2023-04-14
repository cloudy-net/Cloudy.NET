import './main.scss'

import { render } from 'preact'
import { Router } from 'preact-router';

import EntityTypesProvider from './form/contexts/entity-types-provider';
import { useState } from 'preact/hooks';
import DashboardView from './views/dashboard-view';
import EditView from './views/edit-view';
import ListView from './views/list-view';
import EntityListContextProvider from './entity-list/entity-list-context-provider';

window.viteIsLoaded = true;

const Main = () => {
  const [keyValues, setKeyValues] = useState(new URL(document.location).searchParams.getAll('keys'));

  return <EntityTypesProvider>
    <EntityListContextProvider>
      <Router onChange={() => setKeyValues(new URL(document.location).searchParams.getAll('keys'))}>
        <DashboardView path="/Admin" />
        <ListView path="/Admin/List/:entityTypeName" />
        <EditView path="/Admin/Edit/:entityTypeName" mode="edit" keyValues={keyValues} />
      </Router>
    </EntityListContextProvider>
  </EntityTypesProvider>
};

render(<Main />, document.getElementById('app'));

document.addEventListener('keydown', event => {
  if (event.key != 'Enter') {
    return;
  }

  if (event.target.tagName == 'A' && !event.target.getAttribute('href') && event.target.getAttribute('tabindex') == '0') {
    event.target.click();
  }
});