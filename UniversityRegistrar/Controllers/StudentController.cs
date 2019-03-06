using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System;
using System.Collections.Generic;

namespace UniversityRegistrar.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet("/students")]
        public ActionResult Index()
        {
            List<Student> allStudents = Student.GetAll();

            return View(allStudents);
        }

        [HttpPost("students/{studentId}/courses/new")]
        public ActionResult AddCourse(int studentId, int courseId)
        {
            Student foundStudent = Student.Find(studentId);
            Course foundCourse = Course.Find(courseId);
            foundStudent.AddCourse(foundCourse.GetId());
            return RedirectToAction("Show", new {id=studentId});
        }

        [HttpGet("/students/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/students/create")]
        public ActionResult Create(string name, DateTime date)
        {
            Student newStudent = new Student(name, date);
            newStudent.Save();
            return RedirectToAction("Index");

        }

        [HttpGet("/students/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Student selectedStudent = Student.Find(id);
            List <Course> allCourses = Course.GetAll();
            List<Course> studentCourses = selectedStudent.GetCourses();
            model.Add("studentCourses", studentCourses);
            model.Add("selectedStudent", selectedStudent);
            model.Add("allCourses", allCourses);
            return View(model);
        }

        [HttpPost("/students/{studentId}/delete")]
        public ActionResult Delete(int studentId)
        {
            Student deletedStudent = Student.Find(studentId);
            deletedStudent.Delete();
            return RedirectToAction("Index");
        }
    }
    
}