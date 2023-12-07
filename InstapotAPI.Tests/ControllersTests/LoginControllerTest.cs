using InstapotAPI.Controllers;
using InstapotAPI.Entity;
using InstapotAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InstapotAPI.Tests.Controllers
{
    [TestClass]
    public class LoginControllerTest
    {

        private readonly Mock<IProfileReposetory> _profileReposetory;

        private readonly Mock<ILogger<LoginController>> _logger;

        private LoginController _loginController;

        public LoginControllerTest()
        {
            _profileReposetory = new Mock<IProfileReposetory>();
            _logger = new Mock<ILogger<LoginController>>();

        }

        [TestMethod]
        public async Task Teasdsasdt()
        {
            var profile = new Profile() { Username = "test", Password = "testpass", Email = "testmail" };

            _profileReposetory.Setup(p => p.Profile(profile.Id)).ReturnsAsync(profile);

            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Get(profile.Id);
            var actionResultType = actionResult.Result;
            var result = (Profile)((OkObjectResult)actionResultType).Value;


            Assert.IsInstanceOfType(actionResultType, typeof(OkObjectResult));
            Assert.AreEqual(profile.Id, result.Id);
            Assert.AreEqual(profile.Username, result.Username);
            Assert.AreEqual(profile.Password, result.Password);
            Assert.AreEqual(profile.Email, result.Email);
        }

        [TestMethod]
        public async Task Teasdsasdasdt()
        {
            var nonExsistantId = It.IsAny<int>();

            _profileReposetory.Setup(p => p.Profile(nonExsistantId)).ReturnsAsync((Profile)null);

            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Get(nonExsistantId);
            var actionResultType = actionResult.Result;


            Assert.IsInstanceOfType(actionResultType, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task If_Create_Is_Given_A_Valid_Profile_Return_Created_At()
        {
            var profile = new Profile() { Username = "test", Password = "testpass", Email = "testmail" };

            _profileReposetory.Setup(p => p.Create(It.IsAny<Profile>())).ReturnsAsync(profile);

            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Create(profile);
            var actionResultType = actionResult.Result;
            var result = (Profile)((CreatedAtActionResult)actionResultType).Value;


            Assert.IsInstanceOfType(actionResultType, typeof(CreatedAtActionResult));
            Assert.AreEqual(profile.Id, result.Id);
            Assert.AreEqual(profile.Username, result.Username);
            Assert.AreEqual(profile.Password, result.Password);
            Assert.AreEqual(profile.Email, result.Email);
        }

        [TestMethod]
        public async Task Tesqat()
        {
            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Login();
            var actionResultType = actionResult;


            Assert.IsInstanceOfType(actionResultType, typeof(RedirectResult));
        }

        [TestMethod]
        public async Task Teasdst()
        {
            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Logout();
            var actionResultType = actionResult;


            Assert.IsInstanceOfType(actionResultType, typeof(RedirectResult));
        }
    }
}
