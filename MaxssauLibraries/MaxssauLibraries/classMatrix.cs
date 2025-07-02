using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxssauLibraries
{
    using System;

    public class Matrix
    {
        private double[,] data;
        public int Rows { get; }
        public int Columns { get; }

        // Конструктор для создания матрицы заданного размера
        public Matrix(int rows, int columns)
        {
            if (rows <= 0 || columns <= 0)
                throw new ArgumentException("Размеры матрицы должны быть положительными числами.");

            Rows = rows;
            Columns = columns;
            data = new double[rows, columns];
        }

        // Конструктор для создания матрицы из двумерного массива
        public Matrix(double[,] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            Rows = array.GetLength(0);
            Columns = array.GetLength(1);
            data = (double[,])array.Clone();
        }

        // Индексатор для доступа к элементам матрицы
        public double this[int row, int col]
        {
            get
            {
                CheckBounds(row, col);
                return data[row, col];
            }
            set
            {
                CheckBounds(row, col);
                data[row, col] = value;
            }
        }

        // Проверка границ матрицы
        private void CheckBounds(int row, int col)
        {
            if (row < 0 || row >= Rows || col < 0 || col >= Columns)
                throw new IndexOutOfRangeException("Индекс выходит за границы матрицы.");
        }

        // Сложение матриц
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Columns != b.Columns)
                throw new ArgumentException("Размеры матриц должны совпадать для сложения.");

            Matrix result = new Matrix(a.Rows, a.Columns);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i, j] = a[i, j] + b[i, j];
                }
            }
            return result;
        }

        // Вычитание матриц
        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Columns != b.Columns)
                throw new ArgumentException("Размеры матриц должны совпадать для вычитания.");

            Matrix result = new Matrix(a.Rows, a.Columns);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i, j] = a[i, j] - b[i, j];
                }
            }
            return result;
        }

        // Умножение матриц
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Columns != b.Rows)
                throw new ArgumentException("Количество столбцов первой матрицы должно равняться количеству строк второй матрицы.");

            Matrix result = new Matrix(a.Rows, b.Columns);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < b.Columns; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < a.Columns; k++)
                    {
                        sum += a[i, k] * b[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }

        // Умножение матрицы на скаляр
        public static Matrix operator *(Matrix matrix, double scalar)
        {
            Matrix result = new Matrix(matrix.Rows, matrix.Columns);
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    result[i, j] = matrix[i, j] * scalar;
                }
            }
            return result;
        }

        // Умножение скаляра на матрицу
        public static Matrix operator *(double scalar, Matrix matrix)
        {
            return matrix * scalar;
        }

        // Транспонирование матрицы
        public Matrix Transpose()
        {
            Matrix result = new Matrix(Columns, Rows);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result[j, i] = this[i, j];
                }
            }
            return result;
        }

        // Проверка на равенство матриц
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Matrix))
                return false;

            Matrix other = (Matrix)obj;
            if (Rows != other.Rows || Columns != other.Columns)
                return false;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (Math.Abs(this[i, j] - other[i, j]) > double.Epsilon)
                        return false;
                }
            }
            return true;
        }

        // Получение хэш-кода
        public override int GetHashCode()
        {
            return data.GetHashCode();
        }

        // Преобразование матрицы в строку
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result += $"{this[i, j]:F2}\t";
                }
                result += Environment.NewLine;
            }
            return result;
        }

        // Создание единичной матрицы
        public static Matrix Identity(int size)
        {
            if (size <= 0)
                throw new ArgumentException("Размер должен быть положительным числом.");

            Matrix result = new Matrix(size, size);
            for (int i = 0; i < size; i++)
            {
                result[i, i] = 1;
            }
            return result;
        }

        // Создание нулевой матрицы
        public static Matrix Zero(int rows, int columns)
        {
            return new Matrix(rows, columns);
        }
    }
}
