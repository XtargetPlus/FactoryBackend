using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Test.Equipment.Test
{
    [TestFixture]
    public class EquipmnetFailure
    {   
        [SetUp]
        public void SetUp()
        {
            
        }
        [TestCase("", 400)]
        [TestCase(1243, 200)]
        [TestCase("строка", 200)]
        [TestCase(-1, 200)]
        [TestCase("тип", 200)]
        public async Task EquipmnetFailureAddTest(object EquipmnetFailure, int result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/EquipmentFailure/Add", new EquipmentFailureAdd(EquipmnetFailure.ToString()));
            Assert.AreEqual((int)HttpResult.StatusCode, result);
        }
        [TestCase(-1, "sadf", 400)]
        [TestCase(2, -1, 200)]
        [TestCase(-1, 543261, 400)]
        [TestCase(4141, "2136126", 400)]
        [TestCase(1, "стр", 200)]
        public async Task EquipmentFailureTestChange(object id, object name, int result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/EquipmentFailure/Add", new EquipmentFailureAdd("aghahah"));
            await client.PostAsJsonAsync("http://localhost:5018/api/EquipmentFailure/Add", new EquipmentFailureAdd("aghahh"));
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/EquipmentFailure/Change", new EquipmentFailureChange(Convert.ToInt16(id), name.ToString()));
            Assert.AreEqual((int)HttpResult.StatusCode, result);

        }
        [TestCase(-1, 400)]
        [TestCase(1, 200)]
        [TestCase("", 404)]
        [TestCase(654654, 400)]
        public async Task EquipmentFailureTestDelete(object id, int result)
        {
            var client = await DbConnection.GetConnectionAsync();
            await client.PostAsJsonAsync("http://localhost:5018/api/EquipmentFailure/Add", new EquipmentFailureAdd("aghahah"));
            await client.PostAsJsonAsync("http://localhost:5018/api/EquipmentFailure/Add", new EquipmentFailureAdd("aghahh"));
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/EquipmentFailure/Delete/{id}");
            Assert.AreEqual((int)HttpResult.StatusCode, result);
        }

    }
    record class EquipmentFailureAdd(string title);
    record class EquipmentFailureChange(int id, string title);
}
