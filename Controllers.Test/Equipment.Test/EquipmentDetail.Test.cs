using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Test.Equipment.Test
{
    [TestFixture]
    public class EquipmnetDetail
    {
        [SetUp]
        public void SetUp()
        {
        }
        [TestCase("","" , HttpStatusCode.BadRequest)]
        [TestCase(1243,"", HttpStatusCode.InternalServerError)]
        [TestCase("строка","тип",HttpStatusCode.OK)]
        [TestCase(-1,-1 ,HttpStatusCode.InternalServerError)]
        [TestCase("тип","строка" ,HttpStatusCode.OK)]
        public async Task EquipmnetDetailAddTest(object title,object serialNumber, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsync("http://localhost:5018/api/EquipmentDetail/Add", JsonContent.Create(new EquipmentDetailAdd(title,serialNumber)));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }

        [TestCase(-1, "sadf", "gag" ,HttpStatusCode.BadRequest)]
        [TestCase(2, -1, "gasg",HttpStatusCode.InternalServerError)]
        [TestCase(-1, 543261,"asdg", HttpStatusCode.InternalServerError)]
        [TestCase(4141,"saghas", "2136126", HttpStatusCode.BadRequest)]
        [TestCase(1, "стр", "ch", HttpStatusCode.OK)]
        public async Task EquipmentDetailChangeTest(object id, object title, object serialNumber, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsync("http://localhost:5018/api/EquipmentDetail/Add", JsonContent.Create(new EquipmentDetailAdd("asgah", "asgasg")));
            await client.PostAsync("http://localhost:5018/api/EquipmentDetail/Add", JsonContent.Create(new EquipmentDetailAdd("asah", "asgag")));
            using var HttpResult = await client.PostAsync("http://localhost:5018/api/EquipmentDetail/Change", JsonContent.Create(new EquipmentDetailChange(id, title,serialNumber)));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(-1, HttpStatusCode.BadRequest)]
        [TestCase(1, HttpStatusCode.OK)]
        [TestCase("", HttpStatusCode.NotFound)]
        [TestCase(654654, HttpStatusCode.BadRequest)]
        public async Task EquipmentDetailDeleteTest(object id, HttpStatusCode result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsync("http://localhost:5018/api/EquipmentDetail/Add", JsonContent.Create(new EquipmentDetailAdd("asgah", "asgasg")));
            await client.PostAsync("http://localhost:5018/api/EquipmentDetail/Add", JsonContent.Create(new EquipmentDetailAdd("asah", "asgag")));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/EquipmentDetail/Delete/{id}");
            Assert.That(HttpResult.StatusCode,Is.EqualTo(result));
        }
    }
    
    [TestFixture]
    record class EquipmentDetailChange(object id, object title, object serialNumber);
    [TestFixture]
    record class EquipmentDetailAdd(object title, object serialNumber);
    }
