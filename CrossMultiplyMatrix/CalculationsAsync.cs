using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CrossMultiplyMatrix
{
    public class CalculationsAsync
    {
        /// <summary>
        /// Initializes a two dimenial integer array as a matrix
        /// </summary>
        /// <param name="numElements">Number of elements in both rows and columns of matrix</param>
        /// <returns>Matrix initialized to number of rows and columns</returns>
        public int [][] IntializeMatrix(int numElements)
        {
            int[][] mat = new int[numElements][];
            for(int i = 0; i<numElements; ++i)
            {
                mat[i] = new int[numElements];
            }

            return mat;
        }
        /// <summary>
        ///  Naive approach. Parrallel is much better. Shaved off about 13 seconds doing this way.
        /// </summary>
        /// <param name="matrixA">First Matrix</param>
        /// <param name="MatrixB">Second Matrix</param>
        /// <param name="numElements">Number of rows and columns in both matrices</param>
        /// <returns>Matrix representing the product of two matrices</returns>
        /// 
        public async Task<int[][]> CalculateproductAsync(int[][] matrixA, int[][] MatrixB, int numElements)
        {

            int[][] matrixProuct = IntializeMatrix(numElements);
            // run through the rows in A
            for(int i = 0; i<numElements; i++)
            {
                // run through colums in B
                for(int j=0; j<numElements; j++)
                {
                    // Cross multiply
                    for(int k=0; k<numElements; k++)
                    {
                        matrixProuct[i][j] += matrixA[i][k] * MatrixB[k][j];
                    }
                }
            }


            return matrixProuct;
        }

        /// <summary>
        /// Calaculate the cross prodcut of two matrices using the Parallel class if system.threading
        /// </summary>
        /// <param name="matrixA">First Matrix</param>
        /// <param name="MatrixB">Second Matrix</param>
        /// <param name="numElements">Number of rows and columns in both matrices</param>
        /// <returns>Matrix representing the product of two matrices</returns>
        public int[][] CalculateMatrixParallel(int[][] matrixA, int[][] MatrixB, int numElements)
        {
            int[][] matrixProuct = IntializeMatrix(numElements);

            Parallel.For(0, numElements, i =>
            {
                for (int j = 0; j < numElements; j++)
                {
                    // Cross multiply
                    for (int k = 0; k < numElements; k++)
                    {
                        matrixProuct[i][j] += matrixA[i][k] * MatrixB[k][j];
                    }
                }
            });

            return matrixProuct;
        }
        /// <summary>
        /// Calculate the MD5 hash of the result of crossproduct
        /// </summary>
        /// <param name="finalString">Represents the concatenated result of cross product of two arrays</param>
        /// <returns>Returns the MD5 hash of parameter</returns>
        public string BuildMD5String(string finalString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(finalString);
                byte[] outputBytes = md5.ComputeHash(inputBytes);
                foreach (byte b in outputBytes)
                {
                    //stringBuilder.Append(b.ToString("x2"));
                    stringBuilder.Append(b.ToString());
                }

            }

            return stringBuilder.ToString();
        }

    }
}
