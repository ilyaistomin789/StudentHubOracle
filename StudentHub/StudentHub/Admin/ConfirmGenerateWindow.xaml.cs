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
    /// Логика взаимодействия для ConfirmGenerateWindow.xaml
    /// </summary>
    public partial class ConfirmGenerateWindow : Window
    {
        private readonly Student _student;
        public ConfirmGenerateWindow(Student student)
        {
            InitializeComponent();
            _student = student;
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            string updateEmailProcedure = "UPDATE_EMAIL";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand updateEmailCommand = new SqlCommand(updateEmailProcedure, connection);
                    updateEmailCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter studentNameParameter = new SqlParameter
                    {
                        ParameterName = "@StudentName",
                        Value = _student.Name
                    };
                    SqlParameter courseParameter = new SqlParameter
                    {
                        ParameterName = "@Course",
                        Value = _student.Course
                    };
                    SqlParameter groupParameter = new SqlParameter
                    {
                        ParameterName = "@GroupId",
                        Value = _student.Group
                    };
                    SqlParameter facultyParameter = new SqlParameter
                    {
                        ParameterName = "@Faculty",
                        Value = _student.Faculty
                    };
                    SqlParameter emailParameter = new SqlParameter
                    {
                        ParameterName = "@Email",
                        Value = $"{emailTextBox.Text}{mailTextBlock.Text};{passwordTextBox.Text}"
                    };
                    updateEmailCommand.Parameters.Add(studentNameParameter);
                    updateEmailCommand.Parameters.Add(courseParameter);
                    updateEmailCommand.Parameters.Add(groupParameter);
                    updateEmailCommand.Parameters.Add(facultyParameter);
                    updateEmailCommand.Parameters.Add(emailParameter);
                    updateEmailCommand.ExecuteNonQuery();
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
