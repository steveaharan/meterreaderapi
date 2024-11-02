using Microsoft.EntityFrameworkCore;
using EN.Sek.Meter.Entities;
using EN.Sek.Meter.DAL;
using Microsoft.EntityFrameworkCore.Diagnostics;

[TestClass]
public class AccountDataProviderTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public AccountDataProviderTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
    }

    [TestMethod]
    public async Task GetAccountByIdAsync_AccountExists_ReturnsAccount()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var account = new Account { Id = 100, FirstName = "John", LastName = "Doe" };
        context.Account.Add(account);
        await context.SaveChangesAsync();

        var provider = new AccountDataProvider(context);

        // Act
        var result = await provider.GetAccountByIdAsync(100);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(100, result.Id);
    }

    [TestMethod]
    public async Task GetAccountByIdAsync_AccountDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var provider = new AccountDataProvider(context);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => provider.GetAccountByIdAsync(99));
    }

    [TestMethod]
    public async Task CreateAccountAsync_ValidAccount_ReturnsCreatedAccount()
    {
        // Arrange
        var context = new ApplicationDbContext(_dbContextOptions);
        var provider = new AccountDataProvider(context);
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
        var context = new ApplicationDbContext(_dbContextOptions);
        var provider = new AccountDataProvider(context);
        var account = new Account { Id = 200, FirstName = "John", LastName = "Doe" };
        context.Account.Add(account);
        await context.SaveChangesAsync();

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
        var context = new ApplicationDbContext(_dbContextOptions);
        var provider = new AccountDataProvider(context);
        var account = new Account { Id = 300, FirstName = "John", LastName = "Doe" };
        context.Account.Add(account);
        await context.SaveChangesAsync();

        // Act
        await provider.DeleteAccountAsync(300);

        // Assert
        var deletedAccount = await context.Account.FindAsync(300);
        Assert.IsNull(deletedAccount);
    }
}