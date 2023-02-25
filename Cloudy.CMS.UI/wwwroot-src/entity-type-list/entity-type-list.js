import { useEffect, useState } from 'preact/hooks';

export default () => {

  const [entityTypes, setEntityTypes] = useState([]);

  useEffect(function () {
    (async () => {
      await fetch('/Admin/api/entity-type-list/result', { credentials: 'include' })
        .then(r => r.json())
        .then(json => setEntityTypes(json));
    })();
  }, []);

  return entityTypes.map(entityType =>
    <div class="col-sm-4 mb-4 d-flex">
      <div class="card w-100" style="min-height: 200px;">
        <div class="card-body">
          <h5 class="card-title">{entityType.pluralName}</h5>
          <p class="card-text">{entityType.description}</p>
        </div>
        <div class="card-footer">
          {entityType.links.map((link, index) => <>
            <a class={`btn ${index === 0 ? 'btn-primary' : 'btn-beta'}`} href={`/Admin/${link.action}?EntityType=${link.entityTypeName}`}>
              {link.text}
            </a>&nbsp;
          </>)}
        </div>
      </div>
    </div>
  );
}
