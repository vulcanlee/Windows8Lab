using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            start();
            Console.ReadKey();
        }

        private static async void start()
        {
            Console.WriteLine("-------------------- Begin Complete ~~");
            Console.WriteLine("Main UI Thread ID : {0}", Thread.CurrentThread.ManagedThreadId);
            await fun1Async();
            await fun2Async();
            await fun3Async();
            Console.WriteLine("-------------------- Start Complete ~~");




            Console.WriteLine("\r\n\r\n\r\n-------------------- Begin2 Complete ~~");
            Console.WriteLine("Main UI Thread ID : {0}", Thread.CurrentThread.ManagedThreadId);
            fun1Async();
            fun2Async();
            fun3Async();
            Console.WriteLine("-------------------- Start2 Complete ~~");
        }

        public async static Task fun1Async()
        {
            HttpClient webClient = new HttpClient();
            UriBuilder ub = new UriBuilder("http://www.vulcanlab.net/WindowsPhone/Hello.txt");
            Console.WriteLine("Fun1 before Await(1) : {0}", Thread.CurrentThread.ManagedThreadId);
            //HttpResponseMessage response = await webClient.GetAsync(ub.Uri);
            var response1 = webClient.GetAsync(ub.Uri);
            Console.WriteLine("Fun1 after Await(1) : {0}", Thread.CurrentThread.ManagedThreadId);
            var response = response1.Result;
            if (response != null)
            {
                String strResult = "";
                Console.WriteLine("Fun1 before Await(1) : {0}", Thread.CurrentThread.ManagedThreadId);
                strResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Fun1 after Await(2) : {0}", Thread.CurrentThread.ManagedThreadId);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Fun1 Async : {0}", strResult);
                }
            }
        }

        public async static Task fun2Async()
        {
            HttpClient webClient = new HttpClient();
            UriBuilder ub = new UriBuilder("http://www.vulcanlab.net/WindowsPhone/Hello2.txt");
            Console.WriteLine("Fun2 before Await(1) : {0}", Thread.CurrentThread.ManagedThreadId);
            HttpResponseMessage response = await webClient.GetAsync(ub.Uri);
            Console.WriteLine("Fun2 after Await(1) : {0}", Thread.CurrentThread.ManagedThreadId);
            if (response != null)
            {
                String strResult = "";
                Console.WriteLine("Fun2 before Await(2) : {0}", Thread.CurrentThread.ManagedThreadId);
                strResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Fun2 after Await(2) : {0}", Thread.CurrentThread.ManagedThreadId);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Fun2 Async : {0}", strResult);
                }
            }
        }

        public async static Task fun3Async()
        {
            HttpClient webClient = new HttpClient();
            UriBuilder ub = new UriBuilder("http://www.vulcanlab.net/WindowsPhone/Hello3.txt");
            Console.WriteLine("Fun3 before Await(1) : {0}", Thread.CurrentThread.ManagedThreadId);
            HttpResponseMessage response = await webClient.GetAsync(ub.Uri);
            Console.WriteLine("Fun3 after Await(1) : {0}", Thread.CurrentThread.ManagedThreadId);
            if (response != null)
            {
                String strResult = "";
                Console.WriteLine("Fun3 before Await(2) : {0}", Thread.CurrentThread.ManagedThreadId);
                strResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Fun3 after Await(2) : {0}", Thread.CurrentThread.ManagedThreadId);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Fun3 Async : {0}", strResult);
                }
            }
        }
    }
}
