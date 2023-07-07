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
    public interface IProductRepository
    {
        Task<ResponseViewModel<List<ProductViewModel>>> GetProducts();
        Task<ResponseViewModel<int>> InsertProduct(ProductViewModel request);
        Task<ResponseViewModel<ProductViewModel>> SearchProductId(int IdProduct);
    }
    public class ProductRepository : IProductRepository
    {

        private readonly AppDb _db;
        public ProductRepository(AppDb db)
        {
            _db = db;
        }

        /// <summary>
        /// Inserts new Product
        /// It uses Invoke Stored procedure PRODUCTS_INSERT_OUT_ID stored procedures
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The new Product Id inserted</returns>
        public async Task<ResponseViewModel<int>> InsertProduct(ProductViewModel request)
        {
            int idNewProduct = 0;
            HttpStatusCode code = HttpStatusCode.NotFound;
            ResponseViewModel<int> response = new ResponseViewModel<int>();

            try
            {
                using (var connection = _db.Connection)
                {
                    await connection.OpenAsync();
                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        using (var command = new MySqlCommand("PRODUCTS_INSERT_OUT_ID", connection))
                        {
                            //WHEN USING TRANSACTION FROM HERE, WE DON'T WANT TO HAVE INTERNAL "START TRANSACTION;" AND "COMMIT;" STATEMENTS
                            //INSIDE THE STORED PROCEDURES, OTHERWISE, WE WILL NOT BE ABLE TO ROLLBACK THE CHANGES, AND IF WE SPECIFY A SAVEPOINT, THAT WILL ALSO BE DELETED.
                            command.Transaction = transaction;
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(new MySqlParameter("new_Name", request.Name));
                            command.Parameters.Add(new MySqlParameter("new_UnitPrice", request.UnitPrice));
                            command.Parameters.Add(new MySqlParameter("new_Cost", request.Cost));
                            MySqlParameter IdProductParam = new MySqlParameter("new_Id", MySqlDbType.Int32);
                            IdProductParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(IdProductParam);

                            try
                            {
                                await command.ExecuteNonQueryAsync();
                                idNewProduct = (int)IdProductParam.Value;

                                if (idNewProduct > 0)
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
                                    if (idNewProduct == -1)
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
            response.responseObject = idNewProduct;

            return response;
        }

        /// <summary>        
        /// Returns a list of all products by calling the stored procedure PRODUCT_RETURN_NAMES
        /// </summary>
        /// <returns>List of Products</returns>
        public async Task<ResponseViewModel<List<ProductViewModel>>> GetProducts()
        {
            List<ProductViewModel> Products = new List<ProductViewModel>();
            HttpStatusCode code = HttpStatusCode.NotFound;
            ResponseViewModel<List<ProductViewModel>> response = new ResponseViewModel<List<ProductViewModel>>();
            try
            {
                using (var connection = _db.Connection)
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand("PRODUCT_RETURN_NAMES", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ProductViewModel Product = new ProductViewModel();
                                Product.ProductId = reader.GetValueOrDefault<int>("ProductId", 0);
                                Product.Name = reader.GetValueOrDefault<string>("Name", "");
                                Product.UnitPrice = reader.GetValueOrDefault<string>("UnitPrice", "");
                                Product.Cost = reader.GetValueOrDefault<double>("Cost", 0.0);
                                Products.Add(Product);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (Products.Count <= 0)
            {
                code = HttpStatusCode.NoContent;
            }
            else
            {
                code = HttpStatusCode.OK;
            }
            response.StatusCode = code;
            response.responseObject = Products;

            return response;
        }

        /// <summary>
        /// Returns information about a Product id by calling the stored procedure PRODUCT_RETURN_DETAILS_BY_ID
        /// </summary>
        /// <param name="IdProduct">id of the Product</param>
        /// <returns>ProductViewModel</returns>
        public async Task<ResponseViewModel<ProductViewModel>> SearchProductId(int IdProduct)
        {
            ResponseViewModel<ProductViewModel> response = new ResponseViewModel<ProductViewModel>();
            ProductViewModel Product = new ProductViewModel();
            HttpStatusCode code = HttpStatusCode.NotFound;

            try
            {
                using (var connection = _db.Connection)
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand("PRODUCT_RETURN_DETAILS_BY_ID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("NEW_ID", IdProduct));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                Product.ProductId = reader.GetValueOrDefault<int>("ProductId", 0);
                                Product.Name = reader.GetValueOrDefault<string>("Name", "");                                
                                Product.UnitPrice = reader.GetValueOrDefault<string>("UnitPrice", "");
                                Product.Cost = reader.GetValueOrDefault<double>("Cost", 0.0);

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
            response.responseObject = Product;
            return response;
        }

    }
}