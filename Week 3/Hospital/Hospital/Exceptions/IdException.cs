using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Exceptions
{
     class IdException : Exception
    {
        public IdException(string msg) : base (msg)
        {

        }
    }
}
