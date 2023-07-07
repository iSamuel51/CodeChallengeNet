using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataModels.Shared
{
    public class ResponseDictionaryErrorModel
    {
        /// <summary>
        /// Gets or sets the Response Dictonary Error identifier
        /// </summary>
        /// <value>The Response Dictonary Error identifier.</value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the Message
        /// </summary>
        /// <value>The Message.</value>
        public string Message { get; set; }

        public ResponseDictionaryErrorModel()
        {

        }

        /// <summary>
        /// Creates  new instance of Response Dictonary Error Model with the specified parameters to map on the Angular UI
        /// </summary>
        /// <param name="id">Error Id</param>
        /// <param name="message">Error Message</param>        
        public ResponseDictionaryErrorModel(string id, string message)
        {
            Id = id;
            Message = message;
        }
    }
}
