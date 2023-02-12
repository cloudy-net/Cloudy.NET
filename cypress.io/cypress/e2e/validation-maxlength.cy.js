
describe('MaxLength validation', () => {

  beforeEach(() => {
    cy.saveSpy();
    cy.visit('/Admin/New?EntityType=Page');
  })
  
  it('Fire when a too long name is entered', () => {
    cy.typeName('a'.repeat(46));
    cy.clickSave();
    cy.verifyValidationError();
    cy.get('@saving').should('not.have.been.called');
  })
  
})