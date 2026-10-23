using CoursesWebApp.Data;
using CoursesWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CoursesWebApp.Services;

public interface IPaymentProcessor
{
    PaymentMode Mode { get; }
    bool ProcessPayment(Payment payment);
}

public class OnlinePaymentProcessor : IPaymentProcessor
{
    public PaymentMode Mode => PaymentMode.OnlinePayment;
    // Demo only: this project does not connect to a real payment gateway.
    public bool ProcessPayment(Payment payment) => false;
}

public class ClassroomPaymentProcessor : IPaymentProcessor
{
    public PaymentMode Mode => PaymentMode.ClassRoomPayment;
    // Demo only: classroom payments are simulated as successful.
    public bool ProcessPayment(Payment payment) => true;
}

public class PaymentService(ApplicationDbContext db, IEnumerable<IPaymentProcessor> processors)
{
    public async Task<List<Payment>> GetAllAsync() => await db.Payments.AsNoTracking().OrderByDescending(p => p.PaymentId).ToListAsync();

    public async Task<Payment> MakePaymentAsync(int enrollmentId, PaymentMode mode)
    {
        var enrollment = await db.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId)
            ?? throw new KeyNotFoundException("Enrollment not found.");
        if (enrollment.Status == EnrollmentStatus.Cancelled) throw new InvalidOperationException("Cannot pay for a cancelled enrollment.");
        var processor = processors.FirstOrDefault(p => p.Mode == mode) ?? throw new ArgumentException("Unsupported payment mode.");

        var payment = new Payment { EnrollmentId = enrollment.EnrollmentId, Amount = enrollment.FinalFee, Status = PaymentStatus.Pending, Mode = mode, PaymentDate = DateTime.Now };
        payment.Status = processor.ProcessPayment(payment) ? PaymentStatus.Successful : PaymentStatus.Failed;
        if (payment.Status == PaymentStatus.Successful) enrollment.PaymentStatus = PaymentStatus.Successful;
        else if (enrollment.PaymentStatus != PaymentStatus.Successful) enrollment.PaymentStatus = PaymentStatus.Failed;

        db.Payments.Add(payment);
        await db.SaveChangesAsync();
        return payment;
    }
}
