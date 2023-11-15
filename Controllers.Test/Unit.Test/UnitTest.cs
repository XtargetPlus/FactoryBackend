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

namespace Controllers.Test.Unit.Test
{
    [TestFixture]
    public class Unit
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
        public async Task UnitAddTest(object title, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/Unit/Add", new UnitAdd(title.ToString()));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(-1, "sadf", HttpStatusCode.BadRequest)]
        [TestCase(2, -1, HttpStatusCode.OK)]
        [TestCase(-1, 543261, HttpStatusCode.BadRequest)]
        [TestCase(4141, "2136126", HttpStatusCode.BadRequest)]
        [TestCase(1, "стр", HttpStatusCode.OK)]
        public async Task UnitChangeTest(object id, object title, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/Unit/Add", new UnitAdd("sadgasgah"));
            await client.PostAsJsonAsync("http://localhost:5018/api/Unit/Add", new UnitAdd("gdafhafha"));
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/Unit/Change", new UnitChange(Convert.ToInt32(id), title.ToString()));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(-1, HttpStatusCode.BadRequest)]
        [TestCase(1, HttpStatusCode.OK)]
        [TestCase("", HttpStatusCode.NotFound)]
        [TestCase(654654, HttpStatusCode.BadRequest)]
        public async Task UnitDeleteTest(object id, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/Unit/Add", new UnitAdd("sadgasgah"));
            await client.PostAsJsonAsync("http://localhost:5018/api/Unit/Add", new UnitAdd("gdafhafha"));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/Unit/Delete/{id}");
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
    }
    record class UnitAdd(string title);
    record class UnitChange(int id, string title);
}
