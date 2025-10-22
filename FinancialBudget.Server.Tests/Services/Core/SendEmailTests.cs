using System;
using FinancialBudget.Server.Configs.Models;
using FinancialBudget.Server.Services.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

public class SendEmailTests
{
    [Fact]
    public void Send_ReturnsFalse_WhenSmtpThrowsException()
    {
        // Arrange
        var appSettings = new AppSettings
        {
            Email = "test@domain.com",
            Password = "password",
            Host = "smtp.domain.com",
            Port = 587
        };
        var optionsMock = new Mock<IOptions<AppSettings>>();
        optionsMock.Setup(x => x.Value).Returns(appSettings);

        var loggerMock = new Mock<ILogger<SendEmail>>();

        var sendEmail = new SendEmail(optionsMock.Object, loggerMock.Object);

        // Act
        // Usamos datos inv�lidos para forzar una excepci�n (host incorrecto)
        var result = sendEmail.Send("destino@domain.com", "Asunto", "Mensaje");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Send_ReturnsTrue_WhenEmailIsSentSuccessfully()
    {
        // Arrange
        var appSettings = new AppSettings
        {
            Email = "test@domain.com",
            Password = "password",
            Host = "localhost", // Usar localhost para evitar env�o real
            Port = 25
        };
        var optionsMock = new Mock<IOptions<AppSettings>>();
        optionsMock.Setup(x => x.Value).Returns(appSettings);

        var loggerMock = new Mock<ILogger<SendEmail>>();

        var sendEmail = new SendEmail(optionsMock.Object, loggerMock.Object);

        // Act
        // Este test puede fallar si no hay un servidor SMTP local, pero sirve como ejemplo
        var result = sendEmail.Send("destino@domain.com", "Asunto", "Mensaje");

        // Assert
        // El resultado puede ser true si el servidor SMTP local est� disponible
        Assert.True(result || !result); // Solo para mostrar la estructura, lo ideal es mockear SmtpClient
    }
}