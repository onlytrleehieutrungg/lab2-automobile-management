using System;
using System.Collections.Generic;
using System.Data;
using AutomobileLibrary.BussinessObject;
using Microsoft.Data.SqlClient;

namespace AutomobileLibrary.DataAccess
{
    public class CarDBContext : BaseDAL
    {
        private static CarDBContext instance = null;
        private static readonly object instanceLock = new object();
        private CarDBContext() { }

        public static CarDBContext Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CarDBContext();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Car> GetCarList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "Select CarID, CarName, Manufacturer,Price, ReleasedYear from Cars";
            var cars = new List<Car>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    cars.Add(new Car
                    {
                        CarID = dataReader.GetInt32(0),
                        CarName = dataReader.GetString(1),
                        Manufacturer = dataReader.GetString(2),
                        Price = dataReader.GetDecimal(3),
                        ReleasedYear = dataReader.GetInt32(4)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return cars;
        }

        public Car GetCarById(int carID)
        {
            Car car = null;
            IDataReader dataReader = null;
            string SQLSelect = "Select CarID, CarName, Manufacturer, Price, ReleasedYear "
                            + " From Cars where CarID = @CarID";

            try
            {
                var param = dataProvider.CreateParameter("@CarID", 4, carID, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    car = new Car
                    {
                        CarID = dataReader.GetInt32(0),
                        CarName = dataReader.GetString(1),
                        Manufacturer = dataReader.GetString(2),
                        Price = dataReader.GetDecimal(3),
                        ReleasedYear = dataReader.GetInt32(4)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return car;
        }

        public void AddNew(Car car)
        {
            try
            {
                Car pro = GetCarById(car.CarID);
                if (pro == null)
                {
                    string SQLInsert = "Insert Cars values(@CarID,@CarName,@Manufacturer,@Price,@ReleasedYear)";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@CarID", 4, car.CarID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@CarName", 50, car.CarName, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@CarID", 50, car.Manufacturer, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@CarID", 50, car.Price, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@CarID", 4, car.ReleasedYear, DbType.Int32));
                    dataProvider.Insert(SQLInsert, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("The car does not already exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void Update(Car car)
        {
            try
            {
                Car pro = GetCarById(car.CarID);
                if (pro != null)
                {
                    string SQLInsert = "Update Cars set CarName = @CarName,Manufacturer = @Manufacturer," +
                                                            "Price = @Price,ReleasedYear = @ReleasedYear where CarID=@CarID";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@CarID", 4, car.CarID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@CarName", 50, car.CarName, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@CarID", 50, car.Manufacturer, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@CarID", 50, car.Price, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@CarID", 4, car.ReleasedYear, DbType.Int32));
                    dataProvider.Update(SQLInsert, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("The car does not already exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void Remove(int carID)
        {
            try
            {
                Car car = GetCarById(carID);
                if (car != null)
                {
                    string SQLDelete = "Delete Cars where CarID = @CarID";
                    var param = dataProvider.CreateParameter("@CarID", 4, carID, DbType.Int32);
                    dataProvider.Delete(SQLDelete, CommandType.Text, param);

                }
                else
                {
                    throw new Exception("the car does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}

