using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using System.ComponentModel.DataAnnotations;

namespace SAPCORE.Models
{
    public class SetupConfigContext : DbContext
    {
        public SetupConfigContext(DbContextOptions<SetupConfigContext> options)
            : base(options)
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "nash.db");
        }
        public SetupConfigContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "nash.db");
        }
        public string DbPath { get; }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
           
            var connectionString = new SqliteConnectionStringBuilder(@$"Data Source=C:\Logs\Nash.db")
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                
            }.ToString();
            builder.UseSqlite(connectionString);
            // Set All Relationships to Cascade
            
        }

        public DbSet<SetupConfig> SetupConfig { get; set; }
    }


    public class SetupConfig
    {
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
