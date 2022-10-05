import { render } from 'preact'
import { App } from './app'
import './main.scss'

const element = document.getElementById('list-tbody');
const settings = JSON.parse(element.getAttribute('settings'));

render(<App {...settings} />, element);
