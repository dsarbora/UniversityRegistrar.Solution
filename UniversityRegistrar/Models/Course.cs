using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace UniversityRegistrar.Models
{
    public class Course
    {
        private string CourseName;
        private string CourseNumber;
        private int Id;

        public Course(string courseName, string courseNumber, int id=0)
        {
            CourseName = courseName;
            CourseNumber = courseNumber;
            Id = id;
        }

        public string GetName(){ return CourseName;}
        
        public string GetNumber(){ return CourseNumber;}
        public int GetId(){ return Id;}
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText=@"INSERT INTO classes (name, number) VALUES (@name, @number);";
            MySqlParameter prmName = new MySqlParameter();
            prmName.ParameterName = "@name";
            prmName.Value = CourseName;
            cmd.Parameters.Add(prmName);
            MySqlParameter prmNumber = new MySqlParameter();
            prmNumber.ParameterName = "@number";
            prmNumber.Value = CourseNumber;
            cmd.Parameters.Add(prmNumber);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
        }

        public static List<Course> GetAll()
        {
            List<Course> allCoursees = new List<Course>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM classes;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string courseName = rdr.GetString(1);
                string courseNumber = rdr.GetString(2);
                Course newCourse = new Course(courseName, courseNumber, id);
                allCoursees.Add(newCourse);
            }
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
            return allCoursees;
        }

        // public static void ClearAll()
        // {
        //     MySqlConnection conn = DB.Connection();
        //     conn.Open();
        //     MySqlCommand cmd = conn.CreateCommand();
        //     cmd.CommandText = @"DELETE FROM classes;";
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
            cmd.CommandText = @"DELETE FROM classes WHERE id=@id; DELETE FROM students_classes WHERE classes_id=@id;";
            MySqlParameter prmId = new MySqlParameter();
            prmId.ParameterName = "@id";
            prmId.Value = _id;
            cmd.Parameters.Add(prmId);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
        }
    }   
}