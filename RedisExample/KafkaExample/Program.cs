using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace KafkaExample
{
    public class Program
    {
        static void Main(string[] args)
        {
            var options = new KafkaOptions(KafkaConfig.Broker1)
            {
                Log = new ConsoleLog()
            };

            Task.Run(() =>
            {
                var consumer = new Consumer(new ConsumerOptions(KafkaConfig.Topic, new BrokerRouter(options)) { Log = new ConsoleLog() });
                foreach (var data in consumer.Consume())
                {
                    Console.WriteLine("Response: PartitionId={0},Offset={1} :Value={2}", data.Meta.PartitionId, data.Meta.Offset, data.Value.ToUtf8String());
                }
            });
        }
    }
}
