using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using DryveD1API.Common;

namespace DryveD1API.Modules
{
    /// <summary>
    /// 2010h<br />
    /// Activation of the input signal negation of digital inputs.
    /// </summary>
    public class DigitalInputPolarity
    {
        private static byte ByteNumber { get => 4; }

        public bool DI01 { get; set; }
        public bool DI02 { get; set; }
        public bool DI03 { get; set; }
        public bool DI04 { get; set; }
        public bool DI05 { get; set; }
        public bool DI06 { get; set; }
        public bool DI07 { get; set; }
        public bool DI08 { get; set; }
        public bool DI09 { get; set; }
        public bool DI10 { get; set; }
        public bool DI11 { get; set; }
        public bool DI12 { get; set; }
        public bool DI13 { get; set; }
        public bool DI14 { get; set; }
        public bool DI15 { get; set; }
        public bool DI16 { get; set; }


        private void Set(byte byte19, byte byte20)
        {
            // Byte 19
            var bitArray19 = new BitArray(new byte[] { byte19 });
            DI01 = bitArray19[0];
            DI02 = bitArray19[1];
            DI03 = bitArray19[2];
            DI04 = bitArray19[3];
            DI05 = bitArray19[4];
            DI06 = bitArray19[5];
            DI07 = bitArray19[6];
            DI08 = bitArray19[7];

            // Byte 20
            var bitArray20 = new BitArray(new byte[] { byte20 });
            DI09 = bitArray20[0];
            DI10 = bitArray20[1];
            DI11 = bitArray20[2];
            DI12 = bitArray20[3];
            DI13 = bitArray20[4];
            DI14 = bitArray20[5];
            DI15 = bitArray20[6];
            DI16 = bitArray20[7];
        }

        /// <summary>
        /// Reads and sets the ControlWord
        /// </summary>
        /// <param name="s"></param>
        public void Read(Socket s)
        {
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.DigitalInputPolarity, ByteNumber);
            var result = telegram.SendAndReceive(s);
            Set(result.Byte19, result.Byte20);
        }

        /// <summary>
        /// Writes the current ControlWord to the controller
        /// </summary>
        /// <param name="s"></param>
        public void Write(Socket s)
        {
            var bitArray19 = new BitArray(new bool[8] { DI01, DI02, DI03, DI04, DI05, DI06, DI07, DI08 });
            var bitArray20 = new BitArray(new bool[8] { DI09, DI10, DI11, DI12, DI13, DI14, DI15, DI16 });

            byte[] byte19 = new byte[1];
            bitArray19.CopyTo(byte19, 0);

            byte[] byte20 = new byte[1];
            bitArray20.CopyTo(byte20, 0);

            var telegram = new Telegram();
            telegram.Length = 21;
            telegram.Set(1, AddressConst.DigitalInputPolarity, ByteNumber, byte19[0], byte20[0], 0, 0);
            var result = telegram.SendAndReceive(s);
        }

        public bool ToggleDigitalInput(Socket s, int digitalInput)
        {
            Read(s);
            bool firstState = NegateDigitalInput(digitalInput);
            Write(s);
            Thread.Sleep(100);


            Read(s);
            bool secondState = NegateDigitalInput(digitalInput);

            if (secondState == firstState)
            {
                return false;
            }
            Write(s);
            Thread.Sleep(100);
            Read(s);
            bool thirdState = NegateDigitalInput(digitalInput);
            if (thirdState != firstState)
            {
                return false;
            }
            return true;

        }

        private bool NegateDigitalInput(int digitalInput)
        {
            switch (digitalInput)
            {
                case 1: DI01 = !DI01; return !DI01;
                case 2: DI02 = !DI02; return !DI02;
                case 3: DI03 = !DI03; return !DI03;
                case 4: DI04 = !DI04; return !DI04;
                case 5: DI05 = !DI05; return !DI05;
                case 6: DI06 = !DI06; return !DI06;
                case 7: DI07 = !DI07; return !DI07;
                case 8: DI08 = !DI08; return !DI08;
                case 9: DI09 = !DI09; return !DI09;
                case 10: DI10 = !DI10; return !DI10;
                case 11: DI11 = !DI11; return !DI11;
                case 12: DI12 = !DI12; return !DI12;
                case 13: DI13 = !DI13; return !DI13;
                case 14: DI14 = !DI14; return !DI14;
                case 15: DI15 = !DI15; return !DI15;
                case 16: DI16 = !DI16; return !DI16;
                default: return false;
            }
        }
    }
}
