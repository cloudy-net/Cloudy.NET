import { createContext } from 'preact';

const FieldComponentContext = createContext<{ [key: string]: any } | null>(null);

export default FieldComponentContext;