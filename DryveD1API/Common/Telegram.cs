using System.Net.Sockets;

namespace DryveD1API.Common
{
    /// <summary>
    /// Length has to be set if value is not 19
    /// </summary>
    public class Telegram
    {
        /// <summary>
        /// 19-23
        /// </summary>
        public int Length { get; set; } = 19;

        private byte[] data { get => FillData(); }

        /// Big Endian

        /// <summary>
        /// Transaction Identifier.
        /// Value: 0
        /// </summary>
        private byte Byte00 { get => 0; }
        /// <summary>
        /// Transaction Identifier.
        /// Value: 0
        /// </summary>
        private byte Byte01 { get => 0; }
        /// <summary>
        /// Protocol Identifier.
        /// Value: 0
        /// </summary>
        private byte Byte02 { get => 0; }
        /// <summary>
        /// Protocol Identifier.
        /// Value: 0
        /// </summary>
        private byte Byte03 { get => 0; }
        /// <summary>
        /// Lenght.
        /// Value: 0
        /// </summary>
        private byte Byte04 { get => 0; }
        /// <summary>
        /// Lenght.
        /// Value: 13-17
        /// </summary>
        private byte Byte05 { get => (byte)(Length - 6); }
        /// <summary>
        /// Unit Identifier.
        /// Value: 0
        /// </summary>
        private byte Byte06 { get => 0; }
        /// <summary>
        /// Function code.
        /// Value: 43
        /// </summary>
        private byte Byte07 { get => 43; }
        /// <summary>
        /// MEI type.
        /// Value: 13
        /// </summary>
        private byte Byte08 { get => 13; }
        /// <summary>
        /// Protocol control.
        /// Value: 0 = Read / 1 = Write
        /// </summary>
        private byte Byte09 { get; set; }
        /// <summary>
        /// Protocol option field.
        /// Value: 0
        /// </summary>
        private byte Byte10 { get => 0; }
        /// <summary>
        /// Node ID.
        /// Value: 0
        /// </summary>
        private byte Byte11 { get => 0; }
        /// <summary>
        /// Object Index 1.
        /// Value: SDO Object
        /// </summary>
        private byte Byte12 { get; set; }
        /// <summary>
        /// Object Index 2.
        /// Value: SDO Object
        /// </summary>
        private byte Byte13 { get; set; }
        /// <summary>
        /// Sub Index.
        /// Value: SDO Object
        /// </summary>
        private byte Byte14 { get; set; }
        /// <summary>
        /// Starting Address.
        /// Value: 0
        /// </summary>
        private byte Byte15 { get => 0; }
        /// <summary>
        /// Starting Address.
        /// Value: 0
        /// </summary>
        private byte Byte16 { get => 0; }
        /// <summary>
        /// SDO Object.
        /// Value: 0
        /// </summary>
        private byte Byte17 { get => 0; }
        /// <summary>
        /// Byte Number.
        /// Value: 1-4
        /// </summary>
        private byte Byte18 { get; set; }

        /// Little Endian

        /// <summary>
        /// Data Field
        /// </summary>
        public byte Byte19 { get; set; }
        /// <summary>
        /// Data Field
        /// </summary>
        public byte Byte20 { get; set; }
        /// <summary>
        /// Data Field
        /// </summary>
        public byte Byte21 { get; set; }
        /// <summary>
        /// Data Field
        /// </summary>
        public byte Byte22 { get; set; }

        /// <summary>
        /// Creates the final telegram.
        /// </summary>
        /// <param name="byte09">OperationType: 0 = Read / 1 = Write</param>
        /// <param name="byte12">Object index 1.</param>
        /// <param name="byte13">Object index 2</param>
        /// <param name="byte18">Byte number</param>
        /// <param name="byte14">Sub index</param>
        /// <param name="byte19">Data field 1</param>
        /// <param name="byte20">Data field 2</param>
        /// <param name="byte21">Data field 3</param>
        /// <param name="byte22">Data field 4</param>
        /// <returns>The final telegram.</returns>
        public void Set(byte byte09, byte[] address, byte byte18, byte byte19 = 0, byte byte20 = 0, byte byte21 = 0, byte byte22 = 0)
        {
            Byte09 = byte09;
            Byte12 = address[0];
            Byte13 = address[1];
            if (address.Length == 3)
            {
                Byte14 = address[2];
            }
            else
            {
                Byte14 = 0;
            }
            Byte18 = byte18;
            Byte19 = byte19;
            Byte20 = byte20;
            Byte21 = byte21;
            Byte22 = byte22;
        }

        private byte[] FillData()
        {
            if (Length > 23)
            {
                Length = 23;
            }
            byte[] telegram = new byte[Length];
            telegram[0] = Byte00;
            telegram[1] = Byte01;
            telegram[2] = Byte02;
            telegram[3] = Byte03;
            telegram[4] = Byte04;
            telegram[5] = Byte05;
            telegram[6] = Byte06;
            telegram[7] = Byte07;
            telegram[8] = Byte08;
            telegram[9] = Byte09;
            telegram[10] = Byte10;
            telegram[11] = Byte11;
            telegram[12] = Byte12;
            telegram[13] = Byte13;
            telegram[14] = Byte14;
            telegram[15] = Byte15;
            telegram[16] = Byte16;
            telegram[17] = Byte17;
            telegram[18] = Byte18;
            if (Length >= 20)
            {
                telegram[19] = Byte19;
            }
            if (Length >= 21)
            {
                telegram[20] = Byte20;
            }
            if (Length >= 22)
            {
                telegram[21] = Byte21;
            }
            if (Length == 23)
            {
                telegram[22] = Byte22;
            }
            return telegram;
        }

        private void FillBytes(byte[] data)
        {
            Byte09 = data[9];
            Byte12 = data[12];
            Byte13 = data[13];
            Byte14 = data[14];
            Byte18 = data[18];
            Byte19 = data[19];
            Byte20 = data[20];
            Byte21 = data[21];
            Byte22 = data[22];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">Socket</param>
        /// <returns>response</returns>
        public Telegram SendAndReceive(Socket s)
        {
            // Send request to the server.
            _ = s.Send(data, data.Length, 0);
            // Receive data from the server.
            byte[] bytesReceived = new byte[23];
            _ = s.Receive(bytesReceived, bytesReceived.Length, 0);
            Telegram response = new Telegram();
            response.FillBytes(bytesReceived);
            response.Length = bytesReceived[5] + 6;
            return response;
        }
    }
}