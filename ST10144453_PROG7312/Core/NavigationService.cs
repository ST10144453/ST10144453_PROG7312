//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ST10144453_PROG7312.Core
{
    /// <summary>
    /// Provides navigation functionality for switching between user controls.
    /// </summary>
    public class NavigationService
    {
        private readonly ContentControl _contentControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="contentControl">The content control used for displaying user controls.</param>
        public NavigationService(ContentControl contentControl)
        {
            _contentControl = contentControl;
        }

        /// <summary>
        /// Navigates to the specified user control.
        /// </summary>
        /// <param name="newControl">The user control to navigate to.</param>
        public void Navigate(UserControl newControl)
        {
            _contentControl.Content = newControl;
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//
