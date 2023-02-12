
describe('Required validation', () => {

  beforeEach(() => {
    cy.saveSpy();
    cy.visit('/Admin/New?EntityType=Page');
  })
  
  it('Fires for empty value', () => {
    cy.clickSave();
    cy.verifyValidationError();
    cy.get('@saving').should('not.have.been.called');
  })

  it('Not fire when name is entered', () => {
    cy.typeName('some name');
    cy.clickSave();
    cy.verifyNoValidationError();
    cy.get('@saving').should('have.been.calledOnce');
  })

})