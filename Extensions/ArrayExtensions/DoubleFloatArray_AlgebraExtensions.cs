using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.ArrayExtensions
{


    public static class DoubleFloatArray_AlgebraExtensions
    {
        public static double Sum(this double[] array)
        {
            double result = 0;

            for (int i = 0; i < array.Length; i++)
            {

                result += array[i];
            }

            return result;

        }

        public static double Mean(this double[] array)
        {
            return array.Sum() / array.Count();
        }

        /// <summary>
        /// Returns the module of the array. 
        /// 
        /// The module of an array is mathematically equivalent to its own dot product. 
        /// 

        /// 
        /// The Modulus (magnitude) of an array is equivalent to the squared root of the summation of its own elementwise multiplication.
        /// 
        /// Assume a 3D array V1: {-1,2,5}.
        /// 
        /// 
        /// The module of this Array will be: 1 + 4 + 25 = √30.
        /// 
        /// 
        /// Equivalently, the magnitude of an array is equivalent to the squared root of its own Dot Product, and thus, it could be written as: √(V1 . V1)
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns>the Module of the array</returns>
        public static double Magnitude(this double[] array)
        {
            var magnitude = array.DotProduct(array);

            return Math.Sqrt(magnitude);

        }

        /// <summary>
        /// Alias for Magnitude:
        /// 
        /// 
        /// The Modulus (magnitude) of an array is equivalent to the squared root of the summation of its own elementwise multiplication.
        /// 
        /// Assume a 3D array V1: {-1,2,5}.
        /// 
        /// 
        /// The module of this Array will be: 1 + 4 + 25 = √30.
        /// 
        /// 
        /// Equivalently, the magnitude of an array is equivalent to the squared root of its own Dot Product, and thus, it could be written as: √(V1 . V1)
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns>the magnitude of the array</returns>
        public static double Module(this double[] array)
        {
            return array.Magnitude();

        }

        /// <summary>
        /// ElementWise multiplication between two arrays.
        /// 
        /// Assume two 3D vectors:
        ///     V1) {1,2,3} 
        ///     V2) {4,5,6};
        /// 
        /// the returned multiplication will be:
        ///     V3) {4,10,18}
        ///     
        /// </summary>
        /// <param name="array"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double[] Multiply(this double[] array, double[] other)
        {
            var arrayResult = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                arrayResult[i] = array[i] * other[i];
            }

            return arrayResult;

        }

        /// <summary>
        /// ElementWise multiplication between a constant and an array.
        /// 
        /// Assume two 3D vectors:
        ///     V1) {1,2,3} 
        ///     V2) {4,5,6};
        /// 
        /// the returned multiplication will be:
        ///     V3) {4,10,18}
        ///     
        /// </summary>
        /// <param name="array"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double[] Multiply(this double[] array, double constant)
        {
            var arrayResult = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                arrayResult[i] = array[i] * constant;
            }

            return arrayResult;

        }

        /// <summary>
        /// ElementWise division between two arrays.
        /// 
        /// Assume two 3D vectors:
        ///     V1) {1,2,3} 
        ///     V2) {4,5,6};
        /// 
        /// the returned multiplication will be:
        ///     V3) {4,10,18}
        ///     
        /// </summary>
        /// <param name="array"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double[] Divide(this double[] array, double[] other)
        {
            var arrayResult = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                arrayResult[i] = array[i] / other[i];
            }

            return arrayResult;

        }


        /// <summary>
        /// ElementWise division between a constant and an array.
        /// 
        /// Assume two 3D vectors:
        ///     V1) {1,2,3} 
        ///     V2) {4,5,6};
        /// 
        /// the returned multiplication will be:
        ///     V3) {4,10,18}
        ///     
        /// </summary>
        /// <param name="array"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double[] Divide(this double[] array, double constant)
        {
            var arrayResult = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                arrayResult[i] = array[i] / constant;
            }

            return arrayResult;

        }

        /// <summary>
        /// Dot Product of two arrays (scalar multiplication).
        /// </summary>
        /// <param name="array"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double DotProduct(this double[] array, double[] other)
        {
            var arrayResult = array.Multiply(other).Sum();

            return arrayResult;

        }


        public static double Determinant2D(double[] ab, double[] cd)
        {
            if (ab.Length != 2 || cd.Length != 2)
            {
                throw new InvalidOperationException("The provided arrays must of length 2");
            }

            return ( ab[0] * cd[1] ) - ( cd[0] * ab[1] );
        }

        public static double[] Determinant3D(double[] a, double[] b)
        {
            if (a.Length != 3 || b.Length != 3)
            {
                throw new InvalidOperationException("The provided arrays must of length 2");
            }

            double a2b3 = a[1] * b[2];
            double a3b2 = a[2] * b[1];
            double a3b1 = a[2] * b[0];
            double a1b3 = a[0] * b[2];
            double a1b2 = a[0] * b[1];
            double a2b1 = a[1] * b[0];

            return new double[] { a2b3 - a3b2, 
                                  a3b1 - a1b3, 
                                  a1b2 - a2b1 };
        }

        /// <summary>
        /// Cross-Product two 3D arrays (vector product).
        /// </summary>
        /// <param name="array"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double[] CrossProduct3D(this double[] array, double[] other)
        {
            double[] crossProduct = new double[3] { 1, 1, 1 };
            double[] a = new double[3] { 1, 1, 1 };
            double[] b = new double[3] { 1, 1, 1 };

            for (int i = 0; i < array.Count(); i++)
            {
                a[i] = array[i];
                b[i] = other[i];
            }


            if (array.Length != 2)
            {
                throw new InvalidOperationException("The provided arrays must of length 2");
            }

            else
            {
                if (a.Length != 3 || b.Length != 3)
                {
                    throw new InvalidOperationException("The provided arrays must of length 2");
                }

                double a2b3 = a[1] * b[2];
                double a3b2 = a[2] * b[1];
                double a3b1 = a[2] * b[0];
                double a1b3 = a[0] * b[2];
                double a1b2 = a[0] * b[1];
                double a2b1 = a[1] * b[0];


                return new double[] {a2b3 - a3b2, a1b3 - a3b1, a1b2 - a2b1 };

            }
        }

        /// <summary>
        /// Cross-Product two 2D arrays (vector product).
        /// </summary>
        /// <param name="array"></param>
        /// <param name="other"></param>
        /// <returns>Double</returns>
        public static double CrossProduct2D(this double[] array, double[] other)
        {
            double result = array[0]*other[1] - array[1]*other[0];

            return result;
        }

        public static double[] GetUnitVector(this double[] array)
        {
            return array.Divide(array.Magnitude());
        }

        /// <summary>
        /// Returns the angle between two arrays
        /// </summary>
        /// <param name="array"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double Angle(this double[] array, double[] other)
        {
            var theta = array.DotProduct(other) / ( array.Magnitude() * other.Magnitude() );

            return Math.Acos(theta);
        }
    }
}
