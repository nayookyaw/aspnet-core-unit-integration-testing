
using Moq;
using UserApi.Dtos;
using UserApi.Services;
using UserApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace UserApi.UnitTests.Controllers.User;

public class UsersControllerTests
{
    [Fact]
    public async Task GetById_Return_NotFound()
    {
        // Arrange
        var _iUserServiceMock = new Mock<IUserService>();
        _iUserServiceMock.Setup(userSvc => userSvc.GetByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync((UserResponse?)null);
        var _userController = new UserController(_iUserServiceMock.Object);

        // Act
        ActionResult<UserResponse> result = await _userController.GetById(Guid.NewGuid(), default);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetById_Return_User()
    {
        // Arrange
        var expectedUser = new UserResponse(
            Guid.Parse("867ee44e-4a53-44e1-a24a-5e259da1e9e3"), 
            "Alice", 
            "alice@email.com", 
            DateTime.Parse("2025-12-30T10:27:01.7272215")
        );
        var _iUserServiceMock = new Mock<IUserService>();
        _iUserServiceMock.Setup(userSvc => userSvc.GetByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(expectedUser);
        var _userController = new UserController(_iUserServiceMock.Object);

        // Act
        ActionResult<UserResponse> result = await _userController.GetById(expectedUser.Id, default);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<UserResponse>(okResult.Value);
        Assert.Equal(expectedUser.Id, returnValue.Id);
        Assert.Equal(expectedUser.Username, returnValue.Username);
        Assert.Equal(expectedUser.Email, returnValue.Email);
        Assert.Equal(expectedUser.CreatedAt, returnValue.CreatedAt);
    }

    [Fact]
    public async Task GetAllAsync_Return_Empty()
    {
        // Arrange
        var _iUserServiceMock = new Mock<IUserService>();
        _iUserServiceMock.Setup(userSvc => userSvc.GetAllAsync(default))
            .ReturnsAsync(new List<UserResponse>());
        var _userController = new UserController(_iUserServiceMock.Object);
        
        // Act
        ActionResult<IReadOnlyList<UserResponse>> result = await _userController.GetAll(default);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<List<UserResponse>>(okResult.Value);
        Assert.Empty(returnValue);
    }

    [Fact]
    public async Task GetAllAsync_Return_Users()
    {
        // Arrange
        var expectedUsers = new List<UserResponse>
        {
            new UserResponse(Guid.Parse("867ee44e-4a53-44e1-a24a-5e259da1e9e3"), "Alice", "alice@email.com", DateTime.Parse("2025-12-30T10:27:01.7272215")),
            new UserResponse(Guid.Parse("b0d634fd-afa0-4b18-9fc9-60371c08cb34"), "Bob", "bob@email.com", DateTime.Parse("2025-12-30T10:27:44.4450132"))
        };
        var _iUserServiceMock = new Mock<IUserService>();
        _iUserServiceMock.Setup(userSvc => userSvc.GetAllAsync(default))
            .ReturnsAsync(expectedUsers);
        var _userController = new UserController(_iUserServiceMock.Object);

        // Act
        ActionResult<IReadOnlyList<UserResponse>> result = await _userController.GetAll(default);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<List<UserResponse>>(okResult.Value);
        
        // Verify first user
        Assert.Equal(expectedUsers.Count, returnValue.Count);
        Assert.Equal(expectedUsers[0].Id, returnValue[0].Id);
        Assert.Equal(expectedUsers[0].Username, returnValue[0].Username);
        Assert.Equal(expectedUsers[0].Email, returnValue[0].Email);
        Assert.Equal(expectedUsers[0].CreatedAt, returnValue[0].CreatedAt);

        // Verify second user
        Assert.Equal(expectedUsers[1].Id, returnValue[1].Id);
        Assert.Equal(expectedUsers[1].Username, returnValue[1].Username);
        Assert.Equal(expectedUsers[1].Email, returnValue[1].Email);
        Assert.Equal(expectedUsers[1].CreatedAt, returnValue[1].CreatedAt);
    }
}