
using InstapotAPI.Entity;
using InstapotAPI.Infrastructure;
using InstapotAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace InstapotAPI.Tests.InfrastructureTests.RepositoriesTests;
[TestClass]
public class ImageRepositoryTests
{
    private InstapotContext _dbContext;
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
        
        _dbContext.Images.AddRange(_testImages);
        _dbContext.SaveChanges();
        
        _sut = new ImageRepository(_dbContext);
    }
    #region Create New Image
    [TestMethod]
    public async Task When_Creating_New_Image_Return_Created_Image()
    {
        var expected = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020,05,10,8,14,0), Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() };
        
        var result = await _sut.CreateNewImage(expected);

        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Creating_New_Image_It_Is_Added_To_Database()
    {
        var newImage = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() };
        var expected = _dbContext.Images.Count() + 1;

        await _sut.CreateNewImage(newImage);
        var result = _dbContext.Images.Count();

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Creating_New_Image_With_Incomplete_Image_Return_Null()
    {
        var newImage = new Image { };

        var result = await _sut.CreateNewImage(newImage);

        Assert.IsNull(result);
    }
    [TestMethod]
    public async Task When_Creating_New_Image_With_Incomplete_Image_It_Is_Not_Added_To_Database()
    {
        var newImage = new Image { };
        var expected = _dbContext.Images.Count();

        await _sut.CreateNewImage(newImage);
        var result = _dbContext.Images.Count();

        Assert.AreEqual(expected, result);
    }
    #endregion
    #region Get Image By ID
    [TestMethod]
    public async Task When_Getting_Image_With_An_Existing_Id_Return_Image()
    {
        int id = 2;
        var expected = _dbContext.Images.FindAsync(id).Result;

        var result = await _sut.GetImage(id);

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(20)]
    public async Task When_Getting_Image_With_A_Non_Existing_Id_Return_Null(int id)
    {
        var expected = _dbContext.Images.FindAsync(id).Result;

        var result = await _sut.GetImage(id);

        Assert.IsNull(result);
    }
    #endregion
    #region Delete Image
    [TestMethod]
    public async Task When_Successfully_Deleting_Image_Return_Deleted_Image()
    {
        var expected = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() };
        _dbContext.Images.Add(expected);
        _dbContext.SaveChanges();
        var id = _dbContext.Images.Count();

        var result = await _sut.DeleteImage(id);

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Successfully_Deleting_Image_Remove_Image_From_Database()
    {
        var expected = _dbContext.Images.Count();
        var deletedImage = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<Comment>(), isPublished = true, LikedBy = new List<int>() };
        _dbContext.Images.Add(deletedImage);
        _dbContext.SaveChanges();

        await _sut.DeleteImage(expected + 1);
        var result = _dbContext.Images.Count();

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Trying_To_Delete_Non_Existing_Image_Return_Null()
    {
        var id = _dbContext.Images.Count() + 1;

        var result = await _sut.DeleteImage(id);

        Assert.IsNull(result);
    }
    #endregion
    #region Change Title
    [TestMethod]
    public async Task When_Title_Is_Changed_Return_Image_With_New_Title()
    {
        var id = 1;
        var expected = "This is the new title";

        var result = await _sut.ChangeTitel(id, expected);

        Assert.IsInstanceOfType<Image>(result);
        Assert.AreEqual(expected, result.Title);
    }
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public async Task When_Title_Is_Changed_Null_Or_To_Empty_String_Return_Image_With_Old_Title(string newTitle)
    {
        var id = 1;
        var expected = (await _dbContext.Images.FindAsync(id))?.Title;

        var result = await _sut.ChangeTitel(id, newTitle);

        Assert.AreEqual(expected, result?.Title);
    }
    [TestMethod]
    public async Task When_Title_Is_Changed_It_Is_Updated_In_Database()
    {
        var id = 1;
        var expected = "7iqfdhfsauiojr4893";

        await _sut.ChangeTitel(id, expected);
        var result = await _dbContext.Images.FindAsync(id);

        Assert.AreEqual(expected, result?.Title);
    }
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public async Task When_Title_Is_Changed_Null_Or_To_Empty_String_Database_Is_Not_Updated(string newTitle)
    {
        var id = 1;
        var expected = (await _dbContext.Images.FindAsync(id))?.Title;

        await _sut.ChangeTitel(id, newTitle);
        var result = await _dbContext.Images.FindAsync(id);

        Assert.AreEqual(expected, result?.Title);
    }
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(100)]
    public async Task When_Title_Is_Changed_On_An_Id_That_Is_Out_Of_Bounds(int id)
    {
        var result = await _sut.ChangeTitel(id, "NewTitle");

        Assert.IsNull(result);
    }
    #endregion
    #region Change Description

    #endregion
}