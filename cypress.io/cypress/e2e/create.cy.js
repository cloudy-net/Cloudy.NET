
describe('Create test', () => {
  it('Create new entity. Save once.', () => {

    const uniqueName = `${Date.now()}_Cypress was here!`

    cy.visit('/Admin/New?EntityType=Page');
    cy.get('input[name="Name"]').type(uniqueName);
    cy.get('button.btn.btn-primary').contains('Save').click();
    cy.visit('/Admin/List?EntityType=Page');
    cy.get('.table-responsive ul.pagination a.page-link').not(':contains("Next")').last().click();

    cy.get('a').contains(uniqueName).should('exist');
  })

  it('Create new entity. Save twice', () => {

    const uniqueName = `${Date.now()}_Cypress was here!`
    const modifiedUniqueName = `${uniqueName} - Yet again!`;

    cy.visit('/Admin/New?EntityType=Page');

    cy.get('input[name="Name"]').type(uniqueName);
    cy.get('button.btn.btn-primary').contains('Save').click();

    cy.get('input[name="Name"]').clear().type(modifiedUniqueName);
    cy.get('button.btn.btn-primary').contains('Save').click();

    cy.visit('/Admin/List?EntityType=Page');
    cy.get('.table-responsive ul.pagination a.page-link').not(':contains("Next")').last().click();

    cy.get('a').contains(modifiedUniqueName).should('exist');
  })
})