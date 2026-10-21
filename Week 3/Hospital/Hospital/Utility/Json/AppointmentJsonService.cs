
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Hospital.Models;

namespace Hospital.Utility.Json
{
    public class AppointmentJsonService
    {
        private readonly string filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Utility",
            "Json",
            "appointments.json");

        public List<Appointment> LoadAppointments()
        {
            if (!File.Exists(filePath))
            {
                SaveAppointments(new List<Appointment>());
                return new List<Appointment>();
            }

            string json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<Appointment>>(json)
                   ?? new List<Appointment>();
        }

        public void SaveAppointments(List<Appointment> appointments)
        {
            string? directory = Path.GetDirectoryName(filePath);

            if (directory != null)
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonSerializer.Serialize(
                appointments,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(filePath, json);
        }
    }
}
