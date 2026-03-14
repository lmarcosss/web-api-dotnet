using Moq;
using WebApi.Application.Services;
using WebApi.Application.Services.Interfaces;
using WebApi.Domain.Models;
using WebApi.Infra.Repositories.Interfaces;
using Xunit;

namespace WebApi.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public void Login_WhenUserNotFound_ReturnsNull()
    {
        var userRepoMock = new Mock<IUserRepository>();
        var tokenServiceMock = new Mock<ITokenService>();

        userRepoMock
        .Setup(x => x.GetByEmail(It.IsAny<string>()))
        .Returns((User?)null);

        var authService = new AuthService(
            userRepoMock.Object,
            tokenServiceMock.Object
        );

        var result = authService.Login("test@email.com", "123456");

        Assert.Null(result);
    }

    [Fact]
    public void Login_WhenPasswordIsInvalid_ReturnsNull()
    {
        var userRepoMock = new Mock<IUserRepository>();
        var tokenServiceMock = new Mock<ITokenService>();

        var user = new User("test", DateTime.Today, null, "test@email.com", BCrypt.Net.BCrypt.HashPassword("654321"));

        userRepoMock
        .Setup(x => x.GetByEmail(It.IsAny<string>()))
        .Returns(user);

        tokenServiceMock
        .Setup(x => x.GenerateToken(It.IsAny<User>()))
        .Returns("fake-token");

        var authService = new AuthService(
            userRepoMock.Object,
            tokenServiceMock.Object
        );

        var result = authService.Login("test@email.com", "123456");

        Assert.Null(result);
    }

    [Fact]
    public void Login_WhenCredentialsAreValid_ReturnsTokenFromTokenService()
    {
        var userRepoMock = new Mock<IUserRepository>();
        var tokenServiceMock = new Mock<ITokenService>();

        var user = new User("test", DateTime.Today, null, "test@email.com", BCrypt.Net.BCrypt.HashPassword("123456"));

        userRepoMock
        .Setup(x => x.GetByEmail(It.IsAny<string>()))
        .Returns(user);

        tokenServiceMock
        .Setup(x => x.GenerateToken(It.IsAny<User>()))
        .Returns("fake-token");

        var authService = new AuthService(
            userRepoMock.Object,
            tokenServiceMock.Object
        );

        var result = authService.Login("test@email.com", "123456");

        Assert.Equal("fake-token", result);
    }

    [Fact]
    public void Login_WhenCredentialsAreValid_CallsGetByEmailWithGivenEmail()
    {
        var userRepoMock = new Mock<IUserRepository>();
        var tokenServiceMock = new Mock<ITokenService>();

        var user = new User(
            "test",
            DateTime.Today,
            null,
            "test@email.com",
            BCrypt.Net.BCrypt.HashPassword("123456")
        );

        userRepoMock
            .Setup(x => x.GetByEmail(It.IsAny<string>()))
            .Returns(user);

        tokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns("token");

        var authService = new AuthService(
            userRepoMock.Object,
            tokenServiceMock.Object
        );

        authService.Login("test@email.com", "123456");

        userRepoMock.Verify(
            x => x.GetByEmail("test@email.com"),
            Times.Once
        );
    }

    [Fact]
    public void Login_WhenCredentialsAreValid_CallsGenerateTokenWithUser()
    {
        var userRepoMock = new Mock<IUserRepository>();
        var tokenServiceMock = new Mock<ITokenService>();

        var user = new User(
            "test",
            DateTime.Today,
            null,
            "test@email.com",
            BCrypt.Net.BCrypt.HashPassword("123456")
        );

        userRepoMock
            .Setup(x => x.GetByEmail(It.IsAny<string>()))
            .Returns(user);

        tokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns("token");

        var authService = new AuthService(
            userRepoMock.Object,
            tokenServiceMock.Object
        );

        authService.Login("test@email.com", "123456");

        tokenServiceMock.Verify(
            x => x.GenerateToken(user),
            Times.Once
        );

    }
}
