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
        private Window _window;
        private User _user = new User();
        public AdminWindow(string adminName)
        {
            InitializeComponent();
            adminNameTextBlock.Text = ' ' +  adminName;
        }

        public AdminWindow(User user)
        {
            InitializeComponent();
            this._user = user;
        }


        private void AdminWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();
        private void AdjustmentWorkButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new AdjustmentWorkWindow();
            _window.Show();
        }

        private void GapsWorkButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new GapsWorkWindow();
            _window.Show();
        }

        private void RetakeWorkButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new RetakeWorkWindow();
            _window.Show();
        }

        private void SearchQueryButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new SearchQueryWindow();
            _window.Show();
        }

        private void StudentProgressButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new SearchStudentWindow(1);
            _window.Show();
        }

        private void ReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            reportStackPanel.Visibility = Visibility.Visible;
            string processedAdjustments =
                "SELECT COUNT(*) FROM Adjustment where AdjustmentStatus = 1 OR AdjustmentStatus = 2 ";
            string rawAdjustments = "SELECT COUNT(*) FROM Adjustment where AdjustmentStatus = 0";
            string processedRetakes = "SELECT COUNT(*) FROM Retake where RetakeStatus = 1 OR RetakeStatus = 2";
            string rawRetakes = "SELECT COUNT(*) FROM Retake where RetakeStatus = 0";
            string countOfUsers = "SELECT COUNT(*) FROM Users";
            string getStudentsQuery =
                "SELECT StudentName [Student], Course, GroupId [Group], Faculty, Specialization, convert(varchar,Birthday,104) [Birthday] FROM Student";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getProcessedAdjustmentsCommand = new SqlCommand(processedAdjustments,connection);
                    SqlCommand getRawAdjustmentsCommand = new SqlCommand(rawAdjustments,connection);
                    SqlCommand getProcessedRetakesCommand = new SqlCommand(processedRetakes,connection);
                    SqlCommand getRawRetakesCommand = new SqlCommand(rawRetakes,connection);
                    SqlCommand getCountUsersCommand = new SqlCommand(countOfUsers,connection);
                    SqlCommand getStudents = new SqlCommand(getStudentsQuery,connection);
                    getProcessedAdjustmentsCommand.CommandType = CommandType.Text;
                    getRawAdjustmentsCommand.CommandType = CommandType.Text;
                    getProcessedRetakesCommand.CommandType = CommandType.Text;
                    getRawRetakesCommand.CommandType = CommandType.Text;
                    getCountUsersCommand.CommandType = CommandType.Text;
                    getStudents.CommandType = CommandType.Text;
                    this.processedAdjustments.Text = getProcessedAdjustmentsCommand.ExecuteScalar().ToString();
                    this.rawAdjustments.Text = getRawAdjustmentsCommand.ExecuteScalar().ToString();
                    this.processedRetakes.Text = getProcessedRetakesCommand.ExecuteScalar().ToString();
                    this.rawRetakes.Text = getRawRetakesCommand.ExecuteScalar().ToString();
                    this.countOfUsers.Text = getCountUsersCommand.ExecuteScalar().ToString();
                    getStudents.ExecuteNonQuery();
                    SqlDataAdapter studentDataAdapter = new SqlDataAdapter(getStudents);
                    DataTable dt = new DataTable("Student");
                    studentDataAdapter.Fill(dt);
                    dg_Students.ItemsSource = dt.DefaultView;
                    studentDataAdapter.Update(dt);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void EmailButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new SearchStudentWindow(2);
            _window.Show();
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
