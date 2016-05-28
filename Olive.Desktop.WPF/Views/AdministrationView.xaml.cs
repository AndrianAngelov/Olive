using System.Windows;
using System.Windows.Controls;

namespace Olive.Desktop.WPF.Views
{
    /// <summary>
    /// Interaction logic for AdministrationView.xaml
    /// </summary>
    public partial class AdministrationView : UserControl
    {
        public AdministrationView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(this.TextBoxSourceName))
            {
                var errors = Validation.GetErrors(this.TextBoxSourceName);
                string errorMsg = (string)errors[0].ErrorContent;
                MessageBox.Show(errorMsg, "Error");
                return;
            }
        }
    }
}
