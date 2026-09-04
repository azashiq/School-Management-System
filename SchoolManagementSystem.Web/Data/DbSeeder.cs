using System;
using System.Linq;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Infrastructure.Data;

namespace SchoolManagementSystem.Web.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.ClassRooms.Any())
            {
                return;
            }

            var classRoomOne = new ClassRoom { Name = "Grade 9", Section = "A", Capacity = 40 };
            var classRoomTwo = new ClassRoom { Name = "Grade 10", Section = "B", Capacity = 35 };

            context.ClassRooms.AddRange(classRoomOne, classRoomTwo);
            context.SaveChanges();

            var teacherOne = new Teacher
            {
                EmployeeId = "EMP-1001",
                FirstName = "Farhana",
                LastName = "Karim",
                Qualification = "M.Sc. in Mathematics",
                Email = "farhana.karim@school.edu",
                Phone = "01711000001"
            };

            var teacherTwo = new Teacher
            {
                EmployeeId = "EMP-1002",
                FirstName = "Rahim",
                LastName = "Uddin",
                Qualification = "M.A. in English Literature",
                Email = "rahim.uddin@school.edu",
                Phone = "01711000002"
            };

            context.Teachers.AddRange(teacherOne, teacherTwo);
            context.SaveChanges();

            var subjectOne = new Subject
            {
                SubjectCode = "MATH-101",
                Name = "Mathematics",
                TeacherId = teacherOne.Id,
                ClassRoomId = classRoomOne.Id
            };

            var subjectTwo = new Subject
            {
                SubjectCode = "ENG-101",
                Name = "English",
                TeacherId = teacherTwo.Id,
                ClassRoomId = classRoomOne.Id
            };

            context.Subjects.AddRange(subjectOne, subjectTwo);
            context.SaveChanges();

            var studentOne = new Student
            {
                RegistrationNumber = "REG-2026-001",
                FirstName = "Ayaan",
                LastName = "Hasan",
                DateOfBirth = new DateTime(2011, 3, 14),
                Gender = Gender.Male,
                Email = "ayaan.hasan@student.school.edu",
                PhoneNumber = "01911000001",
                Address = "Dhanmondi, Dhaka",
                ClassRoomId = classRoomOne.Id
            };

            var studentTwo = new Student
            {
                RegistrationNumber = "REG-2026-002",
                FirstName = "Nabila",
                LastName = "Islam",
                DateOfBirth = new DateTime(2010, 7, 22),
                Gender = Gender.Female,
                Email = "nabila.islam@student.school.edu",
                PhoneNumber = "01911000002",
                Address = "Mirpur, Dhaka",
                ClassRoomId = classRoomTwo.Id
            };

            context.Students.AddRange(studentOne, studentTwo);
            context.SaveChanges();
        }
    }
}
