using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace EmployeeLibrary
{
    public class Employees
    {
        ObjectCache objectCache = MemoryCache.Default;
        List<string> gemployeeslist = new();
        public Employees(List<string> CSVData)
        {
                                                CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(12)};
                                                List<string> employees = new();
                                                List<string> managers = new();

            foreach (var CSVDataRow in CSVData)
            {
                string[] CSVDataRowArray = CSVDataRow.Split(',');
                if (CSVDataRowArray.Length > 3 || CSVDataRowArray.Length < 2)
                {
                    throw new IndexOutOfRangeException();
                }
                if (!int.TryParse(CSVDataRowArray[2], out _))
                {
                    throw new Exception("Invalid Salary for Employee " + CSVDataRowArray[0]);
                }
                string manager = (string)objectCache.Get(CSVDataRowArray[0]);
                if (manager==null)
                {
                    manager = "";
                }
                if (manager == "CEO")
                {
                    throw new Exception("There can be only one CEO");
                }
                if (!string.IsNullOrEmpty(manager))
                {
                    if (manager!= CSVDataRowArray[1])
                    {
                        throw new Exception("Employee " + CSVDataRowArray[0] + " reports to more than one manager.");
                    }
                }

                //check circular dependency
                if (manager!=null)
                {
                    string circulardependencycheck = (string)objectCache.Get(manager);
                    if (circulardependencycheck == CSVDataRowArray[0])
                    {
                        throw new Exception("Circular Dependency Exception");
                    }
                    if (string.IsNullOrEmpty(CSVDataRowArray[1]))
                    {
                        CSVDataRowArray = new string[] { CSVDataRowArray[0], "CEO" };
                    }
                    objectCache.Set(CSVDataRowArray[0].ToString(), CSVDataRowArray[1].ToString(), policy);
                    employees.Add(CSVDataRowArray[0]);
                    if (CSVDataRowArray[1] != "CEO")
                    {
                        managers.Add(CSVDataRowArray[1]);
                    }
                    foreach (var lmanager in managers)
                    {
                        if (!employees.Contains(lmanager))
                        {
                            throw new Exception(lmanager + "Not listed in the employee column");
                        }
                    }
                }
            }
            gemployeeslist = new List<string>(CSVData);
        }
        public long SalaryBudget(string manager)
        {
            if (string.IsNullOrEmpty(manager))
            {
                throw new Exception("Manager Can't be empty");
            }
            string[] employeedtlarray = null;
            List<long> salaries = new();
            List<string> employees = new();
            foreach (var employeel in gemployeeslist)
            {
                 employeedtlarray = employeel.Split(',');
                if (manager == employeedtlarray[1])
                {
                    employees.Add(employeedtlarray[0]);
                    salaries.Add(Convert.ToInt64(employeedtlarray[2]));
                }
                if (manager == employeedtlarray[0])
                {
                    salaries.Add(Convert.ToInt64(employeedtlarray[2]));
                }
            }
            string newmanager = string.Empty;
            foreach (var empl in employees)
            {
                if (empl == employeedtlarray[1])
                {
                    newmanager = employeedtlarray[1];
                }
            }
            if (!string.IsNullOrEmpty(newmanager))
            {
                foreach (var employeel in gemployeeslist)
                {
                    employeedtlarray = employeel.Split(',');
                    if (newmanager == employeedtlarray[1])
                    {
                        salaries.Add(Convert.ToInt64(employeedtlarray[2]));
                    }
                }
            }
            long sumofdirectemployees = salaries.Sum(x => Convert.ToInt64(x));
            return sumofdirectemployees;
        }
    }
}
