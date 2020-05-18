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
    public class PartsAccessor
    {
        public List<string> returnAllPartTypes()
        {
            List<string> result = new List<string>();

            //Declare variables
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_return_all_part_types");

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
                    result.Add(reader.GetString(0));
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

            return result;
        }

        public bool addNewPart(string partType, string serialNumber, Decimal cost)
        {
            bool result = false;

            //Declare variables
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_add_new_part");

            //Setup cmd object
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Add parameters
            cmd.Parameters.Add("@PartType", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@SerialNumber", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@Cost", SqlDbType.Money);

            //Set parameter values
            cmd.Parameters["@PartType"].Value = partType;
            cmd.Parameters["@SerialNumber"].Value = serialNumber;
            cmd.Parameters["@Cost"].Value = cost;

            //Try to execute the query
            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                result = (1 == rows);
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

        public List<PartVM> returnAllParts()
        {
            List<PartVM> parts = new List<PartVM>();

            //Declare variables
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_return_all_parts");

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
                    string partType = reader.GetString(0);
                    string serialNumber = reader.GetString(1);
                    Decimal cost = reader.GetDecimal(2);
                    int lineItemID = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    PartVM part = new PartVM();
                    part.setSerialNumber(serialNumber);
                    part.setPartType(partType);
                    part.setCost(cost);
                    part.RepairLineItemID = lineItemID;
                    parts.Add(part);
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

            return parts;
        }

        public bool deletePart(string serialNumber)
        {
            bool result = false;

            //Declare variables
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_delete_part");

            //Setup cmd object
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Add parameters
            cmd.Parameters.Add("@SerialNumber", SqlDbType.NVarChar, 100);
            cmd.Parameters["@SerialNumber"].Value = serialNumber;

            //Try to execute the query
            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                result = (1 == rows);
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

        public bool isPartAvailable(string serialNumber, string partType)
        {
            bool result = false;

            //Declare variables
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_is_part_available");

            //Setup cmd object
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Add parameters
            cmd.Parameters.Add("@SerialNumber", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 50);

            //Set parameter values
            cmd.Parameters["@SerialNumber"].Value = serialNumber;
            cmd.Parameters["@Type"].Value = partType;

            //Try to execute the query
            try
            {
                conn.Open();
                int rows = (int)cmd.ExecuteScalar();
                result = (1 == rows);
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
    }
}
