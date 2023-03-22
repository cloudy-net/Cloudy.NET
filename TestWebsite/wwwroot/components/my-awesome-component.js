
const Control = ({ name, path, validators, dependencies }) => {
    const { entityReference, state } = dependencies.useContext(dependencies.EntityContext);

    return dependencies.html`
        <style>
            .my-awsome-heading {
                margin-bottom: 3px;
                padding: 10px;
                border: 1px solid gray;
                border-radius: 5px;
            }

            .my-awesome-input {
                background: lightblue;
            }
        </style>

        <div class="my-awsome-heading">
            👇 Custom component 👇
        </div>

        <input
            style="background: lightblue"
            type="text"
            class=${`my-awesome-input form-control ${dependencies.ValidationManager.getValidationClass(state.validationResults, path)} `}
            id=${dependencies.componentContextProvider.getIdentifier(path)}
            value=${dependencies.simpleChangeHandler.getIntermediateValue(state, path)}
            onInput=${(e) => dependencies.simpleChangeHandler.setValue(entityReference, path, e.target.value, validators)}
          />`;
}

export default Control;