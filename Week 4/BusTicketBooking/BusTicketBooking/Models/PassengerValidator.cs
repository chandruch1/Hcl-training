
using BusTicketBooking.Models;

namespace BusTicketBooking.Validators;

public static class PassengerValidator
{
    public static List<string> Validate(Passenger passenger)
    {
        List<string> errors = new();

        if (string.IsNullOrWhiteSpace(passenger.FullName))
        {
            errors.Add("Passenger name is required.");
        }

        if (string.IsNullOrWhiteSpace(passenger.Email))
        {
            errors.Add("Email is required.");
        }
        else if (!passenger.Email.Contains('@'))
        {
            errors.Add("Enter a valid email address.");
        }

        if (!string.IsNullOrWhiteSpace(passenger.Phone) &&
            !passenger.Phone.All(char.IsDigit))
        {
            errors.Add("Phone number must contain digits only.");
        }

        return errors;
    }
}
