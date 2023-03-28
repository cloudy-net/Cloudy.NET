import { useContext } from 'preact/hooks';
import EntityTypesContext from '@src/form/contexts/entity-types-context';

export default () => {
  const { groupedEntityTypes } = useContext(EntityTypesContext);

  console.log(groupedEntityTypes);

  const Table = () => <>
    <div class="container-fluid">
      {Object.keys(groupedEntityTypes).sort().map(key => <>
        { key != 'null' && <div className="row"><div className="col-12"><h2>{ key }</h2></div></div>}
        <div class="row">
          {groupedEntityTypes[key].map(entityType =>
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
        { key != 'null' && <div className="row"><div className="col-12"><hr/></div></div> }
      </>)}
    </div>
  </>

  return <Table />;
}
