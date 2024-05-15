using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Odbiorca");
        Console.WriteLine("Nacisnij przycisk");
        Console.ReadKey();
        string queueName = "queueZadanie3";
        var factory = new ConnectionFactory
        {
            UserName = "guest",
            Password = "guest",
            HostName = "localhost",
            VirtualHost = "/"
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                 autoDelete: false,
                                  arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Otrzymano: '{message}' od {queueName}");
                var headers = ea.BasicProperties.Headers;
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        Console.WriteLine($"Naglowek: {header.Key} = {Encoding.UTF8.GetString((byte[])header.Value)}");
                    }
                }
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine("Nacisnij przycisk");
            Console.ReadKey();
        }
    }
}