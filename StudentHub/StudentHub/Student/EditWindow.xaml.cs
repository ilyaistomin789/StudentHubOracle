using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Oracle.ManagedDataAccess.Client;
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
        public EditWindow(Student student)
        {
            _student = student;
            InitializeComponent();
            InitializeComboBox();
            e_fioTextBox.Text = student.Name!;
            e_facultyComboBox.Text = student.Faculty;
            e_specializationComboBox.Text = student.Specialization;
            e_courseComboBox.Text = student.Course.ToString();
            e_groupComboBox.Text = student.Group.ToString();
            if (student.Birthday == String.Empty)
            {
                e_birthdayCalendar.SelectedDate = null;
            }
            else
            {
                e_birthdayCalendar.SelectedDate = DateTime.Parse(student.Birthday);
            }
        }

        private void GetInfoFromTables(string cmdText,string element , ComboBox cb, OracleConnection connection)
        {
            using (OracleCommand command = new OracleCommand(cmdText, connection))
            {
                command.CommandType = CommandType.Text;
                var reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                foreach (DataRow row in dt.Rows)
                {
                    cb.Items.Add(row[element].ToString());
                }
            }
        }
        private void InitializeComboBox()
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    GetInfoFromTables("select specialization from specialization", "specialization",e_specializationComboBox, connection);
                    GetInfoFromTables("select faculty from faculty","faculty",e_facultyComboBox, connection);
                    connection.Close();
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
                    OracleParameter name = new OracleParameter
                    {
                        ParameterName = "in_student_name",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = e_fioTextBox.Text
                    };
                    OracleParameter course = new OracleParameter
                    {
                        ParameterName = "in_course",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int64,
                        Value = Convert.ToInt32(e_courseComboBox.Text)
                    };
                    OracleParameter numGroup = new OracleParameter
                    {
                        ParameterName = "in_num_group",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int64,
                        Value = Convert.ToInt32(e_groupComboBox.Text)
                    };
                    OracleParameter specialization = new OracleParameter
                    {
                        ParameterName = "in_specialization",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = e_specializationComboBox.Text
                    };
                    OracleParameter faculty = new OracleParameter
                    {
                        ParameterName = "in_faculty",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = e_facultyComboBox.Text
                    };
                    OracleParameter birthday = new OracleParameter
                    {
                        ParameterName = "in_birthday",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = e_birthdayCalendar.SelectedDate.Value
                    };
                    OracleParameter newUser = new OracleParameter
                    {
                        ParameterName = "new_user",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };
                    using (OracleCommand command = new OracleCommand("updateStudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] {userId,name,course,numGroup,specialization,faculty,birthday,newUser});
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        foreach (DataRow row in dt.Rows)
                        {
                            _student.Name = row["student_name"].ToString();
                            _student.Course = int.Parse(row["course"].ToString());
                            _student.Group = int.Parse(row["num_group"].ToString());
                            _student.Specialization = row["specialization"].ToString();
                            _student.Faculty = row["faculty"].ToString();
                            _student.Birthday = row["birthday"].ToString();
                        }
                    }
                    connection.Close();
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
