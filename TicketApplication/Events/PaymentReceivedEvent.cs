namespace TicketApplication.Events
{
  public class PaymentReceivedEvent
  {
    public PaymentReceivedEvent(int expectedAmount, int receivedAmount)
    {
      ExpectedAmount = expectedAmount;
      ReceivedAmount = receivedAmount;
    }

    public int ExpectedAmount
    {
      get;
    }

    public int ReceivedAmount
    {
      get;
    }
  }
}