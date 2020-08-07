using QuickFix;
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
            Account = message.GetString(1);
            Symbol = message.GetString(55);
            ClOrderID = message.GetString(11);
            Side = message.GetString(54);
            string tmp = message.GetString(38);
            decimal tmpDecimal = 0.0m;
            if( Decimal.TryParse(tmp, out tmpDecimal))
            {
                OrderQty = tmpDecimal;
            }

            Price = message.GetString(44);
            OrderType = message.GetString(40);
            TimeInForce = message.GetString(59);
        }

        public Message GetPendingNewMessage()
        {
            //ExecutionReport a = new QuickFix.ExecutionReport();
            //a.OrderID.setValue( OrderNum );

            Message message = new Message();
            message.SetField(new QuickFix.Fields.BeginString("FIX 4.4"));
            message.SetField(new QuickFix.Fields.OrderID(OrderNum));
            message.SetField(new QuickFix.Fields.ClOrdID(ClOrderID));
            message.SetField(new QuickFix.Fields.ExecType('A'));
            message.SetField(new QuickFix.Fields.OrdStatus('A'));
            message.SetField(new QuickFix.Fields.ExecID(""));
            message.SetField(new QuickFix.Fields.ExecTransType());
            message.SetField(new QuickFix.Fields.Symbol(Symbol));
            message.SetField(new QuickFix.Fields.Side(Side[0]));
            message.SetField(new QuickFix.Fields.LeavesQty(OrderQty));
            message.SetField(new QuickFix.Fields.CumQty(0.0m));
            message.SetField(new QuickFix.Fields.AvgPx(0.0m));

            return message;
        }
    }
}
