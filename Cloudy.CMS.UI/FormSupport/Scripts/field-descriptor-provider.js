


/* FORM FIELD DESCRIPTOR PROVIDER */

class FormFieldDescriptorProvider {
    getFor(formId) {
        return fetch(`Poetry.UI.FormSupport/Field/GetAllForForm?id=${formId}`, {
            credentials: 'include'
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`${response.status} (${response.statusText})`);
                }

                return response.json();
            });
    }
}

export default FormFieldDescriptorProvider;