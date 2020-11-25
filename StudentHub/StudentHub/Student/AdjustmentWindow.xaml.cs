using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;
using StudentHub.DataBase;
using StudentHub.University;

namespace StudentHub
{
    /// <summary>
    /// Логика взаимодействия для AdjustmentWindow.xaml
    /// </summary>
    public partial class AdjustmentWindow : Window
    { 
        private Student _student;
        private byte[] imageCode;
        public AdjustmentWindow()
        {
            InitializeComponent();
        }

        public AdjustmentWindow(Student student)
        {
            InitializeComponent();
            _student = student;
            InitializeComboBox();
            this.Show();
        }
        private void GetInfoFromTables(string cmdText, string element, ComboBox cb, OracleConnection connection, OracleParameter op)
        {
            using (OracleCommand command = new OracleCommand(cmdText, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(op);
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
                    OracleParameter faculty = new OracleParameter
                    {
                        ParameterName = "in_faculty",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = _student.Faculty
                    };
                    connection.Open();
                    GetInfoFromTables("select subject from subjects where faculty = :in_faculty", "subject", a_subjectComboBox,connection, faculty);
                    GetInfoFromTables("select teacher_name from teacher_info where faculty = :in_faculty", "teacher_name", a_teacherComboBox, connection, faculty.Clone() as OracleParameter);
                    connection.Close();
                }

                a_teacherComboBox.SelectedIndex = 0;
                a_subjectComboBox.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void A_sendRequestButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (a_subjectComboBox.Text == String.Empty)
            {
                MessageBox.Show("Please, choose the Subject");
                return;
            }
            if (a_teacherComboBox.Text == String.Empty)
            {
                MessageBox.Show("Please, choose the Teacher");
                return;
            }
            if (imageCode == null)
            {
                MessageBox.Show("Please, choose the image");
                return;
            }
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    OracleParameter userId = new OracleParameter
                    {
                        ParameterName = "in_user_id",
                        OracleDbType = OracleDbType.Int64,
                        Direction = ParameterDirection.Input,
                        Value = _student.UserId
                    };
                    OracleParameter teacherName = new OracleParameter
                    {
                        ParameterName = "in_teacher_name",
                        OracleDbType = OracleDbType.Varchar2,
                        Direction = ParameterDirection.Input,
                        Value = a_teacherComboBox.Text
                    };
                    OracleParameter subject = new OracleParameter
                    {
                        ParameterName = "in_subject",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = a_subjectComboBox.Text
                    };
                    OracleParameter filingDate = new OracleParameter
                    {
                        ParameterName = "in_filing_date",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = a_filingDateCalendar.SelectedDate
                    };
                    OracleParameter img = new OracleParameter
                    {
                        ParameterName = "in_img",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Blob,
                        Value = imageCode
                    };
                    connection.Open();
                    using (OracleCommand command = new OracleCommand("addAdjustment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] {userId,teacherName,subject,filingDate,img});
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                MessageBox.Show("Done");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void A_addFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            if (ofd.ShowDialog() != true) return;
            FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] ic = br.ReadBytes((Int32)fs.Length);
            imageCode = ic;
        }
    }
}
