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
                    MessageBox.Show(message);
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
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    GetInfoFromTables("select subject from subjects where faculty = :in_faculty","subject",p_subjectsComboBox,connection, new OracleParameter[] {faculty}, "Error when reading subjects");
                    GetInfoFromTables("select student_name from student_info where faculty = :in_faculty and specialization = :in_specialization and num_group = :in_num_group and course = :in_course", "student_name",p_studentsComboBox,connection, new [] {faculty.Clone() as OracleParameter, specialization, numGroup, course}, message);
                    connection.Close();
                }// specialization = :in_specialization and and num_group = :in_num_group "and course = :in_course

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
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    OracleParameter studentName = new OracleParameter
                    {
                        ParameterName = "in_student_name",
                        OracleDbType = OracleDbType.Varchar2,
                        Direction = ParameterDirection.Input,
                        Value = p_studentsComboBox.Text
                    };
                    OracleParameter subject = new OracleParameter
                    {
                        ParameterName = "in_subject",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = p_subjectsComboBox.Text
                    };
                    OracleParameter gaps = new OracleParameter
                    {
                        ParameterName = "in_gaps_count",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int64,
                        Value = int.Parse(p_gapsComboBox.Text)
                    };
                    OracleParameter gapsDate = new OracleParameter
                    {
                        ParameterName = "in_gap_date",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = p_gapsCalendar.SelectedDate
                    };
                    connection.Open();
                    using (OracleCommand command = new OracleCommand("setGaps", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new[] {studentName,subject,gaps,gapsDate});
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Done");
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}

