using DataAccess.Utilities;
using DataModels;
using DataModels.Shared;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;

namespace DataAccess.Repository
{

    public interface ICustomerRepository
    {
        Task<ResponseViewModel<List<CustomerViewModel>>> GetCustomers();
        Task<ResponseViewModel<int>> InsertCustomer(CustomerViewModel request);
        Task<ResponseViewModel<CustomerViewModel>> SearchCustomerId(int Idcustomer);
    }
    public class CustomerRepository : ICustomerRepository
    {

        private readonly AppDb _db;
        public CustomerRepository(AppDb db)
        {
            _db = db;
        }

        /// <summary>
        /// Inserts new Customer
        /// It uses Invoke Stored procedure CUSTOMERS_INSERT_OUT_ID stored procedures
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The new Customer Id inserted</returns>
        public async Task<ResponseViewModel<int>> InsertCustomer(CustomerViewModel request)
        {
            int idNewCustomer = 0;
            HttpStatusCode code = HttpStatusCode.NotFound;
            ResponseViewModel<int> response = new ResponseViewModel<int>();

            try
            {
                using (var connection = _db.Connection)
                {
                    await connection.OpenAsync();
                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        using (var command = new MySqlCommand("CUSTOMERS_INSERT_OUT_ID", connection))
                        {
                            //WHEN USING TRANSACTION FROM HERE, WE DON'T WANT TO HAVE INTERNAL "START TRANSACTION;" AND "COMMIT;" STATEMENTS
                            //INSIDE THE STORED PROCEDURES, OTHERWISE, WE WILL NOT BE ABLE TO ROLLBACK THE CHANGES, AND IF WE SPECIFY A SAVEPOINT, THAT WILL ALSO BE DELETED.
                            command.Transaction = transaction;
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(new MySqlParameter("new_Name", request.Name));
                            MySqlParameter IdCustomerParam = new MySqlParameter("new_Id", MySqlDbType.Int32);
                            IdCustomerParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(IdCustomerParam);

                            try
                            {
                                await command.ExecuteNonQueryAsync();
                                idNewCustomer = (int)IdCustomerParam.Value;

                                if (idNewCustomer > 0)
                                {
                                    await transaction.CommitAsync();
                                    code = HttpStatusCode.OK;
                                }
                                else
                                {
                                    try
                                    {
                                        await transaction.RollbackAsync();
                                    }
                                    catch (Exception)
                                    {

                                    }
                                    if (idNewCustomer == -1)
                                    {
                                        code = HttpStatusCode.Conflict;
                                    }
                                    else
                                    {
                                        code = HttpStatusCode.InternalServerError;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    await transaction.RollbackAsync();
                                    code = HttpStatusCode.Conflict;
                                }
                                catch (Exception ex2)
                                {

                                }
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            response.StatusCode = code;
            response.responseObject = idNewCustomer;

            return response;
        }

        /// <summary>        
        /// Returns a list of all clients by calling the stored procedure CUSTOMER_RETURN_NAMES
        /// </summary>
        /// <returns>List of Customers</returns>
        public async Task<ResponseViewModel<List<CustomerViewModel>>> GetCustomers()
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            HttpStatusCode code = HttpStatusCode.NotFound;
            ResponseViewModel<List<CustomerViewModel>> response = new ResponseViewModel<List<CustomerViewModel>>();
            try
            {
                using (var connection = _db.Connection)
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand("CUSTOMER_RETURN_NAMES", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                CustomerViewModel customer = new CustomerViewModel();
                                customer.CustomerId = reader.GetValueOrDefault<int>("CustomerId", 0);
                                customer.Name = reader.GetValueOrDefault<string>("Name", "");
                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (customers.Count <= 0)
            {
                code = HttpStatusCode.NoContent;
            }
            else
            {
                code = HttpStatusCode.OK;
            }
            response.StatusCode = code;
            response.responseObject = customers;

            return response;
        }

        /// <summary>
        /// Returns information about a customer id by calling the stored procedure CUSTOMER_RETURN_DETAILS_BY_ID
        /// </summary>
        /// <param name="Idcustomer">id of the customer</param>
        /// <returns>CustomerViewModel</returns>
        public async Task<ResponseViewModel<CustomerViewModel>> SearchCustomerId(int Idcustomer)
        {
            ResponseViewModel<CustomerViewModel> response = new ResponseViewModel<CustomerViewModel>();
            CustomerViewModel customer = new CustomerViewModel();
            HttpStatusCode code = HttpStatusCode.NotFound;

            try
            {
                using (var connection = _db.Connection)
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand("CUSTOMER_RETURN_DETAILS_BY_ID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("NEW_ID", Idcustomer));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                customer.CustomerId = reader.GetValueOrDefault<int>("CustomerId", 0);
                                customer.Name = reader.GetValueOrDefault<string>("Name", "");

                                code = HttpStatusCode.OK;
                            }
                            else
                            {
                                code = HttpStatusCode.NotFound;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            response.StatusCode = code;
            response.responseObject = customer;
            return response;
        }

    }
}