using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Domain.Models;

namespace Domain.DataMapper
{
    public interface IDataMapper<T> where T : class
    {
        /// <summary>
        /// Load data from a stream. The data is loaded into to DataMapper's internal list
        /// </summary>
        /// <param name="sourceStream">A Stream containing a pipe-separated string</param>
        /// <param name="skipHeader">if true then skip the first line of the stream</param>
        /// <param name="loadErrors">Output parameter. If not empty,then contains a list of failed "inserts"</param>
        void LoadData(Stream sourceStream, bool skipHeader ,out List<string> loadErrors);

        List<T> GetData();
    }

    
}