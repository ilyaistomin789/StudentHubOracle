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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;
using StudentHub.DataBase;
using StudentHub.University;

namespace StudentHub.Admin
{
    /// <summary>
    /// Логика взаимодействия для StudentProgress.xaml
    /// </summary>
    public partial class StudentProgress : Page
    {
        private Deanery _deanery;
        public StudentProgress(Deanery deanery)
        {
            _deanery = deanery;
            InitializeComponent();
            GetStudentProgress();
        }

        private void GetStudentProgress()
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
                    using (OracleCommand command = new OracleCommand("select s.student_name || ' ' || s.course || '-' || s.num_group student, avg(sp.NOTE) from student_progress sp " +
                                                                     "inner join student_info s on sp.user_id = s.user_id " +
                                                                     "where s.faculty = :in_faculty " +
                                                                     "group by s.student_name, s.course, s.num_group " +
                                                                     "order by s.student_name", connection))
                    {
                        command.Parameters.Add(faculty);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}
