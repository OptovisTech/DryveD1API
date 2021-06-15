using System.Net.Sockets;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace DryveD1API.Controllers
{
    /// <summary>
    /// 6041h
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StatusWordController
    {
        /// <summary>
        /// 6041h<br />
        /// Returns the current StatusWord
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        [HttpGet("{hostIp}/{port}")]
        public StatusWord GetStatusWord(string hostIp, int port)
        {
            Socket s = ModbusSocket.GetConnection(hostIp, port);
            StatusWord statusWord = new StatusWord();
            statusWord.Read(s);
            return statusWord;
        }
    }
}
