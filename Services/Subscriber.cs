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
    class Subscriber
    {
        private Guid _microServiceGuid;

        private ConnectionFactory factory = new ConnectionFactory();
        private IModel channel;
        private EventingBasicConsumer consumer;

        private Random random = new Random();

        public Subscriber(Guid microServiceGuid)
        {
            _microServiceGuid = microServiceGuid;
        }

        // Configura conexão do subscriber com o message broker
        public bool SetupSubscriber()
        {
            try
            {
                // Conecta ao message broker
                var factory = new ConnectionFactory() { HostName = "localhost" };
                var connection = factory.CreateConnection();
                channel = connection.CreateModel();

                Log.Information("Criando exchange...");

                // Cria exchange para publicação das mensagens
                channel.ExchangeDeclare(exchange: "exchange",

                                        type: ExchangeType.Fanout);

                Log.Information("Exchange criada com sucesso.");
                Log.Information("Criando fila...");

                // Cria fila
                var queueName = channel
                                .QueueDeclare()
                                .QueueName;

                Log.Information("Fila criada com sucesso.");
                Log.Information("Vinculando fila à exchange...");

                // Vincula fila à exchange
                channel.QueueBind(queue: queueName,
                                    exchange: "exchange",
                                    routingKey: "");

                Log.Information("Fila vinculada com sucesso.");

                // Vincula procedimento para processar mensagem do message broker 
                consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    ProcessMessage(model, ea); 
                };

                // Recebe mensagem
                channel.BasicConsume(queue: queueName,
                                        autoAck: true,
                                        consumer: consumer);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Erro ao enviar requisição: " + ex.Message);
                return false;
            }
        }

        // Processa mensagem e exibe dados no console
        public void ProcessMessage(object? model, BasicDeliverEventArgs ea)
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var data = System.Text.Json.JsonSerializer.Deserialize<Data>(message);

                //// Processa apenas mensagens de outros micro-serviços
                //if (data.MicroServiceGuid == _microServiceGuid)
                //    return;

                message = String.Format("Micro-serviço: {0} Id: {1}\tMensagem: '{2}' Envio: {3}",
                            data.MicroServiceGuid,
                            data.Id,
                            data.Value,
                            data.TimeStamp.ToString("dd/MM/yyyy HH:mm:ss"));

                Log.Information(message);
            }
            catch (Exception ex)
            {
                Log.Error("Erro ao processar requisição: " + ex.Message);
            }
        }
    }
}
