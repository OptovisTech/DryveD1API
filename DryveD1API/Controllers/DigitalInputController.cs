using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace DryveD1API.Controllers
{
    /// <summary>
    ///  Read and write digital inputs.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DigitalInputController : ControllerBase
    {
        /// <summary>
        /// 60FD<br />
        /// Status display of digital inputs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("{hostIp}/{port}")]
        public DigitalInputs Get(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var digitalInputs = new DigitalInputs();
            digitalInputs.Read(connection.Socket);
            return digitalInputs;
        }

        /// <summary>
        /// 60FD<br />
        /// Status display of digital inputs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Async/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var digitalInputs = new DigitalInputs();
                await digitalInputs.ReadAsync(connection.Socket, cancellationToken);
                return Ok(digitalInputs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 2010h<br />
        /// Activation of the input signal negation of digital inputs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("Polarity/{hostIp}/{port:int}")]
        public DigitalInputPolarity GetPolarity(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var digitalInputPolarity = new DigitalInputPolarity();
            digitalInputPolarity.Read(connection.Socket);
            return digitalInputPolarity;
        }

        /// <summary>
        /// 2010h<br />
        /// Activation of the input signal negation of digital inputs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("PolarityAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetPolarityAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var digitalInputPolarity = new DigitalInputPolarity();
                await digitalInputPolarity.ReadAsync(connection.Socket, cancellationToken);
                return Ok(digitalInputPolarity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 2010h<br />
        /// Activation of the input signal negation of digital inputs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="digitalInputPolarity"></param>
        [HttpPut("Polarity/{hostIp}/{port:int}")]
        public void SetPolarity(string hostIp, int port, [FromBody] DigitalInputPolarity digitalInputPolarity)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            digitalInputPolarity.Write(connection.Socket);
        }

        /// <summary>
        /// 2010h<br />
        /// Activation of the input signal negation of digital inputs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="digitalInputPolarity"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("PolarityAsync/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetPolarityAsync(string hostIp, int port, [FromBody] DigitalInputPolarity digitalInputPolarity,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                await digitalInputPolarity.WriteAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 2010h<br />
        /// Toggle a specific digital input.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="digitalInput"></param>
        /// <returns></returns>
        [HttpPut("Toggle/{hostIp}/{port:int}/{digitalInput:int}")]
        public bool TogglePolarity(string hostIp, int port, int digitalInput)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var digitalInputPolarity = new DigitalInputPolarity();
            var result = digitalInputPolarity.ToggleDigitalInput(connection.Socket, digitalInput);
            return result;
        }

        /// <summary>
        /// 2010h<br />
        /// Toggle a specific digital input.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="digitalInput"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("ToggleAsync/{hostIp}/{port:int}/{digitalInput:int}")]
        public async Task<IActionResult> TogglePolarityAsync(string hostIp, int port, int digitalInput, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var digitalInputPolarity = new DigitalInputPolarity();
                var result = await digitalInputPolarity.ToggleDigitalInputAsync(connection.Socket, digitalInput, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
