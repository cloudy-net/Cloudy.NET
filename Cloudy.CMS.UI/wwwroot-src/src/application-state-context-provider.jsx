import ApplicationStateContext from './application-state-context.js';
import { useSignal } from '@preact/signals';
import { useEffect } from 'preact/hooks';

export default ({ children }) => {
  const showChanges = useSignal(false);
  const fieldTypes = useSignal({ $loading: true });

  useEffect(function () {
    (async () => {
      const response = await fetch(
        `/Admin/api/form/fields`,
        {
          credentials: 'include'
        }
      );

      if (!response.ok) {
        throw { response, body: await response.text() };
      }

      var json = await response.json();

      fieldTypes.value = json;
    })();
  }, []);

  return <ApplicationStateContext.Provider value={{ showChanges, fieldTypes }}>
    {children}
  </ApplicationStateContext.Provider>;
};