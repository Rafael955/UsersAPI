using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Infra.Messages.Helpers;
using UsersAPI.Infra.Messages.Models;
using UsersAPI.Infra.Messages.Settings;

namespace UsersAPI.Infra.Messages.Consumers
{
    public class MessageConsumer : BackgroundService
    {
        private readonly RabbitMQSettings _rabbitMQSettings = new RabbitMQSettings();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //configurando a conexão com o servidor de mensageria
            var connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitMQSettings.Host, //caminho do servidor
                Port = _rabbitMQSettings.Port, //porta para conexão
                UserName = _rabbitMQSettings.User, //nome do usuário
                Password = _rabbitMQSettings.Password, //senha do usuário
                VirtualHost = _rabbitMQSettings.VirtualHost //caminho virtual
            };

            //abrindo conexão com o servidor da mensageria
            var connection = connectionFactory.CreateConnection();

            //acessando a fila
            var model = connection.CreateModel();
            model.QueueDeclare(
                queue: _rabbitMQSettings.Queue, //nome da fila
                durable: true, //fila não será excliuda automaticamente
                exclusive: false,//fila compartilhada
                autoDelete: false,//fila não exclui registros automaticamente
                arguments: null
                );

            //configurando uma rotina para ler cada registro contido na mensageria
            var consumer = new EventingBasicConsumer(model);

            //RECEIVED: Ler cada registro obtido da fila
            consumer.Received += (sender, args) =>
            {
                //ler o campo Payload do RabbitMQ
                var payload = args.Body.ToArray(); //formato bytes

                //converter para string (json)
                var json = Encoding.UTF8.GetString(payload);

                //deserializando os dados de JSON para objeto
                var registeredUser = JsonConvert.DeserializeObject<RegisteredUser>(json);

                //enviando o email para o usuário
                var mailHelper = new MailHelper();
                mailHelper.SendMail(registeredUser);

                //retirar o registro da fila
                model.BasicAck(args.DeliveryTag, false);
            };

            //executando e finalizando a leitura..
            model.BasicConsume(
                queue: _rabbitMQSettings.Queue,
                consumer: consumer,
                autoAck: false
                );
        }
    }
}
