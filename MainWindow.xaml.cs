using System.Data.SqlClient;
using System.Windows;


namespace LIBRARY_PROGRAM
{

    public partial class MainWindow : Window
    {
        SqlConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = @"DESKTOP-SE9D8MI\SQLEXPRESS",
                    InitialCatalog = "LIBRARY",
                    IntegratedSecurity = true
                };
                connection = new SqlConnection(builder.ConnectionString);
            }
            catch (SqlException err)
            {
                MessageBox.Show("Ошибка: " + err.Message);
            }
            connection.Open();

        }

        private void Enter(object sender, RoutedEventArgs e)
        {

            if (login.Text.Length > 0 && pass.Password.Length > 0)
            {
                try
                {


                    SqlParameter login_par = new SqlParameter("@login_value", login.Text);
                    SqlParameter pass_par = new SqlParameter("@pass_value", pass.Password);

                    string sqlexpression = "select * from dbo.enter_query WHERE (CodeEmployee COLLATE Cyrillic_General_CS_AS = @login_value) AND (PasswordEmployee COLLATE Cyrillic_General_CS_AS = @pass_value) ";

                    SqlCommand command = new SqlCommand(sqlexpression, connection);

                    command.Parameters.Add(login_par);
                    command.Parameters.Add(pass_par);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        string surname = reader.GetString(0);
                        reader.Close();
                        pass.Password = "";

                        new Menu(connection, this);
                    }
                    else
                    {
                        reader.Close();
                        MessageBox.Show("Неверный пароль или логин!");
                    }
                }
                catch (SqlException err)
                {
                    MessageBox.Show("Ошибка: " + err.Message);
                }

            }
            else
            {
                MessageBox.Show("Поля не заполнены!");
            }
        }
    }
}
