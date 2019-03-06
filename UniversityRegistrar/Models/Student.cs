using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace UniversityRegistrar.Models
{
    public class Student
    {
        private string Name;
        private DateTime DateOfEnrollment;
        private int Id;

        public Student(string name, DateTime dateOfEnrollment, int id=0)
        {
            Name = name;
            DateOfEnrollment = dateOfEnrollment;
            Id = id;
        }

        public string GetName(){ return Name;}
        public DateTime GetDateOfEnrollment(){ return DateOfEnrollment;}
        public int GetId(){ return Id;}

         public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText=@"INSERT INTO students (name, date) VALUES (@name, @date);";
            MySqlParameter prmName = new MySqlParameter();
            prmName.ParameterName = "@name";
            prmName.Value = Name;     //WE WILL NEED TO REVISE THE DATE
            cmd.Parameters.Add(prmName);
            MySqlParameter prmDate = new MySqlParameter();
            prmDate.ParameterName = "@date";
            prmDate.Value = DateOfEnrollment;
            cmd.Parameters.Add(prmDate);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
        }

        public static List<Student> GetAll()
        {
            List<Student> allStudents = new List<Student>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM students;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                DateTime date = rdr.GetDateTime(2);//.ToString();

                Student newStudent = new Student(name, date, id);
                allStudents.Add(newStudent);
            }
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
            return allStudents;
        }

        // public static void ClearAll()
        // {
        //     MySqlConnection conn = DB.Connection();
        //     conn.Open();
        //     MySqlCommand cmd = conn.CreateCommand();
        //     cmd.CommandText = @"DELETE FROM students;";
        //     conn.Close();
        //     if(conn!=null)
        //     {
        //         conn.Dispose();
        //     }
        // }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM students WHERE id=@id; DELETE FROM students_classes WHERE student_id=@id;";
            MySqlParameter prmId = new MySqlParameter();
            prmId.ParameterName = "@id";
            prmId.Value = Id;
            cmd.Parameters.Add(prmId);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
        }

        public static Student Find(int studentId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM students WHERE id=@id;";
            MySqlParameter prmId = new MySqlParameter();
            prmId.ParameterName = "@id";
            prmId.Value = studentId;
            cmd.Parameters.Add(prmId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            string studentName = "";
            DateTime dateOfEnrollment = DateTime.Now;
            while(rdr.Read())
            {
                studentName = rdr.GetString(1);
                dateOfEnrollment = rdr.GetDateTime(2);
            }
            Student foundStudent = new Student(studentName, dateOfEnrollment, studentId);
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
            return foundStudent;
        }

         public List<Course> GetCourses()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT classes.* FROM students
                JOIN students_classes ON (students.id = students_classes.student_id)
                JOIN classes ON (students_classes.class_id = classes.id)
                WHERE students.id = @StudentId;";
            MySqlParameter studentIdParameter = new MySqlParameter();
            studentIdParameter.ParameterName = "@StudentId";
            studentIdParameter.Value = Id;
            cmd.Parameters.Add(studentIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Course> courses = new List<Course>{};
            while(rdr.Read())
            {
            int courseId = rdr.GetInt32(0);
            string courseName = rdr.GetString(1);
            string courseNumber = rdr.GetString(2);
            Course newCourse = new Course(courseName, courseNumber, courseId);
            courses.Add(newCourse);
            }
            conn.Close();
            if (conn != null)
            {
            conn.Dispose();
            }
            return courses;
        }

        public void AddCourse(int courseId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO students_classes (student_id, class_id) VALUES (@student_id, @class_id);";
            MySqlParameter studentId = new MySqlParameter();
            studentId.ParameterName = "@student_id";
            studentId.Value = Id;
            cmd.Parameters.Add(studentId);
            MySqlParameter classId = new MySqlParameter();
            classId.ParameterName = "@class_id";
            classId.Value = courseId;
            cmd.Parameters.Add(classId);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
        }
    }
}