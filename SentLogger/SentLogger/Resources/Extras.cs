using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.DeviceInfo;

namespace SentLogger.Resources
{
   public static class Extras
    {

        private enum Platform
        {
            Android,
            iOS,
            WindowsPhone,
            Windows
        }



        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public static bool IsTaskRunning(Task<int> task)
        {
            if ((task != null) && (task.IsCompleted == false ||
                       task.Status == TaskStatus.Running ||
                       task.Status == TaskStatus.WaitingToRun ||
                       task.Status == TaskStatus.WaitingForActivation))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsWindows()
        {
           if( String.Equals( CrossDeviceInfo.Current.Platform.ToString(), Platform.Windows.ToString()))
            {
                return true;
            }
            return false;
        }


        public static bool IsWindowPhone()
        {
            if (String.Equals(CrossDeviceInfo.Current.Platform.ToString(), Platform.WindowsPhone.ToString()))
            {
                return true;
            }
            return false;
        }

        public static bool IsAndroid()
        {
            if (String.Equals(CrossDeviceInfo.Current.Platform.ToString(), Platform.Android.ToString()))
            {
                return true;
            }
            return false;
        }

        public static bool IsIOS()
        {
            if (String.Equals(CrossDeviceInfo.Current.Platform.ToString(), Platform.iOS.ToString()))
            {
                return true;
            }
            return false;
        }
    }
}
