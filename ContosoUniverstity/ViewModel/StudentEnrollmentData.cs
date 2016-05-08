using ContosoUniverstity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniverstity.ViewModel
{
    public class StudentEnrollmentData
    {
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}