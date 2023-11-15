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
    public class TpArchive
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
        [TestCase(1,"",tpStatusesForArchive.Issue,HttpStatusCode.OK)]
        [TestCase(1,"gagag",tpStatusesForArchive.Issue,HttpStatusCode.OK)]
        [TestCase(-1,"gagag",tpStatusesForArchive.Issue,HttpStatusCode.BadRequest)]
        public async Task Issue(int techProcessId, string note, tpStatusesForArchive tpStatusesForArchive, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpSupervisor/AddTp",
                                                      new AddTp(1,
                                                                DateTime.Now.ToString("yyyy-MM-dd"),
                                                                2,
                                                                6,
                                                                ""));
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpArchive/Issue", 
                                                                             new Issue(techProcessId,
                                                                                       note,
                                                                                       tpStatusesForArchive));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(1,6,1,HttpStatusCode.OK)]
        [TestCase(-1,1,1,HttpStatusCode.BadRequest)]
        [TestCase(1,-1,1,HttpStatusCode.BadRequest)]
        [TestCase(1,1,-1,HttpStatusCode.BadRequest)]
        public async Task IssueDuplicate(int tpId, int subdivisionId, int userId, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpSupervisor/AddTp",
                                                      new AddTp(1,
                                                      DateTime.Now.ToString("yyyy-MM-dd"),
                                                      2,
                                                      6,
                                                      ""));
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpArchive/IssueDuplicate",
                                                                             new IssueDuplicate(
                                                                                 tpId,
                                                                                 subdivisionId,
                                                                                 userId));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(1,1,HttpStatusCode.OK)]
        [TestCase(-1,1,HttpStatusCode.BadRequest)]
        [TestCase(1,-1,HttpStatusCode.BadRequest)]
        public async Task DeleteIssuedDuplicate(int TechProcessId,int UserId, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpSupervisor/AddTp",
                                                      new AddTp(1,
                                                      DateTime.Now.ToString("yyyy-MM-dd"),
                                                      2,
                                                      7,
                                                      ""));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpArchive/Issue",
                                                      new Issue(1,
                                                      "",
                                                      tpStatusesForArchive.Issue));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/TpArchive/IssueDuplicate",
                                                      new IssueDuplicate(1,1,1));
            using var HttpClient = await _httpClient.Result.DeleteAsync($"http://localhost:5018/api/TpArchive/DeleteIssuedDuplicate?TechProcessId={TechProcessId}&UserId={UserId}");
            Assert.That(HttpClient.StatusCode, Is.EqualTo(result));
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
    record class ChangeRoleWithPassword(int userId, int roleId, string password);
    record class Issue(int techProcessId, string note, tpStatusesForArchive TpStatusesForArchive);
    record class IssueDuplicate(int tpId, int subdivisionId, int userId);
  
    public enum tpStatusesForArchive { Issue = 9}
}
