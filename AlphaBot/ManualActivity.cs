using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlphaBot
{
    [Activity(Label = "ManualActivity")]
    public class ManualActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        private void ButtonUp_KeyPress(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == Android.Views.MotionEventActions.Down)
            {
                SendCommand("Forward");
            }
            else if (e.Event.Action == Android.Views.MotionEventActions.Up)
            {
                SendCommand("Stop");
            }
        }

        private void SendCommand(string command)
        {
            throw new NotImplementedException();
        }
    }
}