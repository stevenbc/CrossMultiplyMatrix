using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossMultiplyMatrix
{
    /// <summary>
    /// Class to get an integer value from API
    /// </summary>
    public class ResultOfInt32
    {
        /// <summary>
        /// The integer value returned
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// The cause of failure ususallu null
        /// </summary>
        public string  Cause { get; set; }
        /// <summary>
        /// True for when the request retrieved data
        /// </summary>
        public bool Success { get; set; }
    }
}
