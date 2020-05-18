using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class BillingLineItem
    {
        public int BillingLineItemID { get; set; }
        public Decimal Amount { get; set; }

        public BillingLineItem()
        {

        }

        public BillingLineItem(int billingLineItemID, Decimal amount)
        {
            this.BillingLineItemID = billingLineItemID;
            this.Amount = amount;
        }

        public BillingLineItem(Decimal amount)
        {
            this.BillingLineItemID = 0;
            this.Amount = amount;
        }

        public int getLineItemID()
        {
            return this.BillingLineItemID;
        }

        public Decimal getAmount()
        {
            return this.Amount;
        }
    }
}
