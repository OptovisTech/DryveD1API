namespace DryveD1API.Common
{
    /// <summary>
    /// Object Index 1.
    /// Object Index 2.
    /// Sub Index.
    /// </summary>
    public class AddressConst
    {
        /// <summary>
        /// 6040h<br />
        /// Object for controlling the dryve D1.
        /// </summary>
        public static readonly byte[] ControlWord = { 96, 64 };

        /// <summary>
        /// 6041h<br />
        /// Feedback of status information of the dryve D1.
        /// </summary>
        public static readonly byte[] StatusWord = { 96, 65 };

        /// <summary>
        /// 6060h<br />
        /// Preselection of the operating mode.
        /// </summary>
        public static readonly byte[] ModesOfOperation = { 96, 96 };

        /// <summary>
        /// 6061h<br />
        /// Object for feedback of current operating mode.
        /// </summary>
        public static readonly byte[] ModesOfOperationDisplay = { 96, 97 };

        /// <summary>
        /// 6064h<br />
        /// Indication of the current position of the position encoder.
        /// </summary>
        public static readonly byte[] PositionActualValue = { 96, 100 };

        /// <summary>
        /// 6067h<br />
        /// Indication of a symmetrical area around the target point.<br />
        /// If this area is reached, the target position can be regarded as reached.
        /// </summary>
        public static readonly byte[] PositionWindow = { 96, 103 };

        /// <summary>
        /// 6068h<br />
        /// Indication of a time delay before a "Target Reached" signal can be output.<br />
        /// The time is counted from the moment when the position window (6067h) is reached.
        /// </summary>
        public static readonly byte[] PositionWindowTime = { 96, 104 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] VelocityActualValue = { 96, 108 };

        /// <summary>
        /// 607Ah<br />
        /// Goal position to be reached.
        /// </summary>
        public static readonly byte[] TargetPosition = { 96, 122 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] HomeOffset = { 96, 124 };

        /// <summary>
        /// 6081h<br />
        /// Indication of the speed.
        /// </summary>
        public static readonly byte[] ProfileVelocity = { 96, 129 };

        /// <summary>
        /// 6083h<br />
        /// Indication of acceleration.
        /// </summary>
        public static readonly byte[] ProfileAcceleration = { 96, 131 };

        /// <summary>
        /// 6084h<br />
        /// Indication of deceleration.
        /// </summary>
        public static readonly byte[] ProfileDeceleration = { 96, 132 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] FeedConstant = { 96, 146, 3 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] FeedRate = { 96, 146, 1 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] ShaftRevolution = { 96, 146, 2 };

        /// <summary>
        /// 
        /// </summary>
        public static readonly byte[] HomingMethod = { 96, 152 };

        /// <summary>
        /// Stipulation of the travel speeds during the homing run (6098h).
        /// </summary>
        public static readonly byte[] HomingSpeeds = { 96, 153, 3 };

        /// <summary>
        /// 6099h sub0<br />
        /// Number of Entries.
        /// </summary>
        public static readonly byte[] HomingNumberOfEntries = { 96, 153, 0 };

        /// <summary>
        /// 6099h sub1<br />
        /// Speeds during Search for Switch.
        /// </summary>
        public static readonly byte[] HomingSwitchSearchVelocity = { 96, 153, 1 };

        /// <summary>
        /// 6099h sub2<br />
        /// Speeds during Search for Zero.
        /// </summary>
        public static readonly byte[] HomingZeroSearchVelocity = { 96, 153, 2 };

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
