using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;

namespace Chaser
{
    [Register("com.companyname.chaser.AnswerButton")]
    public class AnswerButton : Button //מחלקה היורשת מהכפתור הבסיסי ומוסיפה לו תכונה של נכון או לא נכון, ומה קורה כשלוחצים עליו
    {
        public bool IsTrue { get; set; }
        public event EventHandler ButtonClick;

        public AnswerButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitializeButton();
        }

        public AnswerButton(Context context, bool isTrue) : base(context)
        {
            this.IsTrue = isTrue;
            InitializeButton();
        }

        public AnswerButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            InitializeButton();
        }

        public AnswerButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            InitializeButton();
        }

        private void InitializeButton()
        {
            // Set properties to mimic the desired appearance
            LayoutParameters = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent
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
