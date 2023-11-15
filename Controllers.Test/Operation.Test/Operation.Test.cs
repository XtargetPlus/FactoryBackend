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

namespace Controllers.Test.Operation.Test
{
    [TestFixture]
    public class Operation
    {

        [SetUp]
        public void SetUp()
        {
        }
        [TestCase("", "rtqy",HttpStatusCode.BadRequest)]
        [TestCase(1243, "yrq", HttpStatusCode.OK)]
        [TestCase("строка", "gfda", HttpStatusCode.OK)]
        [TestCase(-1, "afhg", HttpStatusCode.OK)]
        [TestCase("тип", "gbxvb",HttpStatusCode.OK)]
        public async Task OperationAddTest(object shortName,object fullName ,HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/Operation/Add", new OperationAdd(shortName.ToString(),fullName.ToString()));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(-1, "sadf","hadfh", HttpStatusCode.BadRequest)]
        [TestCase(2, -1,"ja", HttpStatusCode.OK)]
        [TestCase(-1, 543261, "jgaj", HttpStatusCode.BadRequest)]
        [TestCase(4141, "2136126", "jajajgag", HttpStatusCode.BadRequest)]
        [TestCase(1, "стр", "gagfad",HttpStatusCode.OK)]
        public async Task OperationChangeTest(object id, object shortName, object fullName, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/Operation/Add", new OperationAdd("sadgasgah", "gdafhafha"));
            await client.PostAsJsonAsync("http://localhost:5018/api/Operation/Add", new OperationAdd("gdafhafha", "sadgasgah"));
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/Operation/Change", new OperationChange(Convert.ToInt32(id), shortName.ToString(),fullName.ToString()));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(-1, HttpStatusCode.BadRequest)]
        [TestCase(1, HttpStatusCode.OK)]
        [TestCase("", HttpStatusCode.NotFound)]
        [TestCase(654654, HttpStatusCode.BadRequest)]
        public async Task OperationDeleteTest(object id, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/Operation/Add", new OperationAdd("sadgasgah", "gdafhafha"));
            await client.PostAsJsonAsync("http://localhost:5018/api/Operation/Add", new OperationAdd("gdafhafha", "sadgasgah"));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/Operation/Delete/{id}");
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
    }
    record class OperationAdd(string shortName, string fullName);
    record class OperationChange(int id, string shortName, string fullName);
}
