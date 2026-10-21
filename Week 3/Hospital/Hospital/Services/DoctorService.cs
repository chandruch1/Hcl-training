using System;
using System.Collections.Generic;
using System.Text;
using Hospital.Exceptions;
using Hospital.Models;
using Hospital.Utility.Json;
using System.Linq;

namespace Hospital.Services
{
     public class DoctorService
    {
        private readonly DoctorJsonService _json;
        private List<Doctor> doctors;

        public DoctorService(DoctorJsonService json)
        {
            _json = json;
            doctors = _json.LoadDoctors();
        }

        public string AddDoctors(Doctor doctor)
        {
            if (doctor.DoctorId <= 0)
            {
                throw new IdException("Doctor ID must be positive.");
            }

            if (doctor.CounsultFee < 0)
            {
                throw new SalaryException("Salary Must Be Positive");
            }
            if (doctors.Any(d => d.DoctorId == doctor.DoctorId))
            {
                throw new IdException("Doctor Id id Already Available");
            }
            doctors.Add(doctor);
            _json.SaveDoctors(doctors);
            return "Doctors Added Successfully";
        }

        public Doctor GetDoctorById(int id)
        {
            var doctor = doctors.FirstOrDefault(d => d.DoctorId == id);
            if (doctor == null)
            {
                throw new IdException("Doctor Doesn't Exsist");
            }
            return doctor;
        }

        public List<Doctor> GetAllDoctors()
        {
            return doctors.ToList();
        }

        public string UpdateDoctor(int id,Doctor updatedoctor)
        {
            var doctor = GetDoctorById(id);
            if (doctor == null)
            {
                throw new IdException("Doctor Doesn't Exsist");
            }
           
            doctor.FullName = updatedoctor.FullName;
            doctor.CounsultFee = updatedoctor.CounsultFee;
            doctor.Specalization = updatedoctor.Specalization;

            _json.SaveDoctors(doctors);
            return "Doctor Updated Successfully";

        }
        public List<Doctor> SearchDoctorsByName(string name)
        {
            var doctor = doctors.Where(d => d.FullName.Contains(name, StringComparison.OrdinalIgnoreCase)).
            ToList();

            if (!doctor.Any())
            {
                throw new NameException(
                    "Doctor name is not available in the database.");
            }
            return doctor;
        }
        public List<Doctor> SearchDoctorsBySpecialization(string spe)
        {
            var specalization = doctors.Where(d => d.Specalization == spe).ToList();
            if (!specalization.Any())
            {
                throw new NameException(
                    "currently this Specalization Doctor are not available");
            }
            return specalization;
        }

        public string DeleteDoctor(int id)
        {
            var doctor = GetDoctorById(id);
            if(doctor == null)
            {
                throw new NameException(
                    "Doctor id is not available in the database.");
            }
            doctors.Remove(doctor);
            _json.SaveDoctors(doctors);
            return " Doctor Deleted Succesfully";
        }

    }
}
