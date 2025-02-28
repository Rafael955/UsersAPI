using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Infra.Messages.Models;
using UsersAPI.Infra.Messages.Settings;

namespace UsersAPI.Infra.Messages.Producers
{
    public class MessageProducer
    {
        private readonly RabbitMQSettings _rabbitMQSettings = new RabbitMQSettings();

        public void SendMessage(RegisteredUser user)
        {
            //configurando a conexão com o servidor de mensageria
            var connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitMQSettings.Host,
                Port = _rabbitMQSettings.Port,
                UserName = _rabbitMQSettings.User,
                Password = _rabbitMQSettings.Password,
                VirtualHost = _rabbitMQSettings.VirtualHost
            };

            //abrindo conexão com o servidor da mensageria
            using(var connection = connectionFactory.CreateConnection())
            {
                //criando a fila
                using(var model = connection.CreateModel())
                {
                    //construindo / conectando na fila
                    model.QueueDeclare(
                        queue: _rabbitMQSettings.Queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    //serializando os dados em JSON
                    var json = JsonConvert.SerializeObject(user);

                    //gravar os dados do usuário na fila
                    model.BasicPublish(
                        exchange: string.Empty,
                        routingKey: _rabbitMQSettings.Queue,
                        body: Encoding.UTF8.GetBytes(json),
                        basicProperties: null
                        );
                }
            }
        }
    }
}
