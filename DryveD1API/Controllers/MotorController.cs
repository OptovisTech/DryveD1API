using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DryveD1API.Common;
using DryveD1API.Modules;
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
        /// 6060h<br />
        /// Preselection of the operating mode.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>Operation mode</returns>
        [HttpGet("OperationMode/{hostIp}/{port:int}")]
        public sbyte GetOperationMode(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var modesOfOperation = new ModesOfOperation();
            var result = modesOfOperation.Read(connection.Socket);
            return (sbyte)result;
        }

        /// <summary>
        /// 6060h<br />
        /// Preselection of the operating mode.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Operation mode</returns>
        [HttpGet("OperationModeAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetOperationModeAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var modesOfOperation = new ModesOfOperation();
                var result = await modesOfOperation.ReadAsync(connection.Socket, cancellationToken);
                return Ok((sbyte)result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6060h<br />
        /// Preselection of the operating mode.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="operationMode">Operation mode</param>
        [HttpPut("OperationMode/{hostIp}/{port:int}")]
        public void SetOperationMode(string hostIp, int port, [FromBody] sbyte operationMode)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var modesOfOperation = new ModesOfOperation();
            modesOfOperation.Write(connection.Socket, (ModesOfOperation.ModesEnum)operationMode);
        }

        /// <summary>
        /// 6060h<br />
        /// Preselection of the operating mode.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="operationMode">Operation mode</param>
        /// <param name="cancellationToken"></param>
        [HttpPut("OperationModeAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetOperationModeAsync(string hostIp, int port, [FromBody] sbyte operationMode,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var modesOfOperation = new ModesOfOperation();
                await modesOfOperation.WriteAsync(connection.Socket, (ModesOfOperation.ModesEnum)operationMode, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6061h<br />
        /// Object for feedback of current operating mode.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>Operation mode display</returns>
        [HttpGet("OperationModeDisplay/{hostIp}/{port:int}")]
        public sbyte GetOperationModeDisplay(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var modesOfOperation = new ModesOfOperation();
            var currentMode = modesOfOperation.ReadDisplay(connection.Socket);
            return (sbyte)currentMode;
        }

        /// <summary>
        /// 6061h<br />
        /// Object for feedback of current operating mode.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Operation mode display</returns>
        [HttpGet("OperationModeDisplayAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetOperationModeDisplayAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var modesOfOperation = new ModesOfOperation();
                var currentMode = await modesOfOperation.ReadDisplayAsync(connection.Socket, cancellationToken);
                return Ok((sbyte)currentMode);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6064h<br />
        /// Indication of the current position of the position encoder.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>The actual value of the position encoder</returns>
        [HttpGet("ActualPosition/{hostIp}/{port:int}")]
        public double GetActualPosition(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.PositionActualValue, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                         connection.MultiplicationFactor;
            return result;
        }

        /// <summary>
        /// 6064h<br />
        /// Indication of the current position of the position encoder.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The actual value of the position encoder</returns>
        [HttpGet("ActualPositionAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetActualPositionAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.PositionActualValue, 4);
                var response = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                var result = BitConverter.ToInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                             connection.MultiplicationFactor;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6064h<br />
        /// Indication of the current position of the position encoder.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="allowedDeviation">The permissible deviation of the 3 actual position readings from each other.</param>
        /// <returns>The actual value of the position encoder</returns>
        [HttpGet("ActualPositionWithValidation/{hostIp}/{port:int}/{allowedDeviation:double}")]
        public Tuple<bool, double> GetActualPositionWithValidation(string hostIp, int port, double allowedDeviation)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);

            var isValid = false;
            var retryCounter = 0;

            double result = 0;
            while (!isValid)
            {
                // FirstCheck
                var telegram1 = new Telegram();
                telegram1.Set(0, AddressConst.PositionActualValue, 4);
                var response1 = telegram1.SendAndReceive(connection.Socket);
                var result1 = BitConverter.ToInt32(new[] { response1.Byte19, response1.Byte20, response1.Byte21, response1.Byte22 }, 0) /
                              connection.MultiplicationFactor;
                Thread.Sleep(5);
                // SecondCheck
                var telegram2 = new Telegram();
                telegram2.Set(0, AddressConst.PositionActualValue, 4);
                var response2 = telegram2.SendAndReceive(connection.Socket);
                var result2 = BitConverter.ToInt32(new[] { response2.Byte19, response2.Byte20, response2.Byte21, response2.Byte22 }, 0) /
                              connection.MultiplicationFactor;
                Thread.Sleep(5);
                // ThirdCheck
                var telegram3 = new Telegram();
                telegram3.Set(0, AddressConst.PositionActualValue, 4);
                var response3 = telegram3.SendAndReceive(connection.Socket);
                var result3 = BitConverter.ToInt32(new[] { response3.Byte19, response3.Byte20, response3.Byte21, response3.Byte22 }, 0) /
                              connection.MultiplicationFactor;
                if (Math.Abs(result1 - result2) <= allowedDeviation && Math.Abs(result1 - result3) <= allowedDeviation &&
                    Math.Abs(result2 - result3) <= allowedDeviation)
                {
                    isValid = true;
                    result = (result1 + result2 + result3) / 3;
                }
                else
                {
                    Debug.WriteLine($"Actual position reading was not valid: 1: {result1}, 2: {result2}, 3: {result3}");
                    Thread.Sleep(5);
                    retryCounter++;
                    if (retryCounter >= 10)
                    {
                        break;
                    }
                }
            }

            return new Tuple<bool, double>(isValid, result);
        }

        /// <summary>
        /// 6064h<br />
        /// Indication of the current position of the position encoder.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="allowedDeviation">The permissible deviation of the 3 actual position readings from each other.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The actual value of the position encoder</returns>
        [HttpGet("ActualPositionWithValidationAsync/{hostIp}/{port:int}/{allowedDeviation:double}")]
        public async Task<IActionResult> GetActualPositionWithValidationAsync(string hostIp, int port, double allowedDeviation,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);

                var isValid = false;
                var retryCounter = 0;

                double result = 0;
                while (!isValid)
                {
                    // FirstCheck
                    var telegram1 = new Telegram();
                    telegram1.Set(0, AddressConst.PositionActualValue, 4);
                    var response1 = await telegram1.SendAndReceiveAsync(connection.Socket, cancellationToken);
                    var result1 = BitConverter.ToInt32(new[] { response1.Byte19, response1.Byte20, response1.Byte21, response1.Byte22 }, 0) /
                                  connection.MultiplicationFactor;
                    Thread.Sleep(5);
                    // SecondCheck
                    var telegram2 = new Telegram();
                    telegram2.Set(0, AddressConst.PositionActualValue, 4);
                    var response2 = await telegram2.SendAndReceiveAsync(connection.Socket, cancellationToken);
                    var result2 = BitConverter.ToInt32(new[] { response2.Byte19, response2.Byte20, response2.Byte21, response2.Byte22 }, 0) /
                                  connection.MultiplicationFactor;
                    Thread.Sleep(5);
                    // ThirdCheck
                    var telegram3 = new Telegram();
                    telegram3.Set(0, AddressConst.PositionActualValue, 4);
                    var response3 = await telegram3.SendAndReceiveAsync(connection.Socket, cancellationToken);
                    var result3 = BitConverter.ToInt32(new[] { response3.Byte19, response3.Byte20, response3.Byte21, response3.Byte22 }, 0) /
                                  connection.MultiplicationFactor;
                    if (Math.Abs(result1 - result2) <= allowedDeviation && Math.Abs(result1 - result3) <= allowedDeviation &&
                        Math.Abs(result2 - result3) <= allowedDeviation)
                    {
                        isValid = true;
                        result = (result1 + result2 + result3) / 3;
                    }
                    else
                    {
                        Debug.WriteLine($"Actual position reading was not valid: 1: {result1}, 2: {result2}, 3: {result3}");
                        Thread.Sleep(5);
                        retryCounter++;
                        if (retryCounter >= 10)
                        {
                            break;
                        }
                    }
                }

                return Ok(new Tuple<bool, double>(isValid, result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
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
        [HttpGet("PositionWindow/{hostIp}/{port:int}")]
        public double GetPositionWindow(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.PositionWindow, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                         connection.MultiplicationFactor;
            return result;
        }

        /// <summary>
        /// 6067h<br />
        /// Indication of a symmetrical area around the target point.<br />
        /// If this area is reached, the target position can be regarded as reached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The position window</returns>
        [HttpGet("PositionWindowAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetPositionWindowAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.PositionWindow, 4);
                var response = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                var result = BitConverter.ToInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                             connection.MultiplicationFactor;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
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
        [HttpGet("PositionWindowTime/{hostIp}/{port:int}")]
        public ushort GetPositionWindowTime(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.PositionWindowTime, 2);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToUInt16(new[] { response.Byte19, response.Byte20 }, 0);
            return result;
        }

        /// <summary>
        /// 6068h<br />
        /// Indication of a time delay before a "Target Reached" signal can be output.<br />
        /// The time is counted from the moment when the position window (6067h) is reached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The position window time</returns>
        [HttpGet("PositionWindowTimeAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetPositionWindowTimeAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.PositionWindowTime, 2);
                var response = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                var result = BitConverter.ToUInt16(new[] { response.Byte19, response.Byte20 }, 0);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 607Ah<br />
        /// Goal position to be reached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>The currently set target position</returns>
        [HttpGet("TargetPosition/{hostIp}/{port:int}")]
        public double GetTargetPosition(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.TargetPosition, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                         connection.MultiplicationFactor;
            return result;
        }

        /// <summary>
        /// 607Ah<br />
        /// Goal position to be reached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The currently set target position</returns>
        [HttpGet("TargetPositionAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetTargetPositionAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.TargetPosition, 4);
                var response = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                var result = BitConverter.ToInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                             connection.MultiplicationFactor;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 607Ah<br />
        /// Goal position to be reached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="targetPosition">The target position to be set</param>
        [HttpPut("TargetPosition/{hostIp}/{port:int}")]
        public void SetTargetPosition(string hostIp, int port, [FromBody] double targetPosition)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes((int)(targetPosition * connection.MultiplicationFactor));
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.TargetPosition, 4, data[0], data[1], data[2], data[3]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 607Ah<br />
        /// Goal position to be reached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="targetPosition">The target position to be set</param>
        /// <param name="cancellationToken"></param>
        [HttpPut("TargetPositionAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetTargetPositionAsync(string hostIp, int port, [FromBody] double targetPosition,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes((int)(targetPosition * connection.MultiplicationFactor));
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.TargetPosition, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6081h<br />
        /// Indication of the speed.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("ProfileVelocity/{hostIp}/{port:int}")]
        public double GetProfileVelocity(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.ProfileVelocity, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                         connection.MultiplicationFactor;
            return result;
        }

        /// <summary>
        /// 6081h<br />
        /// Indication of the speed.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("ProfileVelocityAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetProfileVelocityAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.ProfileVelocity, 4);
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
        /// 6081h<br />
        /// Indication of the speed.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="profileVelocity"></param>
        [HttpPut("ProfileVelocity/{hostIp}/{port:int}")]
        public void SetProfileVelocity(string hostIp, int port, [FromBody] double profileVelocity)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes((uint)(profileVelocity * connection.MultiplicationFactor));
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.ProfileVelocity, 4, data[0], data[1], data[2], data[3]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 6081h<br />
        /// Indication of the speed.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="profileVelocity"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("ProfileVelocityAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetProfileVelocityAsync(string hostIp, int port, [FromBody] double profileVelocity,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes((uint)(profileVelocity * connection.MultiplicationFactor));
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.ProfileVelocity, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6083h<br />
        /// Indication of acceleration.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("ProfileAcceleration/{hostIp}/{port:int}")]
        public double GetProfileAcceleration(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.ProfileAcceleration, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                         connection.MultiplicationFactor;
            return result;
        }

        /// <summary>
        /// 6083h<br />
        /// Indication of acceleration.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("ProfileAccelerationAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetProfileAccelerationAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.ProfileAcceleration, 4);
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
        /// 6083h<br />
        /// Indication of acceleration.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="profileAcceleration"></param>
        [HttpPut("ProfileAcceleration/{hostIp}/{port:int}")]
        public void SetProfileAcceleration(string hostIp, int port, [FromBody] double profileAcceleration)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes((uint)(profileAcceleration * connection.MultiplicationFactor));
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.ProfileAcceleration, 4, data[0], data[1], data[2], data[3]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 6083h<br />
        /// Indication of acceleration.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="profileAcceleration"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("ProfileAccelerationAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetProfileAccelerationAsync(string hostIp, int port, [FromBody] double profileAcceleration,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes((uint)(profileAcceleration * connection.MultiplicationFactor));
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.ProfileAcceleration, 4, data[0], data[1], data[2], data[3]);
                var unused = await telegram.SendAndReceiveAsync(connection.Socket, cancellationToken);
                return Ok();
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
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>Test</returns>
        [HttpGet("ProfileDeceleration/{hostIp}/{port:int}")]
        public double GetProfileDeceleration(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.ProfileDeceleration, 4);
            var response = telegram.SendAndReceive(connection.Socket);
            var result = BitConverter.ToUInt32(new[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0) /
                         connection.MultiplicationFactor;
            return result;
        }

        /// <summary>
        /// 6084h<br />
        /// Indication of deceleration.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Test</returns>
        [HttpGet("ProfileDecelerationAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetProfileDecelerationAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var telegram = new Telegram();
                telegram.Set(0, AddressConst.ProfileDeceleration, 4);
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
        /// 6084h<br />
        /// Indication of deceleration.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="profileDeceleration"></param>
        [HttpPut("ProfileDeceleration/{hostIp}/{port:int}")]
        public void SetProfileDeceleration(string hostIp, int port, [FromBody] double profileDeceleration)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var data = BitConverter.GetBytes((uint)(profileDeceleration * connection.MultiplicationFactor));
            var telegram = new Telegram
            {
                Length = 23
            };
            telegram.Set(1, AddressConst.ProfileDeceleration, 4, data[0], data[1], data[2], data[3]);
            var unused = telegram.SendAndReceive(connection.Socket);
        }

        /// <summary>
        /// 6084h<br />
        /// Indication of deceleration.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="profileDeceleration"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("ProfileDecelerationAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetProfileDecelerationAsync(string hostIp, int port, [FromBody] double profileDeceleration,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var data = BitConverter.GetBytes((uint)(profileDeceleration * connection.MultiplicationFactor));
                var telegram = new Telegram
                {
                    Length = 23
                };
                telegram.Set(1, AddressConst.ProfileDeceleration, 4, data[0], data[1], data[2], data[3]);
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
