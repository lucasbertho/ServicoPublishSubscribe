using Microsoft.Extensions.Logging;
using Serilog;
using ServicoPublishSubscribe.Services;
using System;
using System.Threading;

namespace ServicoPublishSubscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configura log
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            // Gera identificador único para o micro-serviço
            Guid microServiceGuid = Guid.NewGuid();

            // Instancia publisher e subscriber
            Publisher publisher = new Publisher(microServiceGuid);
            Subscriber subscriber = new Subscriber(microServiceGuid);

            // Configura publisher e subscriber
            if (!publisher.SetupPublisher()            
            ||  !subscriber.SetupSubscriber())
                return;

            while (true)
            {
                // Publica mensagem
                publisher.SendMessage("Hello World");

                // Aguarda 5 segundos até enviar a próxima mensagem
                Thread.Sleep(5000);
            }
        }
    }
}
