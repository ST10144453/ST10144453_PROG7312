//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using ST10144453_PROG7312.MVVM.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312
{
    //============== Class: ViewModelLocator ==============//
    /// <summary>
    /// This class holds the base implementation for the ViewModelLocator class.
    /// </summary>
    public class ViewModelLocator
    {
        //++++++++++++++ Properties: HomeViewModel ++++++++++++++//
        /// <summary>
        /// This property holds the home view model.
        /// </summary>
        public HomeViewModel HomeViewModel { get; } = new HomeViewModel();

    }
}
//0000000000oooooooooo..........End Of File..........ooooooooooo00000000000//