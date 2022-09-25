using System;


namespace StockBox.Actions
{

    /// <summary>
    /// Class <c>ActionResponse</c> is a general response to an SbAction. This
    /// could be an API response, SQL layer response, etc.
    /// </summary>
    public class ActionResponse
    {
        public bool IsSuccess { get; set; }

        /// <summary>
        /// A general object to allow use to return any necessary object in our
        /// repsonse
        /// </summary>
        public object Source { get; set; }

        public ActionResponse()
        {
        }
    }
}
