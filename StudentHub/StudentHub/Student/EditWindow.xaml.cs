using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using StudentHub.DataBase;
using StudentHub.University;

namespace StudentHub
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private Student _student;
        private readonly UniversityEssence university = UniversityEssence.GetInstance();
        //TODO DO EDITING 
        public EditWindow(Student student)
        {
            InitializeComponent();
            InitializeComboBox();
            e_fioTextBox.Text = student.Name!;
            e_facultyComboBox.Text = student.Faculty;
            e_specializationComboBox.Text = student.Specialization;
            e_courseComboBox.Text = student.Course.ToString();
            e_groupComboBox.Text = student.Group.ToString();
            if (student.Birthday == null)
            {
                e_birthdayCalendar.SelectedDate = null;
            }
            else
            {
                e_birthdayCalendar.SelectedDate = DateTime.Parse(student.Birthday);
            }
        }
        //TODO FIX 
        private void InitializeComboBox()
        {
            string getSpecQuery = "SELECT Specialization FROM Specialization";
            string getFacultyQuery = "Select Faculty FROM Faculty";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getSpecCommand = new SqlCommand(getSpecQuery,connection);
                    SqlCommand getFacultyCommand = new SqlCommand(getFacultyQuery,connection);
                    getSpecCommand.CommandType = CommandType.Text;
                    getFacultyCommand.CommandType = CommandType.Text;
                    var specializations = getSpecCommand.ExecuteReader();
                    if (specializations.HasRows)
                    {
                        while (specializations.Read())
                        {
                            e_specializationComboBox.Items.Add(specializations.GetString(0));
                        }
                        specializations.Close();
                    }
                    var faculties = getFacultyCommand.ExecuteReader();
                    if (faculties.HasRows)
                    {
                        while (faculties.Read())
                        {
                            e_facultyComboBox.Items.Add(faculties.GetString(0));
                        }
                        faculties.Close();
                    }

                    foreach (var t in university.courses)
                    {
                        e_courseComboBox.Items.Add(t);
                    }

                    foreach (var t in university.groups)
                    {
                        e_groupComboBox.Items.Add(t);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        private void E_editInformationButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(e_fioTextBox.Text,"^[a-zA-Z\\s]{2,39}$"))
            {
                MessageBox.Show("Incorrect Student FIO");
                return;
            }
            string setStudentFieldsProcedure = "SET_STUDENT_FIELDS";
            try
            {
                //OracleDataBaseConnection.ApplyUserPrivileges();
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand setStudentFieldsCommand = new SqlCommand(setStudentFieldsProcedure, connection);
                    setStudentFieldsCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter studentIdParameter = new SqlParameter
                    {
                        ParameterName = "@StudentId",
                        Value = _student.StudentId
                    };
                    SqlParameter studentNameParameter = new SqlParameter
                    {
                        ParameterName = "@StudentName",
                        Value = e_fioTextBox.Text
                    };
                    SqlParameter courseParameter = new SqlParameter
                    {
                        ParameterName = "@Course",
                        Value = Convert.ToInt32(e_courseComboBox.Text)
                    };
                    SqlParameter groupIdParameter = new SqlParameter
                    {
                        ParameterName = "@GroupId",
                        Value = Convert.ToInt32(e_groupComboBox.Text)
                    };
                    SqlParameter specParameter = new SqlParameter
                    {
                        ParameterName = "@Specialization",
                        Value = e_specializationComboBox.Text
                    };
                    SqlParameter facultyParameter = new SqlParameter
                    {
                        ParameterName = "@Faculty",
                        Value = e_facultyComboBox.Text
                    };
                    SqlParameter birthdayParameter = new SqlParameter
                    {
                        ParameterName = "@Birthday",
                        Value = e_birthdayCalendar.SelectedDate.Value
                    };

                    setStudentFieldsCommand.Parameters.Add(studentIdParameter);
                    setStudentFieldsCommand.Parameters.Add(studentNameParameter);
                    setStudentFieldsCommand.Parameters.Add(courseParameter);
                    setStudentFieldsCommand.Parameters.Add(groupIdParameter);
                    setStudentFieldsCommand.Parameters.Add(specParameter);
                    setStudentFieldsCommand.Parameters.Add(facultyParameter);
                    setStudentFieldsCommand.Parameters.Add(birthdayParameter);
                    var done = setStudentFieldsCommand.ExecuteReader();
                    if (done.HasRows)
                    {
                        while (done.Read())
                        {
                            _student.Name = done.GetString(2);
                            _student.Course = done.GetInt32(4);
                            _student.Group = done.GetInt32(5);
                            _student.Specialization = done.GetString(6);
                            _student.Faculty = done.GetString(7);
                            _student.Birthday = done.GetDateTime(8).ToString("d");
                        }
                        done.Close();
                    }
                    
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
