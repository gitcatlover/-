using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace 多线程
{
    class Program
    {
        public const int Repetitions = 1000;
        static void Main(string[] args)
        {
            //ThreadPool.QueueUserWorkItem(DoWork2,"+");

            //ThreadStart threadStart = DoWork;
            //Thread thread = new Thread(/*threadStart*/DoWork);
            //thread.Start();
            //for (int i = 0; i < Repetitions; i++)
            //{
            //    Console.Write("-");
            //}
            //thread.Priority = ThreadPriority.Highest;
            //bool isalive = thread.IsAlive;//线程是否"活着",工作状态
            //thread.Join();//告诉主线程等候工作线程完成

            Task task = Task.Run(() =>
            {
                for (int i = 0; i < Repetitions; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine(Task.CurrentId);
            });
            for (int j = 0; j < Repetitions; j++)
            {
                Console.Write("+");
            }
            task.Wait();

            Console.WriteLine("Before");
            Task taskA = Task.Run(() => Console.WriteLine("Starting..."))
                .ContinueWith(antecedent => Console.WriteLine($"Continuing A...{Task.CurrentId}"))
                .ContinueWith(a => Console.WriteLine("Success"), TaskContinuationOptions.OnlyOnRanToCompletion);

            Task taskB = taskA.ContinueWith(antecedent => Console.WriteLine($"Continuing B...{Task.CurrentId}"));
            Task taskC = taskA.ContinueWith(antecedent => { Thread.Sleep(2000); Console.WriteLine($"Continuing C...{Task.CurrentId}"); }) ;
            Task.WaitAll(taskB, taskC);
            Console.WriteLine("Finished!");

            Task.WaitAny(taskA, taskB, taskC);
            Console.WriteLine("all---------");

            bool parentTaskFaulted = false;
            Task taskD = new Task(() => throw new InvalidOperationException());
            Task continuationTask = task.ContinueWith((antecedentTask) => parentTaskFaulted = antecedentTask.IsFaulted);
            taskD.Start();
            continuationTask.Wait();
            Trace.Assert(parentTaskFaulted);
            Trace.Assert(taskD.IsFaulted);
            taskD.Exception.Handle(eachException =>
            {
                Console.WriteLine($"ERROR:{eachException.Message}");
                return true;
            });
            
        }

        public static void DoWork()
        {
            for (int i = 0; i < Repetitions; i++)
            {
                Console.Write("+");
            }
        }
        public static void DoWork2(object state)
        {
            for (int i = 0; i < Repetitions; i++)
            {
                Console.Write(state);
            }
        }
    }
}
