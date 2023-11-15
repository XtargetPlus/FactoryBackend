using Plan7.Controllers.Auth;
using ServiceLayer.IServicesRepository.IUserServices;
using Shared.Dto.Users;
using DB;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.UserR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Json;

namespace Controllers.Test
{
    [TestFixture]
    public class LoginControllerTests : Controller
    {

        [SetUp]
        public void Setup()
        {
             
        }
        [TestCase("abc", "abc", StatusCodes.Status401Unauthorized)]
        [TestCase("0001-0001", "admin1", StatusCodes.Status200OK)]
        [TestCase("0001-0001", "admin2", StatusCodes.Status401Unauthorized)]
        [TestCase("0001-0002", "admin1", StatusCodes.Status401Unauthorized)]
        public async Task LoginControllerTest(string login, string password, int result)
        {
            using var context = new DbApplicationContext();
            context.Database.EnsureDeleted();
            context.Database.Migrate();
            using var client = new HttpClient();
            using var HttpResult = await client.PostAsync("http://localhost:5018/api/login", JsonContent.Create(new UserAuth(login, password)));
            Assert.AreEqual((int)HttpResult.StatusCode, result);

        }
    }
}