//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ST10144453_PROG7312.Core
{
    //============== Class: RelayCommand ==============//
    /// <summary>
    /// This class holds the base implementation for the RelayCommand class.
    /// </summary>
    public class RelayCommand : ICommand
    {
        //++++++++++++++ Declarations ++++++++++++++//
        /// <summary>
        /// This action is executed when the command is executed.
        /// </summary>
        private readonly Action _execute;

        /// <summary>
        /// This function determines whether the command can be executed.
        /// </summary>
        private readonly Func<bool> _canExecute;

        //~~~~~~~~~~~~~ Methods: Default Constructor ~~~~~~~~~~~~~//
        /// <summary>
        /// This constructor initializes the RelayCommand class.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        //~~~~~~~~~~~~~ Methods: CanExecute ~~~~~~~~~~~~~//
        /// <summary>
        /// This method determines whether the command can be executed.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        //~~~~~~~~~~~~~ Methods: Execute ~~~~~~~~~~~~~//
        /// <summary>
        /// This method executes the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter) => _execute();

        //~~~~~~~~~~~~~ Events: CanExecuteChanged ~~~~~~~~~~~~~//
        /// <summary>
        /// This event is raised when the command can be executed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//
