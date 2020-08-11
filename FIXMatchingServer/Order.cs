using QuickFix;
using QuickFix.Fields;
//using QuickFix.FIX44;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIXMatchingServer
{
    public class Order
    {
        public string Account { get; set; }
        public string Symbol { get; set; }
        public string ClOrderID { get; set; }
        public string Side { get; set; }
        public decimal OrderQty { get; set; }
        public string Price { get; set; }
        public string OrderType { get; set; }
        public string TimeInForce { get; set; }
        public string OrderNum { get; set; }
        public Order( Message message)
        {            
            Account = Message.GetFieldOrDefault(message, 1, "");
            Symbol = Message.GetFieldOrDefault(message, 55, "");
            ClOrderID = Message.GetFieldOrDefault(message,11, "");
            Side = Message.GetFieldOrDefault(message,54, "0");
            string tmp = Message.GetFieldOrDefault(message, 38, "0");
            decimal tmpDecimal = 0.0m;
            if( Decimal.TryParse(tmp, out tmpDecimal))
            {
                OrderQty = tmpDecimal;
            }

            Price = Message.GetFieldOrDefault(message, 44, "0");
            OrderType = Message.GetFieldOrDefault(message, 40, "1");
            TimeInForce = Message.GetFieldOrDefault(message, 59, "1");
        }

        public Message GetPendingNewMessage()
        {
            //ExecutionReport a = new QuickFix.ExecutionReport();
            //a.OrderID.setValue( OrderNum );

            Message message = new QuickFix.FIX44.ExecutionReport();            
            message.Header.SetField(new QuickFix.Fields.MsgType("8"));            
            message.SetField(new QuickFix.Fields.OrderID(OrderNum));
            message.SetField(new QuickFix.Fields.ClOrdID(ClOrderID));
            message.SetField(new QuickFix.Fields.ExecID("0"));
            message.SetField(new QuickFix.Fields.ExecType('A'));
            message.SetField(new QuickFix.Fields.OrdStatus('A'));            
            //message.SetField(new QuickFix.Fields.ExecTransType('0')) ;
            message.SetField(new QuickFix.Fields.Symbol(Symbol));
            message.SetField(new QuickFix.Fields.Side(Side[0]));
            message.SetField(new QuickFix.Fields.LeavesQty(OrderQty));
            message.SetField(new QuickFix.Fields.CumQty(0.0m));
            message.SetField(new QuickFix.Fields.AvgPx(0.0m));

            return message;
        }
    }
}
