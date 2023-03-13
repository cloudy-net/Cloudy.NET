import { useContext, useEffect, useState } from "preact/hooks";
import EntityTypesContext from "../form/entity-types-context";

const Navbar = ({ }) => {
    const { entityTypes } = useContext(EntityTypesContext);

    const [settings, setSettings] = useState(null);

    useEffect(function () {
        (async () => {
            await fetch('/Admin/api/layout/footer', { credentials: 'include' })
                .then(r => r.json())
                .then(json => setSettings(json));
        })();
    }, []);

    return settings && <>
        <div class="dropdown">
            <a class="dropdown-toggle h3 d-inline-block" data-bs-toggle="dropdown" aria-expanded="false">
                Dashboard
            </a>
            <ul class="dropdown-menu">
                {entityTypes.map(entityType =>
                    <li><a class="dropdown-item" href={`/Admin/${entityType.links[0].action}/${entityType.links[0].entityTypeName}`} title={entityType.description}>{entityType.pluralName}</a></li>
                )}
            </ul>
        </div>
        {!settings.isValidLicense &&
            <div class="col-12 col-lg-8 text-center text-lg-end">
                Unlicensed version of <a href="https://www.cloudy.net/" target="_blank">Cloudy</a>, valid for non-commercial use only.
            </div>
        }
    </>
}

        export default Navbar;