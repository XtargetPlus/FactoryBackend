using Controllers.Test.Detail;
using Controllers.Test.DetailType.Test;
using Controllers.Test.Unit.Test;
using Controllers.Test.UserController.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Test.TechProcess
{
    [TestFixture]
    public class TpDeveloper
    {
        private List<string> UnitList = new List<string>() { "кг", "л", "г" };
        private List<string> DetailTypeList = new List<string>() { "с пп", "без пп", "овк" };
        private List<DetailAdd> DetailAdds = new List<DetailAdd>()
        {
            new DetailAdd("ga","ga",0,1,1),
            new DetailAdd("ga","gaads",0,1,1),
            new DetailAdd("ga","gaadsf",0,1,1),
            new DetailAdd("gfsaa","gaadsf",0,1,1),
            new DetailAdd("gfsagaga","gaadsf",0,1,1),
        };
        private Task<HttpClient> _httpClient;
        [SetUp]
        public void SetUp()
        {
            _httpClient = DbConnection.GetConnectionAsync();
            _httpClient.Wait();
            if (_httpClient.IsCompletedSuccessfully)
            {
                var client = _httpClient.Result;
                AddItem(UnitList, client, "Unit", item => new UnitAdd(item));
                AddItem(DetailTypeList, client, "DetailType", item => new DetailTypeAdd(item));
                AddItem(DetailAdds, client, "Detail", item => item);
                _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/User/Add", new UserAdd("fa", "fas", "fa", "123", "156", 1, 1, 1));
                //_httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/User/ChangeRoleWithPassword", new ChangeRoleWithPassword(6, 7, "password"));
                //_httpClient.Result.GetAsync("http://localhost:5018/api/logout");
                //_httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/login", new UserAuth("123", "password"));
            }
        }
        public async Task AddTpItem(string operationNumber, int operationId, int tpId, int count, string note, int branch, int tpItemId, HttpStatusCode result)
        {

        }
        [TestCase(1,1,HttpStatusCode.OK)]
        [TestCase(-1,1,HttpStatusCode.BadRequest)]
        [TestCase(1,-1,HttpStatusCode.BadRequest)]
        public async Task AddBranch(int tpId, int tpiId, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpSupervisor/AddTp", new AddTp(1,
                                                                                                    DateTime.Now.ToString("yyyy-MM-dd"),
                                                                                                    2,
                                                                                                    6,
                                                                                                    ""));
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpDeveloper/AddBranch",
                                                                            new AddBranch(tpId,tpiId));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase("005",1,1,1,null,1,HttpStatusCode.OK)]
        [TestCase("005",1,1,1,"gasgdsa",1,HttpStatusCode.OK)]
        [TestCase("",1,1,1,"",1,HttpStatusCode.BadRequest)]
        [TestCase("005",-1,1,1,"",1,HttpStatusCode.BadRequest)]
        [TestCase("005",1,-1,1,"",1,HttpStatusCode.BadRequest)]
        [TestCase("005",1,1,-1,"",1,HttpStatusCode.BadRequest)]
        [TestCase("005",1,1,1,"",-1,HttpStatusCode.BadRequest)]
        public async Task ChangeTpItem(string operationNumber, int operationId, int tpId, int count, string note, int tpiId, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpSupervisor/AddTp", new AddTp(1,
                                                                                                    DateTime.Now.ToString("yyyy-MM-dd"),
                                                                                                    2,
                                                                                                    1,
                                                                                                    ""));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpDeveloper/AddBranch", new AddBranch(1,1));
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpDeveloper/ChangeTpItem",
                                                                              new ChangeTpItem(operationNumber,
                                                                              operationId, tpId, count, note, tpiId));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        private static void AddItem<TIn, TOut>(IEnumerable<TIn> source, HttpClient client, string controller, Func<TIn, TOut> func)
            where TOut : class
            where TIn : class
        {
            foreach (var item in source)
            {
                var requestValue = func(item);
                var t = client.PostAsJsonAsync($"http://localhost:5018/api/{controller}/Add", requestValue);
                t.Wait();
            }
        }
    }
    record class AddTpItem(string operationNumber, int operationId, int tpId, int count, string note, int branch, int tpItemId);
    record class AddBranch(int tpId,int tpiId);
    record class ChangeTpItem(string operationNumber, int operationId, int tpId, int count, string note, int tpiId);
}
