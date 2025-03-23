using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAW_Edit
{
    public class classMathMatrix
    {

        public classMathMatrix()
        {

        }

        ~classMathMatrix()
        {

        }

        public double[,] InvertMatrix(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] augmented = new double[n, n * 2];

            // Initialize augmented matrix with the input matrix and the identity matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = matrix[i, j];
                    augmented[i, j + n] = (i == j) ? 1 : 0;
                }
            }

            // Apply Gaussian elimination
            for (int i = 0; i < n; i++)
            {
                int pivotRow = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Math.Abs(augmented[j, i]) > Math.Abs(augmented[pivotRow, i]))
                    {
                        pivotRow = j;
                    }
                }

                if (pivotRow != i)
                {
                    for (int k = 0; k < 2 * n; k++)
                    {
                        double temp = augmented[i, k];
                        augmented[i, k] = augmented[pivotRow, k];
                        augmented[pivotRow, k] = temp;
                    }
                }

                if (Math.Abs(augmented[i, i]) < 1e-10)
                {
                    return null;
                }

                double pivot = augmented[i, i];
                for (int j = 0; j < 2 * n; j++)
                {
                    augmented[i, j] /= pivot;
                }

                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        double factor = augmented[j, i];
                        for (int k = 0; k < 2 * n; k++)
                        {
                            augmented[j, k] -= factor * augmented[i, k];
                        }
                    }
                }
            }

            double[,] result = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = augmented[i, j + n];
                }
            }

            return result;
        }
    }
}


