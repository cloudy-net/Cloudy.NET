
describe('Edit test', () => {
  it('Visit existing entity. Save twice.', () => {
    cy.visit('/Admin/List?EntityType=Page');
    cy.get('table.table--content-list td a').first().click();
    cy.get('input[name="Name"]').type('... cypress was here!');
    cy.get('button.btn.btn-primary').contains('Save').click()
    cy.get('input[name="Name"]').type(' - Yet again!');
    cy.get('button.btn.btn-primary').contains('Save').click()
  })
})