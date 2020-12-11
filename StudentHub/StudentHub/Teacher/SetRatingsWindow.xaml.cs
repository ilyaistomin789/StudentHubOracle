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

namespace StudentHub.Teacher
{
    /// <summary>
    /// Логика взаимодействия для SetRatingsWindow.xaml
    /// </summary>
    public partial class SetRatingsWindow : Window
    {
        private University.Teacher _teacher;
        private UniversityEssence ue = UniversityEssence.GetInstance();
        public SetRatingsWindow(University.Teacher teacher)
        {
            _teacher = teacher;
            InitializeComponent();
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
                    Value = _teacher.Faculty
                };
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    GetInfoFromTables("select subject from subjects where faculty = :in_faculty", "subject", s_subjectsComboBox, connection, new[] { faculty }, "Error when reading subjects");
                    GetInfoFromTables("select student_name || ' ' || course || '-' || num_group student " +
                                      "from student_info where faculty = :in_faculty", "student", s_studentsComboBox, connection, new[] { faculty.Clone() as OracleParameter }, message);
                    connection.Close();
                }

                foreach (var note in ue.notes)
                {
                    s_noteComboBox.Items.Add(note);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
        }
        private void A_sendRequestButton_OnClick(object sender, RoutedEventArgs e)
        {
            char[] int_m = new[] {'1', '2', '3', '4', '5', '-'};
            string fio = "";
            foreach (var t in s_studentsComboBox.Text)
            {
                if (!int_m.Contains(t))
                {
                    fio += t;
                }
            }

            fio = fio.Trim();
            try
            {
                //TODO FIX STUDENTNAME CAUSE STUDENTNAME CONSTIST OF ' ' AND SPLIT (' ') INCLUDE ONLY FIRST LETTER
                OracleParameter name = new OracleParameter
                {
                    ParameterName = "in_student_name",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.Varchar2,
                    Value = fio
                };
                OracleParameter subject = new OracleParameter
                {
                    ParameterName = "in_subject",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.Varchar2,
                    Value = s_subjectsComboBox.Text
                };
                OracleParameter note = new OracleParameter
                {
                    ParameterName = "in_note",
                    OracleDbType = OracleDbType.Int64,
                    Direction = ParameterDirection.Input,
                    Value = int.Parse(s_noteComboBox.Text)
                };
                OracleParameter progressDate = new OracleParameter
                {
                    ParameterName = "in_progress_date",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.Date,
                    Value = s_progressDateCalendar.SelectedDate
                };
                OracleParameter comment = new OracleParameter
                {
                    ParameterName = "in_comment",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.Varchar2,
                    Value = s_commentTextBox.Text
                };
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    using (OracleCommand command = new OracleCommand("setRatings", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new[] {name, subject, note, progressDate, comment});
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Done");
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
        }
    }
}
