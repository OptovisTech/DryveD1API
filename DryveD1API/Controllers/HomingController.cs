using System;
using System.Threading;
using System.Threading.Tasks;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DryveD1API.Controllers
{
    /// <summary>
    ///  Read and write homing specific values.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HomingController : ControllerBase
    {
        /// <summary>
        /// This method starts the homing process
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>True when homing is finished</returns>
        [HttpGet("{hostIp}/{port:int}")]
        public bool Homing(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var modesOfOperation = new ModesOfOperation();
            modesOfOperation.Write(connection.Socket, ModesOfOperation.ModesEnum.Homing);
            while (modesOfOperation.ReadDisplay(connection.Socket) != ModesOfOperation.ModesEnum.Homing)
            {
                Thread.Sleep(100);
            }

            var controlWord = new ControlWord
            {
                // Byte 19: 31
                Bit00 = true, // 1
                Bit01 = true, // 2
                Bit02 = true, // 4
                Bit03 = true, // 8
                Bit04 = true // 16
            };
            controlWord.Write(connection.Socket);

            var statusWord = new StatusWord();
            while (!(statusWord.Bit00 && statusWord.Bit01 && statusWord.Bit02 && statusWord.Bit05 // 39
                     && statusWord.Bit09 && statusWord.Bit10 && statusWord.Bit12)) // 22
            {
                statusWord.Read(connection.Socket);
                Thread.Sleep(100);
            }

            return true;
        }

        /// <summary>
        /// This method starts the homing process
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns>True when homing is finished</returns>
        [HttpGet("Async/{hostIp}/{port:int}")]
        public async Task<IActionResult> HomingAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var modesOfOperation = new ModesOfOperation();
                await modesOfOperation.WriteAsync(connection.Socket, ModesOfOperation.ModesEnum.Homing, cancellationToken);
                while (await modesOfOperation.ReadDisplayAsync(connection.Socket, cancellationToken) != ModesOfOperation.ModesEnum.Homing)
                {
                    await Task.Delay(100, cancellationToken);
                }

                var controlWord = new ControlWord
                {
                    // Byte 19: 31
                    Bit00 = true, // 1
                    Bit01 = true, // 2
                    Bit02 = true, // 4
                    Bit03 = true, // 8
                    Bit04 = true // 16
                };
                await controlWord.WriteAsync(connection.Socket, cancellationToken);

                var statusWord = new StatusWord();
                while (!(statusWord.Bit00 && statusWord.Bit01 && statusWord.Bit02 && statusWord.Bit05 // 39
                         && statusWord.Bit09 && statusWord.Bit10 && statusWord.Bit12)) // 22
                {
                    await statusWord.ReadAsync(connection.Socket, cancellationToken);
                    await Task.Delay(100, cancellationToken);
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6084h<br />
        /// Indication of deceleration.
        /// </summary>
        /// <param name="hostIp"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        [HttpGet("Method/{hostIp}/{port:int}")]
        public sbyte GetMethod(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.HomingMethod, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = (sbyte)response.Byte19;
            return result;
        }

        /// <summary>
        /// 6084h<br />
        /// Indication of deceleration.
        /// </summary>
        /// <param name="hostIp"></param>
        /// <param name="port"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("MethodAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetMethodAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.HomingMethod, 4);
                var response = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                var result = (sbyte)response.Byte19;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6084h<br />
        /// Indication of deceleration.
        /// </summary>
        /// <param name="hostIp"></param>
        /// <param name="port"></param>
        /// <param name="homingMethod"></param>
        [HttpPut("Method/{hostIp}/{port:int}")]
        public void SetMethod(string hostIp, int port, [FromBody] sbyte homingMethod)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes((short)homingMethod);
            var telegram = new Telegram
            {
                Length = 20
            };
            telegram.Set(1, AddressConst.HomingMethod, 4, 0, data[0]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 6084h<br />
        /// Indication of deceleration.
        /// </summary>
        /// <param name="hostIp"></param>
        /// <param name="port"></param>
        /// <param name="homingMethod"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("MethodAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetMethodAsync(string hostIp, int port, [FromBody] sbyte homingMethod, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes((short)homingMethod);
                var telegram = new Telegram
                {
                    Length = 20
                };
                telegram.Set(1, AddressConst.HomingMethod, 4, 0, data[0]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6099h sub1<br />
        /// Speeds during Search for Switch
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>Switch search velocity</returns>
        [HttpGet("SwitchSearchVelocity/{hostIp}/{port:int}")]
        public double GetSwitchSearchVelocity(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.HomingSwitchSearchVelocity, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                         connection.MultiplicationFactor;
            return result;
        }

        /// <summary>
        /// 6099h sub1<br />
        /// Speeds during Search for Switch
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Switch search velocity</returns>
        [HttpGet("SwitchSearchVelocityAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetSwitchSearchVelocityAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.HomingSwitchSearchVelocity, 4);
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
        /// 6099h sub1<br />
        /// Speeds during Search for Switch
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="switchSearchVelocity"></param>
        [HttpPut("SwitchSearchVelocity/{hostIp}/{port:int}")]
        public void SetSwitchSearchVelocity(string hostIp, int port, [FromBody] double switchSearchVelocity)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes((uint)(switchSearchVelocity * connection.MultiplicationFactor));
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.HomingSwitchSearchVelocity, 4, data[0], data[1], data[2], data[3]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 6099h sub1<br />
        /// Speeds during Search for Switch
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="switchSearchVelocity"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("SwitchSearchVelocityAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetSwitchSearchVelocityAsync(string hostIp, int port, [FromBody] double switchSearchVelocity,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes((uint)(switchSearchVelocity * connection.MultiplicationFactor));
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.HomingSwitchSearchVelocity, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6099h sub2<br />
        /// Speeds during Search for Zero
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>Switch search velocity</returns>
        [HttpGet("ZeroSearchVelocity/{hostIp}/{port:int}")]
        public double GetZeroSearchVelocity(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.HomingZeroSearchVelocity, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                         connection.MultiplicationFactor;
            return result;
        }

        /// <summary>
        /// 6099h sub2<br />
        /// Speeds during Search for Zero
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Switch search velocity</returns>
        [HttpGet("ZeroSearchVelocityAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetZeroSearchVelocityAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.HomingZeroSearchVelocity, 4);
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
        /// 6099h sub2<br />
        /// Speeds during Search for Zero
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="zeroSearchVelocity"></param>
        [HttpPut("ZeroSearchVelocity/{hostIp}/{port:int}")]
        public void SetZeroSearchVelocity(string hostIp, int port, [FromBody] double zeroSearchVelocity)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes((uint)(zeroSearchVelocity * connection.MultiplicationFactor));
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.HomingZeroSearchVelocity, 4, data[0], data[1], data[2], data[3]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 6099h sub2<br />
        /// Speeds during Search for Zero
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="zeroSearchVelocity"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("ZeroSearchVelocityAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetZeroSearchVelocityAsync(string hostIp, int port, [FromBody] double zeroSearchVelocity,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes((uint)(zeroSearchVelocity * connection.MultiplicationFactor));
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.HomingZeroSearchVelocity, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 609Ah<br />
        /// Indication of the acceleration during homing runs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("Acceleration/{hostIp}/{port:int}")]
        public double GetAcceleration(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.HomingAcceleration, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                         connection.MultiplicationFactor;
            return result;
        }

        /// <summary>
        /// 609Ah<br />
        /// Indication of the acceleration during homing runs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("AccelerationAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetAccelerationAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.HomingAcceleration, 4);
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
        /// 609Ah<br />
        /// Indication of the acceleration during homing runs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="acceleration"></param>
        [HttpPut("Acceleration/{hostIp}/{port:int}")]
        public void SetAcceleration(string hostIp, int port, [FromBody] double acceleration)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes((uint)(acceleration * connection.MultiplicationFactor));
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.HomingAcceleration, 4, data[0], data[1], data[2], data[3]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 609Ah<br />
        /// Indication of the acceleration during homing runs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="acceleration"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("AccelerationAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetAccelerationAsync(string hostIp, int port, [FromBody] double acceleration,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes((uint)(acceleration * connection.MultiplicationFactor));
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.HomingAcceleration, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
