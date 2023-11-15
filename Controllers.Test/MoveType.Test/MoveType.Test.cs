using Controllers.Test.Material.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Test.MoveType.Test
{
    [TestFixture]
    public class MoveType
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
        public async Task MoveTypeAdd(object title, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/MoveType/Add", new MoveTypeAdd(title.ToString()));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(-1, "sadf", HttpStatusCode.BadRequest)]
        [TestCase(2, -1, HttpStatusCode.OK)]
        [TestCase(-1, 543261, HttpStatusCode.BadRequest)]
        [TestCase(4141, "2136126", HttpStatusCode.BadRequest)]
        [TestCase(1, "стр", HttpStatusCode.OK)]
        public async Task MoveTypeChange(object id, object title, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/MoveType/Add", new MoveTypeAdd("sadgasgah"));
            await client.PostAsJsonAsync("http://localhost:5018/api/MoveType/Add", new MoveTypeAdd("gdafhafha"));
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/MoveType/Change", new MoveTypeChange(Convert.ToInt32(id), title.ToString()));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(-1, HttpStatusCode.BadRequest)]
        [TestCase(1, HttpStatusCode.OK)]
        [TestCase("", HttpStatusCode.NotFound)]
        [TestCase(654654, HttpStatusCode.BadRequest)]
        public async Task MoveTypeDelete(object id, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/MoveType/Add", new MoveTypeAdd("sadgasgah"));
            await client.PostAsJsonAsync("http://localhost:5018/api/MoveType/Add", new MoveTypeAdd("gdafhafha"));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/MoveType/Delete/{id}");
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
    }
    record class MoveTypeAdd(string title);
    record class MoveTypeChange(int id, string title);
}
