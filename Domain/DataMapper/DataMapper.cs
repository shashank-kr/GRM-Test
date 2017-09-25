using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataMapper
{
    public abstract class DataMapper<T> : IDataMapper<T> where T : class
    {
        public abstract void LoadData(Stream sourceStream, bool skipHeader,out List<string> invalidData);

        public abstract List<T> GetData();

        public Type MappedType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Returns a count of the number of public properties on the Mapped Type
        /// </summary>
        public int PropertyCount 
        {
            get
            {
                var pi = this.MappedType.GetProperties();
                return pi.Count(p => p.PropertyType.IsPublic);
            }
        }

        /// <summary>
        /// Convert the  the delimited source string into an array of strings. 
        /// If the number of elements (columns) does not match the number of properties of the Mapped type, then throw an exception.
        /// </summary>
        /// <param name="sourceData"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        protected string[] SplitSourceData(string sourceData, char splitChar = '|')
        {
            if (string.IsNullOrEmpty(sourceData))
            {
                return null;
            }
            var splitArray = sourceData.Trim().Split(splitChar);
            if (splitArray.Length != this.PropertyCount)
            {
                throw new InvalidDataException("The supplied data does have the required number of fields: " 
                    + sourceData);
            }
            return splitArray;
        }

    }
}
