
describe('Page - Edit', () => {
  it('Visit existing entity. Save twice.', () => {

    // Add spy for save request
    cy.saveSpy();
    cy.wait(2000);

    // Visist first page
    cy.visit('/Admin/List?EntityType=Page');
    cy.wait(2000);
    cy.get('table.table--content-list td a').first().click();
    
    cy.wait(2000);
    // Modify name
    cy.typeName('Hi!... cypress was here!', true);

    cy.wait(2000);
    // Save and await request
    cy.clickSave();

    cy.wait(2000);
    
    // Modify name
    cy.typeName(' - Yet again!');
    cy.wait(2000);

    // Save and await request
    cy.clickSave();
    cy.wait(2000);
    cy.get('@saving').should('have.been.calledTwice');

    // Revisit and assert
    cy.get('input[name="Name"]').invoke('val').should('include', '... cypress was here! - Yet again!')
  })
})
