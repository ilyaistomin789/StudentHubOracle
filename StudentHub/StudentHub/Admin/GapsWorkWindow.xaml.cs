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
    /// Логика взаимодействия для GapsWorkWindow.xaml
    /// </summary>
    public partial class GapsWorkWindow : Window
    {
        private Window _window;
        public GapsWorkWindow()
        {
            InitializeComponent();
            GetGaps();
        }
        private void GetGaps()
        {
            string getGapsProcedure = "ADMIN_GET_GAPS";
            try
            {
                using (SqlConnection connection = new SqlConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    SqlCommand getGapsCommand = new SqlCommand(getGapsProcedure, connection);
                    getGapsCommand.CommandType = CommandType.StoredProcedure;
                    getGapsCommand.ExecuteNonQuery();
                    SqlDataAdapter adjustmentDataAdapter = new SqlDataAdapter(getGapsCommand);
                    DataTable dt1 = new DataTable("StudentGaps");
                    adjustmentDataAdapter.Fill(dt1);
                    dg_Gaps.ItemsSource = dt1.DefaultView;
                    adjustmentDataAdapter.Update(dt1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void LotOfStudentGapsButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new BadStudentsWindow();
            _window.Show();
        }
    }
}
