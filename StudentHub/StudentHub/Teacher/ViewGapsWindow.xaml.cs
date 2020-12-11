using System;
using System.Collections.Generic;
using System.Data;
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
using Oracle.ManagedDataAccess.Client;
using StudentHub.DataBase;

namespace StudentHub.Teacher
{
    /// <summary>
    /// Логика взаимодействия для ViewGapsWindow.xaml
    /// </summary>
    public partial class ViewGapsWindow : Window
    {
        private University.Teacher _teacher;

        public ViewGapsWindow(University.Teacher teacher)
        {
            _teacher = teacher;
            InitializeComponent();
        }

        private void GetGaps()
        {

        }
    }
}
