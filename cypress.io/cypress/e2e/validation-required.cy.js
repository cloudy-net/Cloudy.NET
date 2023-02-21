
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
    cy.get('select[id="cld-Color"]').select('');
    cy.get('select[id="cld-SecondColor"]').select('');
    cy.clickSave();
    cy.verifyValidationError();
    cy.get('@saving').should('not.have.been.called');
  })

  it('Not fire when select has value', () => {
    cy.visit('/Admin/List?entityType=PropertyTestBed');
    cy.typeName('a');
    cy.get('select[id="cld-Color"]').select('#fff');
    cy.get('select[id="cld-SecondColor"]').select('#f56c43');
    cy.clickSave();
    cy.verifyNoValidationError();
    cy.get('@saving').should('have.been.calledOnce');
  })

  it('Validation error goes away after setting value', () => {
    cy.visit('/Admin/New?entityType=PropertyTestBed');
    cy.wait(1000);
    cy.clickSave();
    cy.verifyValidationError();
    cy.get('@saving').should('not.have.been.called');

    cy.typeName('a');
    cy.get('select[id="cld-Color"]').select('#fff');
    cy.get('select[id="cld-SecondColor"]').select('#f56c43');

    cy.verifyNoValidationError();
    cy.clickSave();
    cy.verifyNoValidationError();
    cy.get('@saving').should('have.been.calledOnce');
  })

})