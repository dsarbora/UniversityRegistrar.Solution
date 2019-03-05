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
            DateTime DateOfEnrollment = DateTime.Now;
            while(rdr.Read())
            {
                studentName = rdr.GetString(1);
                DateOfEnrollment = rdr.GetDateTime(2);
            }
            Student foundStudent = new Student(studentName, DateOfEnrollment, studentId);
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
            return foundStudent;
        }
    }
}