let consoleSpy;
Cypress.on('window:before:load', (win) => {
    consoleSpy = cy.spy(win.console, "error")
})
afterEach(() => {
    // consoleSpy can be null if test failed already in beforeEach 
    if (consoleSpy) {
      expect(consoleSpy).not.to.be.called
    }
})