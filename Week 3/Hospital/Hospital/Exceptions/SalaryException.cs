using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Exceptions
{
    public class SalaryException : Exception
    {
        public SalaryException(string msg) : base(msg)
        {

        }
    }
}
