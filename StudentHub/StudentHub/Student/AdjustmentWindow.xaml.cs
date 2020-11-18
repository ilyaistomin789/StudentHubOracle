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
    /// Логика взаимодействия для AdjustmentWindow.xaml
    /// </summary>
    public partial class AdjustmentWindow : Window
    {
        private UniversityEssence university = UniversityEssence.GetInstance();
        private Student _student;
        public AdjustmentWindow()
        {
            InitializeComponent();
        }

        public AdjustmentWindow(Student student)
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
                    SqlCommand getSubjectCommand = new SqlCommand(getSubjectsProcedure, connection);
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
                            a_subjectComboBox.Items.Add(subjects.GetString(0));
                        }
                        subjects.Close();
                        this.Show();
                    }
                    else
                    {
                        MessageBox.Show("Data could not be retrieved. You may have entered your personal information incorrectly");
                        this.Close();
                        return;
                    }
                }

                a_subjectComboBox.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private bool CheckAdjustments(SqlConnection connection)
        {
            string checkAdjustmentsQuery =
                "SELECT StudentId,SubjectName,ADate FROM Adjustment where StudentId = @StudentId and SubjectName = @SubjectName and ADate = @ADate";
            SqlCommand checkAdjustmentsCommand = new SqlCommand(checkAdjustmentsQuery, connection);
            checkAdjustmentsCommand.CommandType = CommandType.Text;
            SqlParameter studentIdParameter = new SqlParameter
            {
                ParameterName = "@StudentId",
                Value = _student.StudentId
            };
            SqlParameter subjectNameParameter = new SqlParameter
            {
                ParameterName = "@SubjectName",
                Value = a_subjectComboBox.Text
            };
            SqlParameter aDateParameter = new SqlParameter
            {
                ParameterName = "@ADate",
                Value = a_adjustmentDateCalendar.SelectedDate
            };
            checkAdjustmentsCommand.Parameters.Add(studentIdParameter);
            checkAdjustmentsCommand.Parameters.Add(subjectNameParameter);
            checkAdjustmentsCommand.Parameters.Add(aDateParameter);
            var check = checkAdjustmentsCommand.ExecuteReader();
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

        private void A_sendRequestButton_OnClick(object sender, RoutedEventArgs e)
        {
            string addAdjustmentProcedure = "ADD_ADJUSTMENT";
            if (a_subjectComboBox.Text == String.Empty)
            {
                MessageBox.Show("Please, choose the Subject");
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    if (CheckAdjustments(connection))
                    {
                        MessageBox.Show("This request is exists");
                        return;
                    }
                    SqlCommand addAdjustmentCommand = new SqlCommand(addAdjustmentProcedure, connection);
                    addAdjustmentCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter studentIdParameter = new SqlParameter
                    {
                        ParameterName = "@StudentId",
                        Value = _student.StudentId
                    };
                    SqlParameter subjectNameParameter = new SqlParameter
                    {
                        ParameterName = "@SubjectName",
                        Value = a_subjectComboBox.Text
                    };
                    SqlParameter aDateParameter = new SqlParameter
                    {
                        ParameterName = "@ADate",
                        Value = a_adjustmentDateCalendar.SelectedDate
                    };
                    addAdjustmentCommand.Parameters.Add(studentIdParameter);
                    addAdjustmentCommand.Parameters.Add(subjectNameParameter);
                    addAdjustmentCommand.Parameters.Add(aDateParameter);
                    addAdjustmentCommand.ExecuteNonQuery();
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
