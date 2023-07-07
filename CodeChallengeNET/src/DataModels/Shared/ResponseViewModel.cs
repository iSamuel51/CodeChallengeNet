
using System.Net;

namespace DataModels.Shared
{
    /// <summary>
    /// Class ResponseViewModel
    /// </summary>
    public class ResponseViewModel<T>
    {
        /// <summary>
        /// Gets or sets the Response identifier
        /// </summary>
        /// <value>The Response identifier.</value>
        public int Id { get; set; }               

        /// <summary>
        /// Gets or sets the status code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// gets or sets the object to be returned as response
        /// </summary>
        public T responseObject { get; set; }

        /// <summary>
        /// Gets or sets the Response Dictonary Error Model that are assigned to the Response View Model
        /// </summary>
        public ResponseDictionaryErrorModel ResponseDictionary { get; set; }
    }
}
