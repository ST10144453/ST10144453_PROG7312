using ST10144453_PROG7312.MVVM.Model;
using System.Windows;

namespace ST10144453_PROG7312.MVVM.View
{
    public partial class EventDetailsPopup : Window
    {
        public EventDetailsPopup(EventModel eventModel)
        {
            InitializeComponent();
            DataContext = new
            {
                EventTitle = eventModel.eventTitle,
                EventDate = eventModel.eventDate,
                EventLocation = eventModel.eventLocation,
                EventDescription = eventModel.eventDescription,
                EventPhotos = eventModel.eventPhotos,
                EventTags = eventModel.eventTags
            };
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}