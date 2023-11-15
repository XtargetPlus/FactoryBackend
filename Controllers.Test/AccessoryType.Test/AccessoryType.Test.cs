using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Json;
using Serilog;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using DB;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace Controllers.Test.AccessoryType.Test
{
    [TestFixture]
    public class AccessoryType
    {
        [SetUp]
        public void SetUp()
        {
        }
        [TestCase("", HttpStatusCode.BadRequest)]
        [TestCase(1243, HttpStatusCode.OK)]
        [TestCase("строка",HttpStatusCode.OK)]
        [TestCase(-1, HttpStatusCode.OK)]
        [TestCase("тип", HttpStatusCode.OK)]
        public async Task AccessoryTypeAddTest(object AccessoryType, int result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/AccessoryType/Add", new AccessoryTypeAdd(AccessoryType.ToString()));
            Assert.AreEqual((int)HttpResult.StatusCode, result);
        }
        [TestCase(-1,  "sadf",   HttpStatusCode.BadRequest)]
        [TestCase(2,   -1,       HttpStatusCode.OK)]
        [TestCase(-1,  543261,      HttpStatusCode.BadRequest)]
        [TestCase(4141,"2136126",HttpStatusCode.BadRequest)]
        [TestCase(1,   "стр",    HttpStatusCode.OK)]
        public async Task AccessoryTypeTestChange(object id, object name, int result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/AccessoryType/Add", new AccessoryTypeAdd("ahadhadha"));
            await client.PostAsJsonAsync("http://localhost:5018/api/AccessoryType/Add", new AccessoryTypeAdd("dahfadfhah"));
            var HttpResult = await client.PostAsJsonAsync<AccessoryTypeChange>("http://localhost:5018/api/AccessoryType/Change", new AccessoryTypeChange(Convert.ToInt32(id), name.ToString()));
            Assert.That(HttpResult.StatusCode, Is.EqualTo((HttpStatusCode)result));

                
        }
        [TestCase(-1, HttpStatusCode.BadRequest)]
        [TestCase(1, HttpStatusCode.OK)]
        [TestCase("", HttpStatusCode.NotFound)]
        [TestCase(654654,HttpStatusCode.BadRequest)]
        public async Task AccessoryTypeTestDelete(object id, int result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/AccessoryType/Add", new AccessoryTypeAdd("сфыпрфы"));
            await client.PostAsJsonAsync("http://localhost:5018/api/AccessoryType/Add", new AccessoryTypeAdd("фыпфарр"));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/AccessoryType/Delete/" + id.ToString());
            Assert.AreEqual((int)HttpResult.StatusCode, result);
        }
        [TestCase("ha", 6)]
        [TestCase(null, 8)]
        [TestCase("", 8)]
        public async Task AccessoryTypeTestGetAll(string search, int count)
        {
            List<string> list = new List<string>() { "asgh","gagas","adhah","hahashfah","ahfhasfha","afhahfahfah","ashahasdhfah","ashahaha"};
            var client = await DbConnection.GetConnectionAsync();
            foreach (var item in list)
                await client.PostAsJsonAsync("http://localhost:5018/api/AccessoryType/Add", new AccessoryTypeAdd(item));
            var HttpResult = await client.GetAsync("http://localhost:5018/api/AccessoryType/GetAll" + (search != "" ? $"?text={search}" : ""));
            AccessoryTypeGetAll x = new((List<AccessoryTypeGet>)JsonSerializer.Deserialize(await HttpResult.Content.ReadAsStringAsync(), typeof(List<AccessoryTypeGet>)));
            Assert.That(x.list.Count, Is.EqualTo(count));
        }
    }
    [TestFixture]
    record class AccessoryTypeAdd(string title);
    record class AccessoryTypeChange(int id, string title); 
    record class AccessoryTypeGet(int id, string title);
    record class AccessoryTypeGetAll(List<AccessoryTypeGet> list);
}
