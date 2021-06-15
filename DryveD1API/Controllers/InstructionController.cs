using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DryveD1API.Controllers
{
    /// <summary>
    /// Execute common Instructions
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InstructionController : Controller
    {
        /// <summary>
        /// 607Ah<br />
        /// Set Target Position, start and wait until PositionReached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="targetPosition">The target position to be set</param>
        /// <returns><see langword="true"/>Target position reached</returns>
        [HttpPut("TargetPositionStart/{hostIp}/{port}")]
        public bool SetTargetPositionStart(string hostIp, int port, [FromBody] int targetPosition)
        {
            // Init Variables
            Debug.WriteLine("");
            Debug.WriteLine("");
            Debug.WriteLine("Init Variables");
            var s = ModbusSocket.GetConnection(hostIp, port);
            Debug.WriteLine("Init Variables");
            //PrintStatusWord(s);
            //PrintControlWord(s);
            ControlWord enableOperation = new ControlWord()
            {
                // Byte 19: 15
                Bit00 = true, // 1
                Bit01 = true, // 2
                Bit02 = true, // 4
                Bit03 = true // 8
            };
            ControlWord start = new ControlWord()
            {
                Bit00 = true,
                Bit01 = true,
                Bit02 = true,
                Bit03 = true,
                Bit04 = true,
            };

            ControlWord controlWord = new ControlWord();
            controlWord.Read(s);

            Debug.WriteLine($"Enable Operation");
            //PrintStatusWord(s);
            //PrintControlWord(s);
            if (controlWord.Bit04)
            {
                enableOperation.Write(s);
            }
            while (controlWord.Bit04)
            {
                controlWord.Read(s);
                //PrintStatusWord(s);
                //PrintControlWord(s);
                Task.Delay(TimeSpan.FromMilliseconds(10)).Wait();

            }


            // Write TargetPosition
            Debug.WriteLine($"Write TargetPosition");
            //PrintStatusWord(s);
            //PrintControlWord(s);
            byte[] data = BitConverter.GetBytes(targetPosition);
            var telegram = new Telegram();
            telegram.Length = 23;
            telegram.Set(1, AddressConst.TargetPosition, 4, data[0], data[1], data[2], data[3]);
            var responseSet = telegram.SendAndReceive(s);
            telegram = new Telegram();
            telegram.Set(0, AddressConst.TargetPosition, 4);
            int targetPositionGet = -1;
            while (targetPositionGet != targetPosition)
            {
                var responseGet = telegram.SendAndReceive(s);
                targetPositionGet = BitConverter.ToInt32(new byte[] { responseGet.Byte19, responseGet.Byte20, responseGet.Byte21, responseGet.Byte22 }, 0);
                Debug.WriteLine($"TargetPositionReference: {targetPosition}, TargetPositionGet: {targetPositionGet}");
                PrintStatusWord(s);
                PrintControlWord(s);
                Task.Delay(TimeSpan.FromMilliseconds(10)).Wait();
            }



            StatusWord statusWord = new StatusWord();
            statusWord.Read(s);
            Debug.WriteLine($"Start");
            PrintStatusWord(s);
            PrintControlWord(s);
            start.Write(s);
            while (!controlWord.Bit04)
            {
                controlWord.Read(s);
                PrintStatusWord(s);
                PrintControlWord(s);
                if (!controlWord.Bit04)
                {
                    Task.Delay(TimeSpan.FromMilliseconds(10)).Wait();
                }
            }
            Debug.WriteLine($"Check NewTargetSet");
            //PrintStatusWord(s);
            //PrintControlWord(s);
            statusWord.Read(s);
            while (!statusWord.Bit12)
            {

                Task.Delay(TimeSpan.FromMilliseconds(10)).Wait();
                statusWord.Read(s);
                PrintStatusWord(s);
                PrintControlWord(s);
            }
            Debug.WriteLine($"Reset Start");
            controlWord.Read(s);
            enableOperation.Write(s);
            while (controlWord.Bit04)
            {
                controlWord.Read(s);
                PrintStatusWord(s);
                PrintControlWord(s);
                if (!controlWord.Bit04)
                {
                    Task.Delay(TimeSpan.FromMilliseconds(10)).Wait();
                }
            }
            // Check Target Reached
            Debug.WriteLine($"Check Target Reached");
            PrintStatusWord(s);
            PrintControlWord(s);
            statusWord = new StatusWord();
            var motor = new MotorController();
            bool first = true;
            int minPosition = targetPosition - 100;
            int maxPosition = targetPosition + 100;
            var actualPosition = motor.GetActualPosition(hostIp, port);
            while ((!statusWord.Bit10 || statusWord.Bit12) || !TargetReached(actualPosition, minPosition, maxPosition))
            {
                if (first)
                {
                    Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
                    first = false;
                }
                statusWord.Read(s);
                actualPosition = motor.GetActualPosition(hostIp, port);
                if (statusWord.Bit10 && !TargetReached(actualPosition, minPosition, maxPosition))
                {
                    Debug.WriteLine("");
                    Debug.WriteLine("");
                    Debug.WriteLine("");
                    Debug.WriteLine("");
                    Debug.WriteLine("------------------Fehler------------------");
                    Debug.WriteLine("------------------Fehler------------------");
                    Debug.WriteLine($"{actualPosition}");
                    Debug.WriteLine("");
                    Debug.WriteLine("");
                    Debug.WriteLine("");
                }
                PrintStatusWord(s);
                PrintControlWord(s);
                Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
            }
            Debug.WriteLine($"Target Reached");
            //PrintStatusWord(s);
            //PrintControlWord(s);

            actualPosition = motor.GetActualPosition(hostIp, port);
            Debug.WriteLine($"ActualPosition: {actualPosition}, TargetPosition: {targetPosition}, Bit10: {statusWord.Bit10}");
            Debug.WriteLine("");
            Debug.WriteLine("");
            return true;
        }

        private bool TargetReached(int actual, int min, int max)
        {
            if (actual > min && actual < max)
            {
                return true;
            }
            return false;
        }

        private void PrintStatusWord(Socket s)
        {
            StatusWord sw = new StatusWord();
            sw.Read(s);
            var bitArray19 = new BitArray(new bool[8] { sw.Bit00, sw.Bit01, sw.Bit02, sw.Bit03, sw.Bit04, sw.Bit05, sw.Bit06, sw.Bit07 });
            var bitArray20 = new BitArray(new bool[8] { sw.Bit08, sw.Bit09, sw.Bit10, sw.Bit11, sw.Bit12, sw.Bit13, sw.Bit14, sw.Bit15 });

            byte[] byte19 = new byte[1];
            bitArray19.CopyTo(byte19, 0);

            byte[] byte20 = new byte[1];
            bitArray20.CopyTo(byte20, 0);
            Debug.WriteLine("");
            Debug.WriteLine("StatusWord");
            Debug.WriteLine($"Byte 19: {byte19[0]}");
            Debug.WriteLine($"0: {sw.Bit00}, 1: {sw.Bit01}, 2: {sw.Bit02}, 3: {sw.Bit03}, 4: {sw.Bit04}, 5: {sw.Bit05}, 6: {sw.Bit06}, 7: {sw.Bit07}");
            Debug.WriteLine($"Byte 20: {byte20[0]}");
            Debug.WriteLine($"8: {sw.Bit08}, 9: {sw.Bit09}, 10: {sw.Bit10}, 11: {sw.Bit11}, 12: {sw.Bit12}, 13: {sw.Bit13}, 14: {sw.Bit14}, 15: {sw.Bit15}");
            Debug.WriteLine("");
        }

        private void PrintControlWord(Socket s)
        {
            ControlWord cw = new ControlWord();
            cw.Read(s);
            var bitArray19 = new BitArray(new bool[8] { cw.Bit00, cw.Bit01, cw.Bit02, cw.Bit03, cw.Bit04, cw.Bit05, cw.Bit06, cw.Bit07 });
            var bitArray20 = new BitArray(new bool[8] { cw.Bit08, cw.Bit09, cw.Bit10, cw.Bit11, cw.Bit12, cw.Bit13, cw.Bit14, cw.Bit15 });

            byte[] byte19 = new byte[1];
            bitArray19.CopyTo(byte19, 0);

            byte[] byte20 = new byte[1];
            bitArray20.CopyTo(byte20, 0);
            Debug.WriteLine("");
            Debug.WriteLine("ControlWord");
            Debug.WriteLine($"Byte 19: {byte19[0]}");
            Debug.WriteLine($"0: {cw.Bit00}, 1: {cw.Bit01}, 2: {cw.Bit02}, 3: {cw.Bit03}, 4: {cw.Bit04}, 5: {cw.Bit05}, 6: {cw.Bit06}, 7: {cw.Bit07}");
            Debug.WriteLine($"Byte 20: {byte20[0]}");
            Debug.WriteLine($"8: {cw.Bit08}, 9: {cw.Bit09}, 10: {cw.Bit10}, 11: {cw.Bit11}, 12: {cw.Bit12}, 13: {cw.Bit13}, 14: {cw.Bit14}, 15: {cw.Bit15}");
            Debug.WriteLine("");
        }
    }
}
