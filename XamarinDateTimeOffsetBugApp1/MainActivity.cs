using Android.App;
using Android.OS;
using Android.Util;
using Android.Widget;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace XamarinDateTimeOffsetBugApp1
{
    [Activity(Label = "XamarinDateTimeOffsetBugApp1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    DateTimeOffset offset = new DateTimeOffset(DateTime.Now);
                    string serializedOffset;

                    var serializer = new DataContractJsonSerializer(typeof(DateTimeOffset));
                    Log.Info(nameof(MainActivity), "HACKAGE:about to serialize");
                    serializer.WriteObject(stream, offset);
                    serializedOffset = Encoding.UTF8.GetString(stream.ToArray(), 0, (int)stream.Position);
                    Log.Info(nameof(MainActivity), "HACKAGE: got serialized" + serializedOffset);
                }
                button.Text = string.Format("{0} clicks!", count++);
            };
        }
        static void ExistenceCausesCrash(string serializedObject)
        {
            DateTimeOffset resultEx;
            var serializer = new DataContractJsonSerializer(typeof(DateTimeOffset));
            byte[] serializedObjectBytes = Encoding.UTF8.GetBytes(serializedObject);
            using (var stream = new MemoryStream(serializedObjectBytes))
            {
                resultEx = (DateTimeOffset)serializer.ReadObject(stream);
            }
        }
    }
}

