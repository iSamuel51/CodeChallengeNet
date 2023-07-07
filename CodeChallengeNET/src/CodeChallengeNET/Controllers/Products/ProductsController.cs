using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataModels.Shared;
using DataAccess;
using DataModels;
using DataAccess.Repository;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeChallengeNET.Controllers.Products
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly AppDb _db;
        private readonly IProductRepository _repo;

        public ProductsController(AppDb db)
        {
            _db = db;
            _repo = new ProductRepository(_db);
        }

        /// <summary>        
        /// Inserts new Product
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The new Product Id inserted</returns>
        [HttpPost]
        [Route("InsertProduct")]        
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> InsertProduct([FromBody] ProductViewModel request)
        {
            ResponseViewModel<int> repoResponse = new ResponseViewModel<int>();

            try
            {                

                repoResponse = await _repo.InsertProduct(request);

                if (repoResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(repoResponse.responseObject);
                }
                else
                {
                    return StatusCode((int)repoResponse.StatusCode, repoResponse.ResponseDictionary);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of all Products.
        /// </summary>
        /// <returns>List of all Products.</returns>
        [HttpGet]
        [Route("GetProducts")]        
        [ProducesResponseType(typeof(List<ProductViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetProducts()
        {
            ResponseViewModel<List<ProductViewModel>> repoResponse = new ResponseViewModel<List<ProductViewModel>>();

            try
            {                

                repoResponse = await _repo.GetProducts();

                if (repoResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(repoResponse.responseObject);
                }
                else
                {
                    return StatusCode((int)repoResponse.StatusCode, repoResponse.ResponseDictionary);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets all the information details searching Products by id.
        /// </summary>
        /// <param name="IdProduct">Product Id</param>
        /// <returns>Product Object</returns>
        [HttpGet]
        [Route("SearchProductId/{IdProduct}")]
        [ProducesResponseType(typeof(ProductViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status404NotFound)]        
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SearchProductId([FromRoute]int IdProduct)
        {            
            ResponseViewModel<ProductViewModel> repoResponse = new ResponseViewModel<ProductViewModel>();
            try
            {                
                repoResponse = await _repo.SearchProductId(IdProduct);
                if (repoResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(repoResponse.responseObject);
                }
                else
                {
                    return StatusCode((int)repoResponse.StatusCode, repoResponse.ResponseDictionary);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
