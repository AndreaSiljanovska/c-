using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;

namespace practice
{ 
    //STAFF
    public class Staff
    {
        private float hourlyRate;

        private int hWorked;
        public float TotalPay { get; protected set; }
        public float BasicPay { get; private set; }
        public string NameOfStaff { get; private set; }
        public int HoursWorked 
        {
            get
            {
                return hWorked; 
            }
            set
            {
                if (value > 0)
                {
                    hWorked = value;
                }
                else if (value <= 0)
                {
                    hWorked = 0;
                }
            }
        }

        public Staff(string name, float rate)
        {
            NameOfStaff = name;
            hourlyRate = rate;
        }

        public virtual void CalculatePay()
        {
            Console.WriteLine("Calculating Pay...");
            BasicPay = hWorked * hourlyRate;
            TotalPay = BasicPay;
        }
        public override string ToString()
        {
            return "\n The Name of this stuff is:" + NameOfStaff + "\n houres worked:" + hWorked + "\n the horly rate is:" + hourlyRate + "\n the basic pay is:" + BasicPay + "\n the total pay is" + TotalPay;
        }
    }
    //MANAGER
    public class Manager : Staff
    {
        private const float managerHourlyRate = 50;
        public int Allowance { get; private set; }

        public Manager(string name)
            :base(name, managerHourlyRate)
        {

        }

        public override void CalculatePay()
        {
            base.CalculatePay();//dali e ok?
            Allowance = 1000;
            if (HoursWorked > 160)//160hours /month
            {
                TotalPay = BasicPay + Allowance;
            }
            else
            {
                TotalPay = BasicPay;
            }
        }
        public override string ToString()
        {
            return base.ToString();//dali e ok?
        }
    }
    public class Admin : Staff
    {
        private const float overtimeRate = 15.5F;
        private const int adminHourlyRate = 30;
        public float OverTime { get; private set; }

        public Admin(string name):
            base(name,adminHourlyRate)
        {

        }
        public override void CalculatePay()
        {
            base.CalculatePay();//??
            if (HoursWorked > 160)
            {
                var Overtime = overtimeRate * (HoursWorked - 160);
                TotalPay = BasicPay + Overtime;
            }
        }
        public override string ToString()
        {
            return base.ToString();//???
        }
        
    }
    // FILE READER
    public class FileReader
    {
        public List<Staff> ReadFile()
        {
            List<Staff> myStaff = new List<Staff>();
            string[] result = new string[2];
            string path = "staff.txt";
            string[] separator = { ", " };
            if (File.Exists(path))
            {
                using (StreamReader stream = new StreamReader(path))
                {
                    while(stream.EndOfStream != true)
                    {
                        string[] substrings = stream.ReadLine().Split(separator, StringSplitOptions.None);
                        result[0] = substrings[0];
                        result[1] = substrings[1];
                        if (result[1] == "Manager")
                        {
                            Manager manager = new Manager(result[0]);
                            myStaff.Add(manager);
                        }
                        else if (result[1] == "Admin")
                        {
                            Admin admin = new Admin(result[0]);
                            myStaff.Add(admin);
                        }
                    }
                    stream.Close();
                    
                }
             
            }
            else
            {
                Console.WriteLine("The file does not exist");
            }
        
            return myStaff;
        }
    }
    //PAYSLIP
    public class PaySlip
    {
        private int month;
        private int year;
        enum MonthsOfYear
        {
            NotSet = 0,
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12

        }
        public PaySlip(int paymonth, int payyear)
        {
            month = paymonth;
            year = payyear;
        }
        public void GeneratePaySlip(List<Staff> myStaff)
        {
            string path;
            foreach(Staff staff in myStaff)
            {
                path = staff.NameOfStaff + ".txt";
                StreamWriter sw = new StreamWriter(path);
                sw.WriteLine("PAYSLIP FOR {0} {1}", (MonthsOfYear)month, year);
                sw.WriteLine("===============================");
                sw.WriteLine("Name of Staff: {0}", staff.NameOfStaff);
                sw.WriteLine("Hours Worked: {0}", staff.HoursWorked);
                sw.WriteLine("");
                sw.WriteLine("Basic Pay: {0}", staff.BasicPay.ToString("C", CultureInfo.CurrentCulture));
                if (staff.GetType() == typeof(Manager))
                {
                    sw.WriteLine("Allowence: {0}", ((Manager)staff).Allowance.ToString("C", CultureInfo.CurrentCulture));
                }
                else if (staff.GetType() == typeof(Admin))
                {
                    sw.WriteLine("Overtime Pay: {0}", ((Admin)staff).OverTime.ToString("C", CultureInfo.CurrentCulture));
                }
                sw.WriteLine("");
                sw.WriteLine("===============================");
                sw.WriteLine("Total pay: {0}", staff.TotalPay.ToString("C", CultureInfo.CurrentCulture));
                sw.WriteLine("===============================");
                sw.Close();
            }
            
        }

        public void GenerateSummary(List<Staff> myStaff)
        {
            var result =
                from staff in myStaff
                where staff.HoursWorked < 10
                orderby staff.NameOfStaff ascending
                select new { staff.NameOfStaff, staff.HoursWorked };

            string path = "summary.txt";
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("Staff with less than 10 working hours");
            sw.WriteLine("");
            foreach(var r in result)
            {
                sw.WriteLine("Name of Staff: {0}, Hours Worked: {1}",r.NameOfStaff,r.HoursWorked);
            }
            sw.Close();
        }
            public override string ToString()
            {
                    return base.ToString();
            }
 
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
