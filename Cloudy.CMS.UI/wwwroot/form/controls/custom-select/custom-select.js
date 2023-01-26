import { html, useContext, useEffect, useState } from '../../../preact-htm/standalone.module.js';
import stateManager from '../../../data/state-manager.js';
import EntityContext from '../../entity-context.js';
import simpleChangeHandler from '../../../data/change-handlers/simple-change-handler.js';
import urlFetcher from '../../../util/url-fetcher.js';

export default ({ name, path, settings }) => {
    const [options, setOptions] = useState([]);
    const [optionGroups, setOptionGroups] = useState({});
    const { entityReference, state } = useContext(EntityContext);
    const onChange = event => {
        settings.isMultiSelect
            ? simpleChangeHandler.setValue(entityReference, path, [...event.target.options].filter(o => o.selected).map(o => o.value))
            : simpleChangeHandler.setValue(entityReference, path, event.target.value);
    };

    useEffect(function () {
        (async () => {
            const options = await urlFetcher.fetch(
                `/Admin/api/controls/customselect/list/${settings.factoryAssemblyQualifiedName}`,
                {
                    credentials: 'include'
                },
                'Could not get select options'
            );

            const optionGroups = {};
            options.filter(option => !!option.group).forEach(option => {
                if (!optionGroups[option.group.name]) optionGroups[option.group.name] = { disabled: option.group.disabled, options: [] };
                optionGroups[option.group.name].options.push({
                    value: option.value,
                    text: option.text,
                    selected: option.selected,
                });
            });

            setOptions(options.filter(option => !option.group));
            setOptionGroups(optionGroups);
        })();
    }, []);

    return html`
        <select id=${name} name=${name} value=${simpleChangeHandler.getIntermediateValue(state, path)} onChange=${onChange} class="form-select" multiple=${settings.isMultiSelect} size=${settings.isMultiSelect && "10"}>
            ${!settings.isMultiSelect && html`<option value=""></option>`}
            ${options.map((option) => html`<option disabled=${option.disabled} value=${option.value}>${option.text}</option>`)}

            ${Object.keys(optionGroups).map((optionGroup) => html`
                <optgroup label=${optionGroup} disabled=${optionGroups[optionGroup].disabled}>
                    ${optionGroups[optionGroup].options.map((option) =>
                        html`<option disabled=${option.disabled} value=${option.value}>${option.text}</option>`)}
                </optgroup>`)}
        </select>`
};