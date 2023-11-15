using Controllers.Test.AccessoryType.Test;
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Controllers.Test.BlankType.Test
{

 
    [TestFixture]
    public class BlankType
    {

        [SetUp]
        public void SetUp()
        {
            //DbConnection.Connection("Server=localhost;User=5p4r3_0l3g;Password=Xtarget.plus_ZeroWolf1;Database=test_plan7;");
        }
        [TestCase("", HttpStatusCode.BadRequest)]
        [TestCase(1243, HttpStatusCode.OK)]
        [TestCase("строка", HttpStatusCode.OK)]
        [TestCase(-1, HttpStatusCode.OK)]
        [TestCase("тип", HttpStatusCode.OK)]
        public async Task BlankTypeTestAdd(object BlankType, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/BlankType/Add", new BlankTypeAdd(BlankType.ToString()));
            Assert.AreEqual(HttpResult.StatusCode, result);
        }
        [TestCase(-1, "sadf", HttpStatusCode.BadRequest)]
        [TestCase(2, -1, HttpStatusCode.OK)]
        [TestCase(-1, 543261, HttpStatusCode.BadRequest)]
        [TestCase(4141, "2136126", HttpStatusCode.BadRequest)]
        [TestCase(1, "стр", HttpStatusCode.OK)]
        public async Task BlankTypeTestChange(object id, object name, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/BlankType/Add", new BlankTypeAdd("sadgasgah"));
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/BlankType/Add", new BlankTypeAdd("gdafhafha"));
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/BlankType/Change", new BlankTypeChange(Convert.ToInt32(id), name.ToString()));
            Assert.AreEqual(HttpResult.StatusCode, result);
        }
        [TestCase(-1, HttpStatusCode.BadRequest)]
        [TestCase(1, HttpStatusCode.OK)]
        [TestCase(2, HttpStatusCode.OK)]
        [TestCase("", HttpStatusCode.NotFound)]
        [TestCase(654654, HttpStatusCode.BadRequest)]
        public async Task BlankTypeTestDelete(object id, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/BlankType/Add", new BlankTypeAdd("sadgasgah"));
            await client.PostAsJsonAsync<object>("http://localhost:5018/api/BlankType/Add", new BlankTypeAdd("sadgasggh"));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/BlankType/Delete/{id}");
            Assert.AreEqual(HttpResult.StatusCode, result);
        }
        [TestCase("ha", 6)]
        [TestCase(null, 8)]
        [TestCase("", 8)]
        public async Task BlankTypeGetAll(string search, int count)
        {
            List<string> list = new List<string>() { "asgh", "gagas", "adhah", "hahashfah", "ahfhasfha", "afhahfahfah", "ashahasdhfah", "ashahaha" };
            var client = await DbConnection.GetConnectionAsync();
            foreach (var item in list)
                await client.PostAsJsonAsync("http://localhost:5018/api/BlankType/Add", new BlankTypeAdd(item));
            var HttpResult = await client.GetAsync("http://localhost:5018/api/BlankType/GetAll" + (search != "" ? $"?text={search}" : ""));
            BlankTypeGetAll x = new((List<BlankTypeGet>)JsonSerializer.Deserialize(await HttpResult.Content.ReadAsStringAsync(), typeof(List<BlankTypeGet>)));
            Assert.That(x.list.Count, Is.EqualTo(count));
        }
    }
    [TestFixture]
    record class BlankTypeAdd(string title);
    record class BlankTypeChange(int id, string title);
    record class BlankTypeGet(int id, string title);
    record class BlankTypeGetAll(List<BlankTypeGet> list);
}
