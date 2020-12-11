using System;
using System.Collections.Generic;
using System.Data;
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

namespace StudentHub.Teacher
{
    /// <summary>
    /// Логика взаимодействия для AdjustmentActionWindow.xaml
    /// </summary>
    public partial class AdjustmentActionWindow : Window
    {
        private readonly University.Teacher _teacher;
        public AdjustmentActionWindow(University.Teacher teacher)
        {
            _teacher = teacher;
            InitializeComponent();
            GetAdjustmentsForTeacher();
        }

        private void GetAdjustmentsForTeacher()
        {
            try
            {
                using OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data);
                OracleParameter teacherId = new OracleParameter
                {
                    ParameterName = "in_teacher_id",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.Varchar2,
                    Value = _teacher.UserId
                };
                connection.Open();
                using (OracleCommand command = new OracleCommand("select s.student_name, s.course || '-'|| s.num_group course_group, a.subject, a.status, TO_CHAR(a.filing_date, 'DD.MM.YYYY') filing_date from adjustments a " +
                                                                 "inner join student_info s on a.user_id = s.user_id " +
                                                                 "where a.teacher_id = :in_teacher_id and a.status in ('accept/decline', 'accept/accept')", connection))
                {
                    command.Parameters.Add(teacherId);
                    OracleDataAdapter oda = new OracleDataAdapter(command);
                    DataTable dt = new DataTable("adjustments");
                    oda.Fill(dt);
                    dg_Adjustments.ItemsSource = dt.DefaultView;
                    oda.Update(dt);
                }
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
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
                    OracleParameter teacherId = new OracleParameter
                    {
                        ParameterName = "in_teacher_id",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int64,
                        Value = _teacher.UserId
                    };
                    OracleParameter actionB = new OracleParameter
                    {
                        ParameterName = "action",
                        OracleDbType = OracleDbType.Boolean,
                        Direction = ParameterDirection.Input,
                        Value = action
                    };
                    OracleParameter adjustmentDate = new OracleParameter
                    {
                        ParameterName = "in_adjustment_date",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = a_adjustmentDate.SelectedDate
                    };
                    connection.Open();
                    using (OracleCommand command = new OracleCommand("accept_decline_Adjustment_Teacher", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new[] { name, subject, teacherId, adjustmentDate, filingDate, actionB });
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
            if (a_adjustmentDate.SelectedDate == null)
            {
                MessageBox.Show("Please, select adjustment date");
                return;
            }
            AcceptDeclineAdjustment(true);
        }

        private void DeclineButton_OnClick(object sender, RoutedEventArgs e)
        {
            AcceptDeclineAdjustment(false);
        }
    }
}
