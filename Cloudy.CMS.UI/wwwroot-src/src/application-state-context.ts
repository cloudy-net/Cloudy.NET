import { Signal } from '@preact/signals';
import { createContext } from 'preact';
import FieldType from './data/fieldtype';

const ApplicationStateContext = createContext({
  showChanges: new Signal<boolean>(false),
  fieldTypes: new Signal<{ [key:string]: FieldType[] } | null>(null),
});

export default ApplicationStateContext;