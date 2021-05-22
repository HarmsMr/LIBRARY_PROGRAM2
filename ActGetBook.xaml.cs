using System.Windows;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace LIBRARY_PROGRAM
{

    public partial class ActGetBook : Window
    {
        public class students
        {
            public string idStud { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MidName { get; set; }

        }

        SqlConnection connection;
        Window parent;
        public ActGetBook(SqlConnection connection, Window parent)
        {
            InitializeComponent();
            this.Show();
            parent.Visibility = Visibility.Hidden;
            this.connection = connection;
            this.parent = parent;

            try
            {
                List<students> students_list = new List<students>();
                string sqlexpression = "SELECT DISTINCT * FROM dbo.debtors";
                SqlCommand command = new SqlCommand(sqlexpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        students rec = new students();
                        rec.idStud = reader.GetValue(0).ToString();
                        rec.LastName = reader.GetValue(1).ToString();
                        rec.FirstName = reader.GetValue(2).ToString();
                        rec.MidName = reader.GetValue(3).ToString();

                        students_list.Add(rec);

                    }

                    reader.Close();
                    grid.ItemsSource = students_list;

                }
                else
                {
                    reader.Close();
                }
            }
            catch (SqlException er)
            {
                MessageBox.Show(er.Message);

            }



        }
        private void next_step(object sender, RoutedEventArgs e)
        {
            students pr = (students)grid.SelectedItem;
            if (pr != null)
            {
                string id_student = pr.idStud;
                new DebtorBook(connection, this, id_student); 
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            parent.Visibility = Visibility.Visible;
            this.Close();
        }

        
    }
}
