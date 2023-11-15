using DB.Model.DetailInfo;
using DB.Model.StatusInfo;
using DB.Model.SubdivisionInfo;
using DB.Model.UserInfo.RoleInfo;
using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Test.HelperClass
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;
        public ApplicationDbContext(string connectionString) => _connectionString = connectionString;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString, new MySqlServerVersion(new Version(8, 0, 32)), b => b.MigrationsAssembly("DB"));
        }
    }
}
