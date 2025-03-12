using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskApi.Data
{
    public class TaskDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public TaskDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        }
    }
}