#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHub.University
{
    public class Student
    {
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public string? StudentStatus { get; set; }
        public string? Name { get; set; }
        public string? Birthday { get; set; }
        public string? Specialization { get; set; }
        public int? Course { get; set; }
        public int? Group { get; set; }
        public string? Faculty { get; set; }
        public string? Email { get; set; }
    }
}
