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
        private Profile[] _testProfiles;

        [TestInitialize]
        public void Initializer()
        {
            _testProfiles = [
                                new Profile { Username = "username", Password = "passoword", Email = "email", IsVerified = false, ProfilePicture = "Path to profile picture" },
                                new Profile { Username = "username", Password = "passoword", Email = "email", IsVerified = false },
                                new Profile { Username = "username", Password = "passoword", Email = "email", IsVerified = false, ProfilePicture = "Path to profile picture" },
                                new Profile { Username = "NewUsername!!!", Password = "passoword", Email = "email", IsVerified = false },
                                new Profile { Username = "username", Password = "passoword", Email = "email", IsVerified = false, ProfilePicture = "Path to profile picture" },
                                new Profile { Username = "username", Password = "passoword", Email = "email", IsVerified = false },
                                new Profile { Username = "username", Password = "passoword", Email = "email", IsVerified = false },
                                new Profile { Username = "username", Password = "passoword", Email = "email", IsVerified = false },
                                new Profile { Username = "username", Password = "passoword", Email = "email", IsVerified = false },
                                new Profile { Username = "username", Password = "passoword", Email = "email", IsVerified = false },
                            ];

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            _dbContext = new InstapotContext(new DbContextOptionsBuilder<InstapotContext>().UseSqlite(connection).Options);
            _dbContext.Database.EnsureCreated();

            _dbContext.Profiles.AddRange(_testProfiles);
            _dbContext.SaveChanges();

            _profileReposetory = new ProfileReposetory(_dbContext);
        }

        [TestMethod]
        public async Task If_Create_Is_Called_Return_The_Created_Profile()
        {
            Profile profile = new Profile { Username="Detta är ett anvendar namn ", Password="Detta är ett lösenord", Email="Detta är en email" };
            Profile expected = profile;

            
            var result = await _profileReposetory.Create(profile);


            Assert.IsInstanceOfType(result, typeof(Profile));
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Username, result.Username);
            Assert.AreEqual(expected.Password, result.Password);
            Assert.AreEqual(expected.Email, result.Email);
        }


        [TestMethod]
        [DataRow(1)]
        [DataRow(3)]
        [DataRow(5)]
        public async Task If_Profile_Is_Given_A_Valid_Id_Return_A_Profile(int id)
        {
            var gotenProfile = await _dbContext.Profiles.FindAsync(id);


            var result = await _profileReposetory.Profile(id);


            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-3)]
        [DataRow(500)]
        public async Task If_Profile_Is_Given_A_Invalid_Id_Return_Null(int NonexistentId)
        {
            var result = await _profileReposetory.Profile(NonexistentId);


            Assert.IsNull(result);
        }


        [TestMethod]
        [DataRow(1)]
        [DataRow(3)]
        [DataRow(5)]
        public async Task If_Delete_Is_Given_A_Valid_Id_Return_Profile_With_The_Same_Id(int id)
        {

            var result = await _profileReposetory.Delete(id);


            Assert.IsInstanceOfType(result, typeof(Profile));
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(70)]
        [DataRow(-4)]
        public async Task If_Delete_Is_Given_A_Non_Existent_Id_Return_Null(int nonexistentId)
        {
            var result = await _profileReposetory.Delete(nonexistentId);


            Assert.IsNull(result);
        }
        

        [TestMethod]
        [DataRow("Path to profile picture", 1)]
        [DataRow("Path to profile picture", 3)]
        [DataRow("Path to profile picture", 5)]
        public async Task If_PathToProfileImage_Is_Given_A_Valid_Id_Return_The_Expected_String(string expected, int id)
        {
            var result = await _profileReposetory.PathToProfileImage(id);


            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-4)]
        [DataRow(6)]
        public async Task If_PathToProfileImage_Is_Null_Or_Is_Given_A_Invalid_Id_Return_Null(int id)
        {
            var result = await _profileReposetory.PathToProfileImage(id);


            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow("This is the new profile image path", 2)]
        [DataRow("This is the new profile image path", 4)]
        [DataRow("This is the new profile image path", 6)]
        public async Task If_UpdatePathToProfileImage_Is_Given_A_Profile_With_A_Valid_Id_Update_The_Path_To_The_Profile_Image(string newProfileImagePath,int id)
        {
            Profile updateProfile = await _dbContext.Profiles.FindAsync(id);
            updateProfile.ProfilePicture = newProfileImagePath;


            var result = await _profileReposetory.UpdatePathToProfileImage(updateProfile);


            Assert.IsInstanceOfType(result, typeof(Profile));
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(newProfileImagePath, result.ProfilePicture);
        }

        [DataRow(null, 0)]
        [DataRow(null, -4)]
        [DataRow("This is the new profile image path", 400)]
        [TestMethod]
        public async Task If_UpdatePathToProfileImage_Is_Given_A_Profile_With_A_Invalid_Id_Return_Null(string? pathToProfileImage, int nonexistentId)
        {
            Profile newProfileImage = new Profile { Id = nonexistentId, ProfilePicture = pathToProfileImage };


            var result = await _profileReposetory.UpdatePathToProfileImage(newProfileImage);


            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow("This is a new username", 2)]
        [DataRow("This is a new username", 4)]
        [DataRow("This is a new username", 6)]
        public async Task If_UpdateUsername_Is_Given_A_Profile_With_A_Valid_Id_Update_The_Profiles_Username(string newUsername,int id)
        {
            Profile updateProfile = await _dbContext.Profiles.FindAsync(id);
            updateProfile.Username = newUsername;


            var result = await _profileReposetory.UpdateUsername(updateProfile);


            Assert.IsInstanceOfType(result, typeof(Profile));
            Assert.AreEqual(updateProfile.Id, result.Id);
            Assert.AreEqual(newUsername, result.Username);
        }

        [TestMethod]
        public async Task If_UpdateUsername_Is_Given_A_Profile_With_A_Invalid_Id_Return_Null()
        {
            Profile newUsername = new Profile { Username = "NewUsername!!!" };


            var result = await _profileReposetory.UpdateUsername(newUsername);


            Assert.IsNull(result);
        }


        [TestMethod]
        [DataRow("This is a new password", 2)]
        [DataRow("This is a new password", 4)]
        [DataRow("This is a new password", 6)]
        public async Task If_UpdatePassword_Is_Given_A_Profile_With_A_Valid_Id_Update_The_Profiles_Password(string newPassword,int id)
        {
            Profile updateProfile = await _dbContext.Profiles.FindAsync(id);
            updateProfile.Password = newPassword;


            var result = await _profileReposetory.UpdatePassword(updateProfile);


            Assert.IsInstanceOfType(result, typeof(Profile));
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(newPassword, result.Password);
        }

        [TestMethod]
        public async Task If_UpdatePassword_Is_Given_A_Profile_With_A_Invalid_Id_Return_Null()
        {
            Profile newPassword = new Profile { Password = "NewPassword!!!" };


            var result = await _profileReposetory.UpdatePassword(newPassword);


            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow("This is a new email", 2)]
        [DataRow("This is a new email", 4)]
        [DataRow("This is a new email", 6)]
        public async Task If_UpdateEmail_Is_Given_A_Profile_With_A_Valid_Id_Update_The_Profiles_Email(string newEmail, int id)
        {
            Profile updateProfile = await _dbContext.Profiles.FindAsync(id);
            updateProfile.Email = newEmail;


            var result = await _profileReposetory.UpdateEmail(updateProfile);


            Assert.IsInstanceOfType(result, typeof(Profile));
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(newEmail, result.Email);
        }

        [TestMethod]
        public async Task If_UpdateEmail_Is_Given_A_Profile_With_A_Invalid_Id_Return_Null()
        {
            Profile newEmail = new Profile { Password = "NewEmail!!!" };


            var result = await _profileReposetory.UpdateEmail(newEmail);


            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(4)]
        [DataRow(6)]
        public async Task If_IsVerified_Is_Given_A_Valid_Id_Return_A_Bool_Of_The_Profiles_IsVerified_Value(int id)
        {
            var result = await _profileReposetory.IsVerified(id);


            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(bool));
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-4)]
        [DataRow(100)]
        public async Task If_IsVerified_Is_Given_A_Invalid_Id_Return_Null(int id)
        {
            var result = await _profileReposetory.IsVerified(id);


            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow(true,2)]
        [DataRow(true,4)]
        [DataRow(true,6)]
        public async Task If_Verified_Is_Given_A_Profile_With_A_Valid_Id_Return_The_Profile_With_The_IsVerified_Value_Set_To_True(bool expected, int id)
        {
            Profile updateProfile = await _dbContext.Profiles.FindAsync(id);


            var result = await _profileReposetory.Verified(updateProfile);


            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(expected, result.IsVerified);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-4)]
        [DataRow(100)]
        public async Task If_Verified_Is_Given_A_Invalid_Id_Return_Null(int id)
        {
            Profile updateProfile = new Profile { Id = id };


            var result = await _profileReposetory.Verified(updateProfile);


            Assert.IsNull(result);
        }

    }
}