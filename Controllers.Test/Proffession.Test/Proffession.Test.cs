using Controllers.Test.AccessoryType.Test;
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Test.Profession.Test
{
    [TestFixture]
    public class Profession
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
        public async Task ProffessionAddTest(object DetailType, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync<object>("http://localhost:5018/api/Profession/Add", new ProfessionAdd(DetailType.ToString()));
            Assert.AreEqual(HttpResult.StatusCode, result);
        }
        [TestCase(-1, "sadf", HttpStatusCode.BadRequest)]
        [TestCase(2, -1, HttpStatusCode.OK)]
        [TestCase(-1, 543261, HttpStatusCode.BadRequest)]
        [TestCase(4141, "2136126", HttpStatusCode.BadRequest)]
        [TestCase(1, "стр", HttpStatusCode.OK)]
        public async Task DetailTypeTestChange(object id, object name, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/Profession/Add", new ProfessionAdd("sadgasgah"));
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/Profession/Add", new ProfessionAdd("gdafhafha"));
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/Profession/Change", new ProfessionChange(Convert.ToInt32(id), name.ToString()));
            Assert.AreEqual(HttpResult.StatusCode, result);

        }
        [TestCase(-1, HttpStatusCode.BadRequest)]
        [TestCase(1, HttpStatusCode.OK)]
        [TestCase("", HttpStatusCode.NotFound)]
        [TestCase(654654, HttpStatusCode.BadRequest)]
        public async Task DetailTypeTestDelete(object id, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/DetailType/Add", new ProfessionAdd("sadgasgah"));
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/DetailType/Add", new ProfessionAdd("gdafhafha"));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/DetailType/Delete/{id}");
            Assert.AreEqual(HttpResult.StatusCode, result);
        }
        public async Task DetailTypeTestGetAll()
        {

        }
    }
    record class ProfessionAdd(string title);
    record class ProfessionChange(int id, string title);
}
