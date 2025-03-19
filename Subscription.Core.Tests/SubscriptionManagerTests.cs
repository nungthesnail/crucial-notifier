using Moq;
using Subscription.Core.Implementations;
using Subscription.Core.Interfaces;
using Subscription.Model;
using Subscription.Model.Exceptions;

namespace Subscription.Core.Tests;

[NonParallelizable]
public class SubscriptionManagerTests
{
    private readonly List<SubscriptionModel> _mockStorage = [];
    private IUnitOfWork _unitOfWorkMock;

    [SetUp]
    public void Setup()
    {
        var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
        
        subscriptionRepositoryMock
            .Setup(static x => x.AddAsync(It.IsAny<SubscriptionModel>()))
            .Callback((SubscriptionModel x) =>
            {
                _mockStorage.Add(x);
            });
        
        subscriptionRepositoryMock
            .Setup(static x => x.GetByIdAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => Task.FromResult(_mockStorage.FirstOrDefault(x => x.Id == id)));
        
        subscriptionRepositoryMock
            .Setup(static x => x.GetAllAsync())
            .Returns(() => Task.FromResult<IEnumerable<SubscriptionModel>>(_mockStorage));

        subscriptionRepositoryMock
            .Setup(static x => x.UpdateAsync(It.IsAny<SubscriptionModel>()))
            .Returns(Task.CompletedTask);
        
        subscriptionRepositoryMock
            .Setup(static x => x.DeleteAsync(It.IsAny<Guid>()))
            .Callback((Guid id) =>
            {
                _mockStorage.Remove(_mockStorage.FirstOrDefault(x => x.Id == id)!);
            });
        
        subscriptionRepositoryMock
            .Setup(static x => x.GetByEmailAsync(It.IsAny<string>()))
            .Returns((string email) => Task.FromResult(_mockStorage.FirstOrDefault(x => x.Email == email)));
        
        subscriptionRepositoryMock
            .Setup(static x => x.GetByUserIdAsync(It.IsAny<string>()))
            .Returns((string userId) => Task.FromResult(_mockStorage.FirstOrDefault(x => x.UserId == userId)));
        
        _unitOfWorkMock = Mock.Of<IUnitOfWork>(
            x => x.SubscriptionRepository == subscriptionRepositoryMock.Object
            && x.CommitTransactionAsync() == Task.CompletedTask
            && x.BeginTransactionAsync() == Task.CompletedTask
            && x.RollbackTransactionAsync() == Task.CompletedTask);
    }
    
    [Test, Order(1)]
    public void TestConstructor_InputIsIUnitOfWorkMock_ThrowsNothing()
    {
        // Arrange
        
        // Act
        var throwsNothing = () => new SubscriptionManager(_unitOfWorkMock);

        // Assert
        Assert.That(throwsNothing, Throws.Nothing);
    }

    [Test, Order(2)]
    public async Task TestSubscribeAsync_InputIsSubscriptionModel_AddsCorrectSubscriptionToStorage_Async()
    {
        // Arrange
        const string userId = "7d32f57d-373d-4b1c-bbe7-468ef1aa5784";
        const string userEmail = "someone@example.com";
        const bool expectedActive = true;
        var manager = new SubscriptionManager(_unitOfWorkMock);
        _mockStorage.Clear();

        // Act
        await manager.SubscribeAsync(userId, userEmail);
        var subscription = _mockStorage.First(static x => x.UserId == userId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(subscription.Email, Is.EqualTo(userEmail));
            Assert.That(subscription.Active, Is.EqualTo(expectedActive));
        });
    }

    [Test, Order(3)]
    public async Task TestSubscribeAsync_UserWasSubscribed_OverridesOldRecord_Async()
    {
        // Arrange
        const string userId = "d81f5adb-9967-424d-ac9c-0c11128be8ad";
        const string userEmail1 = "someone1@example.com";
        const string userEmail2 = "someone2@example.com";
        const bool expectedActive = true;
        var manager = new SubscriptionManager(_unitOfWorkMock);
        _mockStorage.Clear();
        
        // Act
        await manager.SubscribeAsync(userId, userEmail1);
        await manager.SubscribeAsync(userId, userEmail2);
        var subscription = _mockStorage.First(static x => x.UserId == userId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(subscription.Email, Is.EqualTo(userEmail2));
            Assert.That(subscription.Active, Is.EqualTo(expectedActive));
        });
    }

    [Test, Order(4)]
    public async Task TestSubscribeAsync_EmailWasSubscribed_ThrowsSubscriptionBadDataException_Async()
    {
        // Arrange
        const string userId1 = "d81f5adb-9967-424d-ac9c-0c11128be8ad";
        const string userId2 = "0560dacd-e985-4f39-84e3-3abd1eb87220";
        const string userEmail = "someone@example.com";
        var manager = new SubscriptionManager(_unitOfWorkMock);
        _mockStorage.Clear();
        
        // Act
        await manager.SubscribeAsync(userId1, userEmail);
        AsyncTestDelegate throws = () => manager.SubscribeAsync(userId2, userEmail);
        
        // Assert
        await Assert.ThatAsync(throws, Throws.InstanceOf<SubscriptionBadDataException>());
    }

    [Test, Order(5)]
    public async Task TestChangeSubscriptionEmailAsync_InputIsUserIdAndNewEmail_ChangesRecordEmail_Async()
    {
        // Arrange
        _mockStorage.Clear();
        
        const string userId = "454bf16c-613d-4937-974d-418f0f81dbad";
        const string email1 = "someone1@example.com";
        const string email2 = "someone2@example.com";
        _mockStorage.Add(new SubscriptionModel
        {
            UserId = userId,
            Email = email1
        });
        var manager = new SubscriptionManager(_unitOfWorkMock);

        // Act
        await manager.ChangeSubscriptionEmailAsync(userId, email2);
        var updatedRecord = _mockStorage.First(static x => x.UserId == userId);
        
        // Assert
        Assert.That(updatedRecord.Email, Is.EqualTo(email2));
    }

    [Test, Order(6)]
    public Task TestChangeSubscriptionEmailAsync_EmailExistsInAnotherRecord_ThrowsSubscriptionBadDataException_Async()
    {
        // Arrange
        _mockStorage.Clear();

        const string userId1 = "41f3e36a-d001-4ccf-8b70-3e4cf4c48a4b";
        const string userId2 = "dc2d6653-c644-4999-b5b5-f66edc4794d5";
        const string email1 = "someone1@example.com";
        const string email2 = "someone2@example.com";
        _mockStorage.Add(new SubscriptionModel
        {
            UserId = userId1,
            Email = email1
        });
        _mockStorage.Add(new SubscriptionModel
        {
            UserId = userId2,
            Email = email2
        });
        var manager = new SubscriptionManager(_unitOfWorkMock);
        
        // Act
        AsyncTestDelegate throws = () => manager.ChangeSubscriptionEmailAsync(userId1, email2);

        // Assert
        return Assert.ThatAsync(throws, Throws.InstanceOf<SubscriptionBadDataException>());
    }

    [Test, Order(7)]
    public Task TestChangeSubscriptionEmailAsync_UserDoesntExists_ThrowsSubscriptionBadDataException_Async()
    {
        // Arrange
        var manager = new SubscriptionManager(_unitOfWorkMock);
        _mockStorage.Clear();
        
        // Act
        AsyncTestDelegate throws = () => manager.ChangeSubscriptionEmailAsync(
            "21153f11-23ab-420b-8f92-3fc2f75c33c7", "someone@example.com");
        
        // Assert
        return Assert.ThatAsync(throws, Throws.InstanceOf<SubscriptionBadDataException>());
    }

    [Test, Order(8)]
    public async Task TestEnableSubscriptionAsync_InputIsInactiveUserId_SetsActiveTrue_Async()
    {
        // Arrange
        _mockStorage.Clear();
        const string userId = "ca26f67b-c18f-4b95-8505-2d97c4720ed2";
        _mockStorage.Add(new SubscriptionModel
        {
            UserId = userId,
            Email = "",
            Active = false
        });
        var manager = new SubscriptionManager(_unitOfWorkMock);
        
        // Act
        await manager.EnableSubscriptionAsync(userId);
        var subscription = _mockStorage.First(static x => x.UserId == userId);
        
        // Assert
        Assert.That(subscription.Active, Is.EqualTo(true));
    }

    [Test, Order(9)]
    public async Task TestEnableSubscriptionAsync_InputIsAlreadyActiveUserId_ChangesNothing_Async()
    {
        // Arrange
        _mockStorage.Clear();
        const string userId = "659e2622-4241-4149-91d3-e5c979a0f099";
        _mockStorage.Add(new SubscriptionModel
        {
            UserId = userId,
            Email = "",
            Active = true
        });
        var manager = new SubscriptionManager(_unitOfWorkMock);
        
        // Act
        await manager.EnableSubscriptionAsync(userId);
        var subscription = _mockStorage.First(static x => x.UserId == userId);
        
        // Assert
        Assert.That(subscription.Active, Is.EqualTo(true));
    }

    [Test, Order(10)]
    public Task TestEnableSubscriptionAsync_InputIsUnexistingUserId_ThrowsSubscriptionBadDataException_Async()
    {
        // Arrange
        var manager = new SubscriptionManager(_unitOfWorkMock);
        _mockStorage.Clear();
        
        // Act
        AsyncTestDelegate throws = () => manager.EnableSubscriptionAsync("b2166512-f47b-4321-9f8c-cd5463f90f1d");

        // Assert
        return Assert.ThatAsync(throws, Throws.InstanceOf<SubscriptionBadDataException>());
    }
    
    [Test, Order(11)]
    public async Task TestDisableSubscriptionAsync_InputIsActiveUserId_SetsActiveFalse_Async()
    {
        // Arrange
        _mockStorage.Clear();
        const string userId = "50f8b03a-f0e7-4e57-998f-fe1ab28129e6";
        _mockStorage.Add(new SubscriptionModel
        {
            UserId = userId,
            Email = "",
            Active = false
        });
        var manager = new SubscriptionManager(_unitOfWorkMock);
        
        // Act
        await manager.DisableSubscriptionAsync(userId);
        var subscription = _mockStorage.First(static x => x.UserId == userId);
        
        // Assert
        Assert.That(subscription.Active, Is.EqualTo(false));
    }

    [Test, Order(12)]
    public async Task TestDisableSubscriptionAsync_InputIsAlreadyInactiveUserId_ChangesNothing_Async()
    {
        // Arrange
        _mockStorage.Clear();
        const string userId = "ef12d345-15bf-43d4-af65-650eab246164";
        _mockStorage.Add(new SubscriptionModel
        {
            UserId = userId,
            Email = "",
            Active = false
        });
        var manager = new SubscriptionManager(_unitOfWorkMock);
        
        // Act
        await manager.DisableSubscriptionAsync(userId);
        var subscription = _mockStorage.First(static x => x.UserId == userId);
        
        // Assert
        Assert.That(subscription.Active, Is.EqualTo(false));
    }

    [Test, Order(13)]
    public Task TestDisableSubscriptionAsync_InputIsUnexistingUserId_ThrowsSubscriptionBadDataException_Async()
    {
        // Arrange
        var manager = new SubscriptionManager(_unitOfWorkMock);
        _mockStorage.Clear();
        
        // Act
        AsyncTestDelegate throws = () => manager.DisableSubscriptionAsync("05e28a22-8df6-4b89-8623-993798fef889");

        // Assert
        return Assert.ThatAsync(throws, Throws.InstanceOf<SubscriptionBadDataException>());
    }

    [Test, Order(14)]
    public async Task TestDeleteSubscriptionAsync_InputIsUserId_DeletesSubscription_Async()
    {
        // Arrange
        _mockStorage.Clear();
        const string userId = "58b34679-c36a-4b47-8613-0fa6e704945a";
        _mockStorage.Add(new SubscriptionModel
        {
            UserId = userId,
            Email = "",
            Active = true
        });
        var manager = new SubscriptionManager(_unitOfWorkMock);
        
        // Act
        await manager.DeleteSubscriptionAsync(userId);
        var recordExists = _mockStorage.Any(static x => x.UserId == userId);
        
        // Assert
        Assert.That(recordExists, Is.False);
    }

    [Test, Order(15)]
    public Task TestDeleteSubscriptionAsync_InputIsUnexistingUserId_ThrowsSubscriptionBadDataException_Async()
    {
        // Arrange
        _mockStorage.Clear();
        var manager = new SubscriptionManager(_unitOfWorkMock);
        
        // Act
        AsyncTestDelegate throws = () => manager.DeleteSubscriptionAsync("5eb988fb-e854-4560-9828-61b43748d5ad");

        // Assert
        return Assert.ThatAsync(throws, Throws.InstanceOf<SubscriptionBadDataException>());
    }

    [Test, Order(16)]
    public async Task TestGetActiveSubscriptionsAsync_ReturnsAllActiveSubscriptions_Async()
    {
        // Arrange
        _mockStorage.Clear();
        
        const string activeUserId1 = "ed7d8094-d7f1-429e-99fb-90f9d1909c0f";
        const string activeUserId2 = "0aed4bc4-62ee-4fd1-951f-1c247b1d1019";
        const string inactiveUserId = "ed1f0633-0711-4400-9892-7bf571a659c6";
        _mockStorage.Add(new SubscriptionModel
        {
            UserId = activeUserId1,
            Email = "",
            Active = true
        });
        _mockStorage.Add(new SubscriptionModel
        {
            UserId = inactiveUserId,
            Email = "",
            Active = false
        });
        _mockStorage.Add(new SubscriptionModel
        {
            UserId = activeUserId2,
            Email = "",
            Active = true
        });
        var manager = new SubscriptionManager(_unitOfWorkMock);
        
        // Act
        var active = await manager.GetActiveSubscriptionsAsync();
        var activeIds = active.Select(static x => x.UserId);
        
        // Assert
        Assert.That(activeIds, Is.EquivalentTo([activeUserId1, activeUserId2]));
    }
}
