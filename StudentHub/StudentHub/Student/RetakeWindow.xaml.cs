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
using StudentHub.DataBase;
using StudentHub.University;

namespace StudentHub
{
    /// <summary>
    /// Логика взаимодействия для RetakeWindow.xaml
    /// </summary>
    public partial class RetakeWindow : Window
    {
        private Student _student;
        public RetakeWindow()
        {
            InitializeComponent();
        }

        public RetakeWindow(Student student)
        {
            InitializeComponent();
            _student = student;
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            string getSubjectsProcedure = "GET_SUBJECTS";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getSubjectCommand = new SqlCommand(getSubjectsProcedure,connection);
                    getSubjectCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter facultyParameter = new SqlParameter
                    {
                        ParameterName = "@Faculty",
                        Value = _student.Faculty
                    };
                    getSubjectCommand.Parameters.Add(facultyParameter);
                    var subjects = getSubjectCommand.ExecuteReader();
                    if (subjects.HasRows)
                    {
                        while (subjects.Read())
                        {
                            r_subjectComboBox.Items.Add(subjects.GetString(0));
                        }
                        subjects.Close();
                    }
                    else
                    {
                        MessageBox.Show("Data could not be retrieved. You or students may have entered your personal information incorrectly");
                        this.Close();
                    }
                }

                r_subjectComboBox.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private bool CheckRetakes(SqlConnection connection)
        {
            string checkRetakesQuery =
                "SELECT StudentId,SubjectName,RDate FROM Retake where StudentId = @StudentId and SubjectName = @SubjectName and RDate = @RDate";
            SqlCommand checkRetakesCommand = new SqlCommand(checkRetakesQuery, connection);
            checkRetakesCommand.CommandType = CommandType.Text;
            SqlParameter studentIdParameter = new SqlParameter
            {
                ParameterName = "@StudentId",
                Value = _student.StudentId
            };
            SqlParameter subjectNameParameter = new SqlParameter
            {
                ParameterName = "@SubjectName",
                Value = r_subjectComboBox.Text
            };
            SqlParameter rDateParameter = new SqlParameter
            {
                ParameterName = "@RDate",
                Value = r_adjustmentDateCalendar.SelectedDate
            };
            checkRetakesCommand.Parameters.Add(studentIdParameter);
            checkRetakesCommand.Parameters.Add(subjectNameParameter);
            checkRetakesCommand.Parameters.Add(rDateParameter);
            var check = checkRetakesCommand.ExecuteReader();
            if (check.HasRows)
            {
                check.Close();
                return true;
            }
            else
            {
                check.Close();
                return false;
            }
        }

        private void R_sendRequestButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (r_subjectComboBox.Text == String.Empty)
            {
                MessageBox.Show("Please, choose the Subject");
                return;
            }
            string addRetakeProcedure = "ADD_RETAKE";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    if (CheckRetakes(connection))
                    {
                        MessageBox.Show("This request is exists");
                        return;
                    }
                    SqlCommand addRetakeCommand = new SqlCommand(addRetakeProcedure, connection);
                    addRetakeCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter studentIdParameter = new SqlParameter
                    {
                        ParameterName = "@StudentId",
                        Value = _student.StudentId
                    };
                    SqlParameter subjectNameParameter = new SqlParameter
                    {
                        ParameterName = "@SubjectName",
                        Value = r_subjectComboBox.Text
                    };
                    SqlParameter rDateParameter = new SqlParameter
                    {
                        ParameterName = "@RDate",
                        Value = r_adjustmentDateCalendar.SelectedDate
                    };
                    addRetakeCommand.Parameters.Add(studentIdParameter);
                    addRetakeCommand.Parameters.Add(subjectNameParameter);
                    addRetakeCommand.Parameters.Add(rDateParameter);
                    var done = addRetakeCommand.ExecuteNonQuery();
                    MessageBox.Show("Done");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
