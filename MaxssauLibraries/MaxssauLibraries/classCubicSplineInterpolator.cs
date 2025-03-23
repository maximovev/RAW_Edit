using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxssauLibraries
{
    public class classCubicSplineInterpolator
    {
        private readonly double[] _x;
        private readonly double[] _y;
        private readonly double[] _a;
        private readonly double[] _b;
        private readonly double[] _c;
        private readonly double[] _d;

        public classCubicSplineInterpolator(double[] x, double[] y)
        {
            if (x.Length != y.Length)
                throw new ArgumentException("Массивы X и Y должны быть одинаковой длины");

            _x = x;
            _y = y;
            int n = x.Length;

            _a = new double[n];
            _b = new double[n];
            _c = new double[n];
            _d = new double[n];

            CalculateCoefficients();
        }

        private void CalculateCoefficients()
        {
            int n = _x.Length - 1;
            double[] h = new double[n];
            for (int i = 0; i < n; i++)
                h[i] = _x[i + 1] - _x[i];

            double[] alpha = new double[n];
            for (int i = 1; i < n; i++)
            {
                alpha[i] = (3.0 / h[i]) * (_y[i + 1] - _y[i]) -
                           (3.0 / h[i - 1]) * (_y[i] - _y[i - 1]);
            }

            double[] l = new double[n + 1];
            double[] mu = new double[n + 1];
            double[] z = new double[n + 1];

            l[0] = 1.0;
            mu[0] = 0.0;
            z[0] = 0.0;

            for (int i = 1; i < n; i++)
            {
                l[i] = 2.0 * (_x[i + 1] - _x[i - 1]) - h[i - 1] * mu[i - 1];
                mu[i] = h[i] / l[i];
                z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
            }

            l[n] = 1.0;
            z[n] = 0.0;
            _c[n] = 0.0;

            for (int j = n - 1; j >= 0; j--)
            {
                _c[j] = z[j] - mu[j] * _c[j + 1];
                _b[j] = (_y[j + 1] - _y[j]) / h[j] - h[j] * (_c[j + 1] + 2.0 * _c[j]) / 3.0;
                _d[j] = (_c[j + 1] - _c[j]) / (3.0 * h[j]);
                _a[j] = _y[j];
            }
        }

        public double Interpolate(double x)
        {
            int n = _x.Length - 1;
            if (x < _x[0]) return _y[0];
            if (x > _x[n]) return _y[n];

            int i = FindSegment(x);
            double dx = x - _x[i];
            return _a[i] + _b[i] * dx + _c[i] * dx * dx + _d[i] * dx * dx * dx;
        }

        private int FindSegment(double x)
        {
            int left = 0;
            int right = _x.Length - 1;

            while (left < right)
            {
                int mid = (left + right) / 2;
                if (x < _x[mid])
                    right = mid - 1;
                else
                    left = mid + 1;
            }

            return Math.Max(0, Math.Min(_x.Length - 2, left));
        }
    }
}
