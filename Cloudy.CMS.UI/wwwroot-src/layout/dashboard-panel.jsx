import { useContext } from 'preact/hooks';
import EntityTypesContext from '@src/form/contexts/entity-types-context';

export default () => {
  const { groupedEntityTypes } = useContext(EntityTypesContext);

  const Table = () => <div class="dashboard">
    {Object.keys(groupedEntityTypes).sort().map(key => <>
      {key != 'null' && <h2 class="card-group-heading">{key}</h2>}
      <div class="cards">
        {groupedEntityTypes[key].map(entityType =>
          <div class="card">
            <div class="card-body">
              <h3 class="card-title">{entityType.pluralName}</h3>
              <p class="card-text">{entityType.description}</p>
            </div>
            <div class="card-buttons">
              {entityType.links.map((link, index) => <>
                <a class={`card-button ${index === 0 ? 'primary' : ''}`} href={`/Admin/${link.action}/${link.entityTypeName}`}>
                  {link.text}
                </a>
              </>)}
            </div>
          </div>
        )}
      </div>
    </>)}
  </div>

  return <Table />;
}
