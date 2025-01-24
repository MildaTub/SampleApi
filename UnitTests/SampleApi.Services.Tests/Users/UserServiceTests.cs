using System.Collections;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using Moq;
using SampleApi.Models.Exceptions;
using SampleApi.Models.Users;
using SampleApi.Services.Users;

namespace SampleApi.Services.Tests.Users;

[TestClass]
public class UserServiceTests
{
    private readonly Fixture _fixture = new();
    private readonly CompareLogic _comparer = new();

    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [TestMethod]
    public async Task Arrange_UserModel_With_UniqueEmail_Act_Create_Assert_UserCreated()
    {
        //Arrange
        User userModel = _fixture.Create<User>();
        User expectedUser = _fixture.Create<User>();

        _userRepositoryMock.Setup(repository => repository.DoesEmailExist(userModel.Email))
            .ReturnsAsync(false);
        _userRepositoryMock.Setup(repository => repository.Create(It.Is<User>(user => user.Id == userModel.Id)))
            .ReturnsAsync(expectedUser);
        //Act
        User actualUser = await _userService.Create(userModel);

        //Assert
        ComparisonResult? result = _comparer.Compare(expectedUser, actualUser);
        Assert.IsTrue(result.AreEqual);
    }


    [TestMethod]
    public async Task Arrange_UserModel_With_ExistingEmail_Act_Create_Assert_DomainExceptionIsThrown()
    {
        //Arrange
        User userModel = _fixture.Create<User>();

        _userRepositoryMock.Setup(repository => repository.DoesEmailExist(userModel.Email))
            .ReturnsAsync(true);

        //Act
        DomainException exception = await Assert.ThrowsExceptionAsync<DomainException>( () => _userService.Create(userModel));

        //Assert
        Assert.AreEqual($"{userModel.Email} already exists.", exception.Message);
    }


    [TestMethod]
    public async Task Arrange_UserId_Act_Get_Assert_UserRetrieved()
    {
        //Arrange
        Guid userId = _fixture.Create<Guid>();
        User expectedUser = _fixture.Create<User>();

        _userRepositoryMock.Setup(repository => repository.FindUserById(userId))
            .ReturnsAsync(expectedUser);
      
        //Act
        User actualUser = await _userService.Get(userId);

        //Assert
        ComparisonResult result = _comparer.Compare(expectedUser, actualUser);
        Assert.IsTrue(result.AreEqual);
    }


    [TestMethod]
    public async Task Arrange_UserId_Act_Get_Assert_MissingEntityExceptionIsThrown()
    {
        //Arrange
        Guid userId = _fixture.Create<Guid>();

        _userRepositoryMock.Setup(repository => repository.FindUserById(userId))
            .ReturnsAsync((User?)null);

        //Act
        MissingEntityException exception = await Assert.ThrowsExceptionAsync<MissingEntityException>(() => _userService.Get(userId));

        //Assert
        Assert.AreEqual("Entity User not found.", exception.Message);
    }

    [TestMethod]
    public async Task Arrange_Act_GetAll_Assert_UsersRetrieved()
    {
        //Arrange
        List<User> expectedUsers = _fixture.CreateMany<User>().ToList();

        _userRepositoryMock.Setup(repository => repository.GetAll())
            .ReturnsAsync(expectedUsers);

        //Act
        List<User> actualUsers = await _userService.GetAll();

        //Assert
        ComparisonResult result = _comparer.Compare(expectedUsers, actualUsers);
        Assert.IsTrue(result.AreEqual);
    }

    [TestMethod]
    public async Task Arrange_UserModel_With_UniqueEmail_Act_Update_Assert_UserUpdated()
    {
        //Arrange
        User updateModel = _fixture.Create<User>();
        User currentUserModel = _fixture.Create<User>();
        User expectedUser = _fixture.Create<User>();


        _userRepositoryMock.Setup(repository => repository.FindUserById(updateModel.Id))
            .ReturnsAsync(currentUserModel);
        _userRepositoryMock.Setup(repository => repository.DoesEmailExist(updateModel.Email))
            .ReturnsAsync(false);
        _userRepositoryMock.Setup(repository => repository.Update(It.Is<User>(user => user.Id == updateModel.Id)))
            .ReturnsAsync(expectedUser);
        //Act
        User actualUser = await _userService.Update(updateModel);

        //Assert
        ComparisonResult? result = _comparer.Compare(expectedUser, actualUser);
        Assert.IsTrue(result.AreEqual);
    }

    [TestMethod]
    public async Task Arrange_UpdateUserModel_With_SameEmail_Act_Update_Assert_UserUpdated()
    {
        //Arrange
        User currentUserModel = _fixture.Create<User>();
        User updateModel = _fixture.Build<User>().With(user => user.Email, currentUserModel.Email).Create();

        User expectedUser = _fixture.Create<User>();


        _userRepositoryMock.Setup(repository => repository.FindUserById(updateModel.Id))
            .ReturnsAsync(currentUserModel);
        _userRepositoryMock.Setup(repository => repository.Update(It.Is<User>(user => user.Id == updateModel.Id)))
            .ReturnsAsync(expectedUser);

        //Act
        User actualUser = await _userService.Update(updateModel);

        //Assert
        ComparisonResult? result = _comparer.Compare(expectedUser, actualUser);
        Assert.IsTrue(result.AreEqual);

        _userRepositoryMock.Verify(repository => repository.DoesEmailExist(updateModel.Email), Times.Never());
    }


    [TestMethod]
    public async Task Arrange_UpdateUserModel_With_ExistingEmail_Act_Update_Assert_DomainExceptionIsThrown()
    {
        //Arrange
        User updateModel = _fixture.Create<User>();
        User userModel = _fixture.Create<User>();

        _userRepositoryMock.Setup(repository => repository.FindUserById(updateModel.Id))
            .ReturnsAsync(userModel);

        _userRepositoryMock.Setup(repository => repository.DoesEmailExist(updateModel.Email))
            .ReturnsAsync(true);

        //Act
        DomainException exception = await Assert.ThrowsExceptionAsync<DomainException>(() => _userService.Update(updateModel));

        //Assert
        Assert.AreEqual($"{updateModel.Email} already exists.", exception.Message);
    }

    [TestMethod]
    public async Task Arrange_UpdateUserModel_Act_Update_Assert_MissingEntityExceptionIsThrown()
    {
        //Arrange
        User updateModel = _fixture.Create<User>();

        _userRepositoryMock.Setup(repository => repository.FindUserById(updateModel.Id))
            .ReturnsAsync((User?)null);

        //Act
        MissingEntityException exception = await Assert.ThrowsExceptionAsync<MissingEntityException>(() => _userService.Update(updateModel));

        //Assert
        Assert.AreEqual("Entity User not found.", exception.Message);
    }

    [TestMethod]
    public async Task Arrange_UserId_Act_Delete_Assert_UserDeleted()
    {
        //Arrange
        Guid userId = _fixture.Create<Guid>();
        User expectedUser = _fixture.Create<User>();

        _userRepositoryMock.Setup(repository => repository.FindUserById(userId))
            .ReturnsAsync(expectedUser);
        _userRepositoryMock.Setup(repository => repository.Delete(userId))
            .Returns(Task.CompletedTask);

        //Act
        await _userService.Delete(userId);

        //Assert
        _userRepositoryMock.Verify(repository => repository.Delete(userId), Times.Once);
    }

    [TestMethod]
    public async Task Arrange_UserId_Act_Delete_Assert_MissingEntityExceptionIsThrown()
    {
        //Arrange
        Guid userId = _fixture.Create<Guid>();

        _userRepositoryMock.Setup(repository => repository.FindUserById(userId))
            .ReturnsAsync((User?)null);

        //Act
        MissingEntityException exception = await Assert.ThrowsExceptionAsync<MissingEntityException>(() => _userService.Delete(userId));

        //Assert
        Assert.AreEqual("Entity User not found.", exception.Message);
    }

}