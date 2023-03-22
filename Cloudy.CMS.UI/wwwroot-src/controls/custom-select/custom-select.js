
export default ({ name, path, settings, validators, dependencies }) => {
    const [options, setOptions] = dependencies.useState([]);
    const [placeholderItemText, setPlaceholderItemText] = dependencies.useState(null);
    const [optionGroups, setOptionGroups] = dependencies.useState({});
    const [hasInitialValue, setHasInitialValue] = dependencies.useState({});
    
    const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);

    dependencies.useEffect(function () {
        
        setHasInitialValue(!!dependencies.simpleChangeHandler.getIntermediateValue(state, path));

        (async () => {
            const responseData = await dependencies.urlFetcher.fetch(
                `/Admin/api/controls/customselect/list/?entityType=${entityReference.entityType}&propertyName=${name}`,
                {
                    credentials: 'include'
                },
                'Could not get select options'
            );

            const options = responseData.items;
            setPlaceholderItemText(responseData.placeholderItemText);

            const optionGroups = {};
            options.filter(option => !!option.group).forEach(option => {
                if (!optionGroups[option.group.name]) optionGroups[option.group.name] = { disabled: option.group.disabled, options: [] };
                optionGroups[option.group.name].options.push({
                    value: option.value,
                    text: option.text,
                });
            });

            setOptions(options.filter(option => !option.group));
            setOptionGroups(optionGroups);
        })();
    }, []);

    return dependencies.html`
        <select required=${settings.isRequired}
                id=${dependencies.componentContextProvider.getIdentifier(path)}
                value=${dependencies.simpleChangeHandler.getIntermediateValue(state, path)}
                onChange=${e => dependencies.simpleChangeHandler.setValue(entityReference, path, e.target.value, validators)}
                class="form-select ${ dependencies.ValidationManager.getValidationClass(state.validationResults, path) }">
        
            ${!!placeholderItemText ? dependencies.html`<option selected=${!hasInitialValue} value="">${placeholderItemText}</option>` : null}

            ${options.map((option) => dependencies.html`<option disabled=${option.disabled} value=${option.value}>${option.text}</option>`)}

            ${Object.keys(optionGroups).map((optionGroup) => dependencies.html`
                <optgroup label=${optionGroup} disabled=${optionGroups[optionGroup].disabled}>
                    ${optionGroups[optionGroup].options.map((option) =>
                        dependencies.html`<option disabled=${option.disabled} value=${option.value}>${option.text}</option>`)}
                </optgroup>`)}
        </select>`
};