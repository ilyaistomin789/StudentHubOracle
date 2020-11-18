using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Oracle.ManagedDataAccess.Client;
using StudentHub.DataBase;
using StudentHub.University;


namespace StudentHub.Account
{
    /// <summary>
    /// Логика взаимодействия для SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private Window _window;
        public SignUp()
        {
            InitializeComponent();
        }

        private void SignUp_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Reg_GoToLogin_OnClick(object sender, RoutedEventArgs e)
        {
            _window = new Login();
            _window.Show();
            this.Close();
        }

        private void Reg_SignUp_OnClick(object sender, RoutedEventArgs e)
        {
            if (reg_UserName.Text == String.Empty)
            {
                MessageBox.Show("Enter the User name");
                return;
            }

            if (reg_Password.Password == String.Empty)
            {
                MessageBox.Show("Enter the Password");
                return;
            }

            if (reg_Password.Password.Length < 5)
            {
                MessageBox.Show("Allowed password length: 5 characters");
                return;
            }
            if (reg_PasswordConfirm.Password == String.Empty)
            {
                MessageBox.Show("Enter the Confirm password");
                return;
            }
            if (reg_Password.Password != reg_PasswordConfirm.Password)
            {
                MessageBox.Show("Passwords don't match");
                return;
            }
            try
            {
                //TODO CREATE USER addUser proc
                using (OracleConnection connection = new OracleConnection(OracleDataBaseConnection.data))
                {
                    connection.Open();
                    OracleParameter login = new OracleParameter
                    {
                        ParameterName = "login",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = reg_UserName.Text
                    };
                    OracleParameter password = new OracleParameter
                    {
                        ParameterName = "password",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = User.GetHashPassword(reg_Password.Password)
                    };
                    using (OracleCommand command = new OracleCommand("addUser",connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] {login,password});
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Done");
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }
    }
}
