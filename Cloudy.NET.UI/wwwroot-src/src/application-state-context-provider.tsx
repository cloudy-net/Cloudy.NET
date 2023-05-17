import ApplicationStateContext from './application-state-context';
import { useSignal } from '@preact/signals';
import { useEffect } from 'preact/hooks';
import { ComponentChildren } from 'preact';
import FieldType from './data/fieldtype';

export default ({ children }: { children: ComponentChildren }) => {
  const showChanges = useSignal(false);
  const fieldTypes = useSignal<{ [key:string]: FieldType[] } | null>(null);

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