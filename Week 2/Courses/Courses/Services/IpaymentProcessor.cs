using Courses.Models;

namespace Courses.Services
{
    public interface IPaymentProcessor
    {
        bool ProcessPayment(Payment payment);
    }
}