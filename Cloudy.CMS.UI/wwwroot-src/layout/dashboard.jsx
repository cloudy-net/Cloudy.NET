import { useContext } from 'preact/hooks';
import EntityTypesContext from '@src/form/contexts/entity-types-context';

export default () => {
  const { entityTypes } = useContext(EntityTypesContext);

  const Table = () => <>
    <div class="container-fluid">
      <div class="row">
        {entityTypes.map(entityType =>
          <div class="col-md-4 mb-4 d-flex">
            <div class="card w-100" style="min-height: 200px;">
              <div class="card-body">
                <h5 class="card-title">{entityType.pluralName}</h5>
                <p class="card-text">{entityType.description}</p>
              </div>
              <div class="card-footer">
                {entityType.links.map((link, index) => <>
                  <a class={`btn ${index === 0 ? 'btn-primary' : 'btn-beta'}`} href={`/Admin/${link.action}/${link.entityTypeName}`}>
                    {link.text}
                  </a>&nbsp;
                </>)}
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  </>

  return <Table />;
}
