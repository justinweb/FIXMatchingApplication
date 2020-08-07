using QuickFix;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIXMatchingServer
{
    class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            string cfgFile = ConfigurationManager.AppSettings["Configure"].ToString();
            SessionSettings settings = new SessionSettings( cfgFile );
            IApplication app = new FIXMatchingApplication();
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
            ILogFactory logFactory = new FileLogFactory(settings);
            IAcceptor acceptor = new ThreadedSocketAcceptor(app, storeFactory, settings, logFactory);

            acceptor.Start();
            Console.WriteLine("press <enter> to quit");

            Logger.Info("Start");

            Console.Read();
            acceptor.Stop();

            Logger.Info("Stop");

            NLog.LogManager.Shutdown();
        }

        static void TestGeneratorOrderNo()
        {
            OrderNoGenerator onGen = new OrderNoGenerator();

            for( int i = 0; i < 100000; ++i)
            {
                string orderNo = onGen.GetOrderNo();

                Logger.Info(string.Format("{0:000000} {1}", i, orderNo));
            }
        }
    }
}
