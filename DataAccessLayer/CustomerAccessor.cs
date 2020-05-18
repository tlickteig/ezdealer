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
    public class CustomerAccessor
    {
        public bool addNewCustomer(string firstName, string lastName, 
            string address, string emailAddress, string phoneNumber)
        {
            bool result = false;

            //Declare variables
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_new_customer");

            //Setup cmd object
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Add parameters
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 75);
            cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 11);

            //Set parameter values
            cmd.Parameters["@FirstName"].Value = firstName;
            cmd.Parameters["@LastName"].Value = lastName;
            cmd.Parameters["@Address"].Value = address;
            cmd.Parameters["@EmailAddress"].Value = emailAddress;
            cmd.Parameters["@PhoneNumber"].Value = phoneNumber;

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

        public List<Customer> returnAllCustomers()
        {
            //Declare variables
            List<Customer> customers = new List<Customer>();
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_return_all_customers");

            //Setup cmd object
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Attempt to execute the query
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string firstName = reader.GetString(0);
                    string lastName = reader.GetString(1);
                    string address = reader.GetString(2);
                    int ID = reader.GetInt32(3);
                    string email = reader.GetString(4);
                    string phoneNumber = reader.GetString(5);
                    Customer cust = new Customer(firstName, lastName, address, 
                        ID, email, phoneNumber);
                    customers.Add(cust);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return customers;
        }

        public int returnCustomerIDByCustomerName(string firstName, string lastName)
        {
            int result = 0;

            //Declare variables
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_return_customer_id_by_name");

            //Setup cmd object
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Add parameters
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50);

            //Set parameter values
            cmd.Parameters["@FirstName"].Value = firstName;
            cmd.Parameters["@LastName"].Value = lastName;

            //Try to execute the query
            try
            {
                conn.Open();
                result = (int)cmd.ExecuteScalar();
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

        public Customer returnCustomerByID(int customerID)
        {
            //Declare variables
            Customer customer = new Customer();
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_return_customer_by_id");

            //Setup cmd object
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Add parameter
            cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
            cmd.Parameters["@CustomerID"].Value = customerID;

            //Try to execute the query
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string firstName = reader.GetString(0);
                    string lastName = reader.GetString(1);
                    string address = reader.GetString(2);
                    string email = reader.GetString(3);
                    string phoneNumber = reader.GetString(4);
                    customer = new Customer(firstName, lastName, address, customerID, email, phoneNumber);
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

            return customer;
        }
    }
}
