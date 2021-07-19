using System;
using System.Collections;
using System.Net.Sockets;
using DryveD1API.Common;

namespace DryveD1API.Modules
{
    /// <summary>
    /// 60FD<br />
    /// Status display of digital inputs.
    /// </summary>
    public class DigitalInputs
    {
        private static byte ByteNumber { get => 4; }

        /// <summary>
        /// DI 9 Negative Limit Switch
        /// </summary>
        public bool Bit00 { get; set; }
        /// <summary>
        /// DI 8 Positive Limit Switch
        /// </summary>
        public bool Bit01 { get; set; }
        /// <summary>
        /// Not Assigned
        /// </summary>
        public bool Bit02 { get; set; }
        /// <summary>
        /// Enable
        /// </summary>
        public bool Bit03 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit04 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit05 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit06 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit07 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit08 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit09 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit10 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit11 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit12 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit13 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit14 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit15 { get; set; }
        /// <summary>
        /// DI 1
        /// </summary>
        public bool Bit16 { get; set; }
        /// <summary>
        /// DI 2
        /// </summary>
        public bool Bit17 { get; set; }
        /// <summary>
        /// DI 3
        /// </summary>
        public bool Bit18 { get; set; }
        /// <summary>
        /// DI 4
        /// </summary>
        public bool Bit19 { get; set; }
        /// <summary>
        /// DI 5
        /// </summary>
        public bool Bit20 { get; set; }
        /// <summary>
        /// DI 6
        /// </summary>
        public bool Bit21 { get; set; }
        /// <summary>
        /// DI 7
        /// </summary>
        public bool Bit22 { get; set; }
        /// <summary>
        /// DI 8
        /// </summary>
        public bool Bit23 { get; set; }
        /// <summary>
        /// DI 9
        /// </summary>
        public bool Bit24 { get; set; }
        /// <summary>
        /// DI 10
        /// </summary>
        public bool Bit25 { get; set; }
        /// <summary>
        /// Not Assigned
        /// </summary>
        public bool Bit26 { get; set; }
        /// <summary>
        /// Not Assigned
        /// </summary>
        public bool Bit27 { get; set; }
        /// <summary>
        /// Not Assigned
        /// </summary>
        public bool Bit28 { get; set; }
        /// <summary>
        /// Not Assigned
        /// </summary>
        public bool Bit29 { get; set; }
        /// <summary>
        /// Not Assigned
        /// </summary>
        public bool Bit30 { get; set; }
        /// <summary>
        /// Not Assigned
        /// </summary>
        public bool Bit31 { get; set; }

        private void Set(byte byte19, byte byte20, byte byte21, byte byte22)
        {
            // Byte 19
            var bitArray19 = new BitArray(new byte[] { byte19 });
            Bit00 = bitArray19[0];
            Bit01 = bitArray19[1];
            Bit02 = bitArray19[2];
            Bit03 = bitArray19[3];
            Bit04 = bitArray19[4];
            Bit05 = bitArray19[5];
            Bit06 = bitArray19[6];
            Bit07 = bitArray19[7];

            // Byte 20
            var bitArray20 = new BitArray(new byte[] { byte20 });
            Bit08 = bitArray20[0];
            Bit09 = bitArray20[1];
            Bit10 = bitArray20[2];
            Bit11 = bitArray20[3];
            Bit12 = bitArray20[4];
            Bit13 = bitArray20[5];
            Bit14 = bitArray20[6];
            Bit15 = bitArray20[7];

            // Byte 21
            var bitArray21 = new BitArray(new byte[] { byte21 });
            Bit16 = bitArray21[0];
            Bit17 = bitArray21[1];
            Bit18 = bitArray21[2];
            Bit19 = bitArray21[3];
            Bit20 = bitArray21[4];
            Bit21 = bitArray21[5];
            Bit22 = bitArray21[6];
            Bit23 = bitArray21[7];

            // Byte 22
            var bitArray22 = new BitArray(new byte[] { byte22 });
            Bit24 = bitArray22[0];
            Bit25 = bitArray22[1];
            Bit26 = bitArray22[2];
            Bit27 = bitArray22[3];
            Bit28 = bitArray22[4];
            Bit29 = bitArray22[5];
            Bit30 = bitArray22[6];
            Bit31 = bitArray22[7];
        }

        public void Read(Socket s)
        {
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.DigitalInputs, ByteNumber);
            var result = telegram.SendAndReceive(s);
            Set(result.Byte19, result.Byte20, result.Byte21, result.Byte22);
        }
    }
}
