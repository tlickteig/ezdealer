using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RepairAccessor
    {
        public int addNewRepair(string repairDescription, int employeeID, string vin, 
                            int customerID, DateTime date, int billingLineItemID, Decimal amount)
        {
            int result = 0;

            //Declare variables
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_new_repair");

            //Setup cmd object
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Add parameters
            cmd.Parameters.Add("@RepairDescription", SqlDbType.NVarChar, 500);
            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters.Add("@VIN", SqlDbType.NVarChar, 20);
            cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
            cmd.Parameters.Add("@Date", SqlDbType.Date);
            cmd.Parameters.Add("@BillingLineItemID", SqlDbType.Int);
            cmd.Parameters.Add("@Amount", SqlDbType.Int);

            //Set parameter values
            cmd.Parameters["@RepairDescription"].Value = repairDescription;
            cmd.Parameters["@EmployeeID"].Value = employeeID;
            cmd.Parameters["@VIN"].Value = vin;
            cmd.Parameters["@CustomerID"].Value = customerID;
            cmd.Parameters["@Date"].Value = date;
            cmd.Parameters["@BillingLineItemID"].Value = billingLineItemID;
            cmd.Parameters["@Amount"].Value = amount;

            //Try to execute the query
            try
            {
                conn.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public List<Repair> returnAllRepairs()
        {
            //Declare variables
            List<Repair> repairs = new List<Repair>();
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_return_all_repairs");

            //Setup cmd object
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Try to execute the query
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int repairID = reader.GetInt32(0);
                    string repairDescription = reader.GetString(1);
                    int employeeID = reader.GetInt32(2);
                    string vin = reader.GetString(3);
                    int customerID = reader.GetInt32(4);
                    Decimal amount = reader.GetDecimal(5);
                    DateTime date = reader.GetDateTime(6);
                    int billingLineItemID = reader.GetInt32(7);
                    Repair repair = new Repair(repairID, repairDescription, employeeID, 
                        customerID, date, billingLineItemID, vin);
                    repairs.Add(repair);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return repairs;
        }

        public bool addNewRepairLineItem(int repairID, Decimal amount, string partType, string serialNumber)
        {
            bool result = false;

            //Declare variables
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_new_repair_line_item");

            //Setup cmd object
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Add parameters
            cmd.Parameters.Add("@RepairID", SqlDbType.Int);
            cmd.Parameters.Add("@Amount", SqlDbType.Money);
            cmd.Parameters.Add("@PartType", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@SerialNumber", SqlDbType.NVarChar, 20);

            //Set parameter values
            cmd.Parameters["@RepairID"].Value = repairID;
            cmd.Parameters["@Amount"].Value = amount;
            cmd.Parameters["@PartType"].Value = partType;
            cmd.Parameters["@SerialNumber"].Value = serialNumber;

            //Try to execute the query
            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                result = (rows == 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public List<RepairLineItem> GetRepairLineItems(int repairID)
        {
            //Declare variables
            List<RepairLineItem> lineItems = new List<RepairLineItem>();
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_get_repair_line_items");

            //Setup cmd object
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Add parameter
            cmd.Parameters.Add("@RepairID", SqlDbType.Int);
            cmd.Parameters["@RepairID"].Value = repairID;

            //Try to execute the query
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int repairLineItemID = reader.GetInt32(1);
                    string serialNumber = reader.GetString(2);
                    Decimal amount = reader.GetDecimal(3);
                    string partType = reader.GetString(4);
                    RepairLineItem lineItem = new RepairLineItem(repairID, repairLineItemID, 
                        serialNumber, amount, partType);
                    lineItems.Add(lineItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return lineItems;
        }
    }
}
