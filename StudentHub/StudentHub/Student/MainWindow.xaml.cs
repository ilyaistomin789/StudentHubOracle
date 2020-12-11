using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Oracle.ManagedDataAccess.Client;
using StudentHub.Account;
using StudentHub.DataBase;
using StudentHub.University;

namespace StudentHub
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Student _student = new Student();
        private Window _window;
        private User _user;

        public MainWindow(User user)
        {
            InitializeComponent();
            _user = user;
            FindStudent(_user.UserId);
            if (_student.StudentStatus == "elder")
            {
                putGapsButton.Visibility = Visibility.Visible;
                setRatingsButton.Visibility = Visibility.Visible;
            }
            if (_student.Name == String.Empty)
            {
                MessageBox.Show("Please, enter information about yourself");
                _window = new EditWindow(_student);
                _window.Show();
            }
            studentNameTextBlock.Text = " " + _student.Name;
            GetStudentRatings();
            GetRetakeAndAdjustment();

            this.Show();
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Student student)
        {
            InitializeComponent();
            _student = student;
            if (_student.StudentStatus == "Elder")
            {
                putGapsButton.Visibility = Visibility.Visible;
                setRatingsButton.Visibility = Visibility.Visible;
            }
            studentNameTextBlock.Text = " " + _student.Name;
            GetStudentRatings();
            GetRetakeAndAdjustment();
            this.Show();
            if (_student.Name == "undefined" && this.IsLoaded)
            {
                MessageBox.Show("Please, edit your information");
            }
        }

        private void FindStudent(int userId)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    OracleParameter id = new OracleParameter
                    {
                        ParameterName = "in_user_id",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int64,
                        Value = userId
                    };
                    OracleParameter student = new OracleParameter
                    {
                        ParameterName = "student",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    };
                    using (OracleCommand command = new OracleCommand("findStudent",connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] {id,student});
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        foreach (DataRow row in dt.Rows)
                        {
                            _student.StudentId = int.Parse(row["id"].ToString());
                            _student.UserId = int.Parse(row["user_id"].ToString());
                            _student.Name = row["student_name"].ToString();
                            _student.StudentStatus = row["status"].ToString();
                            if (row["course"] is System.DBNull)
                            {
                                _student.Course = null;
                            }
                            else
                            {
                                _student.Course = int.Parse(row["course"].ToString());
                            }
                            if (row["num_group"] is System.DBNull)
                            {
                                _student.Group = null;
                            }
                            else
                            {
                                _student.Group = int.Parse(row["num_group"].ToString());
                            }
                            _student.Specialization = row["specialization"].ToString();
                            _student.Faculty = row["faculty"].ToString();
                            _student.Birthday = row["birthday"].ToString();
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        private void GetStudentRatings()
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    OracleParameter userId = new OracleParameter
                    {
                        ParameterName = "in_user_id",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int64,
                        Value = _student.UserId
                    };
                    using (OracleCommand command = new OracleCommand("select subject, note, progress_date from student_progress where user_id = :in_user_id", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(userId);
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            OracleDataAdapter oda = new OracleDataAdapter(command);
                            oda.Fill(dt);
                            dg_Progress.ItemsSource = dt.DefaultView;
                            oda.Update(dt);
                        }
                        else
                        {
                            m_ProgressTextBlock.Visibility = Visibility.Collapsed;
                            m_ProgressSeparator.Visibility = Visibility.Collapsed;
                            dg_Progress.Visibility = Visibility.Collapsed;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void LoadRetakesAndAdjustmentFromTables(string cmdText,OracleConnection connection, OracleParameter op, TextBlock tb, Separator s, DataGrid dg)
        {
            using (OracleCommand command = new OracleCommand(cmdText,connection))
            {
                command.Parameters.Add(op);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    OracleDataAdapter oda = new OracleDataAdapter(command);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    dg.ItemsSource = dt.DefaultView;
                    oda.Update(dt);
                }
                else
                {
                    tb.Visibility = Visibility.Collapsed;
                    s.Visibility = Visibility.Collapsed;
                    dg.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void GetRetakeAndAdjustment()
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    OracleParameter userId = new OracleParameter
                    {
                        ParameterName = "in_user_id",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int64,
                        Value = _student.UserId
                    };
                    LoadRetakesAndAdjustmentFromTables("select subject, status, adjustment_date, access_date from adjustments where user_id = :in_user_id",
                        connection,userId,m_AdjustmentTextBlock,m_AdjustmentSeparator,dg_Adjustments);
                    
                    LoadRetakesAndAdjustmentFromTables("select subject, status, retake_date from retakes where user_id = :in_user_id",
                        connection, userId.Clone() as OracleParameter, m_RetakeTextBlock, m_RetakeSeparator, dg_Retakes);
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void EditInformationButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new EditWindow(_student);
            _window.Show();
        }

        private void AdjustmentButton_OnClick(object sender, RoutedEventArgs e) => _window = new AdjustmentWindow(_student);

        private void RetakeButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new RetakeWindow(_student);
            _window.Show();
        }

        private void ExitButton_OnClick(object sender, RoutedEventArgs e) => this.Close();

        private void LogOutButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new Login();
            _window.Show();
            this.Close();
        }

        private void SetRatingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new SetRatingsWindow(_student);
            _window.Show();
        }

        private void PutGapsButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new PutGapsWindow(_student);
            _window.Show();
        }

        private void ShowInfoButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new ShowInformationWindow(_student);
            _window.Show();
        }
    }
}
