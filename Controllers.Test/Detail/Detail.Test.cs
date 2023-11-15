using Controllers.Test.DetailType.Test;
using Controllers.Test.Unit.Test;
using DB.Model.ProductInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Test.Detail
{
    [TestFixture]
    public class Detail
    {
        private List<string> UnitList = new List<string>() { "кг","л","г" };
        private List<string> DetailTypeList = new List<string>() { "с пп", "без пп","овк" };
        private Task<HttpClient> _httpClient;
        private List<int> UnitIdList = new List<int>();
        private List<int> DetailTypeIdList = new List<int>();
        [SetUp]
        public void SetUp()
        {
            _httpClient = DbConnection.GetConnectionAsync();
            //_httpClient.Start();
            _httpClient.Wait();
            if (_httpClient.IsCompletedSuccessfully)
            {
                var client = _httpClient.Result;
                //foreach (var item in UnitList)
                //{
                //    var t = client.PostAsJsonAsync("http://localhost:5018/api/Unit/Add", new UnitAdd(item));
                //    t.Start();
                //    t.Wait();
                //}
                //foreach(var item in DetailTypeList)
                //{
                //    var t = client.PostAsJsonAsync("http://localhost:5018/api/DetailType/Add", new DetailTypeAdd(item));
                //    t.Start();
                //    t.Wait();
                //}
                AddItem(UnitList, client, "Unit", item => new UnitAdd(item));
                AddItem(DetailTypeList, client, "DetailType", item => new DetailTypeAdd(item));
            }
        }
        [TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.OK)]
        [TestCase("деталь","1234",1.645F,-1,1,HttpStatusCode.BadRequest)]
        [TestCase("деталь","1234",1.645F,1,-1,HttpStatusCode.BadRequest)]
        [TestCase("деталь","1234",1.645F,-1,-1,HttpStatusCode.BadRequest)]
        [TestCase("деталь","1234",-1.645F,1,1,HttpStatusCode.BadRequest)]
        [TestCase("деталь","",1.645F,1,1,HttpStatusCode.BadRequest)]
        [TestCase("","",1.645F,1,1,HttpStatusCode.BadRequest)]
        [TestCase("","1234",1.645F,1,1,HttpStatusCode.BadRequest)]
        [TestCase("деталь","1234",1.645F,1,346138756,HttpStatusCode.BadRequest)]
        [TestCase("деталь","1234",1.645F,79563879,1,HttpStatusCode.BadRequest)]
        [TestCase("","",-1.0F,-1,-1,HttpStatusCode.BadRequest)]
        [TestCase("деталь","",-1.645F,1,-1,HttpStatusCode.BadRequest)]
        [TestCase("деталь","",-1.645F,5164,1,HttpStatusCode.BadRequest)]
        [TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.OK)]
        [TestCase("деталь","1234",1.645F,1,1156671,HttpStatusCode.BadRequest)]
        [TestCase("деталь","1234",1.645F,16138946,1,HttpStatusCode.BadRequest)]
        //[TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.BadRequest)]
        //[TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.OK)]
        //[TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.OK)]
        //[TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.OK)]
        //[TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.OK)]
        //[TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.OK)]
        //[TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.OK)]
        //[TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.OK)]
        //[TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.OK)]
        //[TestCase("деталь","1234",1.645F,1,1,HttpStatusCode.OK)]
        public async Task DetailAdd(string serialNumber,
                                    string title,
                                    float weight,
                                    int detailTypeId,
                                    int unitId,
                                    HttpStatusCode result)
        {
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd(serialNumber,
                                                                                                                                 title,
                                                                                                                                 weight,
                                                                                                                                 detailTypeId,
                                                                                                                                 unitId));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(1,2,1,HttpStatusCode.OK)]
        [TestCase(2,1,1,HttpStatusCode.OK)]
        [TestCase(1,2,-1, HttpStatusCode.BadRequest)]
        [TestCase(-1,2,1, HttpStatusCode.BadRequest)]
        [TestCase(1,-2,1, HttpStatusCode.BadRequest)]
        [TestCase(-1,-2,-1, HttpStatusCode.BadRequest)]
        [TestCase(-1,-2,1,HttpStatusCode.BadRequest)]
        [TestCase(1,-2,-1,HttpStatusCode.BadRequest)]
        [TestCase(-1,2,-1, HttpStatusCode.BadRequest)]
        public async Task DetailAddChild(int fatherId, int childId, int count, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("gasg", "kdgk", 1, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("we", "hdah", 2, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("hfdaas", "nxn", 3, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("tq", "vnz", 4, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("tqwt", "xnzn", 5, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("sagjs", "qertqr", 6, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("yqery", "hsghgs", 7, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("qtq", "sahja", 8, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("trq", "aga", 9, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("vag", "qrtq", 10, 1, 1));
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/AddChild",
                                                                             new DetailAddChild(fatherId,childId,count));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(1, 2,  HttpStatusCode.OK)]
        [TestCase(-1, 2, HttpStatusCode.BadRequest)]
        [TestCase(1, -2, HttpStatusCode.BadRequest)]
        [TestCase(-1, -2, HttpStatusCode.BadRequest)]
        public async Task DetailAddReplaceability(int firstDetailId, int secondDetailId, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("gasg", "kdgk", 1, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("we", "hdah", 2, 1, 1)); 
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/AddReplaceability", 
                                                                             new DetailAddReplaceability(firstDetailId, secondDetailId));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }

        [TestCase(1,  HttpStatusCode.OK)]
        [TestCase(-1, HttpStatusCode.BadRequest)]
        public async Task DeleteDetailTest(int id, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("gasg", "kdgk", 1, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("we", "hdah", 2, 1, 1));
            
            Console.WriteLine($"http://localhost:5018/api/Detail/DeteleValue/{id}");
            using var HttpResult = await _httpClient.Result.DeleteAsync($"http://localhost:5018/api/Detail/DeleteValue/{id}");
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(1,2,HttpStatusCode.OK)]
        [TestCase(2,1,HttpStatusCode.BadRequest)]
        [TestCase(-1,2,HttpStatusCode.BadRequest)]
        public async Task DeleteChildValue(int FirstDetailId, int SecondDetailId,HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("gasg", "kdgk", 1, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("we", "hdah", 2, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/AddChild", new DetailAddChild(1, 2, 1));
            using var HttpResult = await _httpClient.Result.DeleteAsync($"http://localhost:5018/api/Detail/DeleteChildValue?FirstDetailId={FirstDetailId}&SecondDetailId={SecondDetailId}");
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase(1,HttpStatusCode.OK)]
        [TestCase(2,HttpStatusCode.OK)]
        [TestCase(-1,HttpStatusCode.BadRequest)]
        public async Task DetailDeteleReplaceability(int id, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("gasg", "kdgk", 1, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("we", "hdah", 2, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/AddReplaceability",
                                                                             new DetailAddReplaceability(1, 2));
            using var HttpResult = await _httpClient.Result.DeleteAsync($"http://localhost:5018/api/Detail/DeleteReplaceability/{id}");
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        [TestCase("gasg",1,"kdgasgldkhk", 1, 1, 1, HttpStatusCode.OK)]
        [TestCase("gasg",2, "kdgewqk", 1, 1, 1,HttpStatusCode.OK)]
        [TestCase("gasg",-1, "kdgewqk", 1, 1, 1,HttpStatusCode.BadRequest)]
        [TestCase("gasg",-1, "kdgewqk", -1, -1, -1,HttpStatusCode.BadRequest)]
        public async Task DetailChange(string title, int id, string serialNumber, float weight, int detailTypeId, int unitId, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("gasg", "kdgk", 1, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("we", "hdah", 2, 1, 1));
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Change", 
                                                                             new DetailChange(title, 
                                                                                              id, 
                                                                                              serialNumber, 
                                                                                              weight, 
                                                                                              detailTypeId, 
                                                                                              unitId));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        public async Task DetailChangeChild(int fatherId, int childId,int count, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("gasg", "kdgk", 1, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("we", "hdah", 2, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/AddChild", new DetailAddChild(1, 2, 1));
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/ChangeChild",
                                                                       new DetailChangeChild(fatherId, childId, count));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        public async Task DetailInsertBetween(int fatherId, int childFirstId, int childSecondId, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("gasg", "kdgk", 1, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("we", "hdah", 2, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("we", "hadah", 2, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/AddChild", new DetailAddChild(1, 2, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/AddChild", new DetailAddChild(1, 3, 1));
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/InsertBetween",
                            new DetailInsertBetween(fatherId, childFirstId, childSecondId));
            Assert.That(HttpResult.StatusCode, Is.EqualTo(result));
        }
        public async Task DetailSwapChildrenNumbers(int fatherId, int childFirstId, int childSecondId, HttpStatusCode result)
        {
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("gasg", "kdgk", 1, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("we", "hdah", 2, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/Add", new DetailAdd("we", "hadah", 2, 1, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/AddChild", new DetailAddChild(1, 2, 1));
            await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/AddChild", new DetailAddChild(1, 3, 1));
            using var HttpResult = await _httpClient.Result.PostAsJsonAsync("http://localhost:5018/api/Detail/SwapChildrenNumbers",
                            new DetailSwapChildrenNumbers(fatherId, childFirstId, childSecondId));
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
    public record class DetailAdd(string serialNumber, string title, float weight, int detailTypeId, int unitId);
    record class DetailAddChild(int fatherId, int childId, int count);
    record class DetailAddReplaceability(int firstDetailId, int secondDetailId);
    record class DetailChange(string title,int id, string serialNumber, float weight, int detailTypeId, int unitId);
    record class DetailChangeChild(int fatherId, int childId, int count);
    record class DetailInsertBetween(int fatherId, int childFirstId, int childSecondId);
    record class DetailSwapChildrenNumbers(int fatherId, int childFirstId, int childSecondId);
}
