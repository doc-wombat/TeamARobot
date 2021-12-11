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
        static TalonSRX bat = new TalonSRX(2);
        public static void Main()
        {
            leftMotor.ConfigFactoryDefault();
            rightMotor.ConfigFactoryDefault();
            bat.ConfigFactoryDefault();

            while (true)
            {
                CTRE.Phoenix.Watchdog.Feed();
                drive();
                if (gamepad.GetButton(1))
                {
                    swingBat();
                }
                if (gamepad.GetButton(2))
                {
                    auton();
                }
                if (gamepad.GetButton(5))
                {
                    leftMotor.Set(ControlMode.PercentOutput, 0);
                    rightMotor.Set(ControlMode.PercentOutput, 0);
                    bat.Set(ControlMode.PercentOutput, 0);
                }
                if (gamepad.GetButton(8))
                {
                    System.Threading.Thread.CurrentThread.Abort();
                }
            }
        }

        // Values within 10% of 0 get ignored (Controller deadzone)
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
            rightMotor.Set(ControlMode.PercentOutput, -rightThrot);
        }
        
        /* Spins wheel exactly once (hopefully), pulling 
         * back spring, then spins back once to release tension.
         * 
         //*/
        public static void swingBat()
        {
            long startTime = millis();
            while ((millis() - startTime) < 588) 
            {
                bat.Set(ControlMode.PercentOutput, 1);
                CTRE.Phoenix.Watchdog.Feed();
            }
            bat.Set(ControlMode.PercentOutput, 0);
            System.Threading.Thread.Sleep(500);
            startTime = millis();
            while ((millis() - startTime) < 588)
            {
                bat.Set(ControlMode.PercentOutput, -1);
                CTRE.Phoenix.Watchdog.Feed();
            }
            bat.Set(ControlMode.PercentOutput, 0);
        }
        // Autonomous code
        public static void auton()

        {
            long startTime = millis();
            while((millis() - startTime) < 2000)
            {
                // Move forward for 1200 seconds
                leftMotor.Set(ControlMode.PercentOutput, 1);
                rightMotor.Set(ControlMode.PercentOutput, -1);
                CTRE.Phoenix.Watchdog.Feed();
            }
            leftMotor.Set(ControlMode.PercentOutput, 0);
            rightMotor.Set(ControlMode.PercentOutput, 0);
            startTime = millis();
            while ((millis() - startTime) < 500)
            {
                // Turn right for 500 seconds
                leftMotor.Set(ControlMode.PercentOutput, 1);
                rightMotor.Set(ControlMode.PercentOutput, 1);
                CTRE.Phoenix.Watchdog.Feed();
            }
            leftMotor.Set(ControlMode.PercentOutput, 0);
            rightMotor.Set(ControlMode.PercentOutput, 0);
            startTime = millis();
            while ((millis() - startTime) < 390)
            {
                // Move forward for 1200 seconds
                leftMotor.Set(ControlMode.PercentOutput, 1);
                rightMotor.Set(ControlMode.PercentOutput, -1);
                CTRE.Phoenix.Watchdog.Feed();
            }
            leftMotor.Set(ControlMode.PercentOutput, 0);
            rightMotor.Set(ControlMode.PercentOutput, 0);
            startTime = millis();
        }

        // Gets current # of milliseconds
        public static long millis()
        {
            return DateTime.Now.Ticks / (TimeSpan.TicksPerMillisecond);
        }
    }
}
