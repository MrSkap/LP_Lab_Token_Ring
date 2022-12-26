using System.Threading.Channels;

namespace LP_lab_Token_Ring;

public static class ThreadManager
{
    private static readonly List<Channel<Token>> TokenChannel = new();
    private static readonly List<Thread> Threads = new();

    public static void TokenRing(int threadCount)
    {
        TokenChannel.Add(Channel.CreateBounded<Token>(new BoundedChannelOptions(1)));
        WriteMessage(TokenChannel[0], "Main thread is author of this message!", threadCount - 1, 1);

        try
        {
            for (var i = 1; i < threadCount; i++)
            {
                var threadNumb = i;
                TokenChannel.Add(Channel.CreateBounded<Token>(new BoundedChannelOptions(1)));
                var worker = new WorkCollector();

                async void Start()
                {
                    await WorkCollector.TokenWorkInThreads(threadNumb, TokenChannel[threadNumb - 1], TokenChannel[threadNumb]);
                }

                Threads.Add(new Thread(Start));
                Threads[i - 1].Start();
            }

            foreach (var th in Threads) th.Join();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
        
    }

    private static void WriteMessage(Channel<Token, Token> channel, string tokenMessage, int recipient, int timeOfLife)
    {
        channel.Writer.WriteAsync(new Token
        {
            Message = tokenMessage,
            Recipient = recipient,
            TimeOfLife = timeOfLife
        });
        Console.WriteLine($"I'm main thread and my message: {tokenMessage}");
    }
}