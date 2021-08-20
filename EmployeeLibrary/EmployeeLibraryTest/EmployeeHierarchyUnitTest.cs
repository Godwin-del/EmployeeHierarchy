using EmployeeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace EmployeeLibraryTest
{
    public class EmployeeHierarchyUnitTest
    {
        private Employees employees;
        public EmployeeHierarchyUnitTest()
        {
            //read csv file
            var reader = new StreamReader(File.OpenRead(@"C:\Users\gkipkirui\Downloads\TestData.csv"));
            //list of rows in csv
            List<string> rows = new List<string>();
            while (!reader.EndOfStream)
            {
                var csvrow = reader.ReadLine();
                rows.Add(csvrow);
            }
            employees = new Employees(rows);
        }
        [Fact]
        public void Employee1()
        {
            var res = employees.SalaryBudget("Employee1");
            Assert.Equal(3800, res);
        }
        [Fact]
        public void Employee2()
        {
            var res = employees.SalaryBudget("Employee2");
            Assert.Equal(1800, res);
        }
        [Fact]
        public void Employee3()
        {
            var res = employees.SalaryBudget("Employee3");
            Assert.Equal(500, res);
        }
    }
}
