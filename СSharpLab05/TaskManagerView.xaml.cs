using CSharpLab05;
using System.Windows.Controls;

namespace СSharpLab05
{
    /// <summary>
    /// Логика взаимодействия для TaskManagerView.xaml
    /// </summary>
    public partial class TaskManagerView : UserControl, INavigatable
    {
        public TaskManagerView()
        {
            InitializeComponent();
            DataContext = new TaskManagerViewModel();
        }
    }
}
