
export default ({ name, path, dependencies }) => {
    const [options, setOptions] = dependencies.useState([]);
    const [optionGroups, setOptionGroups] = dependencies.useState({});
    const [currentValues, setCurrentValues] = dependencies.useState([]);

    const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);

    dependencies.useEffect(function () {
        setCurrentValues(dependencies.simpleChangeHandler.getIntermediateValue(state, path) || []);

        (async () => {
            const responseData = await dependencies.urlFetcher.fetch(
                `/Admin/api/controls/customselect/list/?entityType=${entityReference.entityType}&propertyName=${name}`,
                {
                    credentials: 'include'
                },
                'Could not get select options'
            );

            const options = responseData.items;

            const optionGroups = {};
            options.filter(option => !!option.group).forEach(option => {
                if (!optionGroups[option.group.name]) optionGroups[option.group.name] = { disabled: option.group.disabled, options: [] };
                optionGroups[option.group.name].options.push({
                    value: option.value,
                    text: option.text,
                    disabled: option.disabled,
                });
            });

            setOptions(options.filter(option => !option.group));
            setOptionGroups(optionGroups);
        })();
    }, []);

    const onChange = (e) => {
        const newValues = e.target.checked
            ? [...currentValues, e.target.value]
            : currentValues.filter(x => x !== e.target.value);
            
        setCurrentValues(newValues);
        dependencies.simpleChangeHandler.setValue(entityReference, path, newValues)
    };

    return dependencies.html`
        <div class="card">
            <div class="card-body">
                ${options.map((option, index) => dependencies.html`
                    <div class="form-check">
                        <input onChange=${onChange} checked=${currentValues.includes(option.value)} disabled=${option.disabled} class="form-check-input" type="checkbox" value="${option.value}" id="cb-${name}-${index}" />
                        <label class="form-check-label" for="cb-${name}-${index}">${option.text}</label>
                    </div>`)
                }

                ${Object.keys(optionGroups).map((optionGroup, optionGroupIndex) => dependencies.html`
                    <h6 class="mt-3">${optionGroup}</h6>
                    
                    ${optionGroups[optionGroup].options.map((option, index) => dependencies.html`
                        <div class="form-check">
                            <input onChange=${onChange} checked=${currentValues.includes(option.value)} disabled=${option.disabled} class="form-check-input" type="checkbox" id="cb-${name}-${optionGroupIndex}-${index}" value=${option.value}>${option.text}</option>
                            <label class="form-check-label" for="cb-${name}-${optionGroupIndex}-${index}">${option.text}</label>
                        </div>
                    `)}
                `)}
            </div>
        </div>
    `
};