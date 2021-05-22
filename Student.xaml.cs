using System.Windows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LIBRARY_PROGRAM
{
    public class students
    {
        public string idStud { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }

    }

    public partial class Student : Window
    {
        SqlConnection connection;
        Window parent;
        string id_product;
        public Student(SqlConnection connection, Window parent, string id_product)
        {
            InitializeComponent();
            this.Show();
            this.parent = parent;
            this.connection = connection;
            this.id_product = id_product;
            parent.Visibility = Visibility.Hidden;

            try
            {
                List<students> students_list = new List<students>();
                string sqlexpression = "SELECT * FROM dbo.READERS";
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            parent.Visibility = Visibility.Visible;
            this.Close();
        }

        private void giveBook(object sender, RoutedEventArgs e)
        {
            students pr = (students)grid.SelectedItem;
            if (pr != null)
            {

                string id_student = pr.idStud;

                try
                {
                    DateTime CurrentDate = DateTime.Now;

                    string sqlexpression = "INSERT INTO GIVEBOOK VALUES(@today_date, null, @id_book, @id_reader)";
                    SqlCommand command = new SqlCommand(sqlexpression, connection);

                    SqlParameter cur_date_par = new SqlParameter("@today_date", CurrentDate);
                    command.Parameters.Add(cur_date_par);

                    SqlParameter book_par = new SqlParameter("@id_book", id_product);
                    command.Parameters.Add(book_par);

                    SqlParameter reader_par = new SqlParameter("@id_reader", id_student);
                    command.Parameters.Add(reader_par);

                    command.ExecuteNonQuery();
                }
                catch (SqlException er)
                {
                    MessageBox.Show(er.Message);
                }





            }
        }

        private void new_reader_Checked(object sender, RoutedEventArgs e)
        {
            grid.Visibility = Visibility.Hidden;
            new_but.Visibility = Visibility.Visible;
            exist_but.Visibility = Visibility.Hidden;
            rows_for_new.Visibility = Visibility.Visible;


        }

        private void exist_reader_Checked(object sender, RoutedEventArgs e)
        {
            grid.Visibility = Visibility.Visible;
            new_but.Visibility = Visibility.Hidden;
            exist_but.Visibility = Visibility.Visible;
            rows_for_new.Visibility = Visibility.Hidden;
        }

        private void giveBoobToNew(object sender, RoutedEventArgs e)
        {
            try
            {
                string id_student = DateTime.Now.ToString("yyyyddMMss");
                DateTime CurrentDate = DateTime.Now;
                string sqlexpression = "insert into READERS VALUES(@reader_id, @reader_surname, @reader_name, @reader_mid)";
                SqlCommand command = new SqlCommand(sqlexpression, connection);

                SqlParameter par1 = new SqlParameter("@reader_id", id_student);
                SqlParameter par2 = new SqlParameter("@reader_surname", surname_t.Text);
                SqlParameter par3 = new SqlParameter("@reader_name", name_t.Text);
                SqlParameter par4 = new SqlParameter("@reader_mid", mid_t.Text);


                command.Parameters.Add(par1);
                command.Parameters.Add(par2);
                command.Parameters.Add(par3);
                command.Parameters.Add(par4);

                command.ExecuteNonQuery();



                sqlexpression = "INSERT INTO GIVEBOOK VALUES(@today_date, null, @id_book, @id_reader)";
                command = new SqlCommand(sqlexpression, connection);

                SqlParameter cur_date_par = new SqlParameter("@today_date", CurrentDate);
                command.Parameters.Add(cur_date_par);

                SqlParameter book_par = new SqlParameter("@id_book", id_product);
                command.Parameters.Add(book_par);

                SqlParameter reader_par = new SqlParameter("@id_reader", id_student);
                command.Parameters.Add(reader_par);

                command.ExecuteNonQuery();
            }
            catch (SqlException er)
            {
                MessageBox.Show(er.Message);
            }

        }
    }
}
