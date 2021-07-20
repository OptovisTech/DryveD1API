using System;
using System.Net.Sockets;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace DryveD1API.Controllers
{
    /// <summary>
    ///  Read and write configuration specific values.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {

        /// <summary>
        /// 6091h sub1<br />
        /// Motor shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("GearRatioMotorShaftRevolutions/{hostIp}/{port}")]
        public uint GetGearRatioMotorShaftRevolutions(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.GearRatioMotorShaftRevolutions, 4);
            var response = telegram.SendAndReceive(connection.socket);
            var result = BitConverter.ToUInt32(new byte[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
            return result;
        }

        /// <summary>
        /// 6091h sub1<br />
        /// Motor shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="feedRate"></param>
        [HttpPut("GearRatioMotorShaftRevolutions/{hostIp}/{port}")]
        public void SetGearRatioMotorShaftRevolutions(string hostIp, int port, [FromBody] uint motorShaftRevolutions)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            byte[] data = BitConverter.GetBytes(motorShaftRevolutions);
            var telegram = new Telegram();
            telegram.Length = 23;
            telegram.Set(1, AddressConst.GearRatioMotorShaftRevolutions, 4, data[0], data[1], data[2], data[3]);
            var response = telegram.SendAndReceive(connection.socket);
        }

        /// <summary>
        /// 6091h sub2<br />
        /// Driving shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("GearRatioDrivingShaftRevolutions/{hostIp}/{port}")]
        public uint GetGearRatioDrivingShaftRevolutions(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.GearRatioDrivingShaftRevolutions, 4);
            var response = telegram.SendAndReceive(connection.socket);
            var result = BitConverter.ToUInt32(new byte[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
            return result;
        }

        /// <summary>
        /// 6091h sub2<br />
        /// Driving shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="feedRate"></param>
        [HttpPut("GearRatioDrivingShaftRevolutions/{hostIp}/{port}")]
        public void SetGearRatioDrivingShaftRevolutions(string hostIp, int port, [FromBody] uint drivingShaftRevolutions)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            byte[] data = BitConverter.GetBytes(drivingShaftRevolutions);
            var telegram = new Telegram();
            telegram.Length = 23;
            telegram.Set(1, AddressConst.GearRatioDrivingShaftRevolutions, 4, data[0], data[1], data[2], data[3]);
            var response = telegram.SendAndReceive(connection.socket);
        }

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
        [HttpPut("SIUnitPosition/{hostIp}/{port}")]
        public void SetSIUnitPosition(string hostIp, int port, [FromBody] string[] data)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var siUnitPosition = new SIUnitPosition();
            if (Enum.TryParse(data[0], out MovementTypeEnum movementType))
            {
                siUnitPosition.MovementType = movementType;
                siUnitPosition.SetMultiplicationFactor(data[1]);
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
