using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernControls
{
    public class NumericBox : ContentControl
    {
        private TextBox PART_NumericTextBox;
        public NumericBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericBox), new FrameworkPropertyMetadata(typeof(NumericBox)));
        }

        public static readonly DependencyProperty ValueProperty =
    DependencyProperty.Register("Value", typeof(int), typeof(NumericBox), new FrameworkPropertyMetadata());
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                PART_NumericTextBox.Text = value >= Minimum ? value.ToString() : Minimum.ToString();
            }
        }

        

        public static readonly DependencyProperty MinimumProperty =
    DependencyProperty.Register("Minimum", typeof(int), typeof(NumericBox), new FrameworkPropertyMetadata());
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty IncrementProperty =
    DependencyProperty.Register("Increment", typeof(int), typeof(NumericBox), new FrameworkPropertyMetadata());
        public int Increment
        {
            get { return (int)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button btn = GetTemplateChild("PART_IncreaseButton") as Button;
            if (btn != null)
            {
                btn.Click += increaseBtn_Click;
            }

            btn = GetTemplateChild("PART_DecreaseButton") as Button;
            if (btn != null)
            {
                btn.Click += decreaseBtn_Click;
            }

            PART_NumericTextBox = GetTemplateChild("PART_NumericTextBox") as TextBox;
            if (PART_NumericTextBox != null)
            {
                if (Value != null)
                {
                    PART_NumericTextBox.Text = Value.ToString();
                }
                PART_NumericTextBox.PreviewTextInput += numericBox_TextInput;
                PART_NumericTextBox.MouseWheel += numericBox_MouseWheel;
            }
            btn = null;
        }
        private void numericBox_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
        }

        private void numericBox_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                Value = Convert.ToInt32((sender as TextBox).Text);
            }
            catch
            {
                Value = Minimum;
            }
        }

        private void decreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            Value -= Increment;
        }

        private void increaseBtn_Click(object sender, RoutedEventArgs e)
        {
            Value += Increment;
        }
    }
}
