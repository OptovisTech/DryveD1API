using System;
using System.Net.Sockets;
using DryveD1API.Common;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DryveD1API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class FeedConstantController : ControllerBase
    {
        /// <summary>
        /// 6092h sub1<br />
        /// Indication of the FeedRate.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("FeedRate/{hostIp}/{port}")]
        public uint GetFeedRate(string hostIp, int port)
        {
            Socket s = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.FeedRate, 4);
            var response = telegram.SendAndReceive(s);
            var result = BitConverter.ToUInt32(new byte[] { response.Byte19, response.Byte20, response.Byte21, response.Byte22 }, 0);
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
        public void SetFeedRate(string hostIp, int port, [FromBody] uint feedRate)
        {
            Socket s = ModbusSocket.GetConnection(hostIp, port);
            byte[] data = BitConverter.GetBytes(feedRate);
            var telegram = new Telegram();
            telegram.Length = 23;
            telegram.Set(1, AddressConst.FeedRate, 4, data[0], data[1], data[2], data[3]);
            var response = telegram.SendAndReceive(s);
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
            Socket s = ModbusSocket.GetConnection(hostIp, port);
            var telegram = new Telegram();
            telegram.Set(0, AddressConst.ShaftRevolution, 4);
            var response = telegram.SendAndReceive(s);
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
            Socket s = ModbusSocket.GetConnection(hostIp, port);
            byte[] data = BitConverter.GetBytes(shaftRevolutions);
            var telegram = new Telegram();
            telegram.Length = 23;
            telegram.Set(1, AddressConst.ShaftRevolution, 4, data[0], data[1], data[2], data[3]);
            var response = telegram.SendAndReceive(s);
        }
    }
}
