
describe('Property test bed - Create', () => {
  it('Test all props', () => {

    // Add spy for save/get request
    cy.saveSpy();

    cy.visit('/Admin/List?EntityType=PropertyTestBed');

    const fieldsAndValues = {
      'cld-Name': 'some-name',
      'cld-Integer': '5',
      'cld-NullableInteger': 10,
      'cld-Double': 5.2,
      'cld-NullableDouble': 5.3,
      'cld-DateTime': '2023-01-02T14:53:21',
      'cld-NullableDateTime': '2023-03-02T14:53:21',
      'cld-DateTimeOffset': '2023-03-02T14:53:21',
      'cld-NullableDateTimeOffset': '2023-04-02T14:53:21',
      'cld-DateTimeWithDate': '2023-08-02T14:53:21',
      'cld-DateTimeOffsetWithDate': '2023-09-02T14:53:21',
      'cld-DateTimeWithTime': '2023-09-02T14:53:21',
      'cld-DateTimeOffsetWithTime': '2023-09-02T14:53:21',
      'cld-DateOnly': '2023-09-02',
      'cld-NullableDateOnly': '2023-09-02',
      'cld-TimeOnly': '12:45',
      'cld-NullableTimeOnly': '12:43',
    };

    Object.keys(fieldsAndValues).map(
      key => cy.get(`input[id="${key}"]`).clear().type(fieldsAndValues[key], { delay: 0 })
    );

    cy.get('input[id="cld-Checkbox"]').click()
    cy.get('input[id="cld-NullableCheckbox"]').click()

    cy.get('select[id="cld-Color"]').select('#fff');
    cy.get('select[id="cld-SecondColor"]').select('#f56c43');

    cy.get('input[id="cld-Colors-0-0"]').click()
    cy.get('input[id="cld-Colors-2"]').click()
    
    cy.get('select[id="cld-Category"]').select('2');

    cy.clickSave();
    cy.get('@saving').should('have.been.calledOnce');
  })
})