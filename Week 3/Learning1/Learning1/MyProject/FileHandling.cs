
//using System;
//using System.Text.Json;
//using System.Collections.Generic;

//public class Stu
//{
//    public string name { get; set; } = "";
//    public int age { get; set; }
//}

//class Program
//{
//    static void Main()
//    {
//        List<Stu> students = new List<Stu>
//        {
//            new Stu { name = "chandru", age = 23 },
//            new Stu { name = "harshini", age = 21 }
//        };

//        string json = JsonSerializer.Serialize(
//            students,
//            new JsonSerializerOptions { WriteIndented = true }
//        );

//        string path = "student.txt";
//        File.WriteAllText(path, json);
//        Console.WriteLine("Added Succesfully");

//        List<Stu>? ans = JsonSerializer.Deserialize<List<Stu>>(json);
//        foreach (Stu item in ans)
//        {
//            Console.WriteLine(item.name);
//        }
//    }
//}
