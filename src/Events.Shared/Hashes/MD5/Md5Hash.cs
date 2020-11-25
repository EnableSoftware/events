using System;

namespace Events.Shared.Hashes.Md5
{
    // TODO remove when possible, used for gravatar hash
    // MD5 has been dropped in dotnet 5 blazor, create own https://github.com/dotnet/aspnetcore/issues/26450
    public class Md5Hash
    {
        private readonly static uint[] T = new uint[64]
            {   0xd76aa478,0xe8c7b756,0x242070db,0xc1bdceee,
                0xf57c0faf,0x4787c62a,0xa8304613,0xfd469501,
                0x698098d8,0x8b44f7af,0xffff5bb1,0x895cd7be,
                0x6b901122,0xfd987193,0xa679438e,0x49b40821,
                0xf61e2562,0xc040b340,0x265e5a51,0xe9b6c7aa,
                0xd62f105d,0x2441453,0xd8a1e681,0xe7d3fbc8,
                0x21e1cde6,0xc33707d6,0xf4d50d87,0x455a14ed,
                0xa9e3e905,0xfcefa3f8,0x676f02d9,0x8d2a4c8a,
                0xfffa3942,0x8771f681,0x6d9d6122,0xfde5380c,
                0xa4beea44,0x4bdecfa9,0xf6bb4b60,0xbebfbc70,
                0x289b7ec6,0xeaa127fa,0xd4ef3085,0x4881d05,
                0xd9d4d039,0xe6db99e5,0x1fa27cf8,0xc4ac5665,
                0xf4292244,0x432aff97,0xab9423a7,0xfc93a039,
                0x655b59c3,0x8f0ccc92,0xffeff47d,0x85845dd1,
                0x6fa87e4f,0xfe2ce6e0,0xa3014314,0x4e0811a1,
                0xf7537e82,0xbd3af235,0x2ad7d2bb,0xeb86d391};

        private readonly uint[] X = new uint[16];

        private Digest dgFingerPrint;

        private byte[] m_byteInput;
        public delegate void ValueChangingEventHandler(object sender, MD5ChangingEventArgs Changing);
        public delegate void ValueChangedEventHandler(object sender, MD5ChangedEventArgs Changed);

        public event ValueChangingEventHandler OnValueChanging;
        public event ValueChangedEventHandler OnValueChanged;

        public string Value
        {
            get
            {
                string st;
                char[] tempCharArray = new Char[m_byteInput.Length];

                for (int i = 0; i < m_byteInput.Length; i++)
                    tempCharArray[i] = (char)m_byteInput[i];

                st = new String(tempCharArray);
                return st;
            }
            set
            {
                this.OnValueChanging?.Invoke(this, new MD5ChangingEventArgs(value));

                if (value != null)
                {
                    m_byteInput = new byte[value.Length];
                    for (int i = 0; i < value.Length; i++)
                        m_byteInput[i] = (byte)value[i];
                    dgFingerPrint = CalculateMD5Value();
                }
                this.OnValueChanged?.Invoke(this, new MD5ChangedEventArgs(value, dgFingerPrint.ToString()));
            }
        }

        public byte[] GetValueAsByte()
        {
            byte[] bt = new byte[m_byteInput.Length];
            for (int i = 0; i < m_byteInput.Length; i++)
                bt[i] = m_byteInput[i];
            return bt;
        }

        public void SetValueAsByte(byte[] value)
        {
            if (value != null)
            {
                this.OnValueChanging?.Invoke(this, new MD5ChangingEventArgs(value));

                m_byteInput = new byte[value.Length];
                for (int i = 0; i < value.Length; i++)
                    m_byteInput[i] = value[i];
                dgFingerPrint = CalculateMD5Value();
            }

            this.OnValueChanged?.Invoke(this, new MD5ChangedEventArgs(value, dgFingerPrint.ToString()));
        }

        public string FingerPrint
        {
            get
            {
                return dgFingerPrint.ToString();
            }
        }

        public Md5Hash()
        {
            Value = "";
        }

        protected Digest CalculateMD5Value()
        {
            byte[] bMsg;
            uint N;
            Digest dg = new Digest();

            bMsg = CreatePaddedBuffer();

            N = (uint)(bMsg.Length * 8) / 32;

            for (uint i = 0; i < N / 16; i++)
            {
                CopyBlock(bMsg, i);
                PerformTransformation(ref dg.A, ref dg.B, ref dg.C, ref dg.D);
            }
            return dg;
        }

        protected void TransF(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + Md5Helper.RotateLeft((a + ((b & c) | (~(b) & d)) + X[k] + T[i - 1]), s);
        }

        protected void TransG(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + Md5Helper.RotateLeft((a + ((b & d) | (c & ~d)) + X[k] + T[i - 1]), s);
        }

        protected void TransH(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + Md5Helper.RotateLeft((a + (b ^ c ^ d) + X[k] + T[i - 1]), s);
        }

        protected void TransI(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + Md5Helper.RotateLeft((a + (c ^ (b | ~d)) + X[k] + T[i - 1]), s);
        }

        protected void PerformTransformation(ref uint A, ref uint B, ref uint C, ref uint D)
        {
            uint AA, BB, CC, DD;

            AA = A;
            BB = B;
            CC = C;
            DD = D;

            TransF(ref A, B, C, D, 0, 7, 1); TransF(ref D, A, B, C, 1, 12, 2); TransF(ref C, D, A, B, 2, 17, 3); TransF(ref B, C, D, A, 3, 22, 4);
            TransF(ref A, B, C, D, 4, 7, 5); TransF(ref D, A, B, C, 5, 12, 6); TransF(ref C, D, A, B, 6, 17, 7); TransF(ref B, C, D, A, 7, 22, 8);
            TransF(ref A, B, C, D, 8, 7, 9); TransF(ref D, A, B, C, 9, 12, 10); TransF(ref C, D, A, B, 10, 17, 11); TransF(ref B, C, D, A, 11, 22, 12);
            TransF(ref A, B, C, D, 12, 7, 13); TransF(ref D, A, B, C, 13, 12, 14); TransF(ref C, D, A, B, 14, 17, 15); TransF(ref B, C, D, A, 15, 22, 16);
            TransG(ref A, B, C, D, 1, 5, 17); TransG(ref D, A, B, C, 6, 9, 18); TransG(ref C, D, A, B, 11, 14, 19); TransG(ref B, C, D, A, 0, 20, 20);
            TransG(ref A, B, C, D, 5, 5, 21); TransG(ref D, A, B, C, 10, 9, 22); TransG(ref C, D, A, B, 15, 14, 23); TransG(ref B, C, D, A, 4, 20, 24);
            TransG(ref A, B, C, D, 9, 5, 25); TransG(ref D, A, B, C, 14, 9, 26); TransG(ref C, D, A, B, 3, 14, 27); TransG(ref B, C, D, A, 8, 20, 28);
            TransG(ref A, B, C, D, 13, 5, 29); TransG(ref D, A, B, C, 2, 9, 30); TransG(ref C, D, A, B, 7, 14, 31); TransG(ref B, C, D, A, 12, 20, 32);
            TransH(ref A, B, C, D, 5, 4, 33); TransH(ref D, A, B, C, 8, 11, 34); TransH(ref C, D, A, B, 11, 16, 35); TransH(ref B, C, D, A, 14, 23, 36);
            TransH(ref A, B, C, D, 1, 4, 37); TransH(ref D, A, B, C, 4, 11, 38); TransH(ref C, D, A, B, 7, 16, 39); TransH(ref B, C, D, A, 10, 23, 40);
            TransH(ref A, B, C, D, 13, 4, 41); TransH(ref D, A, B, C, 0, 11, 42); TransH(ref C, D, A, B, 3, 16, 43); TransH(ref B, C, D, A, 6, 23, 44);
            TransH(ref A, B, C, D, 9, 4, 45); TransH(ref D, A, B, C, 12, 11, 46); TransH(ref C, D, A, B, 15, 16, 47); TransH(ref B, C, D, A, 2, 23, 48);
            TransI(ref A, B, C, D, 0, 6, 49); TransI(ref D, A, B, C, 7, 10, 50); TransI(ref C, D, A, B, 14, 15, 51); TransI(ref B, C, D, A, 5, 21, 52);
            TransI(ref A, B, C, D, 12, 6, 53); TransI(ref D, A, B, C, 3, 10, 54); TransI(ref C, D, A, B, 10, 15, 55); TransI(ref B, C, D, A, 1, 21, 56);
            TransI(ref A, B, C, D, 8, 6, 57); TransI(ref D, A, B, C, 15, 10, 58); TransI(ref C, D, A, B, 6, 15, 59); TransI(ref B, C, D, A, 13, 21, 60);
            TransI(ref A, B, C, D, 4, 6, 61); TransI(ref D, A, B, C, 11, 10, 62); TransI(ref C, D, A, B, 2, 15, 63); TransI(ref B, C, D, A, 9, 21, 64);

            A += AA;
            B += BB;
            C += CC;
            D += DD;
        }

        protected byte[] CreatePaddedBuffer()
        {
            uint pad;
            byte[] bMsg;
            ulong sizeMsg;
            uint sizeMsgBuff;
            int temp = (448 - ((m_byteInput.Length * 8) % 512));

            pad = (uint)((temp + 512) % 512);
            if (pad == 0)
                pad = 512;

            sizeMsgBuff = (uint)((m_byteInput.Length) + (pad / 8) + 8);
            sizeMsg = (ulong)m_byteInput.Length * 8;
            bMsg = new byte[sizeMsgBuff];

            for (int i = 0; i < m_byteInput.Length; i++)
                bMsg[i] = m_byteInput[i];

            bMsg[m_byteInput.Length] |= 0x80;

            for (int i = 8; i > 0; i--)
                bMsg[sizeMsgBuff - i] = (byte)(sizeMsg >> ((8 - i) * 8) & 0x00000000000000ff);

            return bMsg;
        }

        protected void CopyBlock(byte[] bMsg, uint block)
        {
            if (bMsg == null)
            {
                return;
            }
            
            block <<= 6;
            for (uint j = 0; j < 61; j += 4)
            {
                X[j >> 2] = (((uint)bMsg[block + (j + 3)]) << 24) |
                        (((uint)bMsg[block + (j + 2)]) << 16) |
                        (((uint)bMsg[block + (j + 1)]) << 8) |
                        (((uint)bMsg[block + (j)]));
            }
        }
    }
}