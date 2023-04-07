import './main.scss'

import { render } from 'preact'
import { Router } from 'preact-router';

import Form from './form/form';
import Dashboard from './layout/dashboard';
import Delete from './layout/delete'

import Navbar from './layout/navbar';

import EntityTypesProvider from './form/contexts/entity-types-provider';
import NavigationPanel from './layout/navigation-panel';
import { useState } from 'preact/hooks';
import MainMenu from './layout/main-menu';

window.viteIsLoaded = true;

if (document.getElementById('app')) {
  const Main = () => {
    const [keyValues, setKeyValues] = useState(new URL(document.location).searchParams.getAll('keys'));
    const [showNavigationPanel, setShowNavigationPanel] = useState(false);

    return <EntityTypesProvider>
      
      <Router onChange={event => setShowNavigationPanel(event.url != "/Admin")}/>
      <div class={"layout" + (showNavigationPanel ? ' show-navigation-panel' : '')}>
        <MainMenu />
        <Navbar />
        <Router>
          <NavigationPanel path="/Admin/List/:entityTypeName" mode="new" />
          <NavigationPanel path="/Admin/New/:entityTypeName" mode="new" />
          <NavigationPanel path="/Admin/Edit/:entityTypeName" mode="edit" />
          <NavigationPanel path="/Admin/Delete/:entityTypeName" />
        </Router>
        <div className="layout-main-panel">
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