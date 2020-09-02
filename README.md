# ServicoPublishSubscribe

Exemplo de microserviço em .NET core 3.1 que envia a mensagem "Hello World" a cada 5 segundos para uma fila do RabbitMQ no padrão publish/subscribe.

## Arquitetura utilizada

Para este projeto, foi utilizado o padrão de mensageria chamado publish/subscribe, que consiste em um ou mais produtores e um ou mais consumidores.
Maiores informações sobre este padrão podem ser obtidas através da Wikipedia (https://en.wikipedia.org/wiki/Publish%E2%80%93subscribe_pattern) ou diretamente do tutorial do site do RabbitMQ (https://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html) com exemplos de implementação em diversas linguagens de programação.

## Requisitos

.NET Core Runtime 3.1 ou superior  
Docker (testado com 2.3.0.4, pode suportar outras versões)

## Instruções

1 - Clonar o repositório  
```shell
git clone https://github.com/lucasbertho/ServicoPublishSubscribe.git  
cd ServicoPublishSubscribe
```

2 - Criar o container do projeto  
```shell
docker build -t servicopublishsubscribe:1.0 .  
```

3 - Executar o container do RabbitMQ  
```shell
docker run -d --hostname rabbitserver --name rabbitmq-server -p 15672:15672 -p 5672:5672 rabbitmq:3-management  
```

4 - Iniciar o container do projeto  
```shell
docker run -it --rm -d --network="host" --name sps servicopublishsubscribe:1.0  
```

5 - Visualizar log do container  
```shell
docker logs -f sps  
```
  
6 - Criar instâncias adicionais do serviço (opcional)  
```shell
docker run -it --rm -d --network="host" servicopublishsubscribe:1.0  
```

## Execução

1 - RabbitMQ  
O painel do RabbitMQ pode ser acessado através do endereço local através da porta 15672, assumindo que o container está sendo executado conforme as instruções acima.  

http://127.0.0.1:15672/  
Usuário: guest  
Senha: guest  

