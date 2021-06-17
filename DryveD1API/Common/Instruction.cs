using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DryveD1API.Controllers;
using DryveD1API.Modules;

namespace DryveD1API.Common
{
    public class Instruction
    {

        /// <summary>
        /// mm for movement, degrees for rotation.
        /// </summary>
        public double TargetPosition { get; set; }

        /// <summary>
        /// True if the target position is relative to the current.
        /// </summary>
        public bool RelativePosition { get; set; }

        /// <summary>
        /// mm/s for movement, 1/m for rotation
        /// </summary>
        public double TargetVelocity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Acceleration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Deceleration { get; set; }

        /// <summary>
        /// 10 = 0,1mm, 100 = 0,01mm, 1000 = 0,001
        /// </summary>
        public int Precision { get; set; }


        public async Task<bool> MoveToTarget(string ip, int port)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var controlWordEP = new ControlWordController();
            var statusWordEP = new StatusWordController();
            var motorEP = new MotorController();

            uint acceleration = Convert.ToUInt32(Acceleration * Precision);
            uint deceleration = Convert.ToUInt32(Deceleration * Precision);
            uint velocity = Convert.ToUInt32(TargetVelocity * Precision);
            int targetPosition = Convert.ToInt32(TargetPosition * Precision);

            // Set operation mode
            motorEP.SetOperationMode(ip, port, 1);
            Stopwatch operationMode = new Stopwatch();
            operationMode.Start();
            while (motorEP.GetOperationModeDisplay(ip, port) != 1)
            {
                Debug.WriteLine("OPM wrong");
                await Delay(10);
            }
            operationMode.Stop();
            Debug.WriteLine($"OperationMode Set: {operationMode.ElapsedMilliseconds}");

            // Check if start is reseted
            ControlWord controlWord = new ControlWord();
            controlWord = controlWordEP.GetControlWord(ip, port);
            if (controlWord.Bit04)
            {
                Debug.WriteLine("Reset bit 4, Z190");
                controlWord.Bit04 = false;
                controlWordEP.SetControlWord(ip, port, controlWord);
            }

            // Set relative positioning
            if (RelativePosition)
            {
                if (!controlWord.Bit06)
                {

                }
                controlWord.Bit06 = true;
                controlWordEP.SetControlWord(ip, port, controlWord);
                // Check relative positioning is set (Bit 6)
                controlWord = controlWordEP.GetControlWord(ip, port);
                if (controlWord.Bit06 != RelativePosition)
                {
                    return false;
                }
            }
            else if (controlWord.Bit06)
            {
                controlWord.Bit06 = false;
                controlWordEP.SetControlWord(ip, port, controlWord);
                // Check relative positioning is set (Bit 6)
                controlWord = controlWordEP.GetControlWord(ip, port);
                if (controlWord.Bit06 != RelativePosition)
                {
                    return false;
                }
            }


            int positionWindow = motorEP.GetPositionWindow(ip, port);

            // Set acceleration
            motorEP.SetProfileAcceleration(ip, port, acceleration);
            if (acceleration != motorEP.GetProfileAcceleration(ip, port))
            {
                return false;
            }

            // Set deceleration
            motorEP.SetProfileDeceleration(ip, port, deceleration);
            if (deceleration != motorEP.GetProfileDeceleration(ip, port))
            {
                return false;
            }

            // Set velocity
            motorEP.SetProfileVelocity(ip, port, velocity);
            if (velocity != motorEP.GetProfileVelocity(ip, port))
            {
                return false;
            }

            // Set target position
            motorEP.SetTargetPosition(ip, port, targetPosition);
            if (targetPosition != motorEP.GetTargetPosition(ip, port))
            {
                return false;
            }

            // Check controlWord bit 4 (Start)
            controlWord = controlWordEP.GetControlWord(ip, port);
            if (controlWord.Bit04)
            {
                Debug.WriteLine("Reset bit 4 Z243");
                controlWord.Bit04 = false;
                controlWordEP.SetControlWord(ip, port, controlWord);
                await Delay(50);
            }
            ////////////////// Debug
            stopwatch.Stop();
            Debug.WriteLine($"Instruction Time: {stopwatch.ElapsedMilliseconds}");
            stopwatch = new Stopwatch();
            stopwatch.Start();
            //////////////////

            // Start
            controlWord.Bit04 = true;
            controlWordEP.SetControlWord(ip, port, controlWord);
            await Delay(50);

            // Check statusWord bit 12
            while (!NewTargetSet(statusWordEP.GetStatusWord(ip, port)))
            {
                Debug.WriteLine("Check bit 12");
                await Delay(50);
            }

            // Reset start
            controlWord.Bit04 = false;
            controlWordEP.SetControlWord(ip, port, controlWord);

            while (!TargetReached(statusWordEP.GetStatusWord(ip, port)) && !InPositionWindow(targetPosition, motorEP.GetActualPosition(ip, port), positionWindow))
            {
                Stopwatch targetReached = new Stopwatch();
                targetReached.Start();
                if (TargetReached(statusWordEP.GetStatusWord(ip, port)) && !InPositionWindow(targetPosition, motorEP.GetActualPosition(ip, port), positionWindow))
                {
                    Debug.WriteLine("--------------------------");
                    Debug.WriteLine("-----------Error----------");
                    Debug.WriteLine("--------------------------");
                }
                targetReached.Stop();
                Debug.WriteLine($"Target check time: {targetReached.ElapsedMilliseconds}");
                await Delay(50);
            }

            stopwatch.Stop();
            Debug.WriteLine($"Movement Time: {stopwatch.ElapsedMilliseconds}");
            return true;
        }

        private async Task Delay(int milliseconds)
        {
            var delay = Task.Run(async delegate
            {
                await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
            });
            delay.Wait();
        }

        public static bool NewTargetSet(StatusWord statusWord)
        {
            if (statusWord.Bit12)
            {
                return true;
            }
            return false;
        }

        public static bool TargetReached(StatusWord statusWord)
        {
            if (statusWord.Bit10)
            {
                return true;
            }
            return false;
        }

        public static bool InPositionWindow(int targetPosition, int actualPosition, int positionWindow)
        {
            if ((actualPosition >= (targetPosition - positionWindow)) && actualPosition <= (targetPosition + positionWindow))
            {
                return true;
            }
            return false;
        }

        public static bool IsReferenced(StatusWord statusWord)
        {
            if (statusWord.Bit10 && statusWord.Bit12)
            {
                return true;
            }
            return false;
        }
    }
}
