using System;
using System.Threading;
using System.Threading.Tasks;
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
        /// 607Bh sub1<br />
        /// Position range limit min.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="positionRangeLimitMin"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("SetPositionRangeLimitMinAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetPositionRangeLimitMinAsync(string hostIp, int port, [FromBody] double positionRangeLimitMin,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes((int)(positionRangeLimitMin * connection.MultiplicationFactor));
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.PositionRangeLimitMin, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 607Bh sub2<br />
        /// Position range limit max.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="positionRangeLimitMax"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("SetPositionRangeLimitMaxAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetPositionRangeLimitMaxAsync(string hostIp, int port, [FromBody] double positionRangeLimitMax,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes((int)(positionRangeLimitMax * connection.MultiplicationFactor));
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.PositionRangeLimitMax, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6091h sub1<br />
        /// Motor shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("GearRatioMotorShaftRevolutions/{hostIp}/{port:int}")]
        public uint GetGearRatioMotorShaftRevolutions(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.GearRatioMotorShaftRevolutions, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
            return result;
        }

        /// <summary>
        /// 6091h sub1<br />
        /// Motor shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("GearRatioMotorShaftRevolutionsAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetGearRatioMotorShaftRevolutionsAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.GearRatioMotorShaftRevolutions, 4);
                var response = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6091h sub1<br />
        /// Motor shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="motorShaftRevolutions"></param>
        [HttpPut("GearRatioMotorShaftRevolutions/{hostIp}/{port:int}")]
        public void SetGearRatioMotorShaftRevolutions(string hostIp, int port, [FromBody] uint motorShaftRevolutions)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes(motorShaftRevolutions);
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.GearRatioMotorShaftRevolutions, 4, data[0], data[1], data[2], data[3]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 6091h sub1<br />
        /// Motor shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="motorShaftRevolutions"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("GearRatioMotorShaftRevolutionsAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetGearRatioMotorShaftRevolutionsAsync(string hostIp, int port, [FromBody] uint motorShaftRevolutions,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes(motorShaftRevolutions);
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.GearRatioMotorShaftRevolutions, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6091h sub2<br />
        /// Driving shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("GearRatioDrivingShaftRevolutions/{hostIp}/{port:int}")]
        public uint GetGearRatioDrivingShaftRevolutions(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.GearRatioDrivingShaftRevolutions, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
            return result;
        }

        /// <summary>
        /// 6091h sub2<br />
        /// Driving shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("GearRatioDrivingShaftRevolutionsAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetGearRatioDrivingShaftRevolutionsAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.GearRatioDrivingShaftRevolutions, 4);
                var response = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6091h sub2<br />
        /// Driving shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="drivingShaftRevolutions"></param>
        [HttpPut("GearRatioDrivingShaftRevolutions/{hostIp}/{port:int}")]
        public void SetGearRatioDrivingShaftRevolutions(string hostIp, int port, [FromBody] uint drivingShaftRevolutions)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes(drivingShaftRevolutions);
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.GearRatioDrivingShaftRevolutions, 4, data[0], data[1], data[2], data[3]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 6091h sub2<br />
        /// Driving shaft revolutions.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="drivingShaftRevolutions"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("GearRatioDrivingShaftRevolutionsAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetGearRatioDrivingShaftRevolutionsAsync(string hostIp, int port, [FromBody] uint drivingShaftRevolutions,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes(drivingShaftRevolutions);
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.GearRatioDrivingShaftRevolutions, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6092h sub1<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("FeedRate/{hostIp}/{port:int}")]
        public double GetFeedRate(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.FeedConstantFeed, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                         connection.MultiplicationFactor;
            return result;
        }

        /// <summary>
        /// 6092h sub1<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("FeedRateAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetFeedRateAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.FeedConstantFeed, 4);
                var response = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                             connection.MultiplicationFactor;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6092h sub1<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="feedRate"></param>
        [HttpPut("FeedRate/{hostIp}/{port:int}")]
        public void SetFeedRate(string hostIp, int port, [FromBody] double feedRate)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes((uint)(feedRate * connection.MultiplicationFactor));
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.FeedConstantFeed, 4, data[0], data[1], data[2], data[3]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 6092h sub1<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="feedRate"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("FeedRateAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetFeedRateAsync(string hostIp, int port, [FromBody] double feedRate, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes((uint)(feedRate * connection.MultiplicationFactor));
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.FeedConstantFeed, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6092h sub2<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("ShaftRevolutions/{hostIp}/{port:int}")]
        public uint GetShaftRevolutions(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.FeedConstantShaftRevolution, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
            return result;
        }

        /// <summary>
        /// 6092h sub2<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("ShaftRevolutionsAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetShaftRevolutionsAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.FeedConstantShaftRevolution, 4);
                var response = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6092h sub2<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="shaftRevolutions"></param>
        [HttpPut("ShaftRevolutions/{hostIp}/{port:int}")]
        public void SetShaftRevolutions(string hostIp, int port, [FromBody] uint shaftRevolutions)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes(shaftRevolutions);
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.FeedConstantShaftRevolution, 4, data[0], data[1], data[2], data[3]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 6092h sub2<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="shaftRevolutions"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("ShaftRevolutionsAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetShaftRevolutionsAsync(string hostIp, int port, [FromBody] uint shaftRevolutions,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes(shaftRevolutions);
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.FeedConstantShaftRevolution, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 60A8h<br />
        /// Movement type and multiplication factor.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("SIUnitPosition/{hostIp}/{port:int}")]
        public string[] GetSiUnitPosition(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var siUnitPosition = new SiUnitPosition();
            siUnitPosition.Read(connection.Socket);
            return new[] { siUnitPosition.MovementType.ToString(), siUnitPosition.GetMultiplicationFactor() };
        }

        /// <summary>
        /// 60A8h<br />
        /// Movement type and multiplication factor.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("SIUnitPositionAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetSiUnitPositionAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var siUnitPosition = new SiUnitPosition();
                await siUnitPosition.ReadAsync(connection.Socket, cancellationToken);
                return Ok(new[] { siUnitPosition.MovementType.ToString(), siUnitPosition.GetMultiplicationFactor() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Movement type and multiplication factor.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="data">[0] = MovementType | [1] = MultiplicationFactor</param>
        [HttpPut("SIUnitPosition/{hostIp}/{port:int}")]
        public void SetSiUnitPosition(string hostIp, int port, [FromBody] string[] data)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var siUnitPosition = new SiUnitPosition();
            if (Enum.TryParse(data[0], out MovementTypeEnum movementType))
            {
                siUnitPosition.MovementType = movementType;
                siUnitPosition.SetMultiplicationFactor(data[1]);
                siUnitPosition.Write(connection.Socket);
                connection.MultiplicationFactor = siUnitPosition.GetMultiplicationFactorValue();
            }
            else
            {
                throw new Exception("Wrong MovementTypeValue");
            }
        }

        /// <summary>
        /// Movement type and multiplication factor.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="data">[0] = MovementType | [1] = MultiplicationFactor</param>
        /// <param name="cancellationToken"></param>
        [HttpPut("SIUnitPositionAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetSiUnitPositionAsync(string hostIp, int port, [FromBody] string[] data,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var siUnitPosition = new SiUnitPosition();
                if (Enum.TryParse(data[0], out MovementTypeEnum movementType))
                {
                    siUnitPosition.MovementType = movementType;
                    siUnitPosition.SetMultiplicationFactor(data[1]);
                    await siUnitPosition.WriteAsync(connection.Socket, cancellationToken);
                    connection.MultiplicationFactor = siUnitPosition.GetMultiplicationFactorValue();
                }
                else
                {
                    throw new Exception("Wrong MovementTypeValue");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
