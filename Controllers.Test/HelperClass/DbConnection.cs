using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Test.HelperClass
{
    public class DbConnection
    {
        //private static ApplicationDbContext? _context;
        public async static Task<HttpClient> GetConnectionAsync()
        {
            using var context = new DbApplicationContext();
            context.Database.EnsureDeleted();
            context.Database.Migrate();
            var client = new HttpClient();
            await client.PostAsync("http://localhost:5018/api/login", JsonContent.Create(new UserAuth("0001-0001", "admin1")));
            return client;
        }
    }
}
