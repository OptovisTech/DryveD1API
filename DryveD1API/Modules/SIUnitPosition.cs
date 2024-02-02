using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DryveD1API.Common;

namespace DryveD1API.Modules
{
    /// <summary>
    ///
    /// </summary>
    public sealed class SiUnitPosition
    {
        private static byte ByteNumber => 4;

        /// <summary>
        ///
        /// </summary>
        public MovementTypeEnum MovementType { get => (MovementTypeEnum)MovementTypeValue; set => MovementTypeValue = (byte)value; }

        private byte MovementTypeValue { get; set; }

        private byte MultiplicationFactor { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        public void Read(Socket s)
        {
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.SiUnitPosition, ByteNumber);
            var result = telegram.SendAndReceive(s);
            MovementTypeValue = result.Byte21;
            MultiplicationFactor = result.Byte22;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <param name="cancellationToken"></param>
        public async Task ReadAsync(Socket s, CancellationToken cancellationToken)
        {
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.SiUnitPosition, ByteNumber);
            var result = await telegram.SendAndReceiveAsync(s, cancellationToken);
            MovementTypeValue = result.Byte21;
            MultiplicationFactor = result.Byte22;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        public void Write(Socket s)
        {
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.SiUnitPosition, ByteNumber, 0, 0, MovementTypeValue, MultiplicationFactor);
            var unused = telegram.SendAndReceive(s);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <param name="cancellationToken"></param>
        public async Task WriteAsync(Socket s, CancellationToken cancellationToken)
        {
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.SiUnitPosition, ByteNumber, 0, 0, MovementTypeValue, MultiplicationFactor);
            var unused = await telegram.SendAndReceiveAsync(s, cancellationToken);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="multiplicationFactor"></param>
        public void SetMultiplicationFactor(string multiplicationFactor)
        {
            MultiplicationFactor = MovementType switch
            {
                MovementTypeEnum.Linear => (byte)Enum.Parse<MultiplicationFactorLinearEnum>(multiplicationFactor),
                MovementTypeEnum.Rotary => (byte)Enum.Parse<MultiplicationFactorRotaryEnum>(multiplicationFactor),
                _ => (byte)Enum.Parse<MultiplicationFactorLinearEnum>(multiplicationFactor),
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetMultiplicationFactor()
        {
            return MovementType switch
            {
                MovementTypeEnum.Linear => ((MultiplicationFactorLinearEnum)MultiplicationFactor).ToString(),
                MovementTypeEnum.Rotary => ((MultiplicationFactorRotaryEnum)MultiplicationFactor).ToString(),
                _ => MultiplicationFactorLinearEnum.HundredthsMillimeter.ToString(),
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public double GetMultiplicationFactorValue()
        {
            return MovementType switch
            {
                MovementTypeEnum.Linear => GetMultiplicationFactorValueLinear(),
                MovementTypeEnum.Rotary => GetMultiplicationFactorValueRotary(),
                _ => 100,
            };
        }

        private double GetMultiplicationFactorValueLinear()
        {
            return (MultiplicationFactorLinearEnum)MultiplicationFactor switch
            {
                MultiplicationFactorLinearEnum.ThousandthsMillimeter => 1000,
                MultiplicationFactorLinearEnum.HundredthsMillimeter => 100,
                MultiplicationFactorLinearEnum.TenthsMillimeter => 10,
                MultiplicationFactorLinearEnum.Millimeter => 1,
                MultiplicationFactorLinearEnum.Centimeter => 0.1,
                MultiplicationFactorLinearEnum.Decimeter => 0.01,
                MultiplicationFactorLinearEnum.Meter => 0.001,
                MultiplicationFactorLinearEnum.TenMeters => 0.0001,
                MultiplicationFactorLinearEnum.HundredMeters => 0.00001,
                _ => 100,
            };
        }

        private double GetMultiplicationFactorValueRotary()
        {
            return (MultiplicationFactorRotaryEnum)MultiplicationFactor switch
            {
                MultiplicationFactorRotaryEnum.MillionthsDegree => 1000000,
                MultiplicationFactorRotaryEnum.HundredThousandthsDegree => 100000,
                MultiplicationFactorRotaryEnum.TenThousandthsDegree => 10000,
                MultiplicationFactorRotaryEnum.ThousandthsDegree => 1000,
                MultiplicationFactorRotaryEnum.HundredthsDegree => 100,
                MultiplicationFactorRotaryEnum.TenthsDegree => 10,
                MultiplicationFactorRotaryEnum.Degree => 1,
                MultiplicationFactorRotaryEnum.TenDegrees => 0.1,
                MultiplicationFactorRotaryEnum.HundredDegrees => 0.01,
                _ => 100,
            };
        }
    }

    /// <summary>
    ///
    /// </summary>
    public enum MovementTypeEnum
    {
        /// <summary>
        /// 01h
        /// </summary>
        Linear = 1, // 01h

        /// <summary>
        /// 41h
        /// </summary>
        Rotary = 65 // 41h
    }

    /// <summary>
    ///
    /// </summary>
    public enum MultiplicationFactorLinearEnum
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ThousandthsMillimeter = 250, // 1000             FAh
        HundredthsMillimeter = 251, //  100             FBh Default value
        TenthsMillimeter = 252, //   10             FCh
        Millimeter = 253, //    1             FDh
        Centimeter = 254, //    0.1           FEh
        Decimeter = 255, //    0.01          FFh
        Meter = 0, //    0.001         00h
        TenMeters = 1, //    0.0001        01h
        HundredMeters = 2, //    0.00001       02h
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    ///
    /// </summary>
    public enum MultiplicationFactorRotaryEnum
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        MillionthsDegree = 250, // 1,000,000        FAh
        HundredThousandthsDegree = 251, //   100,000        FCh
        TenThousandthsDegree = 252, //    10,000        FCh
        ThousandthsDegree = 253, //     1,000        FDh
        HundredthsDegree = 254, //       100        FEh Default value
        TenthsDegree = 255, //        10        FFh
        Degree = 0, //         1        00h
        TenDegrees = 1, //         0.1      01h
        HundredDegrees = 2, //         0.01     02h
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
