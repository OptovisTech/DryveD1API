using System;
using System.Threading;
using System.Threading.Tasks;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace DryveD1API.Controllers
{
    /// <summary>
    ///  Read and write the control word.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ControlWordController : ControllerBase
    {
        /// <summary>
        /// 6040h<br />
        /// Returns the current ControlWord
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("{hostIp}/{port:int}")]
        public ControlWord GetControlWord(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var controlWord = new ControlWord();
            controlWord.Read(connection.Socket);
            return controlWord;
        }

        /// <summary>
        /// 6040h<br />
        /// Returns the current ControlWord
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Async/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetControlWordAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var controlWord = new ControlWord();
                await controlWord.ReadAsync(connection.Socket, cancellationToken);
                return Ok(controlWord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 6040h<br />
        /// Sets the ControlWord
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="controlWord">The bits to be set</param>
        [HttpPut("{hostIp}/{port:int}")]
        public void SetControlWord(string hostIp, int port, [FromBody] ControlWord controlWord)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            controlWord.Write(connection.Socket);
        }

        /// <summary>
        /// 6040h<br />
        /// Sets the ControlWord
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="controlWord">The bits to be set</param>
        /// <param name="cancellationToken"></param>
        [HttpPut("Async/{hostIp}/{port:int}")]
        public async Task<IActionResult> SetControlWordAsync(string hostIp, int port, [FromBody] ControlWord controlWord,
            CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                await controlWord.WriteAsync(connection.Socket, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
