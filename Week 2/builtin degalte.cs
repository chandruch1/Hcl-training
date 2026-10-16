using System;

class HelloWorld
{
    public void Print(string s)
    {
        Console.WriteLine(s);
    }

    public bool IsValid(int age)
    {
        return age >= 18;
    }

    public int Sum(int a, int b)
    {
        return a + b;
    }

    public string Add(string a, string b)
    {
        return a + b;
    }

    public void Run()
    {
        // Action: takes input and returns nothing
        Action<string> a = Print;
        a("Chandru Action");

        // Predicate: takes input and returns bool
        Predicate<int> b = IsValid;
        Console.WriteLine(b(15));

        // Func: takes two int inputs and returns int
        Func<int, int, int> c = Sum;
        Console.WriteLine(c(12, 13));

        // Func: takes two string inputs and returns string
        Func<string, string, string> d = Add;
        Console.WriteLine(d("Hello ", "Chandru"));
    }

    static void Main()
    {
        HelloWorld obj = new HelloWorld();
        obj.Run();
    }
}