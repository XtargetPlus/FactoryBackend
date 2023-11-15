using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Controllers.Test.Subdivision.Test
{
    [TestFixture]
    internal class Subdivision
    {
        [SetUp]
        public void SetUp()
        {

        }
        [TestCase("",0,HttpStatusCode.BadRequest)]
        [TestCase("gasgsa",0,HttpStatusCode.OK)]
        [TestCase("",2,HttpStatusCode.BadRequest)]
        [TestCase("gsgdsagsa",-1,HttpStatusCode.OK)]
        public async Task SubdivisionAdd(string title, int fatherId,HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/Subdivision/Add", new SubdivisionAdd(title, fatherId));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase("gsahsah",2,HttpStatusCode.OK)]
        [TestCase("gasgha",-1,HttpStatusCode.BadRequest)]
        public async Task SubdivisionChange(string title, int id, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/Subdivision/Add", new SubdivisionAdd("gfagfa", 0));
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/Subdivision/Change", new SubdivisionChange(title, id));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(2,HttpStatusCode.OK)]
        [TestCase(-1,HttpStatusCode.BadRequest)]
        public async Task SubdivisionDelete(int id, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/Subdivision/Add", new SubdivisionAdd("gfagfa", 0));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/Subdivision/Delete/{id}");
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(0,1)]
        [TestCase(1,0)]
        [TestCase(2,0)]
        public async Task SubdivisionGetLevel(int id, int result)
        {
            var client = await DbConnection.GetConnectionAsync();

            using var HttpResult = await client.GetAsync("http://localhost:5018/api/Subdivision/GetLevel" + (id != 0 ? $"?fatherId={id}" : ""));
            List<SubdivisionGet>? list = (List<SubdivisionGet>)JsonSerializer.Deserialize(await HttpResult.Content.ReadAsStringAsync(), typeof(List<SubdivisionGet>))!;
            Assert.That(list.Count, Is.EqualTo(result));
        }
    }
    record class SubdivisionAdd(string title, int fatherId);
    record class SubdivisionChange(string title, int id);
    record class SubdivisionGet(int? fatherId, int countChildren, int id, string title);
}
