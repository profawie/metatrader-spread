using log4net;
using MT4Price.Server.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Configuration;
using System.Text;
using System.Text.Json;

namespace MT4Price.Server
{
    class RabbitmqService
    {
        private IConnection connection = null;
        private IModel channel = null;
        private string accountCode = null;
        public string MQNAME_ACTION
        { get; set; }

        public string MQNAME_QUEUE
        { get; set; }

        private static readonly ILog log = LogManager.GetLogger(typeof(RabbitmqService));

        public Response Connect(string accountCode)
        {
            this.accountCode = accountCode;
            this.MQNAME_ACTION = "MT4Price_" + accountCode + "_Action";
            this.MQNAME_QUEUE  = "MT4Price_" + accountCode + "_Queue";
            Response response  = new Response();
            try
            {
                string rabbitmqHost = ConfigurationManager.AppSettings["RabbitMQ_Host"];
                int rabbitmqPort = int.Parse(ConfigurationManager.AppSettings["RabbitMQ_Port"]);
                string rabbitmqUsername = ConfigurationManager.AppSettings["RabbitMQ_Username"];
                string rabbitmqPassword = ConfigurationManager.AppSettings["RabbitMQ_Password"];
                ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = rabbitmqHost, Port = rabbitmqPort, UserName = rabbitmqUsername, Password = rabbitmqPassword };
                connection = connectionFactory.CreateConnection();
                channel = connection.CreateModel();

                //Declare if not existing
                channel.QueueDeclare(queue: MQNAME_ACTION,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                channel.QueueDeclare(queue: MQNAME_QUEUE,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                //Received (Action only)
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (ch, ea) =>
                {   
                    var byteArray = ea.Body.ToArray();
                    var routingKey = ea.RoutingKey;
                    string message = System.Text.Encoding.UTF8.GetString(byteArray);
                    //log.Debug(routingKey + " = " + message);
                    Received(message);
                };

                channel.BasicConsume(queue: MQNAME_ACTION,
                                autoAck: true,
                                consumer: consumer);

                response.Status = 1;
                response.Message = "";
                return response;
            }
            catch (Exception e)
            {
                response.Status = 0;
                response.Message = "Fail to connect RabbitMQ - " + e.Message;
                return response;
            }
        }

        public void Diconnected()
        {
            if(connection != null)
            {
                connection.Dispose();
                connection = null;
            }
        }

        public void Received(string message)
        {
            try
            {
                Cmd cmd = JsonSerializer.Deserialize<Cmd>(message);
                if(cmd.action == "Expired")
                {
                    log.Info(cmd.values);
                }
            }catch(Exception e)
            {
                log.Error(e.Message, e);
            }
            
        }

        public void Send(Price price)
        {
            if (channel == null)
            {
                return;
            }
            if (channel == null)
            {
                return;
            }

            //set account code
            price.accountCode = accountCode;
            price.platform = "MT4";
            string message = JsonSerializer.Serialize(price);
            var body = Encoding.UTF8.GetBytes(message);
            if (channel != null && channel.IsOpen)
            {
                channel.BasicPublish(exchange: "",
                                            routingKey: MQNAME_QUEUE,
                                            basicProperties: null,
                                            body: body);
            }
            log.Debug(price.symbol+ ", Ask: "+price.ask+", Bid: "+price.bid+", Spread: "+price.spread);
        }


        public void Disconnect()
        {
            try
            {
                if (channel != null)
                {
                    channel.Close();
                    channel = null;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
