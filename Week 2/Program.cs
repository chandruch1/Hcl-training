
using System;

class HelloWorld
{
    public delegate void Delg<T>(T m);
    public delegate int Delnew<T, U>(T m, U n);
    public delegate bool Predicated<T>(T arg);
    public delegate string func<T, U, V>(T a, U n, V msg);

    public void Display<T>(T msg)
    {
        Console.WriteLine(msg);
    }

    public int sum(int a, int b)
    {
        return a + b;
    }

    public bool predict(int arg)
    {
        return true;
    }

    public string function(int a, int b, string msg)
    {
        return msg;
    }

    public void Run()
    {
        Delg<string> a = Display;
        a("hello");

        Delg<int> b = Display;
        b(15);
       

        Delnew<int, int> c = sum;
        Console.WriteLine(c(10, 15));

        Predicated<int> d = predict;
        Console.WriteLine(d(10));
    

    }

    static void Main()
    {
        HelloWorld obj = new HelloWorld();
        obj.Run();
        Action<string> display = msg => Console.WriteLine(msg);
        display("Action");

        // 2. Func: accepts two integers and returns an integer
        Func<int, int, int> sum = (a, b) => a + b;
        Console.WriteLine(sum(10, 45));

        // 3. Predicate: accepts an integer and returns bool
        Predicate<int> predict = n => n > 10;
        Console.WriteLine(predict(15));
        Console.WriteLine(predict(5));
    }
}
