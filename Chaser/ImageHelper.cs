using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chaser
{
    public class ImageHelper
    {
        public static string SaveBitmapToFile(Bitmap bitmap)//מחלקה שעוזרת לי לשמור את התמונה שנבחרה כקובץ, היא משלבת את המקום בו נשמרות התמונות בעזרת השם הייחודי שניתן לתמונה ומחזירה את המסלול שלה
        {
            // Save the bitmap to a file
            string directory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
            string fileName = $"profile_{DateTime.Now.Ticks}.jpg";
            string filePath = System.IO.Path.Combine(directory, fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, fileStream);
            }

            return filePath;
        }
        public static string SaveCircularBitmapToFile(Android.Graphics.Bitmap bitmap)
        {
            // Create a file to save the circular bitmap
            string filePath = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath, "profile_image.png");

            using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                // Save the bitmap with PNG format to preserve transparency
                bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, stream);
            }

            return filePath;
        }
        public static Android.Graphics.Bitmap ResizeBitmap(Android.Graphics.Bitmap bitmap, int widthDp, int heightDp)
        {
            // Get the display density
            float density = Android.App.Application.Context.Resources.DisplayMetrics.Density;

            // Calculate the width and height in pixels
            int widthPx = (int)(widthDp * density + 0.5f);
            int heightPx = (int)(heightDp * density + 0.5f);

            // Resize the bitmap
            Android.Graphics.Bitmap resizedBitmap = Android.Graphics.Bitmap.CreateScaledBitmap(bitmap, widthPx, heightPx, true);

            return resizedBitmap;
        }
        public static Android.Graphics.Bitmap GetCircularBitmap(Android.Graphics.Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int diameter = Math.Min(width, height);

            Android.Graphics.Bitmap output = Android.Graphics.Bitmap.CreateBitmap(diameter, diameter, Android.Graphics.Bitmap.Config.Argb8888);
            Android.Graphics.Canvas canvas = new Android.Graphics.Canvas(output);

            Android.Graphics.Paint paint = new Android.Graphics.Paint();
            paint.AntiAlias = true;

            float radius = diameter / 2f;
            canvas.DrawCircle(radius, radius, radius, paint);

            paint.SetXfermode(new Android.Graphics.PorterDuffXfermode(Android.Graphics.PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(bitmap, (diameter - width) / 2f, (diameter - height) / 2f, paint);

            return output;
        }

        public static Bitmap GetBitmapFromUri(Context context, Android.Net.Uri uri)//פעולה שמשיגה את הקובץ של התמונה ומחזירה אותו בbitmap
        {
            // Convert URI to Bitmap
            return BitmapFactory.DecodeStream(context.ContentResolver.OpenInputStream(uri));
        }
    }
}