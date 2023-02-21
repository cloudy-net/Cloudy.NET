
describe('Page - Create', () => {

  beforeEach(() => {
    // Add spy for save/get request
    cy.getSpy();
    cy.saveSpy();
  })

  it('Create new entity. Save once.', () => {

    const uniqueName = `${Date.now()}_Cypress was here!`

    cy.visit('/Admin/New?EntityType=Page');
    cy.typeName(uniqueName);
    cy.clickSave();

    cy.get('@saving').should('have.been.calledOnce');
    cy.get('@getting').should('have.been.calledOnce');
    cy.verifyNoValidationError();

    cy.visit('/Admin/List?EntityType=Page');
    cy.get('.table-responsive ul.pagination a.page-link').not(':contains("Next")').last().click();

    cy.get('a').contains(uniqueName).should('exist');
  })

  it('Create new entity. Save twice', () => {

    const uniqueName = `${Date.now()}_Cypress was here!`
    const modifiedUniqueName = `${uniqueName} - Yet again!`;

    cy.visit('/Admin/New?EntityType=Page');

    cy.typeName(uniqueName);
    cy.clickSave();
    cy.verifyNoValidationError();

    cy.wait(2000);
    cy.typeName(modifiedUniqueName, true);
    cy.clickSave();

    cy.get('@saving').should('have.been.calledTwice');
    cy.get('@getting').should('have.been.calledThrice');
    cy.verifyNoValidationError();
    
    cy.visit('/Admin/List?EntityType=Page');
    cy.get('.table-responsive ul.pagination a.page-link').not(':contains("Next")').last().click();

    cy.get('a').contains(modifiedUniqueName).should('exist');
  })
})