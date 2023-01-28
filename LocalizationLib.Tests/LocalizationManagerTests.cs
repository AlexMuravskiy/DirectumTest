using Moq;
using System.Globalization;

namespace LocalizationLib.Tests;

public class LocalizationManagerTests
{
    private readonly Mock<ILocalizationSource> _localizationSourceMock;

    public LocalizationManagerTests()
    {
        _localizationSourceMock = new Mock<ILocalizationSource>();
    }

    [Fact]
    public async Task GetStringAsync_SendEmptyCode_ReturnNull()
    {
        //Arrange
        var code = Guid.Empty;
        var culture = new CultureInfo("ru-RU");

        var manager = new LocalizationManager();
        manager.RegisterSource(_localizationSourceMock.Object, culture);

        //Act
        var result = await manager.GetStringAsync(code);

        //Assert
        Assert.Null(result);        
    }

    [Fact]
    public async Task GetStringAsync_SourceNotFound_ReturnNull()
    {
        //Arrange
        var code = Guid.NewGuid();
        var culture = new CultureInfo("ru-RU");

        var manager = new LocalizationManager();

        //Act
        var result = await manager.GetStringAsync(code);

        //Assert
        Assert.Null(result);     
    }

    [Fact]
    public async Task GetStringAsync_CorrectCodeWithCulture_ReturnString()
    {
        //Arrange
        var code = Guid.NewGuid();
        var culture = new CultureInfo("ru-RU");
        string message = "Привет!";

        _localizationSourceMock.Setup(q=>q.FindStringAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(message);

        var manager = new LocalizationManager();
        manager.RegisterSource(_localizationSourceMock.Object, culture);

        //Act
        var result = await manager.GetStringAsync(code, culture: culture);

        //Assert
        Assert.Equal(result, message); 
    }

    [Fact]
    public async Task GetStringAsync_CorrectCodeWithoutCulture_ReturnString()
    {
        //Arrange
        var code = Guid.NewGuid();
        var culture = CultureInfo.CreateSpecificCulture("fr-FR");
        Thread.CurrentThread.CurrentCulture = culture;
        string message = "Hello!";

        _localizationSourceMock.Setup(q=>q.FindStringAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(message);

        var manager = new LocalizationManager();
        manager.RegisterSource(_localizationSourceMock.Object, culture);

        //Act
        var result = await manager.GetStringAsync(code, culture: null);

        //Assert
        Assert.Equal(result, message); 
    }
}