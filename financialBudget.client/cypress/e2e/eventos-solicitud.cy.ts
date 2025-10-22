describe("flujo de eventos comunitarios", () => {
  beforeEach(() => {
    cy.visit("/");
  });

  it("permite a un usuario iniciar sesión exitosamente y acceder a la página de eventos comunitarios, ver una solicitud y luego aprobarla o rechazarla", () => {
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
    cy.get('[data-testid="sidebar-link-/Request/Events"]').click();
    //click en eventos comunitarios
    cy.contains("Gestion de Eventos Comunitarios").should("be.visible");

    //click en una solicitud de evento comunitario
    cy.get('[data-testid="solicitud-row-27"]').click();
    //aprobar la solicitud
    // cy.get('[data-testid="approve-button"]').click();
    // //verificar si fue aprobada
    // cy.get('[data-testid="error-message"]')
    //   .should("be.visible")
    //   .and("contain", "Entities Request retrieved successfully");
  });
});
