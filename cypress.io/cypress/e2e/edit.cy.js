
describe('Edit test', () => {
  it('Visit existing entity. Save twice.', () => {
    cy.intercept('POST', '/Admin/api/form/entity/save').as('saving')

    cy.visit('/Admin/List?EntityType=Page');
    cy.get('table.table--content-list td a').first().click();
    cy.get('input[name="Name"]').clear().type('Hi!... cypress was here!');
    
    cy.get('button.btn.btn-primary').contains('Save').click()
    cy.wait('@saving')
    
    cy.get('input[name="Name"]').type(' - Yet again!');

    cy.get('button.btn.btn-primary').contains('Save').click();
    cy.wait('@saving')

    cy.reload();
    cy.get('input[name="Name"]').invoke('val').should('include', '... cypress was here! - Yet again!')
  })
})
