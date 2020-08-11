using QuickFix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FIXMatchingServer
{
    public class FIXMatchingApplication : IApplication
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private OrderNoGenerator onGenerator = new OrderNoGenerator();

        private Queue<Message> sendMessageQueue = new Queue<Message>();
        private ManualResetEvent eventSendMessage = new ManualResetEvent(false);
        private bool isAbortSendMessage = false;

        private SessionID MySessionID { get; set; }

        public FIXMatchingApplication()
        {
            Thread tSendMessage = new Thread(new ThreadStart(SendMessageThread));
            tSendMessage.IsBackground = true;
            tSendMessage.Start();
        }

        public void FromAdmin(Message message, SessionID sessionID)
        {
            //throw new NotImplementedException();
        }

        public void FromApp(Message message, SessionID sessionID)
        {
            //throw new NotImplementedException();
            Console.WriteLine("FromApp:" + message );
            Logger.Info("FromApp:" + message);

            string tag35 = message.Header.GetString(QuickFix.Fields.Tags.MsgType);
            
            switch( tag35)
            {
                case "D":
                    Order o = new Order(message);
                    o.OrderNum = onGenerator.GetOrderNo();
                    Message mResponse = o.GetPendingNewMessage();
                    Logger.Info("OUT: " + mResponse.ToString());
                    SendMessage(mResponse);
                    break;
            }            
        }

        public void OnCreate(SessionID sessionID)
        {
            //throw new NotImplementedException();
            MySessionID = sessionID;
            Logger.Info("Session connected, " + sessionID.ToString());
        }

        public void OnLogon(SessionID sessionID)
        {
            //throw new NotImplementedException();
        }

        public void OnLogout(SessionID sessionID)
        {
            //throw new NotImplementedException();
        }

        public void ToAdmin(Message message, SessionID sessionID)
        {
            //throw new NotImplementedException();
        }

        public void ToApp(Message message, SessionID sessionID)
        {
            //throw new NotImplementedException();
            Console.WriteLine("ToApp:" + message.ToString() );
        }

        private void SendMessageThread()
        {
            while( eventSendMessage.WaitOne())
            {
                while (true)
                {
                    Message m = null;
                    lock (sendMessageQueue)
                    {
                        if (sendMessageQueue.Count > 0)
                        {
                            m = sendMessageQueue.Dequeue();
                        }
                    }
                    if (m != null)
                    {
                        // send
                        Logger.Info("OUT: " + m.ToString());

                        Session.SendToTarget(m, MySessionID);                        
                    }
                    else
                    {
                        break;
                    }
                }

                if( isAbortSendMessage)
                {
                    break;
                }
            }
        }

        private void SendMessage( Message m)
        {
            lock( sendMessageQueue)
            {
                sendMessageQueue.Enqueue(m);
            }
            eventSendMessage.Set();
        }
    }
}
