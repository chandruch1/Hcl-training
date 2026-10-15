using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Exceptions
{
     class CourseCodeandLearnerIdException :Exception
    {
        public CourseCodeandLearnerIdException(string msg) : base (msg)
        {
            
        }
    }
}
