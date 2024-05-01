using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaser
{
    [Service]
    public class MusicService : Service
    {
        private MediaPlayer mediaPlayer;

        // This method is called when another component wants to bind with the service
        public override IBinder OnBind(Intent intent)
        {
            return null; // Return null because this service does not support binding
        }

        // This method is called when the service is started
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // Initialize the MediaPlayer with the background music file
            mediaPlayer = MediaPlayer.Create(this, Resource.Raw.AngryBirdsTheme);

            // Set looping to true for continuous playback
            mediaPlayer.Looping = true;

            // Start playing the background music
            mediaPlayer.Start();

            // Indicate that this service should continue running until explicitly stopped
            return StartCommandResult.Sticky;
        }

        // This method is called when the service is destroyed
        public override void OnDestroy()
        {
            // Release the MediaPlayer resources when the service is destroyed
            if (mediaPlayer != null)
            {
                mediaPlayer.Stop(); // Stop playback
                mediaPlayer.Release(); // Release resources
                mediaPlayer = null; // Set the reference to null
            }
            base.OnDestroy(); // Call the base class method
        }
    }
}