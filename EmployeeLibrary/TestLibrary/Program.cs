using System;
using System.Collections.Generic;
using System.IO;

namespace TestLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new StreamReader(File.OpenRead(@"C:\Users\gkipkirui\Downloads\TestData.csv"));
            List<string> rows = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                rows.Add(line);
            }
            EmployeeLibrary.Employees employees = new EmployeeLibrary.Employees(rows);
            Console.WriteLine("Result " + employees.SalaryBudget("Employee1"));
        }
    }
}
