using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using ServicoProducerConsumer.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ServicoPublishSubscribe.Services
{
    public class Publisher
    {
        private Guid _microServiceGuid;

        private ConnectionFactory factory = new ConnectionFactory();
        private IModel channel;

        private Random random = new Random();

        public Publisher(Guid microServiceGuid)
        {
            _microServiceGuid = microServiceGuid;
        }

        // Configura conexão do publisher com o message broker
        public bool SetupPublisher()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                var connection = factory.CreateConnection();
                channel = connection.CreateModel();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Erro ao configurar publisher: " + ex.Message);
                return false;
            }
        }

        // Publica mensagem
        public void SendMessage(string value)
        {
            try
            {
                // Cria exchange para publicação das mensagens
                channel.ExchangeDeclare(exchange: "exchange",
                                        type: ExchangeType.Fanout);

                // Instancia dados
                Data data = new Data
                {
                    Id = random.Next(0, 1000000),
                    Value = value,
                    MicroServiceGuid = _microServiceGuid,
                    TimeStamp = System.DateTime.Now
                };

                // Serializa objeto
                var content = JsonSerializer.Serialize(data);

                // Envia mensagem para o message broker
                var body = Encoding.UTF8.GetBytes(content);
                channel.BasicPublish(exchange: "exchange",
                                        routingKey: "",
                                        basicProperties: null,
                                        body: body);
            }
            catch (Exception ex)
            {
                Log.Error("Erro ao enviar requisição: " + ex.Message);
            }
        }
    }
}
