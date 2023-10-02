using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DryveD1API.Common;

namespace DryveD1API.Modules
{
    /// <summary>
    /// 6040h<br />
    /// Object for controlling the dryve D1
    /// </summary>
    public sealed class ControlWord
    {
        private static byte ByteNumber { get => 2; }

        /// <summary>
        /// Switch On
        /// </summary>
        public bool Bit00 { get; set; }

        /// <summary>
        /// Enable Voltage
        /// </summary>
        public bool Bit01 { get; set; }

        /// <summary>
        /// Quick-Stop
        /// </summary>
        public bool Bit02 { get; set; }

        /// <summary>
        /// Enable Operation
        /// </summary>
        public bool Bit03 { get; set; }

        /// <summary>
        /// Mode Specific
        /// </summary>
        public bool Bit04 { get; set; }

        /// <summary>
        /// Mode Specific
        /// </summary>
        public bool Bit05 { get; set; }

        /// <summary>
        /// Mode Specific
        /// </summary>
        public bool Bit06 { get; set; }

        /// <summary>
        /// Fault Reset
        /// </summary>
        public bool Bit07 { get; set; }

        /// <summary>
        /// Halt
        /// </summary>
        public bool Bit08 { get; set; }

        /// <summary>
        /// Mode Specific
        /// </summary>
        public bool Bit09 { get; set; }

        /// <summary>
        /// Reserved
        /// </summary>
        public bool Bit10 { get; set; }

        /// <summary>
        /// Manufacturer Specific
        /// </summary>
        public bool Bit11 { get; set; }

        /// <summary>
        /// Manufacturer Specific
        /// </summary>
        public bool Bit12 { get; set; }

        /// <summary>
        /// Manufacturer Specific
        /// </summary>
        public bool Bit13 { get; set; }

        /// <summary>
        /// Manufacturer Specific
        /// </summary>
        public bool Bit14 { get; set; }

        /// <summary>
        /// Manufacturer Specific
        /// </summary>
        public bool Bit15 { get; set; }

        private void Set(byte byte19, byte byte20)
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
        }

        /// <summary>
        /// Reads and sets the ControlWord
        /// </summary>
        /// <param name="s"></param>
        public void Read(Socket s)
        {
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.ControlWord, ByteNumber);
            var result = telegram.SendAndReceive(s);
            Set(result.Byte19, result.Byte20);
        }

        /// <summary>
        /// Reads and sets the ControlWord
        /// </summary>
        /// <param name="s"></param>
        /// <param name="cancellationToken"></param>
        public async Task ReadAsync(Socket s, CancellationToken cancellationToken)
        {
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.ControlWord, ByteNumber);
            var result = await telegram.SendAndReceiveAsync(s, cancellationToken);
            Set(result.Byte19, result.Byte20);
        }

        /// <summary>
        /// Writes the current ControlWord to the controller
        /// </summary>
        /// <param name="s"></param>
        public void Write(Socket s)
        {
            var bitArray19 = new BitArray(new bool[8] { Bit00, Bit01, Bit02, Bit03, Bit04, Bit05, Bit06, Bit07 });
            var bitArray20 = new BitArray(new bool[8] { Bit08, Bit09, Bit10, Bit11, Bit12, Bit13, Bit14, Bit15 });

            byte[] byte19 = new byte[1];
            bitArray19.CopyTo(byte19, 0);

            byte[] byte20 = new byte[1];
            bitArray20.CopyTo(byte20, 0);

            var telegram = new Telegram();
            telegram.Length = 21;
            telegram.Set(1, AddressConst.ControlWord, ByteNumber, byte19[0], byte20[0]);
            var result = telegram.SendAndReceive(s);
        }

        /// <summary>
        /// Writes the current ControlWord to the controller
        /// </summary>
        /// <param name="s"></param>
        /// <param name="cancellationToken"></param>
        public async Task WriteAsync(Socket s, CancellationToken cancellationToken)
        {
            var bitArray19 = new BitArray(new bool[8] { Bit00, Bit01, Bit02, Bit03, Bit04, Bit05, Bit06, Bit07 });
            var bitArray20 = new BitArray(new bool[8] { Bit08, Bit09, Bit10, Bit11, Bit12, Bit13, Bit14, Bit15 });

            byte[] byte19 = new byte[1];
            bitArray19.CopyTo(byte19, 0);

            byte[] byte20 = new byte[1];
            bitArray20.CopyTo(byte20, 0);

            var telegram = new Telegram();
            telegram.Length = 21;
            telegram.Set(1, AddressConst.ControlWord, ByteNumber, byte19[0], byte20[0]);
            var result = await telegram.SendAndReceiveAsync(s, cancellationToken);
        }

        public void Start(Socket s)
        {
            Task.Delay(TimeSpan.FromMilliseconds(250)).ConfigureAwait(true);

            // Byte 19:     // 31
            Bit00 = true; // 1
            Bit01 = true; // 2
            Bit02 = true; // 4
            Bit03 = true; // 8
            Bit04 = true; // 16
            Write(s);
        }
    }
}
