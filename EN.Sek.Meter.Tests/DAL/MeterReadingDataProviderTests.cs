using Microsoft.EntityFrameworkCore;
using EN.Sek.Meter.Entities;
using EN.Sek.Meter.DAL;
using Microsoft.EntityFrameworkCore.Diagnostics;

[TestClass]
public class MeterReadingDataProviderTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public MeterReadingDataProviderTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
    }

}