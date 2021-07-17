using System;
using System.Net.Sockets;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace DryveD1API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        /// <summary>
        /// 6092h sub1<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("FeedRate/{hostIp}/{port}")]
        public double GetFeedRate(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.FeedConstantFeed, 4);
            var response = telegram.SendAndReceive(connection.socket);
            var result = BitConverter.ToUInt32(new byte[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) / connection.MultiplicationFactor;
            return result;
        }

        /// <summary>
        /// 6092h sub1<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="feedRate"></param>
        [HttpPut("FeedRate/{hostIp}/{port}")]
        public void SetFeedRate(string hostIp, int port, [FromBody] double feedRate)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            byte[] data = BitConverter.GetBytes((uint)(feedRate * connection.MultiplicationFactor));
            var telegram = new Telegram();
            telegram.Length = 23;
            telegram.Set(1, AddressConst.FeedConstantFeed, 4, data[0], data[1], data[2], data[3]);
            var response = telegram.SendAndReceive(connection.socket);
        }

        /// <summary>
        /// 6092h sub2<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("ShaftRevolutions/{hostIp}/{port}")]
        public uint GetShaftRevolutions(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.FeedConstantShaftRevolution, 4);
            var response = telegram.SendAndReceive(connection.socket);
            var result = BitConverter.ToUInt32(new byte[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
            return result;
        }

        /// <summary>
        /// 6092h sub2<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="shaftRevolutions"></param>
        [HttpPut("ShaftRevolutions/{hostIp}/{port}")]
        public void SetShaftRevolutions(string hostIp, int port, [FromBody] uint shaftRevolutions)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            byte[] data = BitConverter.GetBytes(shaftRevolutions);
            var telegram = new Telegram();
            telegram.Length = 23;
            telegram.Set(1, AddressConst.FeedConstantShaftRevolution, 4, data[0], data[1], data[2], data[3]);
            var response = telegram.SendAndReceive(connection.socket);
        }

        /// <summary>
        /// 60A8h<br />
        /// Movement type and multiplication factor.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("SIUnitPosition/{hostIp}/{port}")]
        public string[] GetSIUnitPosition(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var siUnitPosition = new SIUnitPosition();
            siUnitPosition.Read(connection.socket);
            return new string[2] { siUnitPosition.MovementType.ToString(), siUnitPosition.GetMultiplicationFactor() };
        }

        /// <summary>
        /// Movement type and multiplication factor.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="movementTypeString"></param>
        /// <param name="multiplicationFactorString"></param>
        [HttpPut("SIUnitPosition/{hostIp}/{port}/{MovementType}/{MultiplicationFactor}")]
        public void SetSIUnitPosition(string hostIp, int port, string movementTypeString, string multiplicationFactorString)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var siUnitPosition = new SIUnitPosition();
            if (Enum.TryParse(movementTypeString, out MovementTypeEnum movementType))
            {
                siUnitPosition.MovementType = movementType;
                siUnitPosition.SetMultiplicationFactor(multiplicationFactorString);
                siUnitPosition.Write(connection.socket);
                connection.MultiplicationFactor = siUnitPosition.GetMultiplicationFactorValue();
            }
            else
            {
                throw new Exception("Wrong MovementTypeValue");
            }
        }
    }
}
