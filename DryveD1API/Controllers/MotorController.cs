using System;
using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DryveD1API.Controllers
{
    /// <summary>
    /// Read and write motor specific values.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MotorController : ControllerBase
    {

        /// <summary>
        /// 6064h<br />
        /// Indication of the current position of the position encoder.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>The actual value of the position encoder</returns>
        [HttpGet("ActualPosition/{hostIp}/{port}")]
        public int GetActualPosition(string hostIp, int port)
        {
            using (Socket s = ModbusSocket.Connect(hostIp, port))
            {
                var telegram = new Telegram();
                telegram.Set(0, 96, 100, 4);
                var response = telegram.SendAndReceive(s);
                ModbusSocket.Close(s);
                var result = BitConverter.ToInt32(new byte[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
                return result;
            }
        }

        /// <summary>
        /// 6067h<br />
        /// Indication of a symmetrical area around the target point.<br />
        /// If this area is reached, the target position can be regarded as reached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>The position window</returns>
        [HttpGet("PositionWindow/{hostIp}/{port}")]
        public int GetPositionWindow(string hostIp, int port)
        {
            using (Socket s = ModbusSocket.Connect(hostIp, port))
            {
                var telegram = new Telegram();
                telegram.Set(0, 96, 103, 4);
                var response = telegram.SendAndReceive(s);
                ModbusSocket.Close(s);
                var result = BitConverter.ToInt32(new byte[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
                return result;
            }
        }

        /// <summary>
        /// 6068h<br />
        /// Indication of a time delay before a "Target Reached" signal can be output.<br />
        /// The time is counted from the moment when the position window (6067h) is reached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>The position window time</returns>
        [HttpGet("PositionWindowTime/{hostIp}/{port}")]
        public ushort GetPositionWindowTime(string hostIp, int port)
        {
            using (Socket s = ModbusSocket.Connect(hostIp, port))
            {
                var telegram = new Telegram();
                telegram.Set(0, 96, 104, 2);
                var response = telegram.SendAndReceive(s);
                ModbusSocket.Close(s);
                ushort result = BitConverter.ToUInt16(new byte[] { response.Byte19, response.Byte20 }, 0);
                return result;
            }
        }

        /// <summary>
        /// 607Ah<br />
        /// Goal position to be reached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>The currently set target position</returns>
        [HttpGet("TargetPosition/{hostIp}/{port}")]
        public int GetTargetPosition(string hostIp, int port)
        {
            using (Socket s = ModbusSocket.Connect(hostIp, port))
            {
                var telegram = new Telegram();
                telegram.Set(0, 96, 122, 4);
                var response = telegram.SendAndReceive(s);
                ModbusSocket.Close(s);
                int result = BitConverter.ToInt32(new byte[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
                return result;
            }
        }

        /// <summary>
        /// 607Ah<br />
        /// Goal position to be reached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="targetPosition">The target position to be set</param>
        [HttpPut("TargetPosition/{hostIp}/{port}")]
        public void SetTargetPosition(string hostIp, int port, [FromBody] int targetPosition)
        {
            using (Socket s = ModbusSocket.Connect(hostIp, port))
            {
                byte[] data = BitConverter.GetBytes(targetPosition);
                var telegram = new Telegram();
                telegram.Set(1, 96, 122, 4, 0, data[0], data[1], data[2], data[3]);
                var response = telegram.SendAndReceive(s);
                ModbusSocket.Close(s);
            }
        }
    }
}