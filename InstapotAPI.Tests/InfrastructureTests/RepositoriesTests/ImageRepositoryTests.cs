
using InstapotAPI.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace InstapotAPI.Tests.InfrastructureTests.RepositoriesTests;
[TestClass]
public class ImageRepositoryTests
{
    private DbContext _dbContext;
    private ImageRepository _sut;
    [TestInitialize]
    public void Initializer()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
    }

    [TestMethod]
    public void test()
    {

    }
}
