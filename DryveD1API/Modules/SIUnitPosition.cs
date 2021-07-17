using System;
using System.Net.Sockets;
using DryveD1API.Common;

namespace DryveD1API.Modules
{
    public class SIUnitPosition
    {
        private static byte ByteNumber { get => 4; }

        public MovementTypeEnum MovementType
        {
            get
            {
                return (MovementTypeEnum)MovementTypeValue;
            }
            set
            {
                MovementTypeValue = (byte)value;
            }
        }

        private byte MovementTypeValue { get; set; }

        private byte MultiplicationFactor { get; set; }

        public void Read(Socket s)
        {
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.SIUnitPosition, ByteNumber);
            var result = telegram.SendAndReceive(s);
            MovementTypeValue = result.Byte21;
            MultiplicationFactor = result.Byte22;
        }

        public void Write(Socket s)
        {
            var telegram = new Telegram();
            telegram.Length = 23;
            telegram.Set(1, AddressConst.SIUnitPosition, ByteNumber, 0, 0, MovementTypeValue, MultiplicationFactor);
            var result = telegram.SendAndReceive(s);
        }

        public void SetMultiplicationFactor(string multiplicationFactor)
        {
            MultiplicationFactor = MovementType switch
            {
                MovementTypeEnum.Linear => (byte)Enum.Parse<MultiplicationFactorLiniarEnum>(multiplicationFactor),
                MovementTypeEnum.Rotary => (byte)Enum.Parse<MultiplicationFactorRotaryEnum>(multiplicationFactor),
                _ => (byte)Enum.Parse<MultiplicationFactorLiniarEnum>(multiplicationFactor),
            };
        }

        public string GetMultiplicationFactor()
        {
            return MovementType switch
            {
                MovementTypeEnum.Linear => ((MultiplicationFactorLiniarEnum)MultiplicationFactor).ToString(),
                MovementTypeEnum.Rotary => ((MultiplicationFactorRotaryEnum)MultiplicationFactor).ToString(),
                _ => MultiplicationFactorLiniarEnum.ThousandthsMillimeter.ToString(),
            };
        }
    }


    public enum MovementTypeEnum
    {
        Linear = 1,                     // 01h
        Rotary = 65                     // 41h
    }

    public enum MultiplicationFactorLiniarEnum
    {
        ThousandthsMillimeter = 250,    // 1000             FAh
        HundredthsMillimeter = 251,     //  100             FBh
        TenthsMillimeter = 252,         //   10             FCh
        Millimeter = 253,               //    1             FDh
        Centimeter = 254,               //    0.1           FEh
        Decimeter = 255,                //    0.01          FFh
        Meter = 0,                      //    0.001         00h
        TenMeters = 1,                  //    0.0001        01h
        HundredMeters = 2,              //    0.00001       02h
    }

    public enum MultiplicationFactorRotaryEnum
    {
        MillionthsDegree = 250,         // 1,000,000        FAh
        HundredThousandthsDegree = 251, //   100,000        FCh
        TenThousandthsDegree = 252,     //    10,000        FCh
        ThousandthsDegree = 253,        //     1,000        FDh
        HundredthsDegree = 254,         //       100        FEh
        TenthsDegree = 255,             //        10        FFh
        Degree = 0,                     //         1        00h
        TenDegrees = 1,                 //         0.1      01h
        HundredDegrees = 2,             //         0.01     02h
    }
}
