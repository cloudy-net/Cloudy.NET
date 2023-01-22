import { html, useContext, useEffect, useState } from '../../../preact-htm/standalone.module.js';
import stateManager from '../../../data/state-manager.js';
import EntityContext from '../../entity-context.js';
import simpleChangeHandler from '../../../data/change-handlers/simple-change-handler.js';
import Option from './option';

export default ({ name, path, settings }) => {
    const [allOptions, setAllOptions] = useState([]);
    const [options, setOptions] = useState([]);
    const [optionGroups, setOptionGroups] = useState({});
    const [initialValue, setInitialValue] = useState(settings.isMultiSelect ? [] : null);
    const { contentReference, state } = useContext(EntityContext);
    const onChange = event => {
        settings.isMultiSelect
            ? simpleChangeHandler.setValue(stateManager, contentReference, path, Array.from(event.target.options).filter(o => o.selected).map(o => o.value))
            : simpleChangeHandler.setValue(stateManager, contentReference, path, event.target.value);
    };

    useEffect(function () {
        let value = simpleChangeHandler.getIntermediateValue(state, path);
        value = settings.isMultiSelect ? (!!value && value.length ? value : []) : null;
        setInitialValue(value);

        (async () => {
            await fetch(
                `/Admin/api/controls/customselect/list/${settings.factoryAssemblyQualifiedName}`,
                {
                    credentials: 'include'
                }
            )
            .then(r => r.json())
            .then(options => {
                const optionGroups = {};
                options.filter(option => !!option.group).forEach(option => {
                    if (!optionGroups[option.group.name]) optionGroups[option.group.name] = { disabled: option.group.disabled, options: [] };
                    optionGroups[option.group.name].options.push({
                        value: option.value,
                        text: option.text,
                        selected: option.selected,
                    });
                });

                setAllOptions(options);
                setOptions(options.filter(option => !option.group));
                setOptionGroups(optionGroups);
            });
        })();
    }, []);

    useEffect(() => {
        if (state.new) {
            const value = settings.isMultiSelect
                ? allOptions.filter(o => o.selected).map(o => o.value)
                : (allOptions.find(o => o.selected) || {}).value || null;
            
            simpleChangeHandler.setValue(stateManager, contentReference, path, value);
        }
    }, [allOptions]);

    return html`
        <select id=${name} name=${name} onChange=${onChange} class="form-select" multiple=${settings.isMultiSelect} size=${settings.isMultiSelect ? "10" : null}>
            ${options.map((option, index) => html`
                <${Option} 
                    option=${option} 
                    key=${index}
                    isMultiSelect=${settings.isMultiSelect}
                    initialValue=${initialValue}
                    preselect=${state.new} />`)}

            ${Object.keys(optionGroups).map((optionGroup, optionGroupIndex) => html`
                <optgroup key=${optionGroupIndex} label=${optionGroup} disabled=${optionGroups[optionGroup].disabled}>
                    ${optionGroups[optionGroup].options.map((option, index) => html`
                        <${Option} 
                            option=${option} 
                            key=${index}
                            isMultiSelect=${settings.isMultiSelect}
                            initialValue=${initialValue}
                            preselect=${state.new} />`)}
                </optgroup>`)}
        </select>`
};