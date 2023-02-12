
describe('Property test bed - Create', () => {
  it('Test all props', () => {

    // Add spy for save/get request
    //cy.getSpy();
    cy.saveSpy();

    cy.visit('/Admin/List?EntityType=PropertyTestBed');

    const fieldsAndValues = {
      'Integer': '5',
      'NullableInteger': 10,
      'Double': 5.2,
      'NullableDouble': 5.3,
      'DateTime': '2023-01-02T14:53:21',
      'NullableDateTime': '2023-03-02T14:53:21',
      'DateTimeOffset': '2023-03-02T14:53:21',
      'NullableDateTimeOffset': '2023-04-02T14:53:21',
      'DateTimeWithDate': '2023-08-02T14:53:21',
      'DateTimeOffsetWithDate': '2023-09-02T14:53:21',
      'DateTimeWithTime': '2023-09-02T14:53:21',
      'DateTimeOffsetWithTime': '2023-09-02T14:53:21',
      'DateOnly': '2023-09-02',
      'NullableDateOnly': '2023-09-02',
      'TimeOnly': '12:45',
      'NullableTimeOnly': '12:43',
    };

    Object.keys(fieldsAndValues).map(
      key => cy.get(`input[name="${key}"]`).clear().type(fieldsAndValues[key], { delay: 0 })
    );

    cy.get('input[name="Checkbox"]').click()
    cy.get('input[name="NullableCheckbox"]').click()

    cy.get('select[name="Color"]').select('#fff');
    cy.get('select[name="SecondColor"]').select('#f56c43');

    cy.get('input[id="cb-Colors-0-0"]').click()
    cy.get('input[id="cb-Colors-2"]').click()
    
    cy.get('select[name="Category"]').select('2');

    // cy.clickSave();
    // cy.get('@saving').should('have.been.calledOnce');
    //cy.get('@getting').should('have.been.calledOnce');
  })
})