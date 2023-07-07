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
    public interface ISaleRepository
    {
        Task<ResponseViewModel<List<SaleViewModel>>> SearchSalesByDateRange(SalesDateRangeRequest request);
        Task<ResponseViewModel<int>> InsertSale(SaleViewModel request);
    }
    public class SaleRepository : ISaleRepository
    {

        private readonly AppDb _db;
        public SaleRepository(AppDb db)
        {
            _db = db;
        }

        /// <summary>
        /// Inserts new Sale
        /// It uses Invoke Stored procedure SALES_INSERT_OUT_ID stored procedures
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The new Sale Id inserted</returns>
        public async Task<ResponseViewModel<int>> InsertSale(SaleViewModel request)
        {
            int idNewSale = 0;
            HttpStatusCode code = HttpStatusCode.NotFound;
            ResponseViewModel<int> response = new ResponseViewModel<int>();

            try
            {
                using (var connection = _db.Connection)
                {
                    await connection.OpenAsync();
                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        using (var command = new MySqlCommand("SALES_INSERT_OUT_ID", connection))
                        {
                            //WHEN USING TRANSACTION FROM HERE, WE DON'T WANT TO HAVE INTERNAL "START TRANSACTION;" AND "COMMIT;" STATEMENTS
                            //INSIDE THE STORED PROCEDURES, OTHERWISE, WE WILL NOT BE ABLE TO ROLLBACK THE CHANGES, AND IF WE SPECIFY A SAVEPOINT, THAT WILL ALSO BE DELETED.
                            command.Transaction = transaction;
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(new MySqlParameter("new_Date", request.Date));
                            command.Parameters.Add(new MySqlParameter("new_CustomerId", request.CustomerId));
                            command.Parameters.Add(new MySqlParameter("new_Total", request.Total));
                            MySqlParameter IdSaleParam = new MySqlParameter("new_Id", MySqlDbType.Int32);
                            IdSaleParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(IdSaleParam);

                            try
                            {
                                await command.ExecuteNonQueryAsync();
                                idNewSale = (int)IdSaleParam.Value;

                                if (idNewSale > 0)
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
                                    if (idNewSale == -1)
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
            response.responseObject = idNewSale;

            return response;
        }

        /// <summary>        
        /// Returns a list of all Sales by calling the stored procedure SALES_RETURN_DETAILS_BY_DATE_RANGE
        /// </summary>
        /// <returns>List of Sales</returns>
        public async Task<ResponseViewModel<List<SaleViewModel>>> SearchSalesByDateRange(SalesDateRangeRequest request)
        {
            List<SaleViewModel> sales = new List<SaleViewModel>();
            HttpStatusCode code = HttpStatusCode.NotFound;
            ResponseViewModel<List<SaleViewModel>> response = new ResponseViewModel<List<SaleViewModel>>();
            try
            {
                using (var connection = _db.Connection)
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand("SALES_RETURN_DETAILS_BY_DATE_RANGE", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("startDate", request.StartDate));
                        command.Parameters.Add(new MySqlParameter("endDate", request.EndDate));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                SaleViewModel sale = new SaleViewModel();
                                sale.SaleId = reader.GetValueOrDefault<int>("SaleId", 0);
                                sale.Date = reader.GetValueOrDefault<DateTime>("Date", DateTime.MinValue);
                                sale.CustomerId = reader.GetValueOrDefault<int>("CustomerId", 0);
                                sale.Total = reader.GetValueOrDefault<double>("Total", 0.0);
                                sales.Add(sale);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (sales.Count <= 0)
            {
                code = HttpStatusCode.NoContent;
            }
            else
            {
                code = HttpStatusCode.OK;
            }
            response.StatusCode = code;
            response.responseObject = sales;

            return response;
        }


    }
}