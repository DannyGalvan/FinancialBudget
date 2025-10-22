describe("Flujo de Login", () => {
  beforeEach(() => {
    cy.visit("/login");
  });

  it("permite a un usuario iniciar sesión exitosamente", () => {
    // Arrange: Preparar datos
    const email = "pruebas.test29111999@gmail.com";
    const password = "Guatemala1.";

    // Act: Realizar acciones
    cy.get('[data-testid="email-input"]').type(email);
    cy.get('[data-testid="password-input"]').type(password);
    cy.get('[data-testid="login-button"]').click();

    // Assert: Verificar resultados
    cy.url().should("include", "/");
    cy.contains("Resumen de Fondos").should("be.visible");
  });

  it("muestra error con credenciales inválidas", () => {
    cy.get('[data-testid="email-input"]').type(
      "pruebas.test29111999@gmail.com",
    );
    cy.get('[data-testid="password-input"]').type("Guatemala2.");
    cy.get('[data-testid="login-button"]').click();

    cy.get('[data-testid="error-message"]')
      .should("be.visible")
      .and("contain", "Usuario y/o contraseña invalidos");
  });
});
