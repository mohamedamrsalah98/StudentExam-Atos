using RabbitMQ.Client;


namespace StudentSystemService.RabbitMQ
{
    public interface IRabbitMQProducer
    {
        IModel Connect();

        public void SendProductMessage<T>(T message);

    }
}
