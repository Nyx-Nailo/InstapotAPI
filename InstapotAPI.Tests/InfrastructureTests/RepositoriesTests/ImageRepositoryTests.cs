
using InstapotAPI.Entity;
using InstapotAPI.Infrastructure;
using InstapotAPI.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace InstapotAPI.Tests.InfrastructureTests.RepositoriesTests;
[TestClass]
public class ImageRepositoryTests
{
    private DbContext _dbContext;
    private ImageRepository _sut;
    private Image[] _testImages;
    [TestInitialize]
    public void Initializer()
    {
        _testImages = [
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image1", Description = "Desc of Image1", CreatedDate = DateTime.Now, Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image2", Description = "Desc of Image2", CreatedDate = DateTime.Now, Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image3", Description = "Desc of Image3", CreatedDate = DateTime.Now, Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image4", Description = "Desc of Image4", CreatedDate = DateTime.Now, Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image5", Description = "Desc of Image5", CreatedDate = DateTime.Now, Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image6", Description = "Desc of Image6", CreatedDate = DateTime.Now, Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image7", Description = "Desc of Image7", CreatedDate = DateTime.Now, Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image8", Description = "Desc of Image8", CreatedDate = DateTime.Now, Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image9", Description = "Desc of Image9", CreatedDate = DateTime.Now, Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() },
        ];
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        _dbContext = new InstapotContext(new DbContextOptionsBuilder<InstapotContext>().UseSqlite(connection).Options);
        _dbContext.Database.EnsureCreated();

        _sut = new ImageRepository(_dbContext);
    }
    #region CreateNewImage
    [TestMethod]
    public void WhenCreatingNewImageReturnTrue()
    {
        var result = _sut.CreateNewImage(1,"this is a path","title","description",DateTime.Now);

        Assert.IsTrue(result);
    }
    [TestMethod]
    public void WhenCreatingNewImageNewImageIsAddedToDB()
    {
        var newImage = new Image { UserID = 2, Path = "Path", Title = "NewImage", Description = "Description", CreatedDate = DateTime.Now, isPublished = true, Comments = new List<Comment>(), LikedBy = new List<int>() };

        _sut.CreateNewImage(newImage.UserID, newImage.Path, newImage.Title, newImage.Description, newImage.CreatedDate);

        Assert.AreEqual(newImage, _dbContext.Find(10));
    }
    #endregion
}