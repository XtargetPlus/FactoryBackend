using Controllers.Test.AccessoryType.Test;
using Controllers.Test.BlankType.Test;
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Controllers.Test.DetailType.Test
{
    [TestFixture]
    public class DetailType
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
        public async Task DetailTypeAddTest(object DetailType, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync<object>("http://localhost:5018/api/DetailType/Add", new DetailTypeAdd(DetailType.ToString()));
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
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/DetailType/Add", new DetailTypeAdd("sadgasgah"));
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/DetailType/Add", new DetailTypeAdd("gdafhafha"));
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/DetailType/Change", new DetailTypeChange(Convert.ToInt32(id), name.ToString()));
            Assert.AreEqual(HttpResult.StatusCode, result);

        }
        [TestCase(-1, HttpStatusCode.BadRequest)]
        [TestCase(1, HttpStatusCode.OK)]
        [TestCase("", HttpStatusCode.NotFound)]
        [TestCase(654654, HttpStatusCode.BadRequest)]
        public async Task DetailTypeTestDelete(object id, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/DetailType/Add", new DetailTypeAdd("sadgasgah"));
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/DetailType/Add", new DetailTypeAdd("gdafhafha"));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/DetailType/Delete/{id}");
            Assert.AreEqual(HttpResult.StatusCode, result);
        }
        [TestCase("ha", 6)]
        [TestCase(null, 8)]
        [TestCase("", 8)]
        public async Task DetailTypeGetAll(string search, int count)
        {
            List<string> list = new List<string>() { "asgh", "gagas", "adhah", "hahashfah", "ahfhasfha", "afhahfahfah", "ashahasdhfah", "ashahaha" };
            var client = await DbConnection.GetConnectionAsync();
            foreach (var item in list)
                await client.PostAsJsonAsync("http://localhost:5018/api/DetailType/Add", new BlankTypeAdd(item));
            var HttpResult = await client.GetAsync("http://localhost:5018/api/DetailType/GetAll" + (search != "" ? $"?text={search}" : ""));
            DetailTypeGetAll x = new((List<DetailTypeGet>)JsonSerializer.Deserialize(await HttpResult.Content.ReadAsStringAsync(), typeof(List<DetailTypeGet>)));
            Assert.That(x.list.Count, Is.EqualTo(count));
        }
    }
    record class DetailTypeAdd(string title);
    record class DetailTypeChange(int id, string title);
    record class DetailTypeGet(int id, string title);
    record class DetailTypeGetAll(List<DetailTypeGet> list);
}
