import ApplicationStateContext from './application-state-context.js';
import { useSignal } from '@preact/signals';

export default ({ children }) => {
  const showChanges = useSignal(false);

  return <ApplicationStateContext.Provider value={{ showChanges }}>
    {children}
  </ApplicationStateContext.Provider>;
};