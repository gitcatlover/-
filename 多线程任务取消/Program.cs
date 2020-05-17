using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace 多线程任务取消
{
    class Program
    {
        static void Main(string[] args)
        {
            //string starts = "*".PadRight(Console.WindowWidth - 1, '*');
            //Console.WriteLine("Push ENTER to exit");
            //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            //Task task = Task.Run
            //    (
            //    () => WritePi(cancellationTokenSource),
            //    cancellationTokenSource.Token
            //    );
            //Console.ReadLine();
            //Console.WriteLine("ENTER exited...");
            //cancellationTokenSource.Cancel();
            //Console.WriteLine(starts);
            //task.Wait();

            ////告诉调度器耗时任务
            //Task taskB = Task.Factory.StartNew(() => WritePi(cancellationTokenSource), TaskCreationOptions.LongRunning);

            //WebRequest webRequest = WebRequest.Create("https://cn.bing.com/");
            //WebResponse webResponse = webRequest.GetResponse();
            //using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            //{
            //    string text = reader.ReadToEnd();
            //}

            //string url = "http://localhost:54425/";
            //Task taskAsync = WriteWebRequestSizeAsync(url);
            //while (!taskAsync.Wait(100))
            //{
            //    Console.Write(".");
            //}

            //泛型和委托 只支持引用类型的协变活着逆变
            //委托可以接受比目标方法更具体的参数，及逆变
            Action[] actions = new Action[3];
            for (int i = 0; i < 3; i++)
            {
                //int loopScopedi = i;
                actions[i] = () => Console.WriteLine(/*loopScopedi*/i);
            }
            //调用时才执行，执行actions时，委托里面保存什么值，就输出什么。lambda表达式与执行生命周期同步
            foreach (var a in actions)
            {
                a();//0 1 2    3 3 3
            }

            Console.WriteLine();
        }
        public static void WritePi(CancellationTokenSource cancellationTokenSource)
        {
            string piSection = string.Empty;
            int i = 0;
            while (!cancellationTokenSource.IsCancellationRequested || i == int.MaxValue)
            {
                i++;
                piSection = "+" + i;
                Console.Write(piSection);
            }
        }
        public static async Task WriteWebRequestSizeAsync(string url)
        {
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                WebResponse webResponse = await webRequest.GetResponseAsync();
                using(StreamReader reader=new StreamReader(webResponse.GetResponseStream()))
                {
                    string text = await reader.ReadToEndAsync();
                    Console.WriteLine(text.Length);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
