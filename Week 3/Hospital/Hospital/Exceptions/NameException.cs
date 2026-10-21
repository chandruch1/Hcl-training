using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Exceptions
{
     class NameException : Exception
    {
        public NameException(string msg) : base (msg)
        {

        }
    }
}
