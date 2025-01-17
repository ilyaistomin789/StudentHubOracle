﻿using System;
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
using Oracle.ManagedDataAccess.Client;
using StudentHub.DataBase;
using StudentHub.University;

namespace StudentHub.Admin
{
    /// <summary>
    /// Логика взаимодействия для GapsWorkWindow.xaml
    /// </summary>
    public partial class GapsWorkWindow : Window
    {
        private Window _window;
        private Deanery _deanery;
        public GapsWorkWindow(Deanery deanery)
        {
            _deanery = deanery;
            InitializeComponent();
            GetGaps();
        }
        private void GetGaps()
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
                    using (OracleCommand command = new OracleCommand("select s.student_name || ' ' || s.course || '-' || s.num_group student, g.subject, sum(g.gaps_count) count from gaps g " +
                                                                     "inner join student_info s on g.user_id = s.user_id where s.faculty = :in_faculty " +
                                                                     "group by s.student_name, s.course, s.num_group, g.subject", connection))
                    {
                        command.Parameters.Add(faculty);
                        command.ExecuteNonQuery();
                        OracleDataAdapter oda = new OracleDataAdapter(command);
                        DataTable dt = new DataTable("gaps");
                        oda.Fill(dt);
                        dg_Gaps.ItemsSource = dt.DefaultView;
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


        private void CloseStudentGapsButton_OnClick(object sender, RoutedEventArgs e) => this.Close();
    }
}
