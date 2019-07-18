using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
namespace CrossMultiplyMatrix
{
    /// <summary>
    /// This class contains all of the methods to get data from the API
    /// </summary>
    public class GetData
    {
        HttpClient client = null;
        WebClient webclient = new WebClient();
        public GetData()
        {
            webclient = new WebClient
            {
                BaseAddress = "https://recruitment-test.investcloud.com/"
            };
            client = new HttpClient
            {
                BaseAddress = new Uri("https://recruitment-test.investcloud.com/")
            };
        }
        /// <summary>
        /// This method calls the API to initialize the dataset. This method receives an object of type ResultOfInt32
        /// </summary>
        /// <param name="size">Size of data set</param>
        /// <returns>ResultOfInt32 with data from the server</returns>
        public async Task<ResultOfInt32> InitializeDataset(string size)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/numbers/init/{size}");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject(content, typeof(ResultOfInt32));
            ResultOfInt32 result = (ResultOfInt32)resultObj;
            return result;

        }

        /// <summary>
        /// This method fills a matrix with data from the api
        /// </summary>
        /// <param name="set">dataset name either A or B</param>
        /// <param name="numElements">Size of matrix</param>
        /// <returns>matrix filled with data</returns>
        public async Task<int[][]> Fillmatrix(string set, int numElements)
        {
            //ResultOfInt32Array row = await getRowCol(0, set,"row");
            int[][] mat = new int[numElements][];
            for(int i=0; i<numElements; i++)
            {
                // Get each column of numbers from service and add to matrix.

                ResultOfInt32Array col = await GetRowCol(i, set, "col");
                //ResultOfInt32Array col = await GetRowColWC(i, set, "col");
                
                mat[i] = col.Value;
            }

            return mat;
        }
        /// <summary>
        /// Sends the MD5 hash of the final matrix cross product and receives an answer as to whether the calculation was correct.
        /// </summary>
        /// <param name="MD5String"></param>
        /// <returns>Returns: ResultOfString containing a phrase indicating whether the calculation was correct</returns>
        public async Task<ResultOfString> CheckAnswer(string MD5String)
        {
            // Post to service to check whether the matrix was correct.
            string address = "api/numbers/validate";
            
            HttpResponseMessage response = await client.PostAsync(address, new StringContent(MD5String, Encoding.UTF8, "text/json"));
            string content = await response.Content.ReadAsStringAsync();
            object resultObj = JsonConvert.DeserializeObject(content, typeof(ResultOfString));
            ResultOfString result = (ResultOfString)resultObj;

            return result;
        }
        /// <summary>
        /// Gets a row or coluimn of data for a dataset at a specific index. This method uses HttpClient
        /// </summary>
        /// <param name="idx">Index of data needed</param>
        /// <param name="dataset">Name of dataset either A or B</param>
        /// <param name="arrayType">indeciate whether the data is a column (col) or row (row)</param>
        /// <returns>Returns an object of type ResultOfInt32Array with the data requested</returns>
        private async Task<ResultOfInt32Array> GetRowCol(int idx, string dataset, string arrayType)
        {
            HttpResponseMessage response = await client.GetAsync($"api/numbers/{dataset}/{arrayType}/{idx}");
            string content = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject(content, typeof(ResultOfInt32Array)) as ResultOfInt32Array;
            //ResultOfInt32Array result = (ResultOfInt32Array)resultObj;

            return resultObj;
        }
        /// <summary>
        /// Gets a row or coluimn of data for a dataset at a specific index. This method uses WebClient
        /// </summary>
        /// <param name="idx">Index of data needed</param>
        /// <param name="dataset">Name of dataset either A or B</param>
        /// <param name="arrayType">indeciate whether the data is a column (col) or row (row)</param>
        /// <returns>Returns an object of type ResultOfInt32Array with the data requested</returns>
        private async Task<ResultOfInt32Array> GetRowColWC(int idx, string dataset, string arrayType)
        {
            // try to use WebClient instead of httpclient
            Uri ba = new Uri(webclient.BaseAddress + $"api/numbers/{dataset}/{arrayType}/{idx}");
            byte[] bytes =  webclient.DownloadData(ba);
            string content = Encoding.Default.GetString(bytes);
            var resultObj = JsonConvert.DeserializeObject(content, typeof(ResultOfInt32Array)) as ResultOfInt32Array;

            return resultObj;
        }

    }
}
