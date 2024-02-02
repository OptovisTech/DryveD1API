using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DryveD1API.Common;

namespace DryveD1API.Modules
{
    /// <summary>
    /// 6041h<br />
    /// Feedback of status information of the dryve D1<br />
    /// </summary>
    public sealed class StatusWord
    {
        private static byte ByteNumber { get => 2; }

        /// <summary>
        /// Ready to Switch On
        /// </summary>
        public bool Bit00 { get; private set; }

        /// <summary>
        /// Switched On
        /// </summary>
        public bool Bit01 { get; private set; }

        /// <summary>
        /// Operation Enabled
        /// </summary>
        public bool Bit02 { get; private set; }

        /// <summary>
        /// Fault
        /// </summary>
        public bool Bit03 { get; private set; }

        /// <summary>
        /// Voltage Enabled
        /// </summary>
        public bool Bit04 { get; private set; }

        /// <summary>
        /// Quick-Stop
        /// </summary>
        public bool Bit05 { get; private set; }

        /// <summary>
        /// Switch On Disabled
        /// </summary>
        public bool Bit06 { get; private set; }

        /// <summary>
        /// Warning
        /// </summary>
        public bool Bit07 { get; private set; }

        /// <summary>
        /// Manufacturer Specific
        /// </summary>
        public bool Bit08 { get; private set; }

        /// <summary>
        /// Remote(DI7)
        /// </summary>
        public bool Bit09 { get; private set; }

        /// <summary>
        /// Target Reached
        /// </summary>
        public bool Bit10 { get; private set; }

        /// <summary>
        /// Internal Limit Active
        /// </summary>
        public bool Bit11 { get; private set; }

        /// <summary>
        /// Operation Mode Specific
        /// </summary>
        public bool Bit12 { get; private set; }

        /// <summary>
        /// Operation Mode Specific
        /// </summary>
        public bool Bit13 { get; private set; }

        /// <summary>
        /// Manufacturer Specific
        /// </summary>
        public bool Bit14 { get; private set; }

        /// <summary>
        /// Manufacturer Specific
        /// </summary>
        public bool Bit15 { get; private set; }

        private void Set(byte byte19, byte byte20)
        {
            // Byte 19
            var bitArray19 = new BitArray(new[] { byte19 });
            Bit00 = bitArray19[0];
            Bit01 = bitArray19[1];
            Bit02 = bitArray19[2];
            Bit03 = bitArray19[3];
            Bit04 = bitArray19[4];
            Bit05 = bitArray19[5];
            Bit06 = bitArray19[6];
            Bit07 = bitArray19[7];

            // Byte 20
            var bitArray20 = new BitArray(new[] { byte20 });
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
        /// Reads and sets the StatusWord
        /// </summary>
        /// <param name="s"></param>
        public void Read(Socket s)
        {
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.StatusWord, ByteNumber);
            var result = telegram.SendAndReceive(s);
            Set(result.Byte19, result.Byte20);
        }

        /// <summary>
        /// Reads and sets the StatusWord
        /// </summary>
        /// <param name="s"></param>
        /// <param name="cancellationToken"></param>
        public async Task ReadAsync(Socket s, CancellationToken cancellationToken)
        {
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.StatusWord, ByteNumber);
            var result = await telegram.SendAndReceiveAsync(s, cancellationToken);
            Set(result.Byte19, result.Byte20);
        }
    }
}
