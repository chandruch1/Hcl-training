using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Hospital.Models;

namespace Hospital.Utility.Json
{
    public class PatientJsonService
    {
        private readonly string filePath =
            Path.Combine(
                AppContext.BaseDirectory,
                "Utility",
                "Json",
                "patients.json");

        public List<Patient> LoadPatients()
        {
            if (!File.Exists(filePath))
            {
                SavePatients(new List<Patient>());
                return new List<Patient>();
            }

            string json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<Patient>>(json)
                   ?? new List<Patient>();
        }

        public void SavePatients(List<Patient> patients)
        {
            string? directory = Path.GetDirectoryName(filePath);

            if (directory != null)
            {
                Directory.CreateDirectory(directory);
            }

            Console.WriteLine("Saving JSON to: " + filePath);

            string json = JsonSerializer.Serialize(
                patients,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(filePath, json);

            Console.WriteLine("JSON file saved successfully!");
        }
    }
}