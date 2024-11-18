using System.Collections.Generic;
using System.Windows.Controls;
using ST10144453_PROG7312.MVVM.Model;

namespace ST10144453_PROG7312.MVVM.View.Visualizations
{
    public abstract class BaseVisualizationControl : UserControl
    {
        protected Canvas visualizationCanvas;

        public BaseVisualizationControl()
        {
            visualizationCanvas = new Canvas();
            Content = visualizationCanvas;
        }

        public abstract void UpdateVisualization(IEnumerable<ServiceRequestModel> requests);
        public abstract string GetDescription();
    }
}
