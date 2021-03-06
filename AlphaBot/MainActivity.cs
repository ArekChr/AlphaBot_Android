﻿using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaBot
{
    [Activity(Label = "AlphaBot_RC", MainLauncher = true, Icon = "@drawable/icon", Theme ="@style/Theme.AppCompat.DayNight.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        private Singleton objSingle = Singleton.Instance;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "AlphaBot RC";
            objSingle.bluetoothAdapter.getAdapter();

            var send = FindViewById<Button>(Resource.Id.send);

            send.Click += Send_Click;
            if(objSingle.bluetoothAdapter.thisAdapter == null)
            {
                Toast.MakeText(this, "No Bluetooth adapter found.", ToastLength.Long).Show();
            }
            if (!objSingle.bluetoothAdapter.thisAdapter.IsEnabled)
            {
                Intent enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(enableIntent, 2);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.toolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if(id == Resource.Id.connect)
            {
                try
                {
                    if (objSingle.bluetoothAdapter.thisSocket.IsConnected)
                    {
                        Toast.MakeText(this, "Device is already connected.", ToastLength.Short).Show();
                        return true;
                    }
                }
                catch{}

                Toast.MakeText(this, "Connecting...", ToastLength.Short).Show();
                objSingle.bluetoothAdapter.getDevice();

                objSingle.bluetoothAdapter.thisSocket = 
                    objSingle
                    .bluetoothAdapter
                    .thisDevice
                    .CreateRfcommSocketToServiceRecord(
                    Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

                Task.Run(async () =>
                {
                    await objSingle.bluetoothAdapter.thisSocket.ConnectAsync();
                    RunOnUiThread(() =>
                    {
                        if (objSingle.bluetoothAdapter.thisSocket.IsConnected)
                        {
                            Toast.MakeText(this, "Connected succesfull.", ToastLength.Short).Show();
                        }
                    });
                });

                return true;
            }
            else if(id == Resource.Id.manual)
            {
                var manualActivity = new Intent(this, typeof(ManualActivity));
                StartActivity(manualActivity);
                return true;
            }
            return true;
        }

        private void Send_Click(object sender, EventArgs e)
        {
            try
            {
                if (!objSingle.bluetoothAdapter.thisSocket.IsConnected)
                {
                    Toast.MakeText(this, "Device is not connected.", ToastLength.Short).Show();
                    return;
                }
            }
            catch
            {
                Toast.MakeText(this, "Device is not connected.", ToastLength.Short).Show();
                return;
            }

            TextView PosX = FindViewById<TextView>(Resource.Id.posx);
            TextView PosY = FindViewById<TextView>(Resource.Id.posy);

            if (string.IsNullOrEmpty(PosX.Text) && string.IsNullOrEmpty(PosY.Text) || PosX.Text == "0" && PosY.Text == "0")
            {
                Toast.MakeText(this, "Can not send empty value.", ToastLength.Long).Show();
            }
            else
            {
                string Coordinates;

                if (string.IsNullOrEmpty(PosX.Text))
                {
                    Coordinates = "XY(" + "0" + "," + PosY.Text + ")";
                }
                else if (string.IsNullOrEmpty(PosY.Text))
                {
                    Coordinates = "XY(" + PosX.Text + "," + "0" + ")";
                }
                else
                {
                    Coordinates = "XY(" + PosX.Text + "," + PosY.Text + ")";
                }
                PosX.Text = "";
                PosY.Text = "";

                try
                {
                    byte[] text = Encoding.ASCII.GetBytes(Coordinates);
                    objSingle.bluetoothAdapter.thisSocket.OutputStream.Write(text, 0, text.Length);
                    Toast.MakeText(this, "Coordinates sent: " + Coordinates, ToastLength.Long).Show();
                }
                catch (Java.Lang.Exception outPutEX)
                {
                    Toast.MakeText(this, $"Error: {outPutEX}" + Coordinates, ToastLength.Short).Show();
                }

                Coordinates = "";
            }
        }
    }
}