using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ST10144453_PROG7312.Core
{
    public class NavigationService
    {
        private readonly ContentControl _contentControl;

        public NavigationService(ContentControl contentControl)
        {
            _contentControl = contentControl;
        }

        public void Navigate(UserControl newControl)
        {
            _contentControl.Content = newControl;
        }
    }
}
