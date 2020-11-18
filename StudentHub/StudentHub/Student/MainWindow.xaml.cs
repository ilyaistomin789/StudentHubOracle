using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
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
            if (_student.StudentStatus == "Elder")
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
            //TODO GET RET,ADJ,SCOPES


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
            string getRatingsQuery = "SELECT SubjectName [Subject], convert(varchar,PDate,104) [Date of issue], Note FROM Progress where StudentId = @StudentId";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getRatingsCommand = new SqlCommand(getRatingsQuery, connection);
                    getRatingsCommand.CommandType = CommandType.Text;
                    SqlParameter studentIdParameter = new SqlParameter
                    {
                        ParameterName = "@StudentId",
                        Value = _student.StudentId
                    };
                    getRatingsCommand.Parameters.Add(studentIdParameter);
                    var hasProgress = getRatingsCommand.ExecuteReader();
                    if (hasProgress.HasRows)
                    {
                        hasProgress.Close();
                        getRatingsCommand.ExecuteNonQuery();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(getRatingsCommand);
                        DataTable dt = new DataTable("Progress");
                        dataAdapter.Fill(dt);
                        dg_Progress.ItemsSource = dt.DefaultView;
                        dataAdapter.Update(dt);
                    }
                    else
                    {
                        hasProgress.Close();
                        m_ProgressTextBlock.Visibility = Visibility.Collapsed;
                        m_ProgressSeparator.Visibility = Visibility.Collapsed;
                        dg_Progress.Visibility = Visibility.Collapsed;

                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void GetRetakeAndAdjustment()
        {
            string getRetakeQuery =
                "SELECT SubjectName,CASE when RetakeStatus = 0 then 'In processing' when RetakeStatus = 1 then 'Request rejected' when RetakeStatus = 2 then 'Request accepted' END Status, convert(varchar,RDate,104) [Date of retake] FROM Retake where StudentId = @StudentId";
            string getAdjustmentQuery = "SELECT SubjectName,CASE when AdjustmentStatus = 0 then 'In processing' when AdjustmentStatus = 1 then 'Request rejected' when AdjustmentStatus = 2 then 'Request accepted' END Status, convert(varchar,ADate,104) [Date of adjustment] FROM Adjustment where StudentId = @StudentId";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getAdjustmentCommand = new SqlCommand(getAdjustmentQuery,connection);
                    SqlCommand getRetakeCommand = new SqlCommand(getRetakeQuery,connection);
                    getRetakeCommand.CommandType = CommandType.Text;
                    getAdjustmentCommand.CommandType = CommandType.Text;
                    SqlParameter rStudentIdParameter = new SqlParameter
                    {
                        ParameterName = "@StudentId",
                        Value = _student.StudentId
                    };
                    SqlParameter aStudentIdParameter = new SqlParameter
                    {
                        ParameterName = "@StudentId",
                        Value = _student.StudentId
                    };
                    getRetakeCommand.Parameters.Add(rStudentIdParameter);
                    getAdjustmentCommand.Parameters.Add(aStudentIdParameter);
                    var hasAdjustment = getAdjustmentCommand.ExecuteReader();
                    if (hasAdjustment.HasRows)
                    {
                        hasAdjustment.Close();
                        getAdjustmentCommand.ExecuteNonQuery();
                        SqlDataAdapter adjustmentDataAdapter = new SqlDataAdapter(getAdjustmentCommand);
                        DataTable dt1 = new DataTable("Adjustment");
                        adjustmentDataAdapter.Fill(dt1);
                        dg_Adjustments.ItemsSource = dt1.DefaultView;
                        adjustmentDataAdapter.Update(dt1);
                    }
                    else
                    {
                        hasAdjustment.Close();
                        m_AdjustmentTextBlock.Visibility = Visibility.Collapsed;
                        m_AdjustmentSeparator.Visibility = Visibility.Collapsed;
                        dg_Adjustments.Visibility = Visibility.Collapsed;
                    }
                    var hasRetakes = getRetakeCommand.ExecuteReader();
                    if (hasRetakes.HasRows)
                    {
                        hasRetakes.Close();
                        getRetakeCommand.ExecuteNonQuery();
                        SqlDataAdapter retakeDataAdapter = new SqlDataAdapter(getRetakeCommand);
                        DataTable dt2 = new DataTable("Retake");
                        retakeDataAdapter.Fill(dt2);
                        dg_Retakes.ItemsSource = dt2.DefaultView;
                        retakeDataAdapter.Update(dt2);
                    }
                    else
                    {
                        hasRetakes.Close();
                        m_RetakeTextBlock.Visibility = Visibility.Collapsed;
                        m_RetakeSeparator.Visibility = Visibility.Collapsed;
                        dg_Retakes.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
