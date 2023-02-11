import { html, useContext, useEffect, useState } from '../../../preact-htm/standalone.module.js';
import EntityContext from '../../entity-context.js';
import simpleChangeHandler from '../../../data/change-handlers/simple-change-handler.js';
import urlFetcher from '../../../util/url-fetcher.js';

export default ({ name, path }) => {
    const [options, setOptions] = useState([]);
    const [optionGroups, setOptionGroups] = useState({});
    const [currentValues, setCurrentValues] = useState([]);

    const { entityReference, state } = useContext(EntityContext);

    useEffect(function () {
        setCurrentValues(simpleChangeHandler.getIntermediateValue(state, path) || []);

        (async () => {
            const responseData = await urlFetcher.fetch(
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
        simpleChangeHandler.setValue(entityReference, path, newValues)
    };

    return html`
        <div class="card">
            <div class="card-body">
                ${options.map((option, index) => html`
                    <div class="form-check">
                        <input onChange=${onChange} checked=${currentValues.includes(option.value)} disabled=${option.disabled} class="form-check-input" type="checkbox" value="${option.value}" id="cb-${name}-${index}" />
                        <label class="form-check-label" for="cb-${name}-${index}">${option.text}</label>
                    </div>`)
                }

                ${Object.keys(optionGroups).map((optionGroup, optionGroupIndex) => html`
                    <h6 class="mt-3">${optionGroup}</h6>
                    
                    ${optionGroups[optionGroup].options.map((option, index) => html`
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