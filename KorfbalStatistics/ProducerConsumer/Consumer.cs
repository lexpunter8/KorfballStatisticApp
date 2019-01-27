using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace KorfbalStatistics.ProducerConsumer
{
    public class Consumer
    {
        public Consumer(Queue<Action> queue)
        {
            myActionsToDo = queue;

            ConsumeLoopThread = new Thread(() => ConsumeLoop());
            if (myActionsToDo.Count > 0)
                ConsumeLoopThread.Start();
        }
        private Queue<Action> myActionsToDo;
        private Thread ConsumeLoopThread;

        public void AddAction(Action action)
        {
            myActionsToDo.Enqueue(action);

            if (!ConsumeLoopThread.IsAlive)
            {
                ConsumeLoopThread = new Thread(() => ConsumeLoop());
                ConsumeLoopThread.Start();
            }

        }
        public bool IsOnline()
        {
            var cm = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            NetworkInfo networkInfo = cm.ActiveNetworkInfo;
            return networkInfo != null && networkInfo.IsConnected;
        }

        private void ConsumeLoop()
        {
            while (true)
            {
                if (!IsOnline())
                    continue;
                bool actionSucces = false;
                if (myActionsToDo.Count == 0)
                {
                    break;
                }
                try
                {
                    var item = myActionsToDo.Peek();
                    item.Invoke();
                    actionSucces = true;

                } catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if (actionSucces)
                    myActionsToDo.Dequeue();
            }
        }
    }
}