using LP_lab_Token_Ring;

//Вариант с использование Thread
Console.WriteLine("In threads:");
ThreadManager.TokenRing(5);
await Task.Delay(1000);

//Вариант с использованием Task
Console.WriteLine("In tasks:");
await TaskManager.TokenRing(5);