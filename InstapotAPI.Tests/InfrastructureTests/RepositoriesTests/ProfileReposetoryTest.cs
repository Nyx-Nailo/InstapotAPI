using InstapotAPI.Infrastructure;
using InstapotAPI.Infrastructure.Repositories;
using InstapotAPI.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace InstapotAPI.Tests.InfrastructureTests.RepositoriesTests
{
    [TestClass]
    public class ProfileReposetoryTest
    {
        private InstapotContext _dbContext;
        private ProfileReposetory _profileReposetory;
        private Profile[] _FakeProfiles;

        [TestInitialize]
        public void Initializer()
        {
            _FakeProfiles = [
                                new Profile { Username = "username", Password = "passoword", Email = "email", ProfilePicture = "Path to profile picture" },
                                new Profile { Username = "username", Password = "passoword", Email = "email"},
                                new Profile { Username = "username", Password = "passoword", Email = "email", ProfilePicture = "Path to profile picture" },
                                new Profile { Username = "NewUsername!!!", Password = "passoword", Email = "email"  },
                                new Profile { Username = "username", Password = "passoword", Email = "email", ProfilePicture = "Path to profile picture" },
                                new Profile { Username = "username", Password = "passoword", Email = "email" },
                                new Profile { Username = "username", Password = "passoword", Email = "email" },
                                new Profile { Username = "username", Password = "passoword", Email = "email" },
                                new Profile { Username = "username", Password = "passoword", Email = "email" },
                                new Profile { Username = "username", Password = "passoword", Email = "email" },
                            ];

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            _dbContext = new InstapotContext(new DbContextOptionsBuilder<InstapotContext>().UseSqlite(connection).Options);
            _dbContext.Database.EnsureCreated();

            _dbContext.Profiles.AddRange(_FakeProfiles);
            _dbContext.SaveChanges();

            _profileReposetory = new ProfileReposetory(_dbContext);
        }

        [TestMethod]
        public async Task Create_Profile_A_New_Profile()
        {
            Profile profile = new Profile { Username="username", Password="passoword", Email="email" };
            Profile expected = profile;

            
            var result = await _profileReposetory.Create(profile);


            Assert.IsInstanceOfType(result, typeof(Profile));
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Username, result.Username);
            Assert.AreEqual(expected.Password, result.Password);
            Assert.AreEqual(expected.Email, result.Email);
        }


        [TestMethod]
        [DataRow(null, 0)]
        [DataRow(null, 70)]
        [DataRow(null, -4)]
        public async Task If_Given_A_Non_Existent_Id_Return_Null(Profile? expected, int nonExistentId)
        {
            var result = await _profileReposetory.Delete(nonExistentId);


            Assert.AreEqual(expected,result);
        }

       
        [TestMethod]
        [DataRow(1, 1)]
        [DataRow(3, 3)]
        [DataRow(5, 5)]
        public async Task If_Given_A_Valid_Id_Return_Profile_With_The_Same_Id(int expected, int existingtId)
        {

            var result = await _profileReposetory.Delete(existingtId);


            Assert.IsInstanceOfType(result, typeof(Profile));
            Assert.AreEqual(expected, result.Id);
        }

        [TestMethod]
        [DataRow("Path to profile picture", 1)]
        [DataRow("Path to profile picture", 3)]
        [DataRow("Path to profile picture", 5)]
        public async Task If_Given_A_Valid_Id_Return_The_Path_To_The_Profile_Image(string expected, int id)
        {
            var result = await _profileReposetory.PathToProfileImage(id);


            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(null, 2)]
        [DataRow(null, 4)]
        [DataRow(null, 6)]
        public async Task If_Given_A_Valid_Id_And_There_Are_No_Path_To_Profile_Image_Return_Null(string? expected, int id)
        {
            var result = await _profileReposetory.PathToProfileImage(id);


            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task Returns_A_Profile_With_A_Updated_Profile_Picture_If_A_Valid_Id_Is_Given()
        {
            var newProfileImagePath = "This is the new profile image path";
            Profile updateProfile = await _dbContext.Profiles.FirstAsync();
            updateProfile.ProfilePicture = newProfileImagePath;


            var result = await _profileReposetory.UpdatePathToProfileImage(updateProfile);


            Assert.IsInstanceOfType(result, typeof(Profile));
            Assert.AreEqual(updateProfile.Id, result.Id);
            Assert.AreEqual(newProfileImagePath, result.ProfilePicture);
        }

        [TestMethod]
        public async Task Returns_Null_If_A_Invalid_Id_Is_Given_When_Trying_To_Update_Profile_Picture()
        {
            Profile newProfileImage = new Profile { ProfilePicture = "Path To BreadImage" };
            Profile? expected = null;


            var result = await _profileReposetory.UpdatePathToProfileImage(newProfileImage);


            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task Returns_A_Profile_With_A_Updated_Username_If_A_Valid_Id_Is_Given()
        {
            var newUsername = "This is a new username";
            Profile updateProfile = await _dbContext.Profiles.FirstAsync();
            updateProfile.Username = newUsername;


            var result = await _profileReposetory.UpdateUsername(updateProfile);


            Assert.IsInstanceOfType(result, typeof(Profile));
            Assert.AreEqual(updateProfile.Id, result.Id);
            Assert.AreEqual(newUsername, result.Username);
        }

        [TestMethod]
        public async Task Returns_Null_If_A_Invalid_Id_Is_Given_When_Trying_To_Update_Username()
        {
            Profile newUsername = new Profile { Username = "NewUsername!!!" };
            Profile? expected = null;


            var result = await _profileReposetory.UpdateUsername(newUsername);


            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public async Task Returns_A_Profile_With_A_Updated_Password_If_A_Valid_Id_Is_Given()
        {
            Profile newPassword = new Profile { Password = "NewPassword!!!" };


            var result = await _profileReposetory.UpdatePassword(newPassword);


            Assert.IsInstanceOfType(result, typeof(Profile));
            Assert.AreEqual(newPassword.Id, result.Id);
            Assert.AreEqual(newPassword.Password, result.Password);
        }

        [TestMethod]
        public async Task Returns_Null_If_A_Invalid_Id_Is_Given_When_Trying_To_Update_Password()
        {
            Profile newPassword = new Profile { Password = "NewPassword!!!" };
            Profile? expected = null;


            var result = await _profileReposetory.UpdatePassword(newPassword);


            Assert.AreEqual(expected, result);
        }

    }
}