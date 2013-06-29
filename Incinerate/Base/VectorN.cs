using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incinerate.Base
{
    public class VectorN 
    {
        public static int Precision = 4;
        private int m_size;
        private double[] m_data;
        public int Size { get { return m_size; } }

        public VectorN(int n)
        {
            m_size = n;
            m_data = new double[n];
        }

        public void CopyFrom(VectorN original)
        {
            if (original.Size != this.Size)
            {
                m_size = original.Size;
                m_data = new double[original.Size];
            }
            for (int i = 0; i < original.Size; i++)
            {
                this[i] = original[i];
            }
        }

        public override string ToString()
        {
            string result = "(";
            foreach (double d in m_data)
            {
                result += Math.Round(d, Precision) + ";";
            }
            result += ")";
            return result;
        }

        // Возвращает длину вектора
        public double Length()
        {
            return Length(m_size);
        }

        public double Length(int first)
        {
            first = Math.Min(first, m_size);
            double summ = 0;
            for (int i = 0; i < first; i++)
            {
                summ += m_data[i] * m_data[i];
            }
            return Math.Round(Math.Sqrt(summ), Precision);
        }

        public double this[int index]
        {
            get
            {
                if (index < 0 || index >= m_size)
                    throw new IndexOutOfRangeException();
                return Math.Round(m_data[index], Precision);
            }

            set
            {
                if (index < 0 || index >= m_size)
                    throw new IndexOutOfRangeException();
                m_data[index] = value;
            }
        }

        public static VectorN operator +(VectorN v1, VectorN v2)
        {
            if (v1.Size != v2.Size)
                throw new InvalidOperationException("Vector lengths must be equals");
            VectorN result = new VectorN(v1.Size);
            for (int i = 0; i < v1.Size; i++)
            {
                result[i] = v1[i] + v2[i];
            }
            return result;
        }

        public static VectorN operator -(VectorN v1, VectorN v2)
        {
            if (v1.Size != v2.Size)
                throw new InvalidOperationException("Vector lengths must be equals");
            VectorN result = new VectorN(v1.Size);
            for (int i = 0; i < v1.Size; i++)
            {
                result[i] = v1[i] - v2[i];
            }
            return result;
        }

        public static VectorN operator *(VectorN v, double c)
        {
            VectorN result = new VectorN(v.Size);
            for (int i = 0; i < v.Size; i++)
            {
                result[i] = v[i] * c;
            }
            return result;
        }

        public static VectorN operator /(VectorN v, double c)
        {
            VectorN result = new VectorN(v.Size);
            for (int i = 0; i < v.Size; i++)
            {
                result[i] = v[i] / c;
            }
            return result;
        }

        public static VectorN operator /(VectorN v1, VectorN v2)
        {
            if (v1.Size != v2.Size)
                throw new InvalidOperationException("Vector lengths must be equals");
            VectorN result = new VectorN(v1.Size);
            for (int i = 0; i < v1.Size; i++)
            {
                result[i] = v1[i] / v2[i];
            }
            return result;
        }
    }
}
