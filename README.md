# LogicThread
A source code of LogicThread

## Usuage
```cs
LogicThread g_Thread = new LogicThread("LogicThred");
g_Thread.AddWork(() => { Console.WriteLine("I'm Working"); }, 10, "Except.log");
g_Thread.Run(10);
```

## ETC
AddWork( Action, Cycle(Seconds), If there have error, the name of the file to write it to );
