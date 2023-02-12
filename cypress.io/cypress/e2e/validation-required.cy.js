
describe('Required validation', () => {

  beforeEach(() => {
    cy.saveSpy();
  })
  
  it('Fires for empty input value', () => {
    cy.visit('/Admin/New?EntityType=Page');
    cy.clickSave();
    cy.verifyValidationError();
    cy.get('@saving').should('not.have.been.called');
  })

  it('Not fire when input has value', () => {
    cy.visit('/Admin/New?EntityType=Page');
    cy.typeName('a');
    cy.clickSave();
    cy.verifyNoValidationError();
    cy.get('@saving').should('have.been.calledOnce');
  })
  
  it('Fires for empty select value', () => {
    cy.visit('/Admin/List?entityType=PropertyTestBed');
    cy.get('select[name="Color"]').select('');
    cy.get('select[name="SecondColor"]').select('');
    cy.clickSave();
    cy.verifyValidationError();
    cy.get('@saving').should('not.have.been.called');
  })

  // it('Not fire when select has value', () => {
  //   cy.visit('/Admin/List?entityType=PropertyTestBed');
  //   cy.get('select[name="Color"]').select('#fff');
  //   cy.get('select[name="SecondColor"]').select('#f56c43');
  //   cy.clickSave();
  //   cy.verifyNoValidationError();
  //   cy.get('@saving').should('have.been.calledOnce');
  // })

})