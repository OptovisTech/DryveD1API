using System.Net.Sockets;
using DryveD1API.Common;

namespace DryveD1API.Modules
{
    /// <summary>
    /// 6060h RWW<br />
    /// Modes of Operation
    /// </summary>
    public class ModesOfOperation
    {
        private static byte ByteNumber { get => 1; }

        /// <summary>
        /// 
        /// </summary>
        public ModesEnum currentMode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public enum ModesEnum
        {
            /// <summary>
            /// 
            /// </summary>
            NoModeAssigned = 0,
            /// <summary>
            /// The Profile Position Mode (PP) is used for the execution of positioning movements.
            /// </summary>
            ProfilePosition = 1,
            /// <summary>
            /// The Velocity Mode is used to set a motor target velocity.
            /// </summary>
            ProfileVelocity = 3,
            /// <summary>
            /// Homing is used to reach a homing (reference) point and thus specify the zero point of the axis.
            /// </summary>
            Homing = 6,
            /// <summary>
            /// The Cyclic Synchronous Position Mode (CSP) is used to implement motion control by specifying many individual position points.<br />
            /// This mode is particularly suitable for circular movements or for a synchronization of several axes.<br />
            /// Accelerations and velocities are generated internally according to the next Target Position command.
            /// </summary>
            CyclicSynchronousPosition = 8
        }

        /// <summary>
        /// Reads the current mode
        /// </summary>
        /// <param name="s"></param>
        public void Read(Socket s)
        {
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.ModesOfOperation, ByteNumber);
            var result = telegram.SendAndReceive(s);
            currentMode = (ModesEnum)result.Byte19;
        }

        /// <summary>
        /// 6061h<br />
        /// Object for feedback of current operating mode.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public ModesEnum ReadDisplay(Socket s)
        {
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.ModesOfOperationDisplay, ByteNumber);
            var result = telegram.SendAndReceive(s);
            return (ModesEnum)result.Byte19;
        }

        /// <summary>
        /// Writes the mode of operation
        /// </summary>
        /// <param name="s"></param>
        /// <param name="mode"></param>
        public void Write(Socket s, ModesEnum mode)
        {
            var telegram = new Telegram();
            telegram.Length = 20;
            telegram.Set(1, AddressConst.ModesOfOperation, ByteNumber, (byte)mode);
            var result = telegram.SendAndReceive(s);
        }
    }
}
