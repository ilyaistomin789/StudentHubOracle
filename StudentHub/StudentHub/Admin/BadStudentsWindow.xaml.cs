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
    /// Логика взаимодействия для BadStudentsWindow.xaml
    /// </summary>
    public partial class BadStudentsWindow : Window
    {
        public BadStudentsWindow()
        {
            InitializeComponent();
            GetBadStudents();
        }

        private void GetBadStudents()
        {
            string getBadStudentsProcedure = "ADMIN_GET_BADSTUDENTS";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getBadStudentsCommand = new SqlCommand(getBadStudentsProcedure, connection);
                    getBadStudentsCommand.CommandType = CommandType.StoredProcedure;
                    getBadStudentsCommand.ExecuteNonQuery();
                    SqlDataAdapter adjustmentDataAdapter = new SqlDataAdapter(getBadStudentsCommand);
                    DataTable dt1 = new DataTable("BadStudent");
                    adjustmentDataAdapter.Fill(dt1);
                    dg_BadStudents.ItemsSource = dt1.DefaultView;
                    adjustmentDataAdapter.Update(dt1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
