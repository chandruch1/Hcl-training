
using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Exceptions;
using Hospital.Models;
using Hospital.Utility.Json;

namespace Hospital.Services
{
    public class AppointmentService
    {
        private readonly AppointmentJsonService _json;
        private readonly PatientServices _patientService;
        private readonly DoctorService _doctorService;

        private List<Appointment> appointments;

        public AppointmentService(
            AppointmentJsonService json,
            PatientServices patientService,
            DoctorService doctorService)
        {
            _json = json;
            _patientService = patientService;
            _doctorService = doctorService;

            appointments = _json.LoadAppointments();
        }


        public string RescheduleAppointment(int appointmentId, DateTime newDateTime)
        {
            var appointment = GetAppointmentById(appointmentId);

            if (appointment.Status != "Scheduled")
                throw new InvalidOperationException(
                    "Only scheduled appointments can be rescheduled.");

            if (newDateTime <= DateTime.Now)
                throw new ArgumentException(
                    "New appointment time must be in the future.");

            bool doctorBusy = appointments.Any(a =>
                a.AppointmentId != appointmentId &&
                a.DoctorId == appointment.DoctorId &&
                a.AppointmentDateTime == newDateTime &&
                a.Status == "Scheduled");

            if (doctorBusy)
                throw new InvalidOperationException(
                    "Doctor already has an appointment at this time.");

            bool patientBusy = appointments.Any(a =>
                a.AppointmentId != appointmentId &&
                a.PatientId == appointment.PatientId &&
                a.AppointmentDateTime == newDateTime &&
                a.Status == "Scheduled");

            if (patientBusy)
                throw new InvalidOperationException(
                    "Patient already has an appointment at this time.");

            appointment.AppointmentDateTime = newDateTime;

            _json.SaveAppointments(appointments);

            return "Appointment rescheduled successfully.";
        }

        public string BookAppointment(Appointment appointment)
        {
            if (appointment.AppointmentId <= 0)
                throw new IdException("Appointment ID must be positive.");

            if (appointments.Any(a =>
                a.AppointmentId == appointment.AppointmentId))
            {
                throw new IdException("Appointment ID already ex   ists.");
            }

            
            _patientService.GetPatientById(appointment.PatientId);
            _doctorService.GetDoctorById(appointment.DoctorId);

            if (appointment.AppointmentDateTime <= DateTime.Now)
                throw new ArgumentException(
                    "Appointment must be scheduled in the future.");

            if (string.IsNullOrWhiteSpace(appointment.Reason))
                throw new ArgumentException(
                    "Appointment reason cannot be empty.");

          
            bool doctorBusy = appointments.Any(a =>
                a.DoctorId == appointment.DoctorId &&
                a.AppointmentDateTime == appointment.AppointmentDateTime &&
                a.Status == "Scheduled");

            if (doctorBusy)
                throw new InvalidOperationException(
                    "Doctor already has an appointment at this time.");

            
            bool patientBusy = appointments.Any(a =>
                a.PatientId == appointment.PatientId &&
                a.AppointmentDateTime == appointment.AppointmentDateTime &&
                a.Status == "Scheduled");

            if (patientBusy)
                throw new InvalidOperationException(
                    "Patient already has an appointment at this time.");

            appointment.Status = "Scheduled";

            appointments.Add(appointment);
            _json.SaveAppointments(appointments);

            return "Appointment booked successfully.";
        }

       
        public Appointment GetAppointmentById(int id)
        {
            var appointment = appointments
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointment == null)
                throw new IdException("Appointment does not exist.");

            return appointment;
        }

        public List<Appointment> GetAllAppointments()
        {
            return appointments.ToList();
        }

        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            _patientService.GetPatientById(patientId);

            return appointments
                .Where(a => a.PatientId == patientId)
                .ToList();
        }

        public List<Appointment> GetAppointmentsByDoctorId(int doctorId)
        {
            _doctorService.GetDoctorById(doctorId);

            return appointments
                .Where(a => a.DoctorId == doctorId)
                .ToList();
        }

        
        public string CancelAppointment(int id)
        {
            var appointment = GetAppointmentById(id);

            if (appointment.Status != "Scheduled")
                throw new InvalidOperationException(
                    "Only scheduled appointments can be cancelled.");

            appointment.Status = "Cancelled";

            _json.SaveAppointments(appointments);

            return "Appointment cancelled successfully.";
        }

        
        public string CompleteAppointment(int id)
        {
            var appointment = GetAppointmentById(id);

            if (appointment.Status != "Scheduled")
                throw new InvalidOperationException(
                    "Only scheduled appointments can be completed.");

            if (appointment.AppointmentDateTime > DateTime.Now)
                throw new InvalidOperationException(
                    "A future appointment cannot be completed.");

            appointment.Status = "Completed";

            _json.SaveAppointments(appointments);

            return "Appointment marked as completed.";
        }
    }
}
