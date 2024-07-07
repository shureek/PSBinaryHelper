using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSBinaryHelper
{
    public class DataChunk : ICollection<byte>
    {
        readonly byte[] data;

        public int Count => data.Length;

        public bool IsReadOnly => false;

        public DataChunk(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            this.data = data;
        }

        public byte this[int index]
        {
            get
            {
                return data[index];
            }
        }

        public override string ToString()
        {
            int startLength = 30;
            int endLength = 10;

            StringBuilder sb = new StringBuilder(200);
            AppendToString(sb, 0, Math.Min(data.Length, startLength));

            if (data.Length - startLength > endLength)
            {
                sb.AppendFormat(" ({0} bytes)");
                AppendToString(sb, data.Length - endLength, endLength);
            }
            else {
                AppendToString(sb, startLength, data.Length - startLength);
            }

            return sb.ToString();
        }

        void AppendToString(StringBuilder sb, int startIndex, int count)
        {
            for (int i = startIndex; i < startIndex + count; i++)
            {
                if (sb.Length > 0)
                    sb.Append(' ');
                sb.AppendFormat("{0:X02}", data[i]);
            }
        }

        public override int GetHashCode()
        {
            return data.GetHashCode();
        }

        public void Add(byte item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(byte item)
        {
            return data.Contains(item);
        }

        public void CopyTo(byte[] array, int arrayIndex)
        {
            data.CopyTo(array, arrayIndex);
        }

        public bool Remove(byte item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>)data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        public static implicit operator byte[](DataChunk d) => d.data;
        public static explicit operator DataChunk(byte[] b) => new DataChunk(b);
    }
}