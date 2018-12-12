using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class KafkaConfig
    {
        public static readonly Uri Broker1 = new Uri("http://10.112.20.79:9092");
        public static readonly Uri Broker2 = new Uri("http://10.112.20.79:9093");
        public const string Topic = "TestYong";
    }
}
