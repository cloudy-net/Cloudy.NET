import notificationManager from "../NotificationSupport/notification-manager.js";



/* FORM FIELD DESCRIPTOR PROVIDER */

class FormFieldDescriptorProvider {
    getFor(formId) {
        return fetch(`Field/GetAllForForm?id=${formId}`, {
            credentials: 'include'
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`${response.status} (${response.statusText})`);
                }

                return response.json();
            })
            .catch(error => notificationManager.addNotification(item => item.setText(`Could not get field descriptors for form ${formId} (${error.name}: ${error.message})`)));
    }
}

export default FormFieldDescriptorProvider;