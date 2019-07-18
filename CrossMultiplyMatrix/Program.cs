using System;
using System.Text;
using System.Threading.Tasks;



namespace CrossMultiplyMatrix
{
    class Program
    {
        static readonly int numElements = 1000;
        static int[][] datasetA;
        static int[][] datasetB;
        static StringBuilder stringBuilder;
        // used for logging times
        static DateTime firstDate;
        static DateTime secondDate;
        static DateTime thirdDate;
        static DateTime afterload;
        static string message;
        static bool passed = false;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"Started at {DateTime.Now}");
                // run this in an asynchronous manner

                MainAsync().Wait();

                // logging times for debugging purposes.
                DateTime finishTime = DateTime.Now;
                TimeSpan TotalSecs = finishTime.Subtract(firstDate);
                TimeSpan CalcSecs = secondDate.Subtract(afterload);
                TimeSpan AfterMD5 = thirdDate.Subtract(secondDate);
                TimeSpan secsAcfterload = afterload.Subtract(firstDate);

                Console.WriteLine($"Load took: {secsAcfterload.TotalSeconds}");
                Console.WriteLine($"Calculations took {CalcSecs.TotalSeconds} Time {thirdDate}");
                Console.WriteLine($"MD5 hash took {AfterMD5.TotalSeconds} Time: {secondDate}");
                Console.WriteLine($"Total Time {TotalSecs.TotalSeconds} Time: {finishTime}");
                Console.WriteLine($"Success: {passed} Message: {message}");
            }
            catch(Exception ex)
            {
                //catch all exceptions on the main method
                Console.WriteLine($"An error occured and we had to go. Message is {ex.Message}");
            }            
        }
        /// <summary>
        /// Main method run using async
        /// </summary>
        /// <returns></returns>
        static async Task MainAsync()
        {
            // does all calculations 
            CalculationsAsync calculations = new CalculationsAsync();

            stringBuilder = new StringBuilder();
            // initialize datasets
            int[][] cols = new int[numElements][];
            GetData getData = new GetData();
            ResultOfInt32 resultOfInt32 = await getData.InitializeDataset(numElements.ToString());


            firstDate = DateTime.Now;
            datasetA = await getData.Fillmatrix("A", numElements);
            datasetB = await getData.Fillmatrix("B", numElements);
            afterload = DateTime.Now;
            // Better to use parallel processing
            //int[][] datasetProd = await calculations.CalculateproductAsync(datasetA, datasetB, numElements);
            int[][] datasetProd = calculations.CalculateMatrixParallel(datasetA, datasetB, numElements);

            
            // Build the string to hash
            secondDate = DateTime.Now;
            for(int i = 0; i<numElements; i++)
            {
                for(int j=0; j<numElements; j++)
                {
                    stringBuilder.Append(datasetProd[i][j]);
                }
            }
            // build the MD5 hash. Fatser to build string using a stringbuilder
            string finalString = stringBuilder.ToString();
            string md5hash = calculations.BuildMD5String(finalString);

            thirdDate = DateTime.Now;
            ResultOfString result = await getData.CheckAnswer(md5hash);
            passed = result.Success;
            if (result.Value == null)
                message = "No Message";
            else
                message = result.Value;
            

        }

       
    }

    /*
     * Notes
     * httpclient variable client.BaseAddress = new Uri("https://recruitment-test.investcloud.com/");
     * api/numbers/init/{size}
     * 
     * Most of the time taken for this to run takes place in the load matrix function, 105 seconds for 1000. This may be the api or the deserilization
     */
     // Run times from my last 2 tests
    /*
     * Using paralel processing for calculation. calculations took 2 seconds
     * 1000 x 1000
     * Started at 7/17/2019 3:32:03 PM
     * Load took: 105.0452192
     * Calculations after 107.2369902 Time 7/17/2019 3:33:52 PM
     * MD5 hash after 0.2178989 Time: 7/17/2019 3:33:52 PM
     * Total Time 107.5609595 Time: 7/17/2019 3:33:52 PM
     * Success: True Message: Kraken Approves!
     *
     * Using async and await for calculations. calulations took 13 seconds.
     * 1000 x 1000
     * Started at 7/17/2019 3:36:43 PM
     * Load took: 105.8482754
     * Calculations after 117.0178708 Time 7/17/2019 3:38:42 PM
     * MD5 hash after 0.2157588 Time: 7/17/2019 3:38:42 PM
     * Total Time 117.3493454 Time: 7/17/2019 3:38:42 PM
     * Success: True Message: Kraken Approves!
     */
}
