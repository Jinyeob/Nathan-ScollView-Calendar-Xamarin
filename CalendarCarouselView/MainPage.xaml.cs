using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CalendarCarouselView
{
    public partial class MainPage : ContentPage
    {
        private readonly TapGestureRecognizer _tapGestureRecognizer = new TapGestureRecognizer();
        private List<CellStackLayout> cellStackLayouts;
        private static string[] WeekDays = { "일", "월", "화", "수", "목", "금", "토" };

        public MainPage()
        {
            BindingContext = new MonthViewModel();

            var carouselView = new CarouselView
            {
                HeightRequest = 300,
                WidthRequest = 300,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            var now = DateTime.Now;

            _tapGestureRecognizer.Tapped += DateSelected;

            var stack = new StackLayout { Orientation=StackOrientation.Vertical };
            for (var i = 0; i < 6; i++)
            {
                var dateTime = now.AddMonths(i);
                var dateGrid = SetDateGrid(dateTime);
                stack.Children.Add(new Label
                {
                    Text = dateTime.Year.ToString()+"년 "+  dateTime.Month.ToString() + "월",
                    FontSize=20,
                    FontAttributes=FontAttributes.Bold
                });
                stack.Children.Add(dateGrid);
            }
            


            Content = new ScrollView
            {
                Content = stack
            };
        }

        private DateGrid SetDateGrid(DateTime dateTime)
        {
            cellStackLayouts = new List<CellStackLayout>();

            var dateGrid = new DateGrid();

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
                    int date=0;

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
                        label.Text = WeekDays[col];
                        label.TextColor = Color.Black;
                        cell.BackgroundColor = Color.Pink;
                    }
                    else
                    {
                        if (counter < startingDay)
                        {
                            date = previousMonthDaysInMonth - (startingDay - counter - 1);

                            label.Text = date.ToString();
                            label.TextColor = Color.White;

                            cell.DateTimeInfo = new DateTime(dateTime.AddMonths(-1).Year,
                                    dateTime.AddMonths(-1).Month, date);
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
                            else if(DateTime.Now.Year > dateTime.Year ||
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

                        }

                        else if (counter >= (nowMonthDaysInMonth + startingDay))
                        {
                            date = nextMonthCounter++;

                            label.Text = date.ToString();
                            label.TextColor = Color.White;

                            cell.DateTimeInfo = new DateTime(dateTime.AddMonths(+1).Year,
                                   dateTime.AddMonths(+1).Month, date);
                        }
                        counter++;
                    }


                    cellStackLayouts.Add(cell);
                    cell.Children.Add(label);
                    dateGrid.Children.Add(cell, col, row);

                  
                }
            }
            return dateGrid;
        }

        private void AddTapGesture(CellStackLayout cell)
        {
            cell.GestureRecognizers.Clear();
            cell.GestureRecognizers.Add(_tapGestureRecognizer);
        }


        private void DateSelected(object s, EventArgs e)
        {
            var cell = s as CellStackLayout;
            var dateString = $"{cell.dateTime.Year}-{cell.dateTime.Month}-{cell.dateTime.Day}";
            DisplayAlert(dateString, null, "cancel");
        }
    }
}





//carouselView.SetBinding(ItemsView.ItemsSourceProperty, "Months");

//_tapGestureRecognizer.Tapped += async (s, e) =>
//{
//    await DisplayAlert("Tapped", null, "cancel");
//};

//carouselView.ItemTemplate = new DataTemplate(() =>
//  {
//      var dayGrid = new Grid
//      {
//          ColumnSpacing = 0,
//          RowSpacing = 0,
//          HorizontalOptions = LayoutOptions.FillAndExpand,
//          VerticalOptions = LayoutOptions.FillAndExpand,
//      };
//      dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//      dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//      dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//      dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//      dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//      dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//      dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//      dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//      dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//      dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//      dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//      dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//      dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//      dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

//      int i = 1;


//      for (var row = 0; row < 6; row++)
//      {
//          for (var col = 0; col < 6; col++)
//          {
//              var cell = new StackLayout
//              {
//                  VerticalOptions = LayoutOptions.FillAndExpand,
//                  HorizontalOptions = LayoutOptions.FillAndExpand,
//              };
//              var label = new Label
//              {
//                  VerticalOptions = LayoutOptions.FillAndExpand,
//                  HorizontalOptions = LayoutOptions.FillAndExpand,
//                  VerticalTextAlignment = TextAlignment.Center,
//                  HorizontalTextAlignment = TextAlignment.Center,
//              };
//              label.Text = i.ToString();

//              cell.Children.Add(label);

//              cell.GestureRecognizers.Add(_tapGestureRecognizer);

//              dayGrid.Children.Add(cell, col, row);

//              i++;
//          }
//      }
//      var monthLabel = new Label
//      {
//          FontSize = 20,
//          FontAttributes = FontAttributes.Bold
//      };
//      monthLabel.SetBinding(Label.TextProperty, "Date");

//      return new StackLayout
//      {
//          Children =
//          {
//              monthLabel,

//              new Frame
//              {
//                  Content= dayGrid
//              }
//          }
//      };

//  });