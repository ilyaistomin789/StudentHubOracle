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
    /// Логика взаимодействия для ViewProgressWindow.xaml
    /// </summary>
    public partial class ViewProgressWindow : Window
    {
        private string _studentName;
        public ViewProgressWindow()
        {
            InitializeComponent();
        }

        public ViewProgressWindow(string studentName)
        {
            InitializeComponent();
            _studentName = studentName;
            InitializeDataGrid();
        }

        private void InitializeDataGrid()
        {
            string searchStudentProcedure = "SEARCH_STUDENT";
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
                        MessageBox.Show("Error when searching for a student. May be this student has no grades.");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
