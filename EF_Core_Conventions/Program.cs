using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EF_Core_Conventions
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int StudentAge { get; set; }

        public int GradeId { get; set; }
        public Grade Grade { get; set; }
    }

    public class Grade
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }

        public ICollection<Student> Students { get; set; }
    }

    public partial class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=SchoolDB1;Trusted_Connection=True;");
        }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne<Grade>(s => s.Grade)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GradeId);
        }*/
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SchoolContext())
            {
                var gtd = new Grade()
                {
                    GradeName = "A"
                };

                var gtd1 = new Grade()
                {
                    GradeName = "B"
                };

                var gtd2 = new Grade()
                {
                    GradeName = "C"
                };

                var gtd3 = new Grade()
                {
                    GradeName = "D"
                };

                var std = new Student()
                {
                    StudentName = "Wamik",
                    StudentAge = 23,
                    GradeId = 1
                };

                var std1 = new Student()
                {
                    StudentName = "Ihtisham",
                    StudentAge = 20,
                    GradeId = 2
                };

                var std2 = new Student()
                {
                    StudentName = "afaq",
                    StudentAge = 29,
                    GradeId = 2
                };

                var std3 = new Student()
                {
                    StudentName = "Askari",
                    StudentAge = 25,
                    GradeId = 2
                };

                var std4 = new Student()
                {
                    StudentName = "Umair",
                    StudentAge = 21,
                    GradeId = 3
                };

                /*context.Grades.Add(gtd);
                context.Grades.Add(gtd1);
                context.Grades.Add(gtd2);
                context.Grades.Add(gtd3);

                context.Students.Add(std);
                context.Students.Add(std1);
                context.Students.Add(std2);
                context.Students.Add(std3);
                context.Students.Add(std4);*/

                context.SaveChanges();

                //RAW SQL query
                var students = context.Students.FromSqlRaw("Select * from Students where StudentName = 'wamik'");

                foreach (var s in students)
                {
                    Console.WriteLine(s.StudentName);
                    Console.WriteLine(s.StudentAge);
                    Console.WriteLine(s.GradeId);
                    Console.WriteLine(s.GradeId);
                }

                //Eager Loading
                Console.WriteLine("----------------------------Eager Loading----------------------------");

                var stud1 = context.Grades.Include("Students");

                foreach (var s in stud1)
                {
                    foreach (var a in s.Students)
                    {
                        Console.WriteLine(s.GradeName + " " + a.StudentName);
                    }
                }
            }
        }
        
    }
}
