//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System.Windows;

namespace ST10144453_PROG7312.MVVM.View
{
    //============== Class: Form1 ==============//
    /// <summary>
    /// Interaction logic for UnderDevelopmentPopup.xaml
    /// </summary>
    public partial class UnderDevelopmentPopup : Window
    {
        //++++++++++++++ Methods: Default Constructor ++++++++++++++//
        /// <summary>
        /// This method initializes the UnderDevelopmentPopup class.
        /// </summary>
        public UnderDevelopmentPopup()
        {
            InitializeComponent();
        }

        //++++++++++++++ Methods: CloseButton_Click ++++++++++++++//
        /// <summary>
        /// This method closes the UnderDevelopmentPopup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
//0000000000oooooooooo..........End Of File..........ooooooooooo00000000000//