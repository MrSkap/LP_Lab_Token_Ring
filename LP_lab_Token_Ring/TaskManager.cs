using System.Threading.Channels;

namespace LP_lab_Token_Ring;

public class TaskManager
{
    public static async Task TokenRing(int tasksCount)
    {
        List<Task> tasks = new List<Task>();
        List<Channel<Token>> tokenChannels = new();
        tokenChannels.Add(Channel.CreateBounded<Token>(new BoundedChannelOptions(1)));
        await tokenChannels[0].Writer.WriteAsync(new Token()
        {
            Message = "This message is from main task",
            Recipient = tasksCount - 1,
            TimeOfLife = 1,
        });
        Console.WriteLine("I'm main task and my message: Main task is author of this message!");
        try
        {
            for (var i = 1; i < tasksCount; i++)
            {
                var taskNumb = i;
                tokenChannels.Add(Channel.CreateBounded<Token>(new BoundedChannelOptions(1)));
                tasks.Add(WorkCollector.TokenWorkInTasks(taskNumb, tokenChannels[taskNumb - 1], tokenChannels[taskNumb]));
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
        
    }
}