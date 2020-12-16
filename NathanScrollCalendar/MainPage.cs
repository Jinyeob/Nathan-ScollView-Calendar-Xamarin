using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace NathanScrollCalendar
{
    public partial class MainPage : ContentPage
    {
        public event EventHandler<DateTime> DateSelectedEvent;

        private readonly TapGestureRecognizer _tapGestureRecognizer = new TapGestureRecognizer();
        private readonly static string[] WeekDays = { "일", "월", "화", "수", "목", "금", "토" };

        public MainPage()
        {
            Title = "Nathan Calendar";
            On<iOS>().SetUseSafeArea(true);

            var carouselView = new CarouselView
            {
                HeightRequest = 300,
                WidthRequest = 300,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            var now = DateTime.Now;

            _tapGestureRecognizer.Tapped += DateSelected;

            var stack = new StackLayout { Orientation = StackOrientation.Vertical };
            for (var i = 0; i < 6; i++)
            {
                var dateTime = now.AddMonths(i);

                var innerStack = new Frame
                {
                    Padding = 10,
                    Margin = 0,
                    BackgroundColor = Color.Beige,
                    Content = new StackLayout
                    {
                        Children =
                        {
                            new Label
                            {
                                Text = dateTime.Year.ToString()+"년 "+  dateTime.Month.ToString() + "월",
                                FontSize=20,
                                FontAttributes=FontAttributes.Bold
                            },
                            SetDateGrid(dateTime)
                        }
                    }
                };

                stack.Children.Add(innerStack);
            }



            Content = new StackLayout
            {
                Padding = 10,

                Children = {
                    new Xamarin.Forms.ScrollView
                    {
                        HorizontalOptions=LayoutOptions.CenterAndExpand,
                        Orientation = ScrollOrientation.Vertical,
                        Content = stack
                    }
                }
            };
        }

        private int GetWeeksInMonth(DateTime dateTime)
        {
            //extract the month
            int daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            DateTime firstOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            //days of week starts by default as Sunday = 0
            int firstDayOfMonth = (int)firstOfMonth.DayOfWeek;
            int weeksInMonth = (int)Math.Ceiling((firstDayOfMonth + daysInMonth) / 7.0);

            return weeksInMonth;
        }

        private DateGrid SetDateGrid(DateTime dateTime)
        {
            var dateGrid = new DateGrid();

            for (int i = 0; i < GetWeeksInMonth(dateTime); i++)
            {
                dateGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            var nowMonthDaysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month); // 지정한 연도, 달의 날짜 수
            var startingDay = (int)new DateTime(dateTime.Year, dateTime.Month, 1).DayOfWeek; // 시작 요일 (월요일==1) 

            var previousMonthDateTime = dateTime.AddMonths(-1);
            var previousMonthDaysInMonth = DateTime.DaysInMonth(previousMonthDateTime.Year, previousMonthDateTime.Month);
            var counter = 0;
            var nextMonthCounter = 1;



            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    int date = 0;

                    var cell = new CellStackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                    };

                    var label = new Label
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center
                    };

                    if (row == 0)
                    {
                        var color = Color.Black;
                        if (col == 0)
                        {
                            color = Color.Red;
                        }
                        else if (col == 6)
                        {
                            color = Color.Blue;
                        }

                        label.Text = WeekDays[col];
                        label.TextColor = color;
                        label.FontAttributes = FontAttributes.Bold;

                        cell.Children.Add(label);
                        dateGrid.Children.Add(cell, col, row);
                    }
                    else
                    {
                        if (counter < startingDay)
                        {
                            date = previousMonthDaysInMonth - (startingDay - counter - 1);

                            label.Text = date.ToString();
                            label.TextColor = Color.Gray;

                            cell.DateTimeInfo = new DateTime(dateTime.AddMonths(-1).Year,
                                    dateTime.AddMonths(-1).Month, date);


                            //cell.Children.Add(label);
                            //dateGrid.Children.Add(cell, col, row);
                        }

                        else if (counter >= startingDay && (counter - startingDay) < nowMonthDaysInMonth)
                        {
                            date = counter + 1 - startingDay;

                            label.Text = date.ToString();

                            cell.DateTimeInfo = new DateTime(dateTime.Year, dateTime.Month, date);

                            if (DateTime.Now.Year == dateTime.Year &&
                                        DateTime.Now.Month == dateTime.Month &&
                                        DateTime.Now.Day == date)
                            {
                                label.TextColor = Color.Accent;
                            }
                            else if (DateTime.Now.Year > dateTime.Year ||
                                     (DateTime.Now.Year == dateTime.Year && DateTime.Now.Month > dateTime.Month) ||
                                    (DateTime.Now.Year == dateTime.Year && DateTime.Now.Month == dateTime.Month && DateTime.Now.Day > date))
                            {
                                label.TextColor = Color.Gray;
                            }
                            else
                            {
                                label.TextColor = Color.Black;
                                AddTapGesture(cell);
                            }

                            cell.Children.Add(label);
                            dateGrid.Children.Add(cell, col, row);
                        }

                        else if (counter >= (nowMonthDaysInMonth + startingDay))
                        {
                            date = nextMonthCounter++;

                            label.Text = date.ToString();
                            label.TextColor = Color.Gray;

                            cell.DateTimeInfo = new DateTime(dateTime.AddMonths(+1).Year,
                                   dateTime.AddMonths(+1).Month, date);
                        }
                        counter++;
                    }


                    //cellStackLayouts.Add(cell);
                }
            }
            return dateGrid;
        }

        private void AddTapGesture(CellStackLayout cell)
        {
            cell.GestureRecognizers.Clear();
            cell.GestureRecognizers.Add(_tapGestureRecognizer);
        }


        private async void DateSelected(object s, EventArgs e)
        {
            var cell = s as CellStackLayout;
            var dateString = $"{cell.dateTime.Year}-{cell.dateTime.Month}-{cell.dateTime.Day}";
            bool answer = await App.Current.MainPage.DisplayAlert("Date Selected!", dateString + "로 변경하시겠습니까?", "OK", "Cancel");

            if (answer) { DateSelectedEvent?.Invoke(this, cell.dateTime); }

        }
    }
}