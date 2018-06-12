using System;

namespace TicketApplication
{
    class Program
    {
        static void Main(string[] args)
        {
          var app = new AppBootstrapper();
          app.Start();
          Console.ReadLine();
        }
    }
}
