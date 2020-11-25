using System;
using System.Globalization;

namespace Events.Shared.Hashes.Md5
{
    public class Digest
    {
        public uint A;
        public uint B;
        public uint C;
        public uint D;

        public Digest()
        {
            A = unchecked((uint)Md5InitializerConstant.A);
            B = unchecked((uint)Md5InitializerConstant.B);
            C = unchecked((uint)Md5InitializerConstant.C);
            D = unchecked((uint)Md5InitializerConstant.D);
        }

        public override string ToString()
        {
            string st;
            st = Md5Helper.ReverseByte(A).ToString("X8", CultureInfo.InvariantCulture) +
                Md5Helper.ReverseByte(B).ToString("X8", CultureInfo.InvariantCulture) +
                Md5Helper.ReverseByte(C).ToString("X8", CultureInfo.InvariantCulture) +
                Md5Helper.ReverseByte(D).ToString("X8", CultureInfo.InvariantCulture);
            return st;
        }
    }

    sealed public class Md5Helper
    {
        private Md5Helper() { }

        public static uint RotateLeft(uint uiNumber, ushort shift)
        {
            return ((uiNumber >> 32 - shift) | (uiNumber << shift));
        }

        public static uint ReverseByte(uint uiNumber)
        {
            return (((uiNumber & 0x000000ff) << 24) |
                        (uiNumber >> 24) |
                    ((uiNumber & 0x00ff0000) >> 8) |
                    ((uiNumber & 0x0000ff00) << 8));
        }
    }

    public class MD5ChangingEventArgs : EventArgs
    {
        public MD5ChangingEventArgs(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            byte[] NewData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                NewData[i] = data[i];
        }

        public MD5ChangingEventArgs(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            byte[] NewData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                NewData[i] = (byte)data[i];
        }
    }

    public class MD5ChangedEventArgs : EventArgs
    {
        private readonly string FingerPrint;

        public MD5ChangedEventArgs(byte[] data, string HashedValue)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            byte[] NewData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                NewData[i] = data[i];
            FingerPrint = HashedValue;
        }

        public MD5ChangedEventArgs(string data, string HashedValue)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            byte[] NewData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                NewData[i] = (byte)data[i];

            FingerPrint = HashedValue;
        }
    }
}