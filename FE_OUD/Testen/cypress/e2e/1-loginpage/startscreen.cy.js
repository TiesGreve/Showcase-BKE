
describe('start screen is loaded correctly', () => {
    beforeEach(() => {
      cy.visit('http://127.0.0.1:5500/App')
    })
    it("Title is shown", () => {
        cy.get(".title").should("have.text", "Boter Kaas en Eieren")
    })
    it("Buttons exists", () => {
        cy.get("#login").should("have.text", "login")
        cy.get("#register").should("have.text", "registreren")
    })
    it("Login button changes component conent", () => {
        cy.get("#login").click();

        cy.get("#Email").should("exist")
        cy.get("#Password").should("exist")
    })
    it("Able to login to account", () => {
        cy.get("#login").click();

        cy.get("#Email").type("admin@bke.com");
        cy.get("#Password").type("String!!1234");

        cy.get("#login-submit").click();

        cy.url().should("include", "home")
    })
})