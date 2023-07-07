using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataModels.Shared;
using DataAccess;
using DataModels;
using DataAccess.Repository;
using System.Collections.Generic;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeChallengeNET.Controllers.Sales
{
    [Route("api/[controller]")]
    public class SalesController : Controller
    {
        private readonly AppDb _db;
        private readonly ISaleRepository _repo;

        public SalesController(AppDb db)
        {
            _db = db;
            _repo = new SaleRepository(_db);
        }

        /// <summary>        
        /// Inserts new Sale
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The new Sale Id inserted</returns>
        [HttpPost]
        [Route("InsertSale")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> InsertSale([FromBody] SaleViewModel request)
        {
            ResponseViewModel<int> repoResponse = new ResponseViewModel<int>();

            try
            {

                repoResponse = await _repo.InsertSale(request);

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
        /// Gets all the information details searching Sales by date range.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>List of all sales filtered.</returns>
        [HttpPost]
        [Route("SearchSalesByDateRange")]
        [ProducesResponseType(typeof(SaleViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SearchSalesByDateRange([FromBody] SalesDateRangeRequest request)
        {
            ResponseViewModel<List<SaleViewModel>> repoResponse = new ResponseViewModel<List<SaleViewModel>>();
            try
            {
                repoResponse = await _repo.SearchSalesByDateRange(request);
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