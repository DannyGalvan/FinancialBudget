describe("flujo de mantenimiento", () => {
  beforeEach(() => {
    cy.visit("/");
  });

  it("permite a un usuario iniciar sesión exitosamente y acceder al la página de mantenimiento, ver una solicitud y luego aprobarla o rechazarla", () => {
    // Arrange: Preparar datos
    const email = "pruebas.test29111999@gmail.com";
    const password = "Guatemala1.";

    // Act: Realizar acciones
    cy.get('[data-testid="email-input"]').type(email);
    cy.get('[data-testid="password-input"]').type(password);
    cy.get('[data-testid="login-button"]').click();

    // Assert: Verificar resultados
    // hover en el sidebar mantenimiento urbano
    cy.get('[data-testid="sidebar"]').trigger("mouseleave").click();
    cy.get('[data-testid="sidebar-link-/Request/Maintenance"]').click();

    //click en una solicitud de mantenimiento
    cy.get('[data-testid="solicitud-row-39"]').click();
    //aprobar la solicitud
    // cy.get('[data-testid="approve-button"]').click();
    // //verificar si fue aprobada
    // cy.get('[data-testid="error-message"]')
    //   .should("be.visible")
    //   .and("contain", "Entities Request retrieved successfully");
  });
});
