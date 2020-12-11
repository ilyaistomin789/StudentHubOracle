using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using StudentHub.Account;
using StudentHub.Admin;
using StudentHub.DataBase;
using StudentHub.University;

namespace StudentHub
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private bool _studentVisibility;
        private Window _window;
        private User _user = new User();
        private Deanery _deanery = new Deanery();

        public AdminWindow(User user)
        {
            InitializeComponent();
            this._user = user;
            FindDeanery(_user.UserId);
            deaneryTextBlock.Text = ' ' + _user.UserName;
        }

        private void FindDeanery(int userId)
        {
            using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
            {
                OracleParameter userIdParameter = new OracleParameter
                {
                    ParameterName = "in_user_id",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.Int64,
                    Value = userId
                };
                OracleParameter deanery = new OracleParameter
                {
                    ParameterName = "deanery",
                    Direction = ParameterDirection.Output,
                    OracleDbType = OracleDbType.RefCursor

                };
                connection.Open();
                using (OracleCommand command = new OracleCommand("findDeanery", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new OracleParameter[] {userIdParameter,deanery});
                    var reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    foreach (DataRow row in dt.Rows)
                    {
                        _deanery.UserId = int.Parse(row["user_id"].ToString());
                        _deanery.DeaneryId = int.Parse(row["id"].ToString());
                        _deanery.DeaneryName = row["deanery_name"].ToString();
                        _deanery.Faculty = row["faculty"].ToString();
                        _deanery.Telephone = row["telephone"].ToString();
                    }
                }
                connection.Close();
            }
        }


        private void AdminWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();
        private void AdjustmentWorkButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new AdjustmentWorkWindow(_deanery);
            _window.Show();
        }

        private void GapsWorkButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new GapsWorkWindow(_deanery);
            _window.Show();
        }

        private void RetakeWorkButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new RetakeWorkWindow(_deanery);
            _window.Show();
        }


        private void StudentProgressButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_studentVisibility)
            {
                studentProgressFrame.Navigate(new StudentProgress(_deanery));
                studentProgressFrame.Visibility = Visibility.Visible;
                _studentVisibility = true;
            }
            else
            {
                studentProgressFrame.Visibility = Visibility.Collapsed;
                _studentVisibility = false;
            }
        }


        private void LogOutButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new Login();
            _window.Show();
            this.Close();
        }

        private void ExitButton_OnClick(object sender, RoutedEventArgs e) => this.Close();


    }
}
