using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Diagnostics;
using Serilog;
using DB.Model.StatusInfo;

namespace Controllers.Test.UserController.Test
{
    [TestFixture]
    public class UserContorllersGetAll
    {
        [SetUp]
        public void SetUp()
        {
            
        }
        [TestCase("","","","","",0,0,0,500)]
        [TestCase("", "", "", "", "", 0, 0, 0, 500)]
        public async Task UserControllerAdd(string FirstName, 
                                            string LastName, 
                                            string FathersName, 
                                            string ProffesionNumber, 
                                            string Password, 
                                            int ProffesionId,
                                            int SubdivisionId,
                                            int StatusId,
                                            int result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.PostAsync($"http://localhost:5018/api/User/Add", JsonContent.Create(new UserAdd(FirstName, LastName, FathersName, ProffesionNumber, Password, ProffesionId, SubdivisionId, StatusId)));
            Assert.AreEqual(((int)HttpResult.StatusCode), result);
        }
        [TestCase(5, 200)]
        [TestCase(9, 400)]
        [TestCase(-1,400)]
        public async Task UserControllerDelete(int id,int result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.DeleteAsync($"http://localhost:5018/api/User/Delete/{id}");
            Assert.AreEqual(((int)HttpResult.StatusCode), result);
        }

        [TestCase(50,0,0,0,0,5)]
        [TestCase(43,-1,-1,-1,-1,0)]
        [TestCase("", -1, -1, -1, -1, 0)]
        [TestCase(43, "", "", "", "", 5)]
        [TestCase(-743206, "", "", "", "", 0)]
        public async Task UserControllerGetAll(object take, 
                                               object skip, 
                                               object statusId, 
                                               object orderOptions, 
                                               object kindOfOrder,
                                               object result)
        {
            var client = await DbConnection.GetConnectionAsync();
            using var HttpResult = await client.GetAsync($"http://localhost:5018/api/User/GetAll?take={take}&skip={skip}&statusId={statusId}&orderOptions={orderOptions}&kindOfOrder={kindOfOrder}");
            UserGetAll? userGetAll = await HttpResult.Content.ReadFromJsonAsync<UserGetAll>();
            Assert.IsNotNull(userGetAll);   
            Assert.AreEqual(userGetAll.data?.Count ?? 0, result);
        }
        
        public async Task UserControllerGetAllFromProffesion(int proffesionId,int take, int skip, int orderOptions, int kindOfOrder, int result)
        {
            using var client = new HttpClient();
            await client.PostAsync("http://localhost:5018/login", JsonContent.Create(new UserAuth("0001-0001", "admin1")));
            using var HttpResult = await client.GetAsync($"http://localhost:5018/api/User/GetAll?proffesionId={proffesionId}&take={take}&skip={skip}&orderOptions={orderOptions}&kindOfOrder={kindOfOrder}");
            UserGetAll? userGetAll = await HttpResult.Content.ReadFromJsonAsync<UserGetAll>();
            Assert.AreEqual(userGetAll?.data.Count, result);
        }
    }
    record class UserDelete(int id);
    record class UserAdd(string FirstName,
                         string LastName,
                         string FathersName,
                         string ProffesionNumber,
                         string Password,
                         int ProffesionId,
                         int SubdivisionId,
                         int StatusId);
    record class UserGetAll(List<UserData> data, int length);
    record class UserData(string subdivision, string proffesion, int id, string proffesionNumber, string ffl);
}
