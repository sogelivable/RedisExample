using System;
using System.Linq;
using Common;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace KafkaProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new KafkaOptions(KafkaConfig.Broker1)
            {
                Log = new ConsoleLog()
            };

            var router = new BrokerRouter(options);
            var producer = new Producer(router)
            {
                BatchSize = 100,
                BatchDelayTime = TimeSpan.FromMilliseconds(2000)
            }; ;

            Console.WriteLine("打出一条消息按 enter...");
            while (true)
            {
                var message = Console.ReadLine();
                if (message == "quit") break;

                if (string.IsNullOrEmpty(message))
                {
                    //
                    SendRandomBatch(producer, KafkaConfig.Topic, 200);
                }
                else
                {
                    producer.SendMessageAsync(KafkaConfig.Topic, new[] { new Message(message) });
                }
            }

            //释放资源
            using (producer)
            {

            }
        }

        private static async void SendRandomBatch(Producer producer, string topicName, int count)
        {
            //发送多个消息
            var sendTask = producer.SendMessageAsync(topicName, Enumerable.Range(0, count).Select(x => new Message(x.ToString())));

            Console.WriteLine("传送了 #{0} messages.  Buffered:{1} AsyncCount:{2}", count, producer.BufferCount, producer.AsyncCount);

            var response = await sendTask;

            Console.WriteLine("已完成批量发送: {0}. Buffered:{1} AsyncCount:{2}", count, producer.BufferCount, producer.AsyncCount);
            foreach (var result in response.OrderBy(x => x.PartitionId))
            {
                Console.WriteLine("主题:{0} PartitionId:{1} Offset:{2}", result.Topic, result.PartitionId, result.Offset);
            }

        }
    }
}
