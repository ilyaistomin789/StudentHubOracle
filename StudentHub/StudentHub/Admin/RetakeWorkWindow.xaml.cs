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

namespace StudentHub.Admin
{
    /// <summary>
    /// Логика взаимодействия для RetakeWorkWindow.xaml
    /// </summary>
    public partial class RetakeWorkWindow : Window
    {
        public RetakeWorkWindow()
        {
            InitializeComponent();
            GetRetakes();
        }
        private void GetRetakes()
        {
            string getRetakesProcedure = "ADMIN_GET_RETAKE";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getRetakeCommand = new SqlCommand(getRetakesProcedure, connection);
                    getRetakeCommand.CommandType = CommandType.StoredProcedure;
                    getRetakeCommand.ExecuteNonQuery();
                    SqlDataAdapter adjustmentDataAdapter = new SqlDataAdapter(getRetakeCommand);
                    DataTable dt1 = new DataTable("Retake");
                    adjustmentDataAdapter.Fill(dt1);
                    dg_Retakes.ItemsSource = dt1.DefaultView;
                    adjustmentDataAdapter.Update(dt1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void AcceptButton_OnClick(object sender, RoutedEventArgs e)
        {
            string studentName = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["Student"].ToString();
            string subjectName = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["Subject"].ToString();
            string date = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["Date"].ToString();
            string setAcceptProcedure = "ADMIN_ACCEPT_RETAKE";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand setAcceptCommand = new SqlCommand(setAcceptProcedure, connection);
                    setAcceptCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter studentParameter = new SqlParameter
                    {
                        ParameterName = "@StudentName",
                        Value = studentName
                    };
                    SqlParameter subjectParameter = new SqlParameter
                    {
                        ParameterName = "@SubjectName",
                        Value = subjectName
                    };
                    SqlParameter dateParameter = new SqlParameter
                    {
                        ParameterName = "@RDate",
                        Value = date
                    };
                    setAcceptCommand.Parameters.Add(dateParameter);
                    setAcceptCommand.Parameters.Add(studentParameter);
                    setAcceptCommand.Parameters.Add(subjectParameter);
                    setAcceptCommand.ExecuteNonQuery();
                    MessageBox.Show("Done");
                    this.Close();
                    RetakeWorkWindow window = new RetakeWorkWindow();
                    window.ShowDialog();

                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void DeclineButton_OnClick(object sender, RoutedEventArgs e)
        {
            string studentName = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["Student"].ToString();
            string subjectName = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["Subject"].ToString();
            string date = ((DataRowView)dg_Retakes.SelectedItems[0]).Row["Date"].ToString();
            string setDeclineProcedure = "ADMIN_DECLINE_RETAKE";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand setDeclineCommand = new SqlCommand(setDeclineProcedure, connection);
                    setDeclineCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter studentParameter = new SqlParameter
                    {
                        ParameterName = "@StudentName",
                        Value = studentName
                    };
                    SqlParameter subjectParameter = new SqlParameter
                    {
                        ParameterName = "@SubjectName",
                        Value = subjectName
                    };
                    SqlParameter dateParameter = new SqlParameter
                    {
                        ParameterName = "@RDate",
                        Value = date
                    };
                    setDeclineCommand.Parameters.Add(dateParameter);
                    setDeclineCommand.Parameters.Add(studentParameter);
                    setDeclineCommand.Parameters.Add(subjectParameter);
                    setDeclineCommand.ExecuteNonQuery();
                    MessageBox.Show("Done");
                    this.Close();
                    RetakeWorkWindow window = new RetakeWorkWindow();
                    window.ShowDialog();

                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
