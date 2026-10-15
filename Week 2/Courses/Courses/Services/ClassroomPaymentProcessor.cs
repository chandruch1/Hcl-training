using System;
using Courses.Models;

namespace Courses.Services
{
    public class ClassroomPaymentProcessor : IPaymentProcessor
    {
        private readonly bool simulateSuccess;

        public ClassroomPaymentProcessor(bool simulateSuccess = true)
        {
            this.simulateSuccess = simulateSuccess;
        }

        bool IPaymentProcessor.ProcessPayment(Payment payment)
        {
            Console.WriteLine("Processing classroom payment...");
            return simulateSuccess;
        }
    }
}