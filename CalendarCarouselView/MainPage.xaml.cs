using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CalendarCarouselView
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BindingContext = new MonthViewModel();

            var carouselView = new CarouselView { HeightRequest=300, WidthRequest=300, VerticalOptions=LayoutOptions.CenterAndExpand,
            HorizontalOptions=LayoutOptions.CenterAndExpand};
            carouselView.SetBinding(ItemsView.ItemsSourceProperty, "Months");

            carouselView.ItemTemplate = new DataTemplate(() =>
              {
                  var _monthGrid = new Grid
                  {
                      ColumnSpacing = 0,
                      RowSpacing = 0,
                      HorizontalOptions = LayoutOptions.FillAndExpand,
                      VerticalOptions = LayoutOptions.FillAndExpand,
                  };
                  _monthGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                  _monthGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  _monthGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                  for (var row = 0; row < 6; row++)
                  {
                      for (var col = 0; col < 6; col++)
                      {
                          var cell = new StackLayout
                          {
                              VerticalOptions = LayoutOptions.FillAndExpand,
                              HorizontalOptions = LayoutOptions.FillAndExpand,
                          };
                          var label = new Label
                          {
                              VerticalOptions = LayoutOptions.FillAndExpand,
                              HorizontalOptions = LayoutOptions.FillAndExpand,
                              VerticalTextAlignment = TextAlignment.Center,
                              HorizontalTextAlignment = TextAlignment.Center,
                          };
                          label.SetBinding(Label.TextProperty, "Date");

                          cell.Children.Add(label);
                          _monthGrid.Children.Add(cell, col, row);
                   
                      }
                  }
                  return new Frame
                  {
                      Content = cell
                  };
              });

            Content = new StackLayout
            {
                BackgroundColor=Color.Beige,
                Children = { carouselView }
            };
        }
    }
}
