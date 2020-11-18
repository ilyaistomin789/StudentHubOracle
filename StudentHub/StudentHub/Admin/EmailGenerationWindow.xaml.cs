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
    /// Логика взаимодействия для EmailGenerationWindow.xaml
    /// </summary>
    public partial class EmailGenerationWindow : Window
    {
        private readonly string _studentName;
        private Window _window;
        private Student _student = new Student();
        public EmailGenerationWindow(string studentName)
        {
            InitializeComponent();
            _studentName = studentName;
            InitializeDataGrid();
        }

        private void InitializeDataGrid()
        {
            string searchStudentProcedure = "SEARCH_STUDENT_FIELDS";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand searchStudentCommand = new SqlCommand(searchStudentProcedure, connection);
                    searchStudentCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter studentNameParameter = new SqlParameter
                    {
                        ParameterName = "@StudentName",
                        Value = _studentName
                    };
                    searchStudentCommand.Parameters.Add(studentNameParameter);
                    var students = searchStudentCommand.ExecuteReader();
                    if (students.HasRows)
                    {
                        students.Close();
                        searchStudentCommand.ExecuteNonQuery();
                        SqlDataAdapter studentDataAdapter = new SqlDataAdapter(searchStudentCommand);
                        DataTable dt = new DataTable("Student");
                        studentDataAdapter.Fill(dt);
                        dg_Students.ItemsSource = dt.DefaultView;
                        studentDataAdapter.Update(dt);
                        this.Show();
                    }
                    else
                    {
                        MessageBox.Show("Error when searching for a student");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            _student.Name = ((DataRowView)dg_Students.SelectedItems[0]).Row["Student"].ToString();
            _student.Course = Convert.ToInt32(((DataRowView) dg_Students.SelectedItems[0]).Row["Course"].ToString());
            _student.Group = Convert.ToInt32(((DataRowView) dg_Students.SelectedItems[0]).Row["Group"].ToString());
            _student.Faculty = ((DataRowView) dg_Students.SelectedItems[0]).Row["Faculty"].ToString();
            _window = new ConfirmGenerateWindow(_student);
            _window.Show();
        }
    }
}
