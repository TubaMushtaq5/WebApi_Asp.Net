using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DL.DbModels
{
    public class StudentDbContext : DbContext
    {
        public DbSet<StudentDbDto> Students { get; set; }
        public DbSet<StudentSubjectDbDto> StudentSubjects { get; set; }
        public DbSet<SubjectDbDto> Subjects { get; set; }

        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
