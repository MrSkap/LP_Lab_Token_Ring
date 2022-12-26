namespace LP_lab_Token_Ring;

public class Token
{
    public string Message { get; set; } = "Token message";
    public int Recipient { get; set; }
    public int TimeOfLife { get; set; }
}