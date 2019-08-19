using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var conf = new ProducerConfig { BootstrapServers = "localhost:9092" };

            Action<DeliveryReport<Null, string>> handler = r =>
                Console.WriteLine(!r.Error.IsError
                    ? $"Delivered message to {r.TopicPartitionOffset}"
                    : $"Delivery Error: {r.Error.Reason}");

            using (var p = new ProducerBuilder<Null, string>(conf).Build())
            {
                for (int i = 0; i < 100; ++i)
                {
                    //Enter the name of the topic, and start producing 

                    p.Produce("TEST_TOPIC", new Message<Null, string> { Value = "Test " + i.ToString() }, handler);
                }

                // wait for up to 10 seconds for any inflight messages to be delivered.
                p.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}
