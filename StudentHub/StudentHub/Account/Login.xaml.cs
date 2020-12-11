using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Oracle.ManagedDataAccess.Client;
using StudentHub.DataBase;
using StudentHub.Teacher;
using StudentHub.University;

namespace StudentHub.Account
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private Window _window;
        private readonly Student _student = new Student();
        private readonly Deanery _deanery = new Deanery();
        private readonly UniversityEssence ue = UniversityEssence.GetInstance();
        public Login()
        {
            InitializeComponent();
        }

        private void Login_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void ExitButton_OnClick(object sender, RoutedEventArgs e) => this.Close();

        private void SignUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new SignUp();
            _window.Show();
            this.Close();
        }


        private void LogInButton_OnClick(object sender, RoutedEventArgs e)
        {
            User currentUser = new User();
            if (logIn_UserName.Text == String.Empty)
            {
                MessageBox.Show("Please, enter the User name");
                return;
            }

            if (logIn_Password.Password == String.Empty)
            {
                MessageBox.Show("Please, enter the Password");
                return;
            }

            if (logIn_Password.Password.Length < 5)
            {
                MessageBox.Show("Allowed password length: 5 characters");
                return;
            }
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();

                    OracleParameter login = new OracleParameter
                    {
                        ParameterName = "in_login",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = logIn_UserName.Text
                    };
                    OracleParameter password = new OracleParameter
                    {
                        ParameterName = "in_password",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = logIn_Password.Password
                    };
                    OracleParameter user = new OracleParameter
                    {
                        ParameterName = "user_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("findUser"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] {login,password,user});
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        foreach (DataRow row in dt.Rows)
                        {
                            currentUser.UserId = int.Parse(row["id"].ToString());
                            currentUser.UserName = row["login"].ToString();
                            currentUser.Password = row["password"].ToString();
                            currentUser.Role = row["role"].ToString();
                        }
                    }
                    //TODO CHECK ROLES
                    connection.Close();
                    if (currentUser.Role == ue.STUDENT_ROLE)
                    {
                        _window = new MainWindow(currentUser);
                    }

                    if (currentUser.Role == ue.DEANERY_ROLE)
                    {
                        _window = new AdminWindow(currentUser);
                        _window.Show();
                    }

                    if (currentUser.Role == ue.TEACHER_ROLE)
                    {
                        _window = new TeacherWindow(currentUser);
                        _window.Show();
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
