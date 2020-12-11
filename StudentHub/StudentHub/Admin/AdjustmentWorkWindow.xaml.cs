using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
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
using Oracle.ManagedDataAccess.Client;
using StudentHub.DataBase;
using StudentHub.University;

namespace StudentHub.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdjustmentWorkWindow.xaml
    /// </summary>
    public partial class AdjustmentWorkWindow : Window
    {
        private Deanery _deanery;
        public AdjustmentWorkWindow(Deanery deanery)
        {
            _deanery = deanery;
            InitializeComponent();
            GetAdjustments();
        }

        private void GetAdjustments()
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
                        Value = _deanery.Faculty
                    };
                    connection.Open();
                    using (OracleCommand command = new OracleCommand("select s.student_name,s.faculty,a.subject,a.status, TO_CHAR(a.filing_date, 'DD.MM.YYYY') filing_date " +
                                                                     "from adjustments a inner join student_info s on a.user_id = s.user_id and s.faculty = :in_faculty", connection))
                    {
                        //TODO FIX
                        command.Parameters.Add(faculty);
                        command.ExecuteNonQuery();
                        OracleDataAdapter oda = new OracleDataAdapter(command);
                        DataTable dt = new DataTable("adjustments");
                        oda.Fill(dt);
                        dg_Adjustments.ItemsSource = dt.DefaultView;
                        oda.Update(dt);
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void AcceptDeclineAdjustment(bool action)
        {
            string studentName = ((DataRowView)dg_Adjustments.SelectedItems[0]).Row["student_name"].ToString();
            string subjectName = ((DataRowView)dg_Adjustments.SelectedItems[0]).Row["subject"].ToString();
            string date = ((DataRowView)dg_Adjustments.SelectedItems[0]).Row["filing_date"].ToString();
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    OracleParameter name = new OracleParameter
                    {
                        ParameterName = "in_student_name",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = studentName
                    };
                    OracleParameter subject = new OracleParameter
                    {
                        ParameterName = "in_subject",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = subjectName
                    };
                    OracleParameter filingDate = new OracleParameter
                    {
                        ParameterName = "in_filing_date",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = DateTime.Parse(date)
                    };
                    OracleParameter actionB = new OracleParameter
                    {
                        ParameterName = "action",
                        OracleDbType = OracleDbType.Boolean,
                        Direction = ParameterDirection.Input,
                        Value = action
                    };
                    connection.Open();
                    using (OracleCommand command = new OracleCommand("accept_decline_Adjustment",connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new[] {name, subject, filingDate, actionB});
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

        private void AcceptButton_OnClick(object sender, RoutedEventArgs e)
        {
            AcceptDeclineAdjustment(true);
        }

        private void DeclineButton_OnClick(object sender, RoutedEventArgs e)
        {
            AcceptDeclineAdjustment(false);
        }

        private void ImageButton_OnClick(object sender, RoutedEventArgs e)
        {
            string studentName = ((DataRowView)dg_Adjustments.SelectedItems[0]).Row["student_name"].ToString();
            string subjectName = ((DataRowView)dg_Adjustments.SelectedItems[0]).Row["subject"].ToString();
            string faculty = ((DataRowView)dg_Adjustments.SelectedItems[0]).Row["faculty"].ToString();
            string date = ((DataRowView)dg_Adjustments.SelectedItems[0]).Row["filing_date"].ToString();
            if (subjectName == String.Empty)
            {
                MessageBox.Show("Please, choose row");
                return;
            }
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    OracleParameter student = new OracleParameter
                    {
                        ParameterName = "in_student_name",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = studentName
                    };
                    OracleParameter subject = new OracleParameter
                    {
                        ParameterName = "in_subject",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = subjectName
                    };
                    OracleParameter facultyP = new OracleParameter
                    {
                        ParameterName = "in_faculty",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = faculty
                    };
                    OracleParameter filingDate = new OracleParameter
                    {
                        ParameterName = "in_filing_date",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = DateTime.Parse(date)
                    };
                    connection.Open();
                    using (OracleCommand command = new OracleCommand("select a.img " +
                                                                     "from adjustments a " +
                                                                     "inner join student_info s on a.user_id = s.user_id " +
                                                                     "where s.student_name = :in_student_name and a.subject = :in_subject and s.faculty = :in_faculty", connection))
                    {
                        command.Parameters.AddRange(new[] {student,subject,facultyP});
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            MemoryStream ms = new MemoryStream();
                            foreach (DbDataRecord record in reader)
                            {
                                ms.Write((byte[])record["img"], 0, ((byte[])record["img"]).Length);
                            }
                            var image = new BitmapImage();
                            image.BeginInit();
                            image.StreamSource = ms;
                            image.EndInit();
                            image.Freeze();
                            string path =
                                $"{Directory.GetCurrentDirectory()}\\Adjustments\\${studentName + faculty + subjectName + date}.png";
                            File.WriteAllBytes(path, ms.GetBuffer());
                            Process.Start(path);

                        }
                    }
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
