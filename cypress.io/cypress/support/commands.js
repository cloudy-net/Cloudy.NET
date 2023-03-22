// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add('login', (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })


Cypress.Commands.add('typeName', (value, clear) => {
    const input = cy.get('input[id="cld-Name"]');

    if(clear || !value){
        input.clear();
    }

    if(value){
        input.type(value, { delay: 0, force: true });
    }
})

Cypress.Commands.add('verifyNoValidationError', () => { 
    cy.get('.alert.alert-warning').should('not.exist');
    cy.get('.is-invalid').should('not.exist');
    cy.get('.invalid-feedback').should('not.exist');
})

Cypress.Commands.add('verifyValidationError', () => { 
    cy.get('.alert.alert-warning').should('exist');
    cy.get('.is-invalid').should('exist');
    cy.get('.alert.alert-warning').should('exist');
})

Cypress.Commands.add('clickSave', () => { 
    cy.get('button.btn.btn-primary').contains('Save').click();
})

Cypress.Commands.add('saveSpy', () => { 
    cy.intercept('POST', '/Admin/api/form/entity/save', cy.spy().as('saving'))
})

Cypress.Commands.add('getSpy', () => { 
    cy.intercept('POST', '/Admin/api/form/entity/get', cy.spy().as('getting'))
})