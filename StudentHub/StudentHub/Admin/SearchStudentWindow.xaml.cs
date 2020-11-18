using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudentHub.Admin
{
    /// <summary>
    /// Логика взаимодействия для SearchStudentWindow.xaml
    /// </summary>
    public partial class SearchStudentWindow : Window
    {
        private Window _window;
        private int _index;
        public SearchStudentWindow(int index)
        {
            InitializeComponent();
            _index = index;
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_index == 1)
            {
                _window = new ViewProgressWindow(studentNameTextBox.Text);
            }

            if (_index == 2)
            {
                _window = new EmailGenerationWindow(studentNameTextBox.Text);
            }

        }
    }
}
