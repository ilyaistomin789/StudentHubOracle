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
using StudentHub.Account;
using StudentHub.DataBase;
using StudentHub.University;

namespace StudentHub.Teacher
{
    /// <summary>
    /// Логика взаимодействия для TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        private Window _window;
        private User _user;
        private University.Teacher _teacher = new University.Teacher();
        public TeacherWindow(User user)
        {
            InitializeComponent();
            _user = user;
            FindTeacher(_user.UserId);
            teacherNameTextBlock.Text = ' ' + _user.UserName;
        }

        private void FindTeacher(int userId)
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
                OracleParameter teacher = new OracleParameter
                {
                    ParameterName = "teacher",
                    Direction = ParameterDirection.Output,
                    OracleDbType = OracleDbType.RefCursor

                };
                connection.Open();
                using (OracleCommand command = new OracleCommand("findTeacher", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new [] { userIdParameter, teacher });
                    var reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    foreach (DataRow row in dt.Rows)
                    {
                        _teacher.UserId = int.Parse(row["user_id"].ToString());
                        _teacher.TeacherId = int.Parse(row["id"].ToString());
                        _teacher.Faculty = row["faculty"].ToString();
                        _teacher.Telephone = row["telephone"].ToString();
                        _teacher.TeacherName = row["teacher_name"].ToString();
                    }
                }
                connection.Close();
            }
        }

        private void TeacherWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void LogOutButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new Login();
            _window.Show();
            this.Close();
        }
        private void ExitButton_OnClick(object sender, RoutedEventArgs e) => this.Close();

        private void setRatingsButton_Click(object sender, RoutedEventArgs e)
        {
            _window = new SetRatingsWindow(_teacher);
            _window.Show();
        }

        private void AdjustmentWorkButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new AdjustmentActionWindow(_teacher);
            _window.Show();
        }

        private void RetakeWorkButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new ViewRetakeWindow(_teacher);
            _window.Show();
        }
    }
}
