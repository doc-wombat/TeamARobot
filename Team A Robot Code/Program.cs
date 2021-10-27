using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System;
using System.Threading;

using CTRE.Phoenix;
using CTRE.Phoenix.Controller;
using CTRE.Phoenix.MotorControl;
using CTRE.Phoenix.MotorControl.CAN;

namespace fartbutt
{
    public class Program
    {
        public static void Main()
        {
            GameController gamepad = new GameController(UsbHostDevice.GetInstance(0));
            TalonSRX motor = new TalonSRX(0);
            motor.ConfigFactoryDefault();

            while (true)
            {
                /* print the axis value */
                Debug.Print("axis:" + gamepad.GetAxis(1));
                motor.Set(ControlMode.PercentOutput, gamepad.GetAxis(1)); //axis changed from left stick X to left stick Y
                CTRE.Phoenix.Watchdog.Feed();
                System.Threading.Thread.Sleep(20);
            }
        }
    }
}
