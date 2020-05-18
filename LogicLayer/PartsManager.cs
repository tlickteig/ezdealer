using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class PartsManager
    {
        private PartsAccessor accessor = new PartsAccessor();

        public List<string> returnAllPartTypes()
        {
            List<string> parts = new List<string>();

            try
            {
                parts = accessor.returnAllPartTypes();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return parts;
        }

        public void addNewPart(Part part)
        {
            string serialNumber = part.getSerialNumber();
            string partType = part.getPartType();
            Decimal cost = part.getPartCost();

            try
            {
                accessor.addNewPart(partType, serialNumber, cost);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PartVM> getAllParts()
        {
            List<PartVM> parts = new List<PartVM>();

            try
            {
                parts = accessor.returnAllParts();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return parts;
        }

        public void deletePart(string serialNumber)
        {
            try
            {
                accessor.deletePart(serialNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool isPartAvailable(string serialNumber, string partType)
        {
            bool result;

            try
            {
                result = accessor.isPartAvailable(serialNumber, partType);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
