import { html, useContext, useEffect, useState } from '../../preact-htm/standalone.module.js';
import stateManager from '../../data/state-manager.js';
import EntityContext from '../entity-context.js';
import simpleChangeHandler from '../../data/change-handlers/simple-change-handler.js';

export default ({ name, path, settings }) => {
    const [options, setOptions] = useState([]);
    const [optionGroups, setOptionGroups] = useState({});
    const [initialValue, setInitialValue] = useState();
    const { contentReference, state } = useContext(EntityContext);

    const onChange = event => {
        simpleChangeHandler.setValue(stateManager, contentReference, path, event.target.value)
    };

    useEffect(function () {
        setInitialValue(simpleChangeHandler.getIntermediateValue(state, path));

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

                setOptions(options.filter(option => !option.group));
                setOptionGroups(optionGroups);

                const preselectedOption = options.find(o => o.selected);
                if (!initialValue && preselectedOption) {
                    simpleChangeHandler.setValue(stateManager, contentReference, path, preselectedOption.value);
                }
            });
        })();
    }, []);

    return options.length || Object.keys(optionGroups).length
        ? html`<select id=${name} name=${name} onChange=${onChange} class="form-select">
            ${options.length
                ? options.map((option, index) =>
                    html`<option
                        disabled=${option.disabled}
                        selected=${initialValue == option.value}
                        value=${option.value}
                        key=${index}>${option.text}
                    </option>`
                )
                : null
            }

            ${Object.keys(optionGroups).length
                ? Object.keys(optionGroups).map((optionGroup, optionGroupIndex) =>
                    html`
                    <optgroup key=${optionGroupIndex} label=${optionGroup} disabled=${optionGroups[optionGroup].disabled}>
                        ${optionGroups[optionGroup].options.map((option, optionIndex) => html`
                            <option
                                disabled=${option.disabled}
                                selected=${initialValue == option.value || (!initialValue && option.selected)}
                                value=${option.value}
                                key=${optionIndex}>${option.text}
                            </option>`
                        )}
                    </optgroup>`
                )
                : null
            }
        </select>`
        : null;
};