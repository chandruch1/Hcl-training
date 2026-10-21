using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Hospital.Models;

namespace Hospital.Utility.Json
{
    public class DoctorJsonService
    {
        private readonly string filePath =
            Path.Combine(
                AppContext.BaseDirectory,
                "Utility",
                "Json",
                "Doctor.json");


        public List<Doctor> LoadDoctors()
        {
            if (!File.Exists(filePath))
            {
                SaveDoctors(new List<Doctor>());
                return new List<Doctor>();
            }

            string json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<Doctor>>(json)
                   ?? new List<Doctor>();
        }

        public void SaveDoctors(List<Doctor> doctors)
        {
            string? directory = Path.GetDirectoryName(filePath);
            if (directory != null)
            {
                Directory.CreateDirectory(directory);
            }
            Console.WriteLine("File is Saving in to Doctor Json");

            string json = JsonSerializer.Serialize(doctors,
                 new JsonSerializerOptions
                 {
                     WriteIndented = true
                 });
            File.WriteAllText(filePath, json);

            Console.WriteLine("JSON file saved successfully!");

        }
    }
}
