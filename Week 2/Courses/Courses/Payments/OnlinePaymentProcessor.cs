using Courses.Models;
using Courses.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Payments
{
     class OnlinePaymentProcessor : IPaymentProcessor
    {
        private readonly bool SimulateSuccess;

        public OnlinePaymentProcessor(bool SimulateSuccess = true)
        {
            this.SimulateSuccess = SimulateSuccess;
        }

        public bool ProcessPayment(Payment payment)
        {
            Console.WriteLine("Payment Is Processing............");
            return SimulateSuccess;
        }
    }
}
