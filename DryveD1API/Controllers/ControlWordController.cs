using System.Net.Sockets;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace DryveD1API.Controllers
{
    /// <summary>
    /// 6040h
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ControlWordController
    {
        /// <summary>
        /// 6040h<br />
        /// Returns the current ControlWord
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        [HttpGet("{hostIp}/{port}")]
        public ControlWord GetControlWord(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            ControlWord controlWord = new ControlWord();
            controlWord.Read(connection.socket);
            return controlWord;
        }

        /// <summary>
        /// 6040h<br />
        /// Sets the ControlWord
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="controlWord">The bits to be set</param>
        [HttpPut("{hostIp}/{port}")]
        public void SetControlWord(string hostIp, int port, [FromBody] ControlWord controlWord)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            controlWord.Write(connection.socket);
        }
    }
}
