using Controllers.Test.DetailType.Test;
using DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Test.UserFro.Test
{
    [TestFixture]
    public class UserForm
    {

        [SetUp]
        public void SetUp()
        {
        }
        [TestCase("", HttpStatusCode.BadRequest)]
        [TestCase(1243, HttpStatusCode.OK)]
        [TestCase("строка", HttpStatusCode.OK)]
        [TestCase(-1, HttpStatusCode.OK)]
        [TestCase("тип", HttpStatusCode.OK)]
        public async Task UserFormAddTest(object title, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/UserForm/Add", new UserFormAdd(title.ToString()));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(-1, "sadf", HttpStatusCode.BadRequest)]
        [TestCase(2, -1, HttpStatusCode.OK)]
        [TestCase(-1, 543261, HttpStatusCode.BadRequest)]
        [TestCase(4141, "2136126", HttpStatusCode.BadRequest)]
        [TestCase(1, "стр", HttpStatusCode.OK)]
        public async Task UserFormChangeTest(object id, object title, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/UserForm/Add", new UserFormAdd("sadgasgah"));
            await client.PostAsJsonAsync("http://localhost:5018/api/UserForm/Add", new UserFormAdd("gdafhafha"));
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/UserForm/Change", new UserFormChange(Convert.ToInt32(id), title.ToString()));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(-1, HttpStatusCode.BadRequest)]
        [TestCase(1, HttpStatusCode.OK)]
        [TestCase("", HttpStatusCode.NotFound)]
        [TestCase(654654, HttpStatusCode.BadRequest)]
        public async Task UserFormDeleteTest(object id, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/UserForm/Add", new UserFormAdd("sadgasgah"));
            await client.PostAsJsonAsync("http://localhost:5018/api/UserForm/Add", new UserFormAdd("gdafhafha"));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/UserForm/Delete/{id}");
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
    }
    record class UserFormAdd(string title);
    record class UserFormChange(int id, string title);
}
