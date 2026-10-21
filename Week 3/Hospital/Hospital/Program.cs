
using System;
using System.Linq;
using System.Globalization;
using Hospital.Models;
using Hospital.Services;
using Hospital.Utility.Json;

namespace Hospital
{
    class Program
    {
        static void Main(string[] args)
        {
            PatientJsonService patientJson = new PatientJsonService();
            PatientServices patientService =
                new PatientServices(patientJson);

            DoctorJsonService doctorJson = new DoctorJsonService();
            DoctorService doctorService =
                new DoctorService(doctorJson);

            AppointmentJsonService appointmentJson =
                new AppointmentJsonService();

            AppointmentService appointmentService =
                new AppointmentService(
                    appointmentJson,
                    patientService,
                    doctorService);

            while (true)
            {
                Console.WriteLine("\n===== HOSPITAL MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. Patient Management");
                Console.WriteLine("2. Doctor Management");
                Console.WriteLine("3. Appointment Management");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            PatientMenu(patientService);
                            break;

                        case "2":
                            DoctorMenu(doctorService);
                            break;

                        case "3":
                            AppointmentMenu(appointmentService);
                            break;

                        case "0":
                            Console.WriteLine("Exiting application...");
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        // PATIENT MENU
        static void PatientMenu(PatientServices service)
        {
            while (true)
            {
                Console.WriteLine("\n----- PATIENT MANAGEMENT -----");
                Console.WriteLine("1. Add Patient");
                Console.WriteLine("2. View All Patients");
                Console.WriteLine("3. Get Patient By ID");
                Console.WriteLine("4. Search Patient By Name");
                Console.WriteLine("5. Update Patient");
                Console.WriteLine("6. Delete Patient");
                Console.WriteLine("0. Back");
                Console.Write("Enter choice: ");

                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            {
                                Patient p = new Patient();

                                Console.Write("Patient ID: ");
                                p.PatientId = int.Parse(Console.ReadLine()!);

                                Console.Write("Full Name: ");
                                p.FullName = Console.ReadLine() ?? "";

                                Console.Write("Age: ");
                                p.Age = int.Parse(Console.ReadLine()!);

                                Console.Write("Phone Number: ");
                                p.PhoneNumber = Console.ReadLine() ?? "";

                                Console.Write("Email: ");
                                p.Email = Console.ReadLine();

                                Console.WriteLine(service.AddPatient(p));
                                break;
                            }

                        case "2":
                            {
                                var patients = service.GetAllPatients();

                                if (!patients.Any())
                                {
                                    Console.WriteLine("No patients found.");
                                    break;
                                }

                                foreach (var p in patients)
                                {
                                    Console.WriteLine(
                                        $"ID: {p.PatientId}, Name: {p.FullName}, " +
                                        $"Age: {p.Age}, Phone: {p.PhoneNumber}, " +
                                        $"Email: {p.Email}");
                                }
                                break;
                            }

                        case "3":
                            {
                                Console.Write("Patient ID: ");
                                int id = int.Parse(Console.ReadLine()!);

                                var p = service.GetPatientById(id);

                                Console.WriteLine(
                                    $"ID: {p.PatientId}, Name: {p.FullName}, " +
                                    $"Age: {p.Age}, Phone: {p.PhoneNumber}, " +
                                    $"Email: {p.Email}");
                                break;
                            }

                        case "4":
                            {
                                Console.Write("Enter patient name: ");
                                string name = Console.ReadLine() ?? "";

                                var patients = service.SearchPatientsByName(name);

                                foreach (var p in patients)
                                {
                                    Console.WriteLine(
                                        $"{p.PatientId} - {p.FullName} - {p.PhoneNumber}");
                                }
                                break;
                            }

                        case "5":
                            {
                                Console.Write("Patient ID to update: ");
                                int id = int.Parse(Console.ReadLine()!);

                                Patient updated = new Patient();

                                Console.Write("New Full Name: ");
                                updated.FullName = Console.ReadLine() ?? "";

                                Console.Write("New Age: ");
                                updated.Age = int.Parse(Console.ReadLine()!);

                                Console.Write("New Phone Number: ");
                                updated.PhoneNumber = Console.ReadLine() ?? "";

                                Console.Write("New Email: ");
                                updated.Email = Console.ReadLine();

                                Console.WriteLine(service.UpdatePatient(id, updated));
                                break;
                            }

                        case "6":
                            {
                                Console.Write("Patient ID to delete: ");
                                int id = int.Parse(Console.ReadLine()!);

                                Console.WriteLine(service.DeletePatient(id));
                                break;
                            }

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        // DOCTOR MENU
        static void DoctorMenu(DoctorService service)
        {
            while (true)
            {
                Console.WriteLine("\n----- DOCTOR MANAGEMENT -----");
                Console.WriteLine("1. Add Doctor");
                Console.WriteLine("2. View All Doctors");
                Console.WriteLine("3. Get Doctor By ID");
                Console.WriteLine("4. Search Doctor By Name");
                Console.WriteLine("5. Search Doctor By Specialization");
                Console.WriteLine("6. Update Doctor");
                Console.WriteLine("7. Delete Doctor");
                Console.WriteLine("0. Back");
                Console.Write("Enter choice: ");

                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            {
                                Doctor d = new Doctor();

                                Console.Write("Doctor ID: ");
                                d.DoctorId = int.Parse(Console.ReadLine()!);

                                Console.Write("Full Name: ");
                                d.FullName = Console.ReadLine() ?? "";

                                Console.Write("Specialization: ");
                                d.Specalization = Console.ReadLine() ?? "";

                                Console.Write("Consultation Fee: ");
                                d.CounsultFee = decimal.Parse(Console.ReadLine()!);

                                Console.WriteLine(service.AddDoctors(d));
                                break;
                            }

                        case "2":
                            {
                                var doctors = service.GetAllDoctors();

                                if (!doctors.Any())
                                {
                                    Console.WriteLine("No doctors found.");
                                    break;
                                }

                                foreach (var d in doctors)
                                {
                                    Console.WriteLine(
                                        $"ID: {d.DoctorId}, Name: {d.FullName}, " +
                                        $"Specialization: {d.Specalization}, " +
                                        $"Fee: {d.CounsultFee}");
                                }
                                break;
                            }

                        case "3":
                            {
                                Console.Write("Doctor ID: ");
                                int id = int.Parse(Console.ReadLine()!);

                                var d = service.GetDoctorById(id);

                                Console.WriteLine(
                                    $"ID: {d.DoctorId}, Name: {d.FullName}, " +
                                    $"Specialization: {d.Specalization}, " +
                                    $"Fee: {d.CounsultFee}");
                                break;
                            }

                        case "4":
                            {
                                Console.Write("Enter doctor name: ");
                                string name = Console.ReadLine() ?? "";

                                var doctors = service.SearchDoctorsByName(name);

                                foreach (var d in doctors)
                                {
                                    Console.WriteLine(
                                        $"{d.DoctorId} - {d.FullName} - {d.Specalization}");
                                }
                                break;
                            }

                        case "5":
                            {
                                Console.Write("Enter specialization: ");
                                string specialization = Console.ReadLine() ?? "";

                                var doctors =
                                    service.SearchDoctorsBySpecialization(specialization);

                                foreach (var d in doctors)
                                {
                                    Console.WriteLine(
                                        $"{d.DoctorId} - {d.FullName} - " +
                                        $"{d.Specalization} - Fee: {d.CounsultFee}");
                                }
                                break;
                            }

                        case "6":
                            {
                                Console.Write("Doctor ID to update: ");
                                int id = int.Parse(Console.ReadLine()!);

                                Doctor updated = new Doctor();

                                Console.Write("New Full Name: ");
                                updated.FullName = Console.ReadLine() ?? "";

                                Console.Write("New Specialization: ");
                                updated.Specalization = Console.ReadLine() ?? "";

                                Console.Write("New Consultation Fee: ");
                                updated.CounsultFee = decimal.Parse(Console.ReadLine()!);

                                Console.WriteLine(service.UpdateDoctor(id, updated));
                                break;
                            }

                        case "7":
                            {
                                Console.Write("Doctor ID to delete: ");
                                int id = int.Parse(Console.ReadLine()!);

                                Console.WriteLine(service.DeleteDoctor(id));
                                break;
                            }

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        // APPOINTMENT MENU
        static void AppointmentMenu(AppointmentService service)
        {
            while (true)
            {
                Console.WriteLine("\n----- APPOINTMENT MANAGEMENT -----");
                Console.WriteLine("1. Book Appointment");
                Console.WriteLine("2. View All Appointments");
                Console.WriteLine("3. Get Appointment By ID");
                Console.WriteLine("4. View Appointments By Patient ID");
                Console.WriteLine("5. View Appointments By Doctor ID");
                Console.WriteLine("6. Reschedule Appointment");
                Console.WriteLine("7. Cancel Appointment");
                Console.WriteLine("8. Complete Appointment");
                Console.WriteLine("0. Back");
                Console.Write("Enter choice: ");

                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            {
                                Appointment a = new Appointment();

                                Console.Write("Appointment ID: ");
                                a.AppointmentId = int.Parse(Console.ReadLine()!);

                                Console.Write("Patient ID: ");
                                a.PatientId = int.Parse(Console.ReadLine()!);

                                Console.Write("Doctor ID: ");
                                a.DoctorId = int.Parse(Console.ReadLine()!);

                                Console.Write("Date and time (yyyy-MM-dd HH:mm): ");
                                a.AppointmentDateTime = DateTime.ParseExact(
                                    Console.ReadLine()!,
                                    "yyyy-MM-dd HH:mm",
                                    CultureInfo.InvariantCulture);

                                Console.Write("Reason: ");
                                a.Reason = Console.ReadLine() ?? "";

                                Console.WriteLine(service.BookAppointment(a));
                                break;
                            }

                        case "2":
                            {
                                var appointments = service.GetAllAppointments();

                                if (!appointments.Any())
                                {
                                    Console.WriteLine("No appointments found.");
                                    break;
                                }

                                foreach (var a in appointments)
                                {
                                    Console.WriteLine(
                                        $"Appointment ID: {a.AppointmentId}, " +
                                        $"Patient ID: {a.PatientId}, Doctor ID: {a.DoctorId}, " +
                                        $"Date: {a.AppointmentDateTime:yyyy-MM-dd HH:mm}, " +
                                        $"Reason: {a.Reason}, Status: {a.Status}");
                                }
                                break;
                            }

                        case "3":
                            {
                                Console.Write("Appointment ID: ");
                                int id = int.Parse(Console.ReadLine()!);

                                var a = service.GetAppointmentById(id);

                                Console.WriteLine(
                                    $"Appointment ID: {a.AppointmentId}, " +
                                    $"Patient ID: {a.PatientId}, Doctor ID: {a.DoctorId}, " +
                                    $"Date: {a.AppointmentDateTime:yyyy-MM-dd HH:mm}, " +
                                    $"Reason: {a.Reason}, Status: {a.Status}");
                                break;
                            }

                        case "4":
                            {
                                Console.Write("Patient ID: ");
                                int id = int.Parse(Console.ReadLine()!);

                                var appointments = service.GetAppointmentsByPatientId(id);

                                if (!appointments.Any())
                                {
                                    Console.WriteLine("No appointments found for this patient.");
                                    break;
                                }

                                foreach (var a in appointments)
                                {
                                    Console.WriteLine(
                                        $"{a.AppointmentId} - " +
                                        $"{a.AppointmentDateTime:yyyy-MM-dd HH:mm} - {a.Status}");
                                }
                                break;
                            }

                        case "5":
                            {
                                Console.Write("Doctor ID: ");
                                int id = int.Parse(Console.ReadLine()!);

                                var appointments = service.GetAppointmentsByDoctorId(id);

                                if (!appointments.Any())
                                {
                                    Console.WriteLine("No appointments found for this doctor.");
                                    break;
                                }

                                foreach (var a in appointments)
                                {
                                    Console.WriteLine(
                                        $"{a.AppointmentId} - Patient ID: {a.PatientId} - " +
                                        $"{a.AppointmentDateTime:yyyy-MM-dd HH:mm} - {a.Status}");
                                }
                                break;
                            }

                        case "6":
                            {
                                Console.Write("Appointment ID: ");
                                int id = int.Parse(Console.ReadLine()!);

                                Console.Write("New date and time (yyyy-MM-dd HH:mm): ");
                                DateTime newDateTime = DateTime.ParseExact(
                                    Console.ReadLine()!,
                                    "yyyy-MM-dd HH:mm",
                                    CultureInfo.InvariantCulture);

                                Console.WriteLine(
                                    service.RescheduleAppointment(id, newDateTime));
                                break;
                            }

                        case "7":
                            {
                                Console.Write("Appointment ID to cancel: ");
                                int id = int.Parse(Console.ReadLine()!);

                                Console.WriteLine(service.CancelAppointment(id));
                                break;
                            }

                        case "8":
                            {
                                Console.Write("Appointment ID to complete: ");
                                int id = int.Parse(Console.ReadLine()!);

                                Console.WriteLine(service.CompleteAppointment(id));
                                break;
                            }

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
