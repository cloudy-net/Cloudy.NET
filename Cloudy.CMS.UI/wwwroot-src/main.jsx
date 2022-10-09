import { render } from 'preact'
import { ListTable } from './list-table'
import './main.scss'

const element = document.querySelector('.list-table');

if(element){
    const settings = JSON.parse(element.getAttribute('settings'));

    render(<ListTable {...settings} />, element);
}