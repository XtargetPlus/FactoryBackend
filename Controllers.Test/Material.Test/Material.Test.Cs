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

namespace Controllers.Test.Material.Test
{
    [TestFixture]
    public class Material
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
        public async Task MaterialAddTest(object title, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/Material/Add", new MaterialAdd(title.ToString()));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(-1, "sadf", HttpStatusCode.BadRequest)]
        [TestCase(2, -1, HttpStatusCode.OK)]
        [TestCase(-1, 543261, HttpStatusCode.BadRequest)]
        [TestCase(4141, "2136126", HttpStatusCode.BadRequest)]
        [TestCase(1, "стр", HttpStatusCode.OK)]
        public async Task MaterialChangeTest(object id, object title, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/Material/Add", new MaterialAdd("sadgasgah"));
            await client.PostAsJsonAsync("http://localhost:5018/api/Material/Add", new MaterialAdd("gdafhafha"));
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/Material/Change", new MaterialChange(Convert.ToInt32(id),title.ToString()));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(-1, HttpStatusCode.BadRequest)]
        [TestCase(1, HttpStatusCode.OK)]
        [TestCase("", HttpStatusCode.NotFound)]
        [TestCase(654654, HttpStatusCode.BadRequest)]
        public async Task MaterialDeleteTest(object id, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/Material/Add", new MaterialAdd("sadgasgah"));
            await client.PostAsJsonAsync("http://localhost:5018/api/Material/Add", new MaterialAdd("gdafhafha"));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/Material/Delete/{id}");
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
    }
    record class MaterialAdd(string title);
    record class MaterialChange(int id, string title);
}
