using System;
using RxMessenger;
using TicketApplication.Events;
using TicketApplication.InternalProcessors;

namespace TicketApplication.Ui
{
  public class Display : AppEventParticipant
  {
    private IDisposable _eventStream;

    public Display(Messenger messenger) : base(messenger)
    {
    }

    public override void Start()
    {
      base.Start();
      Console.WriteLine("Inicializace. Prosim cekejte...");
      _eventStream = Messenger.GetEventStream<Object>().Subscribe(e =>
      {
        switch (e)
        {
          case PaymentReceivedEvent pre:
          {
            Console.WriteLine($"Přijata platba. " +
                              $"Přijato: {pre.ReceivedAmount} " +
                              $"Celková částka:  {pre.ExpectedAmount}");
            break;
          }
          case RestartEvent re:
          {
            Console.WriteLine("Vyberte typ listku.");
            break;
          }
          case PrintTicketRequestEvent ptre:
          {
            Console.WriteLine("Tisknu listek...");
            break;
          }
          case PrintTicketResponseEvent printTicketResponseEvent:
          {
            Console.WriteLine("Dekujeme za zakoupeni listku.");
            break;
          }
        }
      });
    }

    public void CancelButtonPressed()
    {
      Messenger.PublishEvent(new CanceledEvent());
    }

    public void SelectTicketButtonPressed()
    {
      Messenger.PublishEvent(new TicketSelectedEvent());
    }

    public void PaymentButtonPressed()
    {
      Messenger.PublishEvent(new PaymentRequestEvent(PaymentProcessor.TICKET_PRICE));
    }

    protected override void ReleaseSubscriptions()
    {
      _eventStream.Dispose();
    }
  }
}