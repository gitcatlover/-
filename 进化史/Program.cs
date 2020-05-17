using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace 进化史
{
    delegate void StringProcessor(string input);
    class Program
    {
        public static List<T> XmlToList<T>(string xml, string rootName) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootName));
            using (StringReader sr = new StringReader(xml))
            {
                List<T> list = serializer.Deserialize(sr) as List<T>;
                return list;
            }
        }
        static void Main(string[] args)
        {
            #region MyRegion
            List<Product> products = Product.GetSampleProducts();

            List<Supplier> suppliers = Supplier.GetSampleSupplier();
            var filtered = from p in products
                           join s in suppliers
                           on p.SupplierID equals s.SupplierID
                           where p.Price > 10
                           orderby s.Name, p.Name
                           select new { SupplierName = s.Name, ProductName = p.Name };

            foreach (var item in filtered)
            {
                Console.WriteLine("Supplier={0};Product={1}", item.ProductName, item.SupplierName);
            }
            Demo<int>();

            //string[] words = { "zero", "one", "two", "three", "four" };
            //int[] numbers = { 0, 1, 2, 3, 4 };
            ////连接两个序列
            //IEnumerable<int> a = numbers.Concat(new[] { 7, 9, 8 });
            //Dictionary<string, string> dic = words.ToDictionary(w => w.Substring(0, 2));
            //ILookup<string, string> look = words.ToLookup(w => w.Substring(0, 1));
            //words.First(w => w.Length == 3);
            //words.SingleOrDefault(w => w.Length == 10);//NULL
            //IEnumerable<IGrouping<int, string>> vs = words.GroupBy(w => w.Length);

            //string[] names = { "Robin", "Ruth", "Bob", "Emma" };
            //string[] colors = { "Red", "Blue", "Beige", "Green" };
            //var n = names.Join//左边序列
            //    (colors,//右边序列
            //    name => name[0],//左边键选择器
            //    color => color[0],//右边键选择器
            //    (name, color) => name + "-" + color//为结果对投影
            //    );
            //words.Contains("FOUR", StringComparer.OrdinalIgnoreCase);
            //var m = words.OrderByDescending(w => w.Length);
            //var q = from w in words
            //        let len = w.Length//let子句，减少w.Lengthd的重复计算
            //        where len > 2
            //        orderby len descending
            //        select w;
            ////交叉连接，左边第n个序列和右边序列交叉n次
            //var query = from left in Enumerable.Range(1, 4)
            //            from right in Enumerable.Range(11, left)
            //            select new { Left = left, Right = right };
            //foreach (var item in query)
            //{
            //    Console.WriteLine("Left={0};Right={1}", item.Left, item.Right);
            //} 
            #endregion

            Console.WriteLine("-----------------------------");
            Person jon = new Person("Jon");
            Person tom = new Person("Tom");
            StringProcessor jonSay, tomSay, background;
            jonSay = new StringProcessor(jon.SayHi);
            tomSay = new StringProcessor(tom.SayHi);
            background = new StringProcessor(Background.Note);
            jonSay("Hello");
            tomSay("Bye");
            background("go away");

            StringBuilder builder = new StringBuilder("666");
            TestReference(builder);
            Console.WriteLine(builder);
            builder.Append("777");
            Console.WriteLine(builder);

            DirectoryInfo info = new DirectoryInfo(@"C:\Users\fengj\Desktop\20191206");
            var dirQuery = from di in info.GetDirectories()
                           orderby di.Name
                           select new { di.Name };

            foreach (var item in dirQuery)
            {
                Console.WriteLine(item.Name);
            }

            var procQuery = from proc in Process.GetProcesses()
                            orderby proc.WorkingSet64 descending
                            select new { proc.Id, proc.ProcessName, proc.WorkingSet64 };


            List<student> list = new List<student>
            {
                new student{sku=123,name="123_1",num=2,boxnum=1},
                new student{sku=123,name="123_2",num=3,boxnum=2},
                new student{sku=124,name="124_1",num=1,boxnum=3},
                new student{sku=124,boxnum=4},
                new student{sku=125,boxnum=5}
            };
            var newlist = list.GroupBy(s => s.sku)
                .Select(l => new newstudent { Name = l.First().name, SKU = l.Key, Num = l.Sum(n => n.num), BoxNum = l.Key }).ToList();

            PostPonyGetLabelFileRequest request = new PostPonyGetLabelFileRequest()
            {
                UserCredentialone = new UserCredential
                {
                    Key = "key",
                    Pwd = "pwd"
                },
                LabelIds = new List<string>()
            };

            for (int i = 0; i < 2; i++)
            {
                request.LabelIds.Add("3");
            }
            var postjson = JsonConvert.SerializeObject(request);

            TestTrackNo test = new TestTrackNo();
            string trackNo = test.GetTrack("9999");

            Console.ReadKey();
        }
        static void Demo<x>()
        {
            Console.WriteLine(typeof(x));
            Console.WriteLine(typeof(Dictionary<,>));
        }
        static void TestReference(StringBuilder builder)
        {
            builder = null;
        }
    }
    class Person
    {
        string name;

        public Person(string name) { this.name = name; }
        public void SayHi(string message) { Console.WriteLine($"{name} says: {message}"); }
    }
    class Background
    {
        public static void Note(string note) { Console.WriteLine($"({note})"); }
    }
    public class Works : DataContext
    {
        public Works(IDbConnection connection) : base(connection)
        {
        }
        public Table<DirectoryInforamation> DirectoryInformation;
    }

    public class DirectoryInforamation
    {
        [Column(DbType = "varchar(50)")]
        public string DirectoryName { get; set; }
        [Column(DbType = "varchar(50)")]
        public string DirectoryDescription { get; set; }
    }
    public class student
    {
        public int sku { get; set; }
        public string name { get; set; }
        public int num { get; set; }
        public int boxnum { get; set; }
    }
    public class newstudent
    {
        public int SKU { get; set; }
        public string Name { get; set; }
        public int Num { get; set; }
        public int BoxNum { get; set; }
    }

    [Serializable]
    [XmlRoot("BatchDownloadRequest")]
    public class PostPonyGetLabelFileRequest
    {
        [XmlElement("UserCredential")]
        public UserCredential UserCredentialone { get; set; }

        /// <summary>
        /// Labelid 数组列表
        /// </summary>
        [XmlArray("LabelIds"), XmlArrayItem("int")]
        public List<string> LabelIds { get; set; }
    }
    public class UserCredential
    {
        public string Key { get; set; }
        public string Pwd { get; set; }
    }

    //public class LabelId
    //{
    //    [XmlElement("int")]
    //    public string Id { get; set; }
    //}

    [Serializable, XmlType("class")]
    public class ClassSet
    {
        [XmlElement("classname")]
        public string Name { get; set; }

        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("remark")]
        public string Remark { get; set; }

        [XmlArray("students")]
        public List<Student> Students { get; set; }

        [Serializable, XmlType("student")]
        public class Student
        {
            [XmlElement("name")]
            public string Name { get; set; }

            [XmlElement("sex")]
            public string Sex { get; set; }

            [XmlElement("age")]
            public int Age { get; set; }

        }
    }
}
