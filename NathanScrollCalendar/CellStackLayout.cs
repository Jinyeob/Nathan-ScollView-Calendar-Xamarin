using System;
using Xamarin.Forms;

namespace NathanScrollCalendar
{
    public class CellStackLayout : StackLayout
    {
        public DateTime dateTime;
        public bool isSelected = false;
       // public Label selectedDot;

        public CellStackLayout()
        {
            Spacing = 0;
            Padding = 0;
            Margin = 0;
            HeightRequest = 40;
            WidthRequest = 40;
        }

        public DateTime DateTimeInfo
        {
            get { return dateTime; }
            set
            {
                dateTime = value;
            }
        }
        public bool IsSelected
        {
            get { return IsSelected; }
            set
            {
                isSelected = value;
            }
        }
    }
}
