
describe('Page - Edit', () => {
  it('Visit existing entity. Save twice.', () => {

    // Add spy for save request
    cy.saveSpy();

    // Visist first page
    cy.visit('/Admin/List?EntityType=Page');
    cy.get('table.table--content-list td a').first().click();
    
    // Modify name
    cy.typeName('Hi!... cypress was here!', true);

    // Save and await request
    cy.clickSave();
    
    // Modify name
    cy.typeName(' - Yet again!');

    // Save and await request
    cy.clickSave();
    cy.get('@saving').should('have.been.calledTwice');

    // Revisit and assert
    cy.reload();
    cy.get('input[name="Name"]').invoke('val').should('include', '... cypress was here! - Yet again!')
  })
})
