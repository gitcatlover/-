using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace new修饰符使用
{
    class Program
    {
        //private static string number = "5";
        static void Main(string[] args)
        {
            SuperSubDerivedClass superSubDerivedClass = new SuperSubDerivedClass();
            SubDerivedClass subDerivedClass = superSubDerivedClass;
            DerivedClass derivedClass = superSubDerivedClass;
            BaseClass baseClass = superSubDerivedClass;

            superSubDerivedClass.DisplayName();
            subDerivedClass.DisplayName();
            derivedClass.DisplayName();
            baseClass.DisplayName();
            StringBuilder number = new StringBuilder();
            number.Append("5");

            Console.WriteLine(DateTime.Now.AddDays(1.1).ToString("yyyy-MM-dd"));

            var desk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //Thermostat thermostat = new Thermostat();
            //Heater heater = new Heater(60);
            //Cooler cooler = new Cooler(80);
            //string temperature;
            //thermostat.OntemperatureChange += heater.OnTemperatureCahnged;
            //thermostat.OntemperatureChange += cooler.OnTemperatureChanged;
            //Console.WriteLine("Enter temperature:");
            //temperature = Console.ReadLine();
            //thermostat.CurrentTemperature = int.Parse(temperature);
            //thermostat.OntemperatureChange.Invoke(thermostat.CurrentTemperature);

            Stack<int> stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);
            Stack<int>.Enumerator enumerator = stack.GetEnumerator();
            int num;
            while (enumerator.MoveNext())
            {
                num = enumerator.Current;
                Console.WriteLine(num);
            }


            List<Employee> employees = new List<Employee>() { new Employee { DepartmentID = 1, Name = "w" } };
            List<Department> departments = new List<Department> { new Department { ID = 1, MyProperty = "aaa" } };
            var list = employees.Join(departments,
                  employee => employee.DepartmentID,
                  department => department.ID,
                  (employee, department) => new { id = employee.DepartmentID, name = department.MyProperty }

                  );
            foreach (var item in list)
            {
                Console.WriteLine(item.name);
            }
            string[] text = { "na me", "bo dy", "ar ea" };
            List<int> odd = new List<int>() { 1, 2, 4, 3, 5, 6 };
            List<int> newodd = new List<int> { 6, 6, 7, 8, 8, 9, 3, 2 };
            List<string[]> tokens = text.Select(t => t.Split(' ')).ToList();

            var ttt = text.SelectMany(t => t.Split(' '));

            List<object> buffer = new List<object> { new object(), 1, 2, "3", "a", Guid.NewGuid() };
            foreach (var item in buffer.OfType<int>())
            {
                Console.WriteLine(item);
            }
            var all = text.Union(text);
            var newall = odd.Union(newodd);//去除重复值连接两个集合
            var newnew = odd.Concat(newodd);
            var newinse = odd.Intersect(newodd);//取出两个集合中相同值
            bool isEqual = odd.SequenceEqual(newodd);//两个集合里面值是否相同，顺序是否一致

            var fileNames = from fileName in Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
                            let file = new FileInfo(fileName)
                            orderby file.Length descending, fileName
                            select file;
            foreach (var file in fileNames)
            {
                Console.WriteLine(file.Name);
            }

            var result = odd.FindAll(Event);

            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.ContainsKey(2);
            dic.ContainsValue("a");

            var type = DateTime.Now.GetType();

            foreach (var item in type.GetProperties())
            {
                Console.WriteLine(item.Name);
            }

            Console.ReadKey();
        }
        public static bool Event(int number) => (number % 2) == 0;
    }
    public class Employee
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }
    }
    public class Department
    {
        public int ID { get; set; }
        public string MyProperty { get; set; }
    }

    public class Cooler
    {
        public int Id { get; set; }
        public float Temperature { get; set; }
        public Cooler(float temperature)
        {
            Temperature = temperature;
        }
        public void OnTemperatureChanged(float newTemperature)
        {
            if (newTemperature > Temperature)
            {
                Console.WriteLine("Cooler:On");
            }
            else
            {
                Console.WriteLine("Cooler:Off");
            }
        }
    }
    public class Heater
    {
        public int Id { get; set; }
        public float Temperature { get; set; }
        public Heater(float temperature)
        {
            Temperature = temperature;
        }
        public void OnTemperatureCahnged(float newTemperature)
        {
            if (newTemperature < Temperature)
            {
                Console.WriteLine("Heater:On");
            }
            else
            {
                Console.WriteLine("Heater:Off");
            }
        }
    }
    public class Thermostat
    {
        public Action<float> OntemperatureChange { get; set; }
        public float CurrentTemperature { get; set; }
        public float _CurrentTemperature
        {
            get { return _CurrentTemperature; }
            set
            {
                if (value != _CurrentTemperature)
                {
                    _CurrentTemperature = value;
                    Action<float> action = OntemperatureChange;
                    if (action != null)
                    {
                        OntemperatureChange(value);
                    }
                }
            }
        }
    }

    public interface IBaseClass
    {
        void Message();
    }
    public class BaseClass : IBaseClass
    {
        public void DisplayName()
        {
            Console.WriteLine("BaseClass");
        }

        public void Message()
        {
            throw new NotImplementedException();
        }
    }
    public class DerivedClass : BaseClass
    {
        public virtual void DisplayName()
        {
            Console.WriteLine("DerivedClass");
        }
    }
    public class SubDerivedClass : DerivedClass
    {
        public override void DisplayName()
        {
            Console.WriteLine("SubDerivedClass");
        }
    }
    public class SuperSubDerivedClass : SubDerivedClass
    {
        public new void DisplayName()
        {
            Console.WriteLine("SuperSubDerivedClass");
        }
    }
}
