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
    public class TpSupervisor
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
                _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/User/Add", new UserAdd("fa", "fas", "fa", "123", "156", 2, 1, 1));
                _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/User/ChangeRoleWithPassword", new ChangeRoleWithPassword(6, 7, "password"));
                //_httpClient.Result.GetAsync("http://localhost:5018/api/logout");
                //_httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/login", new UserAuth("123", "password"));
            }
        }
        [TestCase(1,1,1,"",HttpStatusCode.OK)]
        [TestCase(1,1,1,"klfjalhal",HttpStatusCode.OK)]
        [TestCase(-1,1,1,"klfjalhal",HttpStatusCode.BadRequest)]
        [TestCase(1,-1,1,"gsag",HttpStatusCode.BadRequest)]
        [TestCase(1,1,-1,"asgas",HttpStatusCode.BadRequest)]
        public async Task AddTp(int detailId, int developmentPriority, int developerId, string note, HttpStatusCode result)
        {
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpSupervisor/AddTp",
                                                                      new AddTp(detailId,
                                                                      DateTime.Now.ToString("yyyy-MM-dd"),
                                                                      developmentPriority,
                                                                      developerId,
                                                                      note));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(1,"",TpStatusesForDirector.Completed,HttpStatusCode.OK)]
        [TestCase(1,"",TpStatusesForDirector.NotInWork,HttpStatusCode.OK)]
        [TestCase(1,"",TpStatusesForDirector.ReturnForRework,HttpStatusCode.OK)]
        [TestCase(-1, "", TpStatusesForDirector.Completed, HttpStatusCode.BadRequest)]
        [TestCase(-1, "", TpStatusesForDirector.NotInWork, HttpStatusCode.BadRequest)]
        [TestCase(-1, "", TpStatusesForDirector.ReturnForRework, HttpStatusCode.BadRequest)]
        public async Task ChangeStatus(int techProcessId,string note, TpStatusesForDirector tpStasesForDirector, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpSupervisor/AddTp",
                                                                                                    new AddTp(1,
                                                                                                    DateTime.Now.ToString("yyyy-MM-dd"),
                                                                                                    2, 
                                                                                                    1, 
                                                                                                    ""));
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpSupervisor/ChangeStatus",
                                                                            new ChangeStatus(techProcessId,note,tpStasesForDirector));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(1,2,HttpStatusCode.OK)]
        [TestCase(-1,2,HttpStatusCode.BadRequest)]
        [TestCase(1,-2,HttpStatusCode.BadRequest)]
        public async Task ChangeProcessDeveloper(int techProcessId, int developerId, HttpStatusCode result)
        {
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpSupervisor/ChangeProcessDeveloper",
                                                                            new ChangeProcessDeveloper(techProcessId, developerId));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(1,HttpStatusCode.OK)]
        [TestCase(-1,HttpStatusCode.BadRequest)]
        public async Task DeleteTp(int techProcessId, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpSupervisor/AddTp",
                                                                                                    new AddTp(1,
                                                                                                    DateTime.Now.ToString("yyyy-MM-dd"),
                                                                                                    2,
                                                                                                    1,
                                                                                                    ""));
            using var HttpResult = await _httpClient.Result.DeleteAsync($"http://localhost:5018/api/TpSupervisor/DeleteTp/{techProcessId}");
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
    record class AddTp(int detailId,string finishDate,int developmentPriority,int developerId, string note);
    record class ChangeStatus(int techProcessId, string note, TpStatusesForDirector tpStatusesForDirector);
    record class ChangeProcessDeveloper(int techProcessId, int developerId);
    public enum TpStatusesForDirector{ NotInWork = 2,ReturnForRework = 7,Completed = 11}
}
