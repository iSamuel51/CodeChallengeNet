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

namespace CodeChallengeNET.Controllers.Clients
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly AppDb _db;        
        private readonly ICustomerRepository _repo;

        public CustomersController(AppDb db)
        {
            _db = db;
            _repo = new CustomerRepository(_db);
        }

        /// <summary>        
        /// Inserts new Customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The new Customer Id inserted</returns>
        [HttpPost]
        [Route("InsertCustomer")]        
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> InsertCustomer([FromBody] CustomerViewModel request)
        {
            ResponseViewModel<int> repoResponse = new ResponseViewModel<int>();

            try
            {                

                repoResponse = await _repo.InsertCustomer(request);

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
        /// Gets a list of all clients.
        /// </summary>
        /// <returns>List of all clients.</returns>
        [HttpGet]
        [Route("GetCustomers")]        
        [ProducesResponseType(typeof(List<CustomerViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCustomers()
        {
            ResponseViewModel<List<CustomerViewModel>> repoResponse = new ResponseViewModel<List<CustomerViewModel>>();

            try
            {                

                repoResponse = await _repo.GetCustomers();

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
        /// Gets all the information details searching clients by id.
        /// </summary>
        /// <param name="IdCustomer">Customer Id</param>
        /// <returns>Customer Object</returns>
        [HttpGet]
        [Route("SearchCustomerId/{IdCustomer}")]
        [ProducesResponseType(typeof(CustomerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status404NotFound)]        
        [ProducesResponseType(typeof(ResponseDictionaryErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SearchCustomerId([FromRoute]int IdCustomer)
        {            
            ResponseViewModel<CustomerViewModel> repoResponse = new ResponseViewModel<CustomerViewModel>();
            try
            {                
                repoResponse = await _repo.SearchCustomerId(IdCustomer);
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
