import SelectOneDropdown from './select-one-dropdown.js';

export default ({ name, path, settings, validators, dependencies }) => {
    const [value, setValue] = dependencies.useState();
    const [preview, setPreview] = dependencies.useState();
    const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);

    const onChange = (newValue) => {
        setValue(newValue);
        dependencies.simpleChangeHandler.setValue(entityReference, path, newValue, validators)
    };

    dependencies.useEffect(() => {
        const value = dependencies.simpleChangeHandler.getIntermediateValue(state, path);
        setValue(value);

        (async () => {
            if (!value) {
                return;
            }

            if (preview && preview.reference == value) {
                return;
            }

            var response = await fetch(
                `/Admin/api/controls/select/preview?entityType=${settings.referencedTypeName}&reference=${value}&simpleKey=${settings.simpleKey}`,
                {
                    credentials: 'include'
                }
            );

            if (response.status == 404) {
                setPreview({ notFound: true });
                return;
            }
            
            var json = await response.json();
            setPreview(json);
        })();
    }, []);

    return dependencies.html`
        <input type="hidden" class="form-control" name=${dependencies.componentContextProvider.getIdentifier(path)} value=${value} />

        ${value && !preview && dependencies.html`<div class=${`input-group mb-3 select-one ${settings.imageable ? ' imageable' : ''}`}>
            <span class="input-group-text" ></span>
            <div class="form-control">&nbsp;</div>
        </div>`}

        ${preview && preview.notFound ? dependencies.html`<div class=${`input-group mb-3 select-one ${settings.imageable ? ' imageable' : ''}`}>
            <div class="form-control"><span class="information-missing">Could not find <code>${settings.simpleKey ? value : JSON.parse(value).join(', ')}</code></span></div>
            <button class="btn btn-beta" type="button" onClick=${() => { onChange(null); setPreview(null); }}>Remove</button>
        </div>` : null}

        ${preview && !preview.notFound ? dependencies.html`<div class=${`input-group mb-3 select-one ${settings.imageable ? ' imageable' : ''}`}>
            <span class="input-group-text" ></span>
            ${preview.image && dependencies.html`<img src=${preview.image} class="select-one-preview-image" alt="" />`}
            <div class="form-control">${preview.name}</div>
            <a class="btn btn-beta" href=${`/Admin/Edit?EntityType=${settings.referencedTypeName}&${settings.simpleKey ? `keys=${preview.reference}` : preview.reference.map(key => `keys=${key}`).join('&')}`} target="_blank">Edit</a>
            <button class="btn btn-beta" type="button" onClick=${() => { onChange(null); setPreview(null); }}>Remove</button>
        </div>` : null}

        <${SelectOneDropdown} 
            dependencies=${dependencies}
            entityType=${settings.referencedTypeName}
            pageSize=${10}
            value=${value}
            onSelect=${item => { onChange(settings.simpleKey ? item.reference : JSON.stringify(item.reference)); setPreview(item); }}
            simpleKey=${settings.simpleKey}
            imageable=${settings.imageable} />
    `;
}
