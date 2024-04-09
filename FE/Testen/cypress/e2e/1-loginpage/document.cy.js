/// <reference types="cypress" />

describe('start screen is loaded correctly', () => {
    beforeEach(() => {
      cy.visit('http://127.0.0.1:5500/App')
    })
    it('cy.document() - get the document object', () => {
        // https://on.cypress.io/document
        cy.document().should('have.property', 'charset').and('eq', 'UTF-8')
    })
    
    it('cy.title() - get the title', () => {
    // https://on.cypress.io/title
    cy.title().should('include', 'Boter Kaas en Eieren');
    })
})
  