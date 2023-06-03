// @ts-nocheck

import './main.scss'

import { render } from 'preact'
import { Router } from 'preact-router';

import EntityTypesProvider from './form/contexts/entity-types-provider';
import { useState } from 'preact/hooks';
import DashboardView from './views/dashboard-view';
import EditView from './views/edit-view';
import ListView from './views/list-view';
import EntityListContextProvider from './entity-list/entity-list-context-provider';
import ApplicationStateContextProvider from './application-state-context-provider';

declare global {
  interface Window { 
    viteIsLoaded: boolean;
    viteDevServerIsRunning: boolean;
  }
}

window.viteIsLoaded = true;

const Main = () => {
  const [keyValues, setKeyValues] = useState(new URL(document.location.href).searchParams.getAll('keys'));

  return <ApplicationStateContextProvider>
    <EntityTypesProvider>
      <EntityListContextProvider>
        <Router onChange={() => setKeyValues(new URL(document.location.href).searchParams.getAll('keys'))}>
          <DashboardView path="/Admin" />
          <ListView path="/Admin/List/:entityTypeName" />
          <EditView path="/Admin/Edit/:entityTypeName" mode="edit" keyValues={keyValues} />
          <EditView path="/Admin/New/:entityTypeName" mode="new" />
        </Router>
      </EntityListContextProvider>
    </EntityTypesProvider>
  </ApplicationStateContextProvider>
};

render(<Main />, document.getElementById('app'));

document.addEventListener('keydown', event => {
  if (event.key != 'Enter') {
    return;
  }

  const target = event.target as HTMLElement;

  if (!target) {
    return;
  }

  if (target.tagName == 'A' && !target.getAttribute('href') && target.getAttribute('tabindex') == '0') {
    target.click();
  }
});