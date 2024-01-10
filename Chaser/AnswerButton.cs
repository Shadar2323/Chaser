using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;

namespace Chaser
{
    public class AnswerButton : Button
    {
        public bool isTrue { get; set;}
        public event EventHandler ButtonClick;
        public AnswerButton(Context context, bool isTrue) : base(context)
        {
            this.isTrue = isTrue;
            InitializeButton();
        }
        private void InitializeButton()
        {
            // Set properties to mimic the desired appearance
            LayoutParameters = new LinearLayout.LayoutParams(
                0,
                ViewGroup.LayoutParams.WrapContent,
                1.0f
            );
            SetTextColor(Color.White);
            BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Color.Black);

            // Set other properties as needed
            Click += (sender, e) =>
            {
                // Raise the custom event when the button is clicked
                ButtonClick?.Invoke(this, EventArgs.Empty);
            };
        }

    }
}