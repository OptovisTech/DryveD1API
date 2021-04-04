using System;

namespace DryveD1API
{
    public class DryveD1ObjectAddresses
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] ControlWord = { 96, 64 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] StatusWord = { 96, 65 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] ModesOfOperation = { 96, 96 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] ModesOfOperationDisplay = { 96, 97 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] PositionActualValue = { 96, 100 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] PositionWindow = { 96, 103 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] PositionWindowTime = { 96, 104 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] VelocityActualValue = { 96, 108 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] TargetPosition = { 96, 122 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] HomeOffset = { 96, 124 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] ProfileVelocity = { 96, 129 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] ProfileAcceleration = { 96, 131 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] ProfileDeceleration = { 96, 132 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] FeedConstant = { 96, 146 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] HomingMethod = { 96, 152 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] HomingSpeeds = { 96, 153 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] HomingAcceleration = { 96, 154 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] InterpolationTimePeriod = { 96, 194 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] DigitalInputs = { 96, 253 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] DigitalOutputs = { 96, 254 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] TargetVelocity = { 96, 255 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] SupportedDriveModes = { 101, 2 };
    }
}
