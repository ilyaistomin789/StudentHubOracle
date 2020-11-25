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
using Oracle.ManagedDataAccess.Client;
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

        private void GetInfoFromTables(string cmdText, string element, ComboBox cb, OracleConnection connection, OracleParameter[] op, string message)
        {
            using (OracleCommand command = new OracleCommand(cmdText, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(op);
                var reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new Exception(message);
                }
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
            string message =
                "Data could not be retrieved. You or students may have entered your personal information incorrectly";
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    OracleParameter faculty = new OracleParameter
                    {
                        ParameterName = "in_faculty",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = _student.Faculty
                    };
                    OracleParameter specialization = new OracleParameter
                    {
                        ParameterName = "in_specialization",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = _student.Specialization
                    };
                    OracleParameter numGroup = new OracleParameter
                    {
                        ParameterName = "in_num_group",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int64,
                        Value = _student.Group
                    };
                    OracleParameter course = new OracleParameter
                    {
                        ParameterName = "in_course",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int64,
                        Value = _student.Course
                    };
                    GetInfoFromTables("select subject from subjects where faculty = :in_faculty","subject",p_subjectsComboBox,connection, new OracleParameter[] {faculty}, "Error when reading subjects");
                    GetInfoFromTables("select student_name from student_info where specialization = :in_specialization and faculty = :in_faculty and num_group = :in_num_group " +
                                      "and course = :in_course", "student_name",p_studentsComboBox,connection, new OracleParameter[] {faculty.Clone() as OracleParameter, specialization, numGroup,course}, message);
                    connection.Open();
                    connection.Close();
                }

                foreach (var t in _university.countOfGaps) p_gapsComboBox.Items.Add(t);

                p_studentsComboBox.SelectedIndex = 0;
                p_subjectsComboBox.SelectedIndex = 0;
                p_gapsComboBox.SelectedIndex = 0;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                this.Close();
            }
        }
        //TODO fix
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

