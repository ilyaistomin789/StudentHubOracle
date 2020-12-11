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
    /// Логика взаимодействия для ViewRetakeWindow.xaml
    /// </summary>
    public partial class ViewRetakeWindow : Window
    {
        private University.Teacher _teacher;
        public ViewRetakeWindow(University.Teacher teacher)
        {
            _teacher = teacher;
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
                        Value = _teacher.Faculty
                    };
                    connection.Open();
                    using (OracleCommand command = new OracleCommand("select s.student_name || ' ' || s.course || '-' || s.num_group student,s.faculty,r.subject,r.status,r.retake_date " +
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

    }
}

