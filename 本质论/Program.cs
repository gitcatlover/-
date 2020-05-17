using System;
using System.Collections.Generic;
using System.Linq;

namespace 本质论
{
    class Program
    {
        public static int staID;
        static void Main(string[] args)
        {
            Console.WriteLine($"0x{42:X}");//42以十六进制输出
            double s = 1.618033988749895;
            string text = $"{s}";
            string text2 = string.Format("{0:R}", s);//避免double转字符串，再转double精度丢失

            string a = "Help";
            int c = string.Compare(a, "help", false);
            Console.WriteLine("\0");//null
            Console.WriteLine("\t");

            Console.WriteLine($"{2.5,10:C3}");//￥2.500 四舍五入保留3位小数，宽度10内右对齐（左对齐-10）
            int.TryParse("4/", out int b);
            Console.WriteLine(b);
            int.TryParse("3", out int p);
            Console.WriteLine(p);
            int? count = null;
            Console.WriteLine(count);

            bool has = Enum.IsDefined(typeof(gender), 1);
            string nan = Enum.GetName(typeof(gender), 0);
            bool t = new List<int> { 1, 25, 4 }.Contains(5);

            (string name, int age) person = ("feng", 18);
            Console.WriteLine($"my name is{person.name},old {person.age}");
            var person2 = ("feng", 18);
            Console.WriteLine(person2.Item1, person2.Item2);

            Console.WriteLine(ParseNames("feng").Item1);
            int[,] cells = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            Console.WriteLine(cells[2, 2] + $"长度{cells.Length},维数{cells.Rank}");//9
            foreach (var item in cells)
            {
                Console.WriteLine("----" + item);
            }

            int[][] move = { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 7, 8 } };
            Console.WriteLine(move[2][0] + $"长度{move.Length}");//move索引为2的数组第一个元素

            int[] arry = { 4, 5, 3 };
            var newArry = arry.Clone();
            Array.Sort(arry);
            Console.WriteLine(Array.BinarySearch(arry, 3));
            string[] strs = { "ab", "cd" };
            Console.WriteLine(strs[0].ToCharArray());
            Console.WriteLine(strs[0][0]);
            var dec = 2.15m;
            var n = (int)(dec * 5);
            Console.WriteLine("\":\"");

            List<int> lissss = new List<int> { 2, 6, 87, 45, 3 };

            lissss.Sort();
            Console.WriteLine(lissss.ToArray()[lissss.Count - 1]);
            Console.WriteLine(lissss[lissss.Count - 1]);

            Console.WriteLine("ss" + "*" + 3);

            List<string> strssss = new List<string> { "aaa", "bbb" };
            Console.WriteLine(string.Join(",", strssss));
            Console.WriteLine(DateTime.Now.ToString("s"));
            //List<Product> ps = new List<Product> { new Product { name = "", ID = 1 }, new Product { name = "", ID = 2 } };

            //var pmo = ps.Any(p => (p.ID == 1 && p.name == "3") || (p.ID == 2 && p.name == "2"));


            char current = 'a';
            int unicode;
            do
            {
                unicode = current;
                Console.WriteLine($"{current}={unicode}\t");
                current++;
            } while (current <= 'z');

            for (int i = 0, y = 6; i < 5 && y > 0; i++, y--)
            {
                Console.WriteLine($"{i}{((i < y) ? '<' : '>')}{y}");
            }
            Product product = new Product();
            Console.WriteLine(Product.SID);
            string wareids = "2";
            var warelist = wareids.Trim(',').Split(',').ToList();
            
            Console.ReadKey();
        }

        public enum gender
        {
            nan = 0,
            nv = 1,
        }

        public static ValueTuple<string, string, string> ParseNames(string fullName)
        {
            return ("firstName", "secondName", fullName);
        }
    }
    public class Product : Person
    {
        public static int SID = 22;
        public Product()
        {
            SID = 23;
        }

        public int ID { get; set; }
        public string name { get; set; } = "aaaa";
        public void Save()
        {
            var leng = this.Address.Length;
        }
    }
    public class Person
    {
        protected string Address { get; set; }
    }
}
