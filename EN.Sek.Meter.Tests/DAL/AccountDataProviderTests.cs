using Microsoft.EntityFrameworkCore;
using EN.Sek.Meter.Entities;
using EN.Sek.Meter.DAL;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Microsoft.Extensions.DependencyInjection;

[TestClass]
public class AccountDataProviderTests
{
    private DbContextOptions<ApplicationDbContext>? _dbContextOptions;

    [TestInitialize]
    public void TestInitialize()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name for each test
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
    }

    [TestMethod]
    public async Task AccountExists_ReturnsBool()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var account = new Account { Id = 1, FirstName = "John", LastName = "Doe" };
        context.Account.Add(account);
        await context.SaveChangesAsync();

        var serviceProvider = new ServiceCollection()
            .AddScoped<ApplicationDbContext>(_ => context)
            .BuildServiceProvider();

        var provider = new AccountDataProvider(context, serviceProvider);

        // Act
        var result = await provider.AccountExists(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public async Task CreateAccountAsync_ValidAccount_ReturnsCreatedAccount()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var mockServiceProvider = new Mock<IServiceProvider>();

        var provider = new AccountDataProvider(context, mockServiceProvider.Object);
        var account = new Account { FirstName = "Jane", LastName = "Doe" };

        // Act
        var result = await provider.CreateAccountAsync(account);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Jane", result.FirstName);
        Assert.AreEqual("Doe", result.LastName);

        // Verify the account was added to the database
        var createdAccount = await context.Account.FindAsync(result.Id);
        Assert.IsNotNull(createdAccount);
        Assert.AreEqual("Jane", createdAccount.FirstName);
        Assert.AreEqual("Doe", createdAccount.LastName);
    }

    [TestMethod]
    public async Task UpdateAccountAsync_ValidAccount_ReturnsUpdatedAccount()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);

        var mockServiceProvider = new Mock<IServiceProvider>();
        var provider = new AccountDataProvider(context, mockServiceProvider.Object);
        var account = new Account { Id = 1, FirstName = "John", LastName = "Doe" };
        context.Account.Add(account);
        await context.SaveChangesAsync();

        // Detach the entity to simulate a real-world scenario where the entity is fetched and then updated
        context.Entry(account).State = EntityState.Detached;

        // Update account details
        account.FirstName = "Jane";
        account.LastName = "Smith";

        // Act
        var result = await provider.UpdateAccountAsync(account);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Jane", result.FirstName);
        Assert.AreEqual("Smith", result.LastName);

        // Verify the account was updated in the database
        var updatedAccount = await context.Account.FindAsync(result.Id);
        Assert.IsNotNull(updatedAccount);
        Assert.AreEqual("Jane", updatedAccount.FirstName);
        Assert.AreEqual("Smith", updatedAccount.LastName);
    }

    [TestMethod]
    public async Task DeleteAccountAsync_AccountExists_DeletesAccount()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var mockServiceProvider = new Mock<IServiceProvider>();
        var provider = new AccountDataProvider(context, mockServiceProvider.Object);
        var account = new Account { Id = 1, FirstName = "John", LastName = "Doe" };
        context.Account.Add(account);
        await context.SaveChangesAsync();

        // Detach the entity to simulate a real-world scenario where the entity is fetched and then deleted
        context.Entry(account).State = EntityState.Detached;

        // Act
        await provider.DeleteAccountAsync(1);

        // Assert
        var deletedAccount = await context.Account.FindAsync(1);
        Assert.IsNull(deletedAccount);
    }

    [TestMethod]
    public async Task DeleteAccountAsync_AccountDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var mockServiceProvider = new Mock<IServiceProvider>();
        var provider = new AccountDataProvider(context, mockServiceProvider.Object);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => provider.DeleteAccountAsync(99));
    }
}