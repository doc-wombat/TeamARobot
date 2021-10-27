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
                CTRE.Phoenix.Watchdog.Feed();
                motor.Set(ControlMode.PercentOutput, gamepad.GetAxis(0));
                System.Threading.Thread.Sleep(20);
            }
        }
    }
}
