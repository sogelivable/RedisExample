using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using StackExchange.Redis;

namespace RedisExample
{
    class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger("App");

        static void Main(string[] args)
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            string filePath = codeBase;
            if (codeBase.StartsWith(@"file:"))
            {
                filePath = new Uri(codeBase).LocalPath;
            }
            filePath = Path.GetDirectoryName(filePath);
            string fileName = Path.Combine(filePath, "log4net.config");
            if (fileName.StartsWith(@"file:\"))
            {
                fileName = fileName.Substring(@"file:\".Length);
            }

            XmlConfigurator.ConfigureAndWatch(new FileInfo(fileName));


            _logger.Info("Start..");

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            // ^^^ store and re-use this!!!

            //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("server1:6379,server2:6379");
            var db = redis.GetDatabase();

            //int databaseNumber = ...
            //object asyncState = ...
            //IDatabase db = redis.GetDatabase(databaseNumber, asyncState);

            string value1 = "abcdefg";
             db.StringSet("mykey", value1);

            string value2 =  db.StringGet("mykey");
            Console.WriteLine(value2); // writes: "abcdefg"

            //Note that the String... prefix here denotes the String redis type, and is largely separate to the .NET String type, although both can store text data. However, redis allows raw binary data for both keys and values
            byte[] key = Encoding.ASCII.GetBytes("123"), value = Encoding.ASCII.GetBytes("abc");
            db.StringSet(key, value);

            byte[] result = db.StringGet(key);

            IServer server = redis.GetServer("127.0.0.1", 6379);

            EndPoint[] endpoints = redis.GetEndPoints();

            DateTime lastSave = server.LastSave();
            //ClientInfo[] clients = server.ClientList();

            var number = 1;
            db.StringSet("number", number);
            db.StringIncrement("number", flags: CommandFlags.FireAndForget);
        }
    }
}
