using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System;
using System.Threading;

using CTRE.Phoenix;
using CTRE.Phoenix.Controller;
using CTRE.Phoenix.MotorControl;
using CTRE.Phoenix.MotorControl.CAN;

namespace TeamA
{
    public class Program
    {
        static GameController gamepad = new GameController(UsbHostDevice.GetInstance(0));
        static TalonSRX leftMotor = new TalonSRX(0);
        static TalonSRX rightMotor = new TalonSRX(1);
        public static void Main()
        {
            leftMotor.ConfigFactoryDefault();
            rightMotor.ConfigFactoryDefault();

            while (true)
            {
                CTRE.Phoenix.Watchdog.Feed();
                drive();
                System.Threading.Thread.Sleep(20);
            }
        }

        /* Values within 10% of 0 get ignored (Controller deadzone) */
        public static void deadzone(double axis)
        {
            if ((axis > .1) && (axis < -.1))
            {

            }
            else
            {
                axis = 0;
            }
        }

        /*
         * y = forward/backward throttle
         * twist = turn
         * Checks for deadzone, ignores axis values that are
         * < 0.1 and > -0.1
         */
        public static void drive()
        {
            double y = (gamepad.GetAxis(1) * -1);
            double twist = gamepad.GetAxis(0);

            deadzone(y);
            deadzone(twist);

            double leftThrot = y + twist;
            double rightThrot = y - twist;

            leftMotor.Set(ControlMode.PercentOutput, leftThrot);
            rightMotor.Set(ControlMode.PercentOutput, rightThrot);
        }
    }
}
