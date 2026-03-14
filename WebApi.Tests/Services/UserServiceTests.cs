using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using WebApi.Application.Services;
using WebApi.Application.ViewModel;
using WebApi.Domain.DTOs;
using WebApi.Domain.Models;
using WebApi.Infra.Repositories.Interfaces;
using Xunit;

namespace WebApi.Tests.Services;

public class UserServiceTests
{
    private const string BucketName = "test-bucket";

    private static Mock<IConfiguration> CreateConfigMock(string? bucketName = BucketName)
    {
        var mock = new Mock<IConfiguration>();
        mock.Setup(c => c["Cloud:FileStorageBucketName"]).Returns(bucketName);
        return mock;
    }

    [Fact]
    public void Constructor_WhenBucketNameNotConfigured_ThrowsArgumentNullException()
    {
        var repoMock = new Mock<IUserRepository>();
        var fileStorageMock = new Mock<Application.Services.Interfaces.IFileStorageService>();
        var mapperMock = new Mock<IMapper>();
        var configMock = CreateConfigMock(null);

        void Act() => _ = new UserService(
            repoMock.Object,
            fileStorageMock.Object,
            mapperMock.Object,
            configMock.Object);

        Assert.Throws<ArgumentNullException>(Act);
    }

    [Fact]
    public void GetAll_CallsRepositoryWithPageNumberAndPageSize_ReturnsMappedList()
    {
        var pageNumber = 2;
        var pageSize = 10;
        var users = new List<User>
        {
            new User("Alice", new DateTime(1990, 1, 1), null, "alice@test.com", "hash")
        };
        var dtos = new List<UserDTO>
        {
            new UserDTO { Id = 1, Name = "Alice", Email = "alice@test.com" }
        };

        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetAll(pageNumber, pageSize)).Returns(users);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<List<UserDTO>>(users)).Returns(dtos);

        var sut = new UserService(
            repoMock.Object,
            new Mock<Application.Services.Interfaces.IFileStorageService>().Object,
            mapperMock.Object,
            CreateConfigMock().Object);

        var result = sut.GetAll(pageNumber, pageSize);

        repoMock.Verify(r => r.GetAll(pageNumber, pageSize), Times.Once);
        Assert.Same(dtos, result);
    }

    [Fact]
    public void GetAll_WhenRepositoryReturnsEmptyList_ReturnsEmptyMappedList()
    {
        var users = new List<User>();
        var dtos = new List<UserDTO>();

        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(users);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<List<UserDTO>>(users)).Returns(dtos);

        var sut = new UserService(
            repoMock.Object,
            new Mock<Application.Services.Interfaces.IFileStorageService>().Object,
            mapperMock.Object,
            CreateConfigMock().Object);

        var result = sut.GetAll(1, 5);

        Assert.Empty(result);
        Assert.Same(dtos, result);
    }

    [Fact]
    public void GetById_WhenUserNotFound_ReturnsNull()
    {
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((User?)null);

        var sut = new UserService(
            repoMock.Object,
            new Mock<Application.Services.Interfaces.IFileStorageService>().Object,
            new Mock<IMapper>().Object,
            CreateConfigMock().Object);

        var result = sut.GetById(99);

        Assert.Null(result);
        repoMock.Verify(r => r.GetById(99), Times.Once);
    }

    [Fact]
    public void GetById_WhenUserExists_ReturnsMappedUserDTO()
    {
        var user = new User("Bob", new DateTime(1985, 5, 5), null, "bob@test.com", "hash");
        var dto = new UserDTO { Id = 1, Name = "Bob", Email = "bob@test.com" };

        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetById(1)).Returns(user);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<UserDTO>(user)).Returns(dto);

        var sut = new UserService(
            repoMock.Object,
            new Mock<Application.Services.Interfaces.IFileStorageService>().Object,
            mapperMock.Object,
            CreateConfigMock().Object);

        var result = sut.GetById(1);

        Assert.Same(dto, result);
        repoMock.Verify(r => r.GetById(1), Times.Once);
    }

    [Fact]
    public async Task Add_WhenPhotoIsNull_DoesNotCallFileStorage_AddsUserWithHashedPassword()
    {
        User? capturedUser = null;
        var userView = new UserViewModel
        {
            Name = "Charlie",
            DateOfBirth = new DateTime(1992, 3, 15),
            Email = "charlie@test.com",
            Password = "plainPassword",
            Photo = null
        };
        var dto = new UserDTO { Id = 1, Name = "Charlie", Email = "charlie@test.com" };

        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.Add(It.IsAny<User>()))
            .Callback<User>(u => capturedUser = u)
            .Returns(Task.CompletedTask);

        var fileStorageMock = new Mock<Application.Services.Interfaces.IFileStorageService>();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>())).Returns(dto);

        var sut = new UserService(
            repoMock.Object,
            fileStorageMock.Object,
            mapperMock.Object,
            CreateConfigMock().Object);

        var result = await sut.Add(userView);

        fileStorageMock.Verify(
            f => f.UploadAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IFormFile>()),
            Times.Never);
        repoMock.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        Assert.NotNull(capturedUser);
        Assert.NotEqual(userView.Password, capturedUser!.password);
        Assert.Same(dto, result);
    }

    [Fact]
    public async Task Add_WhenPhotoHasFileName_CallsFileStorageWithCorrectArgs_AddsUserWithPhotoUrl()
    {
        const string fileUrl = "https://bucket.s3.amazonaws.com/profileImage-charlie@test.com-charlie";
        User? capturedUser = null;
        var photoMock = new Mock<IFormFile>();
        photoMock.Setup(p => p.FileName).Returns("photo.jpg");

        var userView = new UserViewModel
        {
            Name = "Charlie",
            DateOfBirth = new DateTime(1992, 3, 15),
            Email = "charlie@test.com",
            Password = "plainPassword",
            Photo = photoMock.Object
        };
        var dto = new UserDTO { Id = 1, Name = "Charlie", Email = "charlie@test.com", Photo = fileUrl };

        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.Add(It.IsAny<User>()))
            .Callback<User>(u => capturedUser = u)
            .Returns(Task.CompletedTask);

        var fileStorageMock = new Mock<Application.Services.Interfaces.IFileStorageService>();
        fileStorageMock
            .Setup(f => f.UploadAsync(BucketName, "profileImage-charlie@test.com-charlie", photoMock.Object))
            .ReturnsAsync(fileUrl);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>())).Returns(dto);

        var sut = new UserService(
            repoMock.Object,
            fileStorageMock.Object,
            mapperMock.Object,
            CreateConfigMock().Object);

        var result = await sut.Add(userView);

        fileStorageMock.Verify(
            f => f.UploadAsync(BucketName, "profileImage-charlie@test.com-charlie", photoMock.Object),
            Times.Once);
        Assert.NotNull(capturedUser);
        Assert.Equal(fileUrl, capturedUser!.photo);
        Assert.Same(dto, result);
    }

    [Fact]
    public async Task Add_PasswordIsHashed_BeforePassingToRepository()
    {
        User? capturedUser = null;
        var userView = new UserViewModel
        {
            Name = "Diana",
            DateOfBirth = new DateTime(1988, 7, 20),
            Email = "diana@test.com",
            Password = "secret123",
            Photo = null
        };
        var dto = new UserDTO { Id = 1, Name = "Diana", Email = "diana@test.com" };

        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.Add(It.IsAny<User>()))
            .Callback<User>(u => capturedUser = u)
            .Returns(Task.CompletedTask);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>())).Returns(dto);

        var sut = new UserService(
            repoMock.Object,
            new Mock<Application.Services.Interfaces.IFileStorageService>().Object,
            mapperMock.Object,
            CreateConfigMock().Object);

        await sut.Add(userView);

        Assert.NotNull(capturedUser);
        Assert.NotEqual(userView.Password, capturedUser!.password);
        Assert.False(string.IsNullOrEmpty(capturedUser.password));
        Assert.True(capturedUser.password.Length > 20);
    }
}
