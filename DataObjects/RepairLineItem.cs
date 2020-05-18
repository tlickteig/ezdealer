using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class RepairLineItem
    {
        public int RepairID { get; set; }
        public int RepairLineItemID { get; set; }
        public string SerialNumber { get; set; }
        public Decimal Amount { get; set; }
        public string Type { get; set; }

        public RepairLineItem(int repairID, int repairLineItemID, string serialNumber,
                    Decimal amount, string type)
        {
            this.RepairID = repairID;
            this.RepairLineItemID = repairLineItemID;
            this.SerialNumber = serialNumber;
            this.Amount = amount;
            this.Type = type;
        }

        public RepairLineItem()
        {
            this.RepairID = 0;
            this.RepairLineItemID = 0;
            this.SerialNumber = "";
            this.Amount = 0;
            this.Type = "COOLANT";
        }

        public string getPartType()
        {
            return this.Type;
        }

        public int getRepairID()
        {
            return this.RepairID;
        }

        public string getSerialNumber()
        {
            return this.SerialNumber;
        }

        public Decimal getAmount()
        {
            return this.Amount;
        }
    }
}
