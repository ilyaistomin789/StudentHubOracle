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
    /// Логика взаимодействия для RetakeWorkWindow.xaml
    /// </summary>
    public partial class RetakeWorkWindow : Window
    {
        private readonly Deanery _deanery;
        public RetakeWorkWindow(Deanery deanery)
        {
            _deanery = deanery;
            InitializeComponent();
            GetRetakes();
        }
        private void GetRetakes()
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
                    using (OracleCommand command = new OracleCommand("select s.student_name,s.faculty,r.subject,r.status,TO_CHAR(r.retake_date, 'DD.MM.YYYY') retake_date, r.img " +
                                                                     "from retakes r inner join student_info s on r.user_id = s.user_id and s.faculty = :in_faculty", connection))
                    {
                        command.Parameters.Add(faculty);
                        command.ExecuteNonQuery();
                        OracleDataAdapter oda = new OracleDataAdapter(command);
                        DataTable dt = new DataTable("retakes");
                        oda.Fill(dt);
                        dg_Retakes.ItemsSource = dt.DefaultView;
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
        private void AcceptDeclineRetake(bool action)
        {
            string studentName = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["student_name"].ToString();
            string subjectName = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["subject"].ToString();
            string date = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["retake_date"].ToString();
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
                    OracleParameter retakeDate = new OracleParameter
                    {
                        ParameterName = "in_retake_date",
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
                    using (OracleCommand command = new OracleCommand("accept_decline_Retake", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new[] { name, subject, retakeDate, actionB });
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
            AcceptDeclineRetake(true);
        }

        private void DeclineButton_OnClick(object sender, RoutedEventArgs e)
        {
            AcceptDeclineRetake(false);
            //this.Close();
            //RetakeWorkWindow window = new RetakeWorkWindow();
            //window.ShowDialog();
        }

        private void ImageButton_OnClick(object sender, RoutedEventArgs e)
        {
            string studentName = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["student_name"].ToString();
            string subjectName = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["subject"].ToString();
            string faculty = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["faculty"].ToString();
            string date = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["retake_date"].ToString();
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
                    connection.Open();
                    using (OracleCommand command = new OracleCommand("select r.img " +
                                                                     "from retakes r " +
                                                                     "inner join student_info s on r.user_id = s.user_id " +
                                                                     "where s.student_name = :in_student_name and r.subject = :in_subject and s.faculty = :in_faculty", connection))
                    {
                        command.Parameters.AddRange(new[] { student, subject, facultyP });
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
                                $"{Directory.GetCurrentDirectory()}\\Retakes\\${studentName + faculty + subjectName + date}.png";
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
