
describe('Edit test', () => {
  it('Visit existing entity. Save twice.', () => {

    // Add alias for save request
    cy.intercept('POST', '/Admin/api/form/entity/save').as('saving')

    // Visist first page
    cy.visit('/Admin/List?EntityType=Page');
    cy.get('table.table--content-list td a').first().click();
    
    // Modify name
    cy.get('input[name="Name"]').clear().type('Hi!... cypress was here!');
    cy.wait(1000);

    // Save and await request
    cy.get('button.btn.btn-primary').contains('Save').click()
    cy.wait('@saving')
    cy.wait(1000);
    
    // Modify name
    cy.get('input[name="Name"]').type(' - Yet again!');
    cy.wait(1000);

    // Save and await request
    cy.get('button.btn.btn-primary').contains('Save').click();
    cy.wait('@saving')
    cy.wait(1000);

    // Revisit and assert
    cy.reload();
    cy.get('input[name="Name"]').invoke('val').should('include', '... cypress was here! - Yet again!')
  })
})
