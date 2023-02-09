import { html, useContext, useEffect, useState } from '../../../preact-htm/standalone.module.js';
import EntityContext from '../../entity-context.js';
import simpleChangeHandler from '../../../data/change-handlers/simple-change-handler.js';
import urlFetcher from '../../../util/url-fetcher.js';
import ValidationManager from '../../../data/validation-manager.js';

export default ({ name, path, settings, validators }) => {
    const [options, setOptions] = useState([]);
    const [placeholderItemText, setPlaceholderItemText] = useState(null);
    const [optionGroups, setOptionGroups] = useState({});
    const [hasInitialValue, setHasInitialValue] = useState({});
    
    const { entityReference, state } = useContext(EntityContext);

    useEffect(function () {
        
        setHasInitialValue(!!simpleChangeHandler.getIntermediateValue(state, path));

        (async () => {
            const responseData = await urlFetcher.fetch(
                `/Admin/api/controls/customselect/list/?entityType=${settings.entityType}&propertyName=${settings.propertyName}`,
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

    return html`
        <select required=${settings.isRequired} id=${name} name=${name} value=${simpleChangeHandler.getIntermediateValue(state, path)} onChange=${e => simpleChangeHandler.setValue(entityReference, path, e.target.value, validators)} class="form-select ${ ValidationManager.getValidationClass(validators, state.validationResults, path) }">
        
            ${!!placeholderItemText ? html`<option selected=${!hasInitialValue} value="">${placeholderItemText}</option>` : null}

            ${options.map((option) => html`<option disabled=${option.disabled} value=${option.value}>${option.text}</option>`)}

            ${Object.keys(optionGroups).map((optionGroup) => html`
                <optgroup label=${optionGroup} disabled=${optionGroups[optionGroup].disabled}>
                    ${optionGroups[optionGroup].options.map((option) =>
                        html`<option disabled=${option.disabled} value=${option.value}>${option.text}</option>`)}
                </optgroup>`)}
        </select>`
};