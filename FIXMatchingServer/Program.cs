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
            Console.Read();
            acceptor.Stop();
        }
    }
}
