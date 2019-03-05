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