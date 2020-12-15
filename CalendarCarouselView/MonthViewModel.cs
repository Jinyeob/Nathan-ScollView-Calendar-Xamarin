using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace CalendarCarouselView
{
    public class MonthViewModel : INotifyPropertyChanged
    {
        private readonly IList<Month> source;
        public ObservableCollection<Month> Months { get; set; }

        public MonthViewModel()
        {
            source = new List<Month>();
            setData();

        }

        private void setData()
        {
            for(int i = 1; i <= 12; i++)
            {
                source.Add(new Month
                {
                    Date = i.ToString()
                });
            }
            Months = new ObservableCollection<Month>(source);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
