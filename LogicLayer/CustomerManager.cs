using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class CustomerManager
    {
        private CustomerAccessor accessor = new CustomerAccessor();

        public bool addNewCustomer(Customer customer)
        {
            bool result = false;

            try
            {
                result = accessor.addNewCustomer(customer.getFirstName(), customer.getLastName(),
                    customer.getAddress(), customer.getEmailAddress(), customer.getPhoneNumber());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<Customer> returnAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                customers = accessor.returnAllCustomers();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return customers;
        }

        public int getCustomerIDByCustomerName(string firstName, string lastName)
        {
            int result = 0;

            try
            {
                result = accessor.returnCustomerIDByCustomerName(firstName, lastName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public Customer getCustomerByID(int customerID)
        {
            Customer customer;

            try
            {
                customer = accessor.returnCustomerByID(customerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return customer;
        }
    }
}
