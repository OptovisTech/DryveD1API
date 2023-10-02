using System;
using System.Threading;
using System.Threading.Tasks;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace DryveD1API.Controllers
{
    /// <summary>
    /// Read the status word.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StatusWordController : ControllerBase
    {
        /// <summary>
        /// 6041h<br />
        /// Returns the current StatusWord
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        [HttpGet("{hostIp}/{port:int}")]
        public StatusWord GetStatusWord(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var statusWord = new StatusWord();
            statusWord.Read(connection.Socket);
            return statusWord;
        }

        /// <summary>
        /// 6041h<br />
        /// Returns the current StatusWord
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        [HttpGet("Async/{hostIp}/{port:int}")]
        public async Task<IActionResult> GetStatusWordAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var statusWord = new StatusWord();
                await statusWord.ReadAsync(connection.Socket, cancellationToken);
                return Ok(statusWord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
