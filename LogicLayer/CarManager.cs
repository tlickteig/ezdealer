using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class CarManager
    {
        private CarAccessor _carAccessor;

        public CarManager()
        {
            _carAccessor = new CarAccessor();
        }

        public List<string> returnCarTypes()
        {
            List<string> types = new List<string>();
            
            try
            {
                types = _carAccessor.returnAllCarTypes();
            }
            catch (Exception ex)
            {
                throw ex;
            }            

            return types;
        }

        public List<string> returnMakes()
        {
            List<string> makes = new List<string>();

            try
            {
                makes = _carAccessor.returnAllMakes();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return makes;
        }

        public bool addCar(Car car, Decimal shipmentAmount, int employeeID, DateTime date)
        {
            bool result = false;

            string VIN = car.getVIN();
            string make = car.getMake();
            string model = car.getModel();
            int year = car.getYear();
            string color = car.getColor();
            Decimal msrp = car.getMSRP();
            int saleID = car.getSalesID();
            int shipmentID = car.getShipmentID();
            string carType = car.getCarType();
            bool isUsed = car.isUsed();
            int tradeInID = car.getTradeInID();

            try
            {
                result = _carAccessor.addCar(VIN, make, model, year, color, msrp, saleID, shipmentID, carType, isUsed, tradeInID);

                if (result)
                {
                    result = _carAccessor.addShipmentToCar(car.getVIN(), shipmentAmount, employeeID, date);
                }
                else
                {
                    throw new Exception("Wrong number of rows affected");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public bool addTradeIn(Car car, string tradeInDestination, int employeeID, DateTime date)
        {
            bool result = false;

            string VIN = car.getVIN();
            string make = car.getMake();
            string model = car.getModel();
            int year = car.getYear();
            string color = car.getColor();
            Decimal msrp = car.getMSRP();
            int saleID = car.getSalesID();
            int shipmentID = car.getShipmentID();
            string carType = car.getCarType();
            bool isUsed = car.isUsed();
            int tradeInID = car.getTradeInID();

            try
            {
                result = _carAccessor.addCar(VIN, make, model, year, color, msrp, saleID, shipmentID, carType, isUsed, tradeInID);

                if (result)
                {
                    result = _carAccessor.addTradeInToCar(car.getVIN(), tradeInDestination, employeeID, date);
                }
                else
                {
                    throw new Exception("Wrong number of rows affected");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<Car> returnAllCars()
        {
            List<Car> cars = new List<Car>();

            try
            {
                cars = _carAccessor.returnAllCars();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return cars;
        }
        public Decimal returnShipmentAmount(int tradeInID)
        {
            Decimal result = 0;

            try
            {
                result = _carAccessor.returnShipmentAmount(tradeInID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public string returnTradeInDestination(int tradeInID)
        {
            string result = " ";

            try
            {
                result = _carAccessor.getTradeInDestination(tradeInID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public Car getCarByVIN(string vin)
        {
            Car car = new Car();

            try
            {
                car = _carAccessor.getCarByVIN(vin);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return car;
        }
    }
}
