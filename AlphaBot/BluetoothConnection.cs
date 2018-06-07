using System.Linq;

using Android.Bluetooth;

namespace AlphaBot
{
    public class BluetoothConnection
    {
        public BluetoothAdapter thisAdapter { get; set; }

        public BluetoothDevice thisDevice { get; set; }

        public BluetoothSocket thisSocket { get; set; }

        public void getAdapter()
        {
            thisAdapter = BluetoothAdapter.DefaultAdapter;
        }

        public void getDevice()
        {
            thisDevice = (from bd in thisAdapter.BondedDevices where bd.Name == "HC-05" select bd).FirstOrDefault();
        }
    }

    public sealed class Singleton
    {
        public BluetoothConnection bluetoothAdapter = new BluetoothConnection();

        static readonly Singleton instance = new Singleton();

        private Singleton()
        {
        }

        public static Singleton Instance
        {
            get
            {
                return instance;
            }
        }
    }
}