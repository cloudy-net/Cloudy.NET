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

  return <>
    <div class="container">
        <h1 class="h2 mb-3">
            What to edit?
        </h1>
    </div>
    <div class="container">
      <div class="row">
        {entityTypes.map(entityType =>
          <div class="col-sm-4 mb-4 d-flex">
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
}
