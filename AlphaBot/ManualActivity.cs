using System.Text;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace AlphaBot
{
    [Activity(Label = "ManualActivity", Theme = "@style/Theme.AppCompat.DayNight.NoActionBar")]
    public class ManualActivity : AppCompatActivity
    {
        private Singleton objSingle = Singleton.Instance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Manual);

            var UP = FindViewById<Button>(Resource.Id.UP);
            var LEFT = FindViewById<Button>(Resource.Id.LEFT);
            var RIGHT = FindViewById<Button>(Resource.Id.RIGHT);
            var DOWN = FindViewById<Button>(Resource.Id.DOWN);

            DOWN.Touch += ButtonDown_KeyPress;
            UP.Touch += ButtonUp_KeyPress;
            LEFT.Touch += ButtonLeft_KeyPress;
            RIGHT.Touch += ButtonRight_KeyPress;

        }

        private void ButtonRight_KeyPress(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                SendCommand("Right");
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                SendCommand("Stop");
            }
        }

        private void ButtonLeft_KeyPress(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                SendCommand("Left");
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                SendCommand("Stop");
            }
        }

        private void ButtonDown_KeyPress(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                SendCommand("Backward");
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                SendCommand("Stop");
            }
        }

        private void ButtonUp_KeyPress(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                SendCommand("Forward");
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                SendCommand("Stop");
            }
        }

        private void SendCommand(string command)
        {
            try
            {
                if (objSingle.bluetoothAdapter.thisSocket.IsConnected)
                {
                    byte[] text = Encoding.ASCII.GetBytes(command);
                    objSingle.bluetoothAdapter.thisSocket.OutputStream.Write(text, 0, text.Length);
                }
                else
                {
                    Toast.MakeText(this, "Device is not connected.", ToastLength.Short).Show();
                }
            }
            catch
            {
                Toast.MakeText(this, "Device is not connected.", ToastLength.Short).Show();
            }
        }
    }
}