using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System;
using System.Collections.Generic;

namespace UniversityRegistrar.Controllers
{
    public class CourseController : Controller
    {
        [HttpGet("/courses")]
        public ActionResult Index()
        {
            List<Course> allCourses = new List<Course>{};
            return View(allCourses);
        }

        [HttpGet("/courses/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/courses")]
        public ActionResult Create(string name, string number)
        {
            Course newCourse = new Course(name, number);
            newCourse.Save();
            List<Course> allCourses = Course.GetAll();
            return View("Index", allCourses);
        }

        [HttpGet("/courses/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Course selectedCourse = Course.Find(id);
            List <Student> allStudents = Student.GetAll();
            model.Add("selectedCourse", selectedCourse);
            model.Add("allStudents", allStudents);
            return View(model);
        }

        [HttpPost("/courses/{courseId}/delete")]
        public ActionResult Delete(int courseId)
        {
            Course deletedCourse = Course.Find(courseId);
            deletedCourse.Delete();
            return RedirectToAction("Index");
        }

    }
}