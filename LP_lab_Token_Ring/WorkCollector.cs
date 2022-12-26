using System.Threading.Channels;

namespace LP_lab_Token_Ring;

public class WorkCollector
{
    public static async Task TokenWorkInThreads(int thisThreadNumb, Channel<Token, Token> readFrom, Channel<Token> writeTo)
    {
        await readFrom.Reader.WaitToReadAsync();
        var startTime = DateTime.Now;
        var token = await readFrom.Reader.ReadAsync();
        Console.WriteLine(token.Recipient == thisThreadNumb
            ? $"I'm {thisThreadNumb} and I'h got {token.Message}"
            : $"I'm {thisThreadNumb} and it's not my token");
        await writeTo.Writer.WriteAsync(token);
        var endTime = DateTime.Now;
        if ((endTime - startTime).TotalSeconds > token.TimeOfLife)
            throw new Exception("Token life is over...");
    }
    
    public static async Task TokenWorkInTasks(int thisTaskNumb, Channel<Token, Token> readFrom, Channel<Token> writeTo)
    {
        await readFrom.Reader.WaitToReadAsync();
        
        var startTime = DateTime.Now;
        
        var token = await readFrom.Reader.ReadAsync();
        Console.WriteLine(token.Recipient == thisTaskNumb
            ? $"I'm {thisTaskNumb} and I'h got {token.Message}"
            : $"I'm {thisTaskNumb} and it's not my token");
        await writeTo.Writer.WriteAsync(token);
        
        var endTime = DateTime.Now;
        
        if ((endTime - startTime).TotalSeconds > token.TimeOfLife)
            throw new Exception("Token life is over...");
    }
}