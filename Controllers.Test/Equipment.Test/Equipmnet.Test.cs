using Controllers.Test.DetailType.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Test.Equipment.Test
{
    [TestFixture]
    public class Equipmnet
    {
        [SetUp]
        public void SetUp()
        {
           
        }

        //[TestCase("", 400)]
        //[TestCase(1243, 400)]
        //[TestCase("строка", 200)]
        //[TestCase(-1, 400)]
        //[TestCase("тип", 200)]
        public async Task EquipmnetAddTest(object title, object serialNumber, object subdivId, int result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/Equipment/Add", new EquipmentAdd(title, serialNumber,subdivId));
            Assert.AreEqual((int)HttpResult.StatusCode, result);
        }
        //[TestCase(-1, "sadf", 400)]
        //[TestCase(2, -1, 400)]
        //[TestCase(-1, 543261, 400)]
        //[TestCase(4141, "2136126", 400)]
        //[TestCase(1, "стр", 200)]
        public async Task EquipmentTestChange(object id, object title, object serialNumber, object subdivId,int result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsJsonAsync("http://localhost:5018/api/Equipment/Change", new EquipmentChange(id, title,serialNumber,subdivId));
            Assert.AreEqual((int)HttpResult.StatusCode, result);

        }
        //[TestCase(-1, 400)]
        //[TestCase(1, 200)]
        //[TestCase("", 400)]
        //[TestCase(654654, 400)]
        public async Task EquipmentTestDelete(object id, int result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/Equipment/Delete/{id}");
            Assert.AreEqual((int)HttpResult.StatusCode, result);
        }
        public async Task EquipmentGetAll()
        {

        }
    }
    [TestFixture]
    record class EquipmentChange(object id, object title, object serialNumber, object subdivId);
    [TestFixture]
    record class EquipmentAdd(object title, object serialNumber,object subdivId);
}
