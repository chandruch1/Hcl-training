using Hospital.Exceptions;
using Hospital.Models;
using Hospital.Utility.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Services
{
    public class PatientServices
    {
        private readonly PatientJsonService _json;
        private List<Patient> patients;

        public PatientServices(PatientJsonService json)
        {
            _json = json;
            patients = _json.LoadPatients();
        }

        public string AddPatient(Patient patient)
        {
            if (patient.PatientId <= 0)
                throw new IdException("Patient ID must be positive.");

            if (patients.Any(p => p.PatientId == patient.PatientId))
                throw new IdException("Patient ID already exists.");

            if (patient.Age < 0 || patient.Age > 120)
                throw new ArgumentException(
                    "Age must be between 0 and 120.");

            patients.Add(patient);
            _json.SavePatients(patients);

            return "Patient added successfully.";
        }

        public Patient GetPatientById(int id)
        {
            var patient = patients.FirstOrDefault(
                p => p.PatientId == id);

            if (patient == null)
                throw new IdException("Patient does not exist.");

            return patient;
        }

        public List<Patient> GetAllPatients()
        {
            return patients.ToList();
        }

        public string UpdatePatient(int id, Patient updatePatient)
        {
            if (updatePatient.Age < 0 || updatePatient.Age > 120)
                throw new ArgumentException(
                    "Age must be between 0 and 120.");

            var patient = GetPatientById(id);

            patient.FullName = updatePatient.FullName;
            patient.Age = updatePatient.Age;
            patient.PhoneNumber = updatePatient.PhoneNumber;
            patient.Email = updatePatient.Email;

            _json.SavePatients(patients);

            return "Patient updated successfully.";
        }

        public List<Patient> SearchPatientsByName(string name)
        {
            var result = patients
                .Where(p => p.FullName.Contains(
                    name,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!result.Any())
                throw new NameException(
                    "Patient name is not available in the database.");

            return result;
        }

        public string DeletePatient(int id)
        {
            var patient = GetPatientById(id);

            patients.Remove(patient);
            _json.SavePatients(patients);

            return "Patient deleted successfully.";
        }
    }
}