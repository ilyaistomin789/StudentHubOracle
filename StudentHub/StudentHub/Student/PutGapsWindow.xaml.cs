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
    /// Логика взаимодействия для PutGapsWindow.xaml
    /// </summary>
    public partial class PutGapsWindow : Window
    {
        private UniversityEssence _university = UniversityEssence.GetInstance();
        private Student _student;
        public PutGapsWindow(Student student)
        {
            InitializeComponent();
            _student = student;
            InitializeComboBox();
        }
        private void InitializeComboBox()
        {
            string getStudentsQuery =
                "SELECT StudentName from Student where Course = @Course and GroupId = @GroupId and Specialization = @Specialization and Faculty = @Faculty";
            string getSubjectsProcedure = "GET_SUBJECTS";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getStudentsCommand = new SqlCommand(getStudentsQuery, connection);
                    SqlCommand getSubjectCommand = new SqlCommand(getSubjectsProcedure, connection);
                    getSubjectCommand.CommandType = CommandType.StoredProcedure;
                    getStudentsCommand.CommandType = CommandType.Text;
                    SqlParameter courseParameter = new SqlParameter
                    {
                        ParameterName = "@Course",
                        Value = _student.Course
                    };
                    SqlParameter groupIdParameter = new SqlParameter
                    {
                        ParameterName = "@GroupId",
                        Value = _student.Group
                    };
                    SqlParameter specializationParameter = new SqlParameter
                    {
                        ParameterName = "@Specialization",
                        Value = _student.Specialization
                    };
                    SqlParameter facultyParameter = new SqlParameter
                    {
                        ParameterName = "@Faculty",
                        Value = _student.Faculty
                    };
                    SqlParameter facultyParameterSub = new SqlParameter
                    {
                        ParameterName = "@Faculty",
                        Value = _student.Faculty
                    };
                    getStudentsCommand.Parameters.Add(courseParameter);
                    getStudentsCommand.Parameters.Add(groupIdParameter);
                    getStudentsCommand.Parameters.Add(specializationParameter);
                    getStudentsCommand.Parameters.Add(facultyParameter);
                    getSubjectCommand.Parameters.Add(facultyParameterSub);
                    var students = getStudentsCommand.ExecuteReader();
                    if (students.HasRows)
                    {
                        while (students.Read())
                        {
                            p_studentsComboBox.Items.Add(students.GetString(0));
                        }
                        students.Close();
                    }
                    else
                    {
                        MessageBox.Show("Data could not be retrieved. You or students may have entered your personal information incorrectly");
                        this.Close();
                    }
                    var subjects = getSubjectCommand.ExecuteReader();
                    if (subjects.HasRows)
                    {
                        while (subjects.Read())
                        {
                            p_subjectsComboBox.Items.Add(subjects.GetString(0));
                        }
                        subjects.Close();
                    }
                    else
                    {
                        MessageBox.Show("Data could not be retrieved. You or students may have entered your personal information incorrectly");
                        this.Close();
                    }
                }

                foreach (var t in _university.countOfGaps)
                {
                    p_gapsComboBox.Items.Add(t);
                }

                p_studentsComboBox.SelectedIndex = 0;
                p_subjectsComboBox.SelectedIndex = 0;
                p_gapsComboBox.SelectedIndex = 0;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void P_saveButton_OnClick(object sender, RoutedEventArgs e)
        {
            string setProgressProcedure = "SET_GAPS";
            if (p_studentsComboBox.Text == String.Empty)
            {
                MessageBox.Show("Please, choose the Student");
                return;
            }

            if (p_subjectsComboBox.Text == String.Empty)
            {
                MessageBox.Show("Please, choose Subject");
                return;
            }

            if (p_gapsComboBox.Text == String.Empty)
            {
                MessageBox.Show("Please, choose the Count of gaps");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand setProgressCommand = new SqlCommand(setProgressProcedure, connection);
                    setProgressCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter studentNameParameter = new SqlParameter
                    {
                        ParameterName = "@StudentName",
                        Value = p_studentsComboBox.Text
                    };
                    SqlParameter subjectNameParameter = new SqlParameter
                    {
                        ParameterName = "@SubjectName",
                        Value = p_subjectsComboBox.Text
                    };
                    SqlParameter noteParameter = new SqlParameter
                    {
                        ParameterName = "@Gaps",
                        Value = Convert.ToInt32(p_gapsComboBox.Text)
                    };
                    setProgressCommand.Parameters.Add(studentNameParameter);
                    setProgressCommand.Parameters.Add(subjectNameParameter);
                    setProgressCommand.Parameters.Add(noteParameter);
                    var done = setProgressCommand.ExecuteScalar();
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

