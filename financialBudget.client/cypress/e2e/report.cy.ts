describe("flujo de reportes", () => {
  beforeEach(() => {
    cy.visit("/");
  });

  it("permite a un usuario iniciar sesión exitosamente y acceder a la página de eventos comunitarios", () => {
    // Arrange: Preparar datos
    const email = "pruebas.test29111999@gmail.com";
    const password = "Guatemala1.";

    // Act: Realizar acciones
    cy.get('[data-testid="email-input"]').type(email);
    cy.get('[data-testid="password-input"]').type(password);
    cy.get('[data-testid="login-button"]').click();

    // Assert: Verificar resultados
    // hover en el sidebar eventos comunitarios
    cy.get('[data-testid="sidebar"]').trigger("mouseleave").click();
    cy.get('[data-testid="sidebar-link-/Report/Financial"]').click();
    //click en reportes financieros
    cy.contains("Reporte Financiero").should("be.visible");
  });
});
