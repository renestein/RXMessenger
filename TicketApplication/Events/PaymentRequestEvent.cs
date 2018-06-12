namespace TicketApplication.Events
{
  public class PaymentRequestEvent
  {
    public PaymentRequestEvent(int price)
    {
      Price = price;
    }

    public int Price
    {
      get;
      private set;
    }
  }
}