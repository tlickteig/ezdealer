using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Part
    {
        public string PartType { get; set; }
        public string SerialNumber { get; set; }
        public Decimal Cost { get; set; }

        public Part(string partType, string serialNumber, Decimal cost)
        {
            this.PartType = partType;
            this.SerialNumber = serialNumber;
            this.Cost = cost;
        }

        public Part()
        {
            this.PartType = "";
            this.SerialNumber = "";
            this.Cost = 0;
        }

        public void setCost(Decimal cost)
        {
            this.Cost = cost;
        }

        public void setSerialNumber(string serialNumber)
        {
            this.SerialNumber = serialNumber;
        }

        public void setPartType(string partType)
        {
            this.PartType = partType;
        }

        public string getSerialNumber()
        {
            return this.SerialNumber;
        }

        public string getPartType()
        {
            return this.PartType;
        }

        public Decimal getPartCost()
        {
            return this.Cost;
        }

        public string getPartName()
        {
            string partType = this.PartType;
            partType = partType.Substring(0, 1) + partType.Substring(1, partType.Length - 1).ToLower();
            partType.Replace("_", " ");
            return partType + " " + this.SerialNumber;
        }
    }
}
