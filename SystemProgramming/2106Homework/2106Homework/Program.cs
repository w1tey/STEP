using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ClassroomId { get; set; }
    public Classroom Classroom { get; set; }
}

public class Classroom
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Student> Students { get; set; }
}

public class HomeworkDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = configuration.GetConnectionString("LocalConnection");

        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasOne(e => e.Classroom)
            .WithMany(d => d.Students)
            .HasForeignKey(e => e.ClassroomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class Program
{
    static void Main(string[] args)
    {
        using (var db = new HomeworkDbContext())
        {
            db.Database.EnsureCreated();

            //create classrooms
            var class25 = new Classroom { Name = "Room 25" };
            var class1 = new Classroom { Name = "Room 1" };
            db.Classrooms.AddRange(class25, class1);
            db.SaveChanges();

            //create students + assign classrooms
            var student_emil = new Student { Name = "Emilio", ClassroomId = class25.Id };
            var student_maga = new Student { Name = "Mahammad", ClassroomId = class25.Id };
            var student_radjab = new Student { Name = "Radjabos", ClassroomId = class1.Id };
            var student_mamed = new Student { Name = "Mamed", ClassroomId = class1.Id };
            db.Students.AddRange(student_emil, student_maga, student_radjab, student_mamed);
            db.SaveChanges();

            var studentsWithClassrooms = db.Students
                .Join(
                    db.Classrooms,
                    e => e.ClassroomId,
                    d => d.Id,
                    (e, d) => new { Student = e, Classroom = d }
                )
                .ToList();

            // display all students + classroom names
            foreach (var item in studentsWithClassrooms)
            {
                Console.WriteLine($"ID: {item.Student.Id}\nName: {item.Student.Name}\nClassroom: {item.Classroom.Name}");
            }

            var student_orhan = new Student { Name = "Orhanus", ClassroomId = class1.Id };
            db.Students.Add(student_orhan);
            db.SaveChanges();

            // change student name (update)
            var changeStudentName = db.Students.FirstOrDefault(e => e.Name == "Emilio");

            if (changeStudentName != null)
            {
                changeStudentName.Name = "Murad";
                db.SaveChanges(); }

            // delete a student
            var deleteStudent = db.Students.FirstOrDefault(e => e.Name == "Murad");

            if (deleteStudent != null)
            {
                db.Students.Remove(deleteStudent);
                db.SaveChanges(); }
        }
    }
}
