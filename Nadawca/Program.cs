using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Nadawca
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Wyslane");
            Console.WriteLine("Nacisnij przycisk");
            Console.ReadKey();

            string queueName = "queueZadanie3";

            var factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.HostName = "localhost";
            factory.VirtualHost = "/";

            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare(queue: queueName,
                                      durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                      arguments: null);

            var channelProperties = channel.CreateBasicProperties();
            channelProperties.Headers = new Dictionary<string, object>();
            {
                channelProperties.Headers.Add("queueName", queueName);
                channelProperties.Headers.Add("Autor", "Bartosz Kolakowski");
            }
            for (int i = 1; i <= 10; i++)
            {
                string message = $"Wyslano {i}";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                        routingKey: queueName,
                                        basicProperties: channelProperties,
                                        body: body);

                Console.WriteLine($"Wyslano: '{message}'");
            }

            Console.WriteLine("Nacisnij przycisk");
            Console.ReadKey();

            channel.Close();
            conn.Close();
        }
    }
}