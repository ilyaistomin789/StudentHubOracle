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

namespace StudentHub.Admin
{
    /// <summary>
    /// Логика взаимодействия для SearchQueryWindow.xaml
    /// </summary>
    public partial class SearchQueryWindow : Window
    {
        private UniversityEssence university = UniversityEssence.GetInstance();
        public SearchQueryWindow()
        {
            InitializeComponent();
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            string getSpecializations = "SELECT * from Specialization";
            string getFaculties = "SELECT * FROM Faculty";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getFacultiesCommand = new SqlCommand(getFaculties, connection);
                    SqlCommand getSpecialization = new SqlCommand(getSpecializations,connection);
                    getSpecialization.CommandType = CommandType.Text;
                    getFacultiesCommand.CommandType = CommandType.Text;
                    var specialization = getSpecialization.ExecuteReader();
                    while (specialization.Read())
                    {
                        curr_SpecializationComboBox.Items.Add(specialization.GetString(0));
                        opt_SpecializationComboBox.Items.Add(specialization.GetString(0));
                    }
                    specialization.Close();
                    var faculties = getFacultiesCommand.ExecuteReader();
                    while (faculties.Read())
                    {
                        curr_FacultyComboBox.Items.Add(faculties.GetString(0));
                    }
                    faculties.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error when initialize combo box" + $"\n{e.Message}");
                return;
            }
            foreach (var t in university.courses)
            {
                opt_CourseComboBox.Items.Add(t);
                curr_CourseComboBox.Items.Add(t);
            }

            foreach (var t in university.groups)
            {
                opt_GroupComboBox.Items.Add(t);
                curr_GroupComboBox.Items.Add(t);
            }
        }

        private void SearchStudentButton_OnClick(object sender, RoutedEventArgs e)
        {
            string getStudentInfo = "SELECT * FROM Student WHERE StudentName LIKE '%' + @StudentName + '%' ";
            string getCountRows = "SELECT COUNT(*) FROM Student WHERE StudentName LIKE '%' + @StudentName + '%' ";
            if (studentNameTextBox.Text == String.Empty)
            {
                MessageBox.Show("Please, enter the Student name");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    if (curr_StudentCourseStackPanel.Visibility == Visibility.Visible)
                    {
                        getStudentInfo +=
                            "AND Course = @Course AND GroupId = @Group AND Specialization = @Specialization";
                        SqlCommand getStudentInfoCommand = new SqlCommand(getStudentInfo, connection);
                        getStudentInfoCommand.CommandType = CommandType.Text;
                        SqlParameter courseParameter = new SqlParameter
                        {
                            ParameterName = "@Course",
                            Value = Int32.Parse(opt_CourseComboBox.Text)
                        };
                        SqlParameter groupParameter = new SqlParameter
                        {
                            ParameterName = "@Group",
                            Value = Int32.Parse(opt_GroupComboBox.Text)
                        };
                        SqlParameter specializationParameter = new SqlParameter
                        {
                            ParameterName = "@Specialization",
                            Value = opt_SpecializationComboBox.Text
                        };
                        SqlParameter studentName = new SqlParameter
                        {
                            ParameterName = "@StudentName",
                            Value = studentNameTextBox.Text
                        };
                        getStudentInfoCommand.Parameters.Add(studentName);
                        getStudentInfoCommand.Parameters.Add(courseParameter);
                        getStudentInfoCommand.Parameters.Add(groupParameter);
                        getStudentInfoCommand.Parameters.Add(specializationParameter);
                        var student = getStudentInfoCommand.ExecuteReader();
                        if (student.HasRows)
                        {
                            while (student.Read())
                            {
                                curr_studentName.Text = student.GetString(2);
                                curr_studentStatus.Text = student.GetString(3);
                                curr_studentCourse.Text = student.GetInt32(4).ToString();
                                curr_studentGroup.Text = student.GetInt32(5).ToString();
                                curr_studentSpec.Text = student.GetString(6);
                                curr_studentFaculty.Text = student.GetString(7);
                                curr_studentBirthday.Text = student.GetDateTime(8).ToString("d");
                                curr_studentEmail.Text = student.GetString(9);
                            }
                            firstColumnStackPanel.Visibility = Visibility.Visible;
                            secondColumnStackPanel.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            MessageBox.Show("Error when searching for students");
                            return;
                        }
                    }
                    else
                    {
                        SqlCommand getCountRowsCommand = new SqlCommand(getCountRows,connection);
                        SqlCommand getStudentInfoCommand = new SqlCommand(getStudentInfo, connection);
                        getStudentInfoCommand.CommandType = CommandType.Text;
                        getCountRowsCommand.CommandType = CommandType.Text;
                        SqlParameter studentName = new SqlParameter
                        {
                            ParameterName = "@StudentName",
                            Value = studentNameTextBox.Text
                        };
                        SqlParameter studentName4Rows = new SqlParameter
                        {
                            ParameterName = "@StudentName",
                            Value = studentNameTextBox.Text
                        };
                        getStudentInfoCommand.Parameters.Add(studentName);
                        getCountRowsCommand.Parameters.Add(studentName4Rows);
                        var rows = getCountRowsCommand.ExecuteReader();
                        int count = 0;
                        while (rows.Read())
                        {
                            count = rows.GetInt32(0);
                        }
                        rows.Close();
                        if (count > 1)
                        {
                            MessageBox.Show(
                                "More than one student was found, please fill in the new fields for a more accurate search");
                            curr_StudentCourseStackPanel.Visibility = Visibility.Visible;
                            curr_StudentGroupStackPanel.Visibility = Visibility.Visible;
                            curr_StudentSpecStackPanel.Visibility = Visibility.Visible;
                            return;
                        }
                        else
                        {
                            firstColumnStackPanel.Visibility = Visibility.Visible;
                            secondColumnStackPanel.Visibility = Visibility.Visible;
                            var student = getStudentInfoCommand.ExecuteReader();
                            if (student.HasRows)
                            {
                                while (student.Read())
                                {
                                    curr_studentName.Text = student.GetString(2);
                                    curr_studentStatus.Text = student.GetString(3);
                                    curr_studentCourse.Text = student.GetInt32(4).ToString();
                                    curr_studentGroup.Text = student.GetInt32(5).ToString();
                                    curr_studentSpec.Text = student.GetString(6);
                                    curr_studentFaculty.Text = student.GetString(7);
                                    curr_studentBirthday.Text = student.GetDateTime(8).ToString("d");
                                    curr_studentEmail.Text = student.GetString(9);
                                }
                                student.Close();
                            }
                            else
                            {
                                MessageBox.Show("Error when searching for students");
                                return;
                            }
                        }

                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SearchGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (curr_CourseComboBox.Text == String.Empty)
            {
                MessageBox.Show("Please, choose the course");
                return;
            }
            if (curr_GroupComboBox.Text == String.Empty)
            {
                MessageBox.Show("Please, choose the group");
                return;
            }
            if (curr_SpecializationComboBox.Text == String.Empty)
            {
                MessageBox.Show("Please, choose the specialization");
                return;
            }
            if (curr_FacultyComboBox.Text == String.Empty)
            {
                MessageBox.Show("Please, choose the faculty");
                return;
            }
            string getGroupQuery =
                "SELECT StudentName [Student], Course, GroupId [Group], Faculty, Specialization, convert(varchar,Birthday,104) [Birthday] FROM Student WHERE Course = @Course AND GroupId = @Group AND Specialization = @Specialization AND Faculty = @Faculty";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getGroupCommand = new SqlCommand(getGroupQuery,connection);
                    SqlParameter courseParameter = new SqlParameter
                    {
                        ParameterName = "@Course",
                        Value = curr_CourseComboBox.Text
                    };
                    SqlParameter groupParameter = new SqlParameter
                    {
                        ParameterName = "@Group",
                        Value = curr_GroupComboBox.Text
                    };
                    SqlParameter specializationParameter = new SqlParameter
                    {
                        ParameterName = "@Specialization",
                        Value = curr_SpecializationComboBox.Text
                    };
                    SqlParameter facultyParameter = new SqlParameter
                    {
                        ParameterName = "@Faculty",
                        Value = curr_FacultyComboBox.Text
                    };
                    getGroupCommand.Parameters.Add(facultyParameter);
                    getGroupCommand.Parameters.Add(courseParameter);
                    getGroupCommand.Parameters.Add(groupParameter);
                    getGroupCommand.Parameters.Add(specializationParameter);
                    getGroupCommand.ExecuteNonQuery();
                    SqlDataAdapter groupDataAdapter = new SqlDataAdapter(getGroupCommand);
                    DataTable dt = new DataTable("Student");
                    groupDataAdapter.Fill(dt);
                    dg_StudentsOfGroup.ItemsSource = dt.DefaultView;
                    groupDataAdapter.Update(dt);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SearchAllEldersButton_OnClick(object sender, RoutedEventArgs e)
        {
            string getAllEldersQuery =
                "SELECT StudentName [Student], Course, GroupId [Group], Faculty, Specialization FROM Student WHERE StudentStatus = 'Elder'";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getAllEldersCommand = new SqlCommand(getAllEldersQuery,connection);
                    getAllEldersCommand.CommandType = CommandType.Text;
                    getAllEldersCommand.ExecuteNonQuery();
                    SqlDataAdapter eldersDataAdapter = new SqlDataAdapter(getAllEldersCommand);
                    DataTable dt = new DataTable("Student");
                    eldersDataAdapter.Fill(dt);
                    dg_Elders.ItemsSource = dt.DefaultView;
                    eldersDataAdapter.Update(dt);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SearchElderButton_OnClick(object sender, RoutedEventArgs e)
        {
            string getElderQuery =
                "SELECT StudentName [Student], Course, GroupId [Group], Faculty, Specialization FROM Student WHERE StudentName LIKE '%' + @ElderName + '%' AND StudentStatus = 'Elder'";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getElderCommand = new SqlCommand(getElderQuery,connection);
                    getElderCommand.CommandType = CommandType.Text;
                    SqlParameter elderNameParameter = new SqlParameter
                    {
                        ParameterName = "@ElderName",
                        Value = elderNameTextBox.Text
                    };
                    getElderCommand.Parameters.Add(elderNameParameter);
                    var elder = getElderCommand.ExecuteReader();
                    if (elder.HasRows)
                    {
                        elder.Close();
                        getElderCommand.ExecuteNonQuery();
                        SqlDataAdapter elderDataAdapter = new SqlDataAdapter(getElderCommand);
                        DataTable dt = new DataTable("Student");
                        elderDataAdapter.Fill(dt);
                        dg_Elders.ItemsSource = dt.DefaultView;
                        elderDataAdapter.Update(dt);
                    }
                    else
                    {
                        elder.Close();
                        MessageBox.Show("Error when searching elder");
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ElderNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            searchAllEldersButton.IsEnabled = elderNameTextBox.Text == string.Empty;
        }
    }
}
