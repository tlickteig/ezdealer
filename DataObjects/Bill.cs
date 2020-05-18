using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Bill
    {
        public int BillingID { get; set; }
        public int CustomerID { get; set; } 
        public List<BillingLineItem> LineItems { get; set; }
        public Decimal AmountDue { get; set; }
        public Decimal AmountPaid { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        public Bill()
        {

        }

        public Bill(int billingID, int customerID, Decimal amountDue, 
                Decimal amountPaid, DateTime issueDate, 
                DateTime dueDate)
        {
            this.BillingID = billingID;
            this.AmountDue = amountDue;
            this.IssueDate = issueDate;
            this.DueDate = dueDate;
            this.CustomerID = customerID;
            this.LineItems = new List<BillingLineItem>();
        }

        public Bill(int billingID, int customerID, Decimal amountDue,
                Decimal amountPaid, DateTime issueDate,
                DateTime dueDate, List<BillingLineItem> billingLineItems)
        {
            this.BillingID = billingID;
            this.AmountDue = amountDue;
            this.IssueDate = issueDate;
            this.DueDate = dueDate;
            this.CustomerID = customerID;
            this.LineItems = billingLineItems;
        }

        public int getBillingID()
        {
            return this.BillingID;
        }

        public Decimal getAmountDue()
        {
            return this.AmountDue;
        }

        public int getCustomerID()
        {
            return this.CustomerID;
        }

        public Decimal getAmountPaid()
        {
            return this.AmountPaid;
        }

        public DateTime getIssueDate()
        {
            return this.IssueDate;
        }

        public DateTime getDueDate()
        {
            return this.DueDate;
        }

        public List<BillingLineItem> getLineItems()
        {
            return this.LineItems;
        }

        public void setLineItems(List<BillingLineItem> lineItems)
        {
            this.LineItems = lineItems;
        }
    }
}
