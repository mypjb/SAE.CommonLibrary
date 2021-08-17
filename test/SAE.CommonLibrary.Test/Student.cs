using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Test
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum Sex
    {
        Nav,
        Man
    }

    /// <summary>
    /// 学生
    /// </summary>
    public class Student
    {
        public Student()
        {
            this.CreateTime = DateTime.Now;
            this.Name = Utils.GenerateId();
            this.Age = Utils.GenerateId().GetHashCode() % 100;
            this.Sex = this.Age % 2 == 0 ? Sex.Nav : Sex.Man;
        }
        public DateTime CreateTime { get; set; }
        public string Name
        {
            get; set;
        }
        public int Age
        {
            get; set;
        }
        public Sex Sex
        {
            get; set;
        }
    }
    /// <summary>
    /// 班级
    /// </summary>
    public class ClassGrade
    {
        public DateTime CreateTime { get; set; }
        public string Id { get; set; }
        public ClassGrade()
        {
            this.Id = Utils.GenerateId();
            this.Name = Utils.GenerateId();
            Students = new List<Student>
            {   new Student(),
                new Student(),
                new Student(),
                new Student(),
                new Student(),
                new Student(),
            };
            this.CreateTime = DateTime.Now;
        }
        public string Name
        {
            get; set;
        }
        public IEnumerable<Student> Students { get; set; }

        public IEnumerator GetEnumerator()
        {
            return this.Students?.GetEnumerator();
        }
    }
}
