using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Exceptions
{
     class IdnotFoundException : Exception
    {
        public IdnotFoundException(string msg ) : base (msg)
        {

        }
    }
}
