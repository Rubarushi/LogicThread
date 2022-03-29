using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadModule
{
    public class LogicThread
    {
        public class Node
        {
            public Action Action;
            public long TickCycle = 0;
            public long LastTick = DateTime.Now.Ticks;
            public string ExceptFile;

            public Node(Action action, long tick, string except = null)
            {
                Action = action;
                TickCycle = tick;
                ExceptFile = except;
            }
        }

        public LogicThread(string name)
        {
            _thread = new Thread(DoWork);
            _thread.Name = name;
        }

        private Thread _thread;

        private SynchronizedCollection<Node> Nodes = new SynchronizedCollection<Node>();

        private int m_iStopped = 0;

        private int sleeptime = 0;

        public void Run(int ms)
        {
            sleeptime = ms;
            _thread.Start();
        }

        public void AddWork(Action action, int second, string except = null)
        {
            Nodes.Add(new Node(action, second * 10000000, except));
        }

        public void Stop()
        {
            Interlocked.Increment(ref m_iStopped);
        }

        private void DoWork()
        {
            while(m_iStopped == 0)
            {
                for(int i = 0; i < Nodes.Count; i++)
                {
                    if(Nodes[i].TickCycle + Nodes[i].LastTick < DateTime.Now.Ticks)
                    {
                        try
                        {
                            Nodes[i].Action();
                        }
                        catch(Exception e)
                        {
                            Log.Exception(e);
                            if(Nodes[i].ExceptFile != null)
                            {
                                using( StreamWriter w = new StreamWriter(Nodes[i].ExceptFil, true ) )
                                {
                                     w.WriteLine( e.ToString() );
                                     w.WriteLine();
                                 }
                            }
                        }
                        Nodes[i].LastTick = DateTime.Now.Ticks;
                    }
                }

                Thread.Sleep(sleeptime);
            }
        }
    }
}
