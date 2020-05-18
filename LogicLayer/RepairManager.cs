using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class RepairManager
    {
        private RepairAccessor accessor = new RepairAccessor();

        public bool addNewRepair(Repair repair)
        {
            int result = 0;

            int employeeID = repair.getEmployeeID();
            string repairDescription = repair.getRepairDescription();
            int customerID = repair.getCustomerID();
            string vin = repair.getVIN();
            DateTime date = repair.getDate();
            List<RepairLineItem> lineItems = repair.GetLineItems();
            int billingLineItemID = repair.getbillingLineItemID();
            Decimal amount = repair.getAmount();

            try
            {
                result = accessor.addNewRepair(repairDescription, employeeID,
                    vin, customerID, date, billingLineItemID, amount);

                foreach (RepairLineItem lineItem in lineItems)
                {
                    string partType = lineItem.getPartType();
                    Decimal amount2 = lineItem.getAmount();
                    string serialNumber = lineItem.getSerialNumber();
                    accessor.addNewRepairLineItem(result, amount, partType, serialNumber);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result == 1;
        }

        public List<Repair> getAllRepairs()
        {
            List<Repair> repairs = new List<Repair>();
            List<RepairLineItem> lineItems = new List<RepairLineItem>();

            try
            {
                repairs = accessor.returnAllRepairs();                

                foreach (Repair repair in repairs)
                {
                    lineItems = accessor.GetRepairLineItems(repair.getRepairID());

                    foreach (RepairLineItem lineItem in lineItems)
                    {
                        repair.addRepairLineItem(lineItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return repairs;
        }
    }
}
