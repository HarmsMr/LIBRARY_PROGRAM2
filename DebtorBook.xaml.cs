using System;
using System.Collections.Generic;
using System.Windows;
using System.Data.SqlClient;

namespace LIBRARY_PROGRAM
{
    public class books_debt
    {
        public string DateGive { get; set; }
        public string titleBook { get; set; }
        public string Year { get; set; }
        public string get_read_id { get; set; }
        public byte[] photo { get; set; }
        public string idBook { get; set; }
    }


    public partial class DebtorBook : Window
    {
        SqlConnection connection;
        Window parent;
        string id_reader;
        public DebtorBook(SqlConnection connection, Window parent, string id_reader)
        {
            InitializeComponent();
            this.connection = connection;
            this.parent = parent;
            this.Show();
            parent.Visibility = Visibility.Hidden;
            this.id_reader = id_reader;
            update_window();

        }

        public void update_window()
        {
            try
            {
                List<books_debt> debt_list = new List<books_debt>();
                string sqlexpression = "SELECT * FROM debt_books WHERE  (CodeReader = @reader_value) ";
                SqlCommand command = new SqlCommand(sqlexpression, connection);
                SqlParameter par_reader = new SqlParameter("@reader_value", id_reader);
                command.Parameters.Add(par_reader);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        books_debt rec = new books_debt();
                        rec.DateGive = reader.GetValue(0).ToString();
                        rec.titleBook = reader.GetValue(1).ToString();
                        rec.Year = reader.GetValue(2).ToString();
                        rec.get_read_id = reader.GetValue(3).ToString();

                        rec.photo = reader[4] as byte[];
                        rec.idBook = reader.GetValue(5).ToString();

                        debt_list.Add(rec);

                    }

                    reader.Close();
                    

                }
                else
                {
                    reader.Close();
                    
                }
                grid.ItemsSource = debt_list;
            }
            catch (SqlException er)
            {
                MessageBox.Show(er.Message);

            }


        }

        private void next_step(object sender, RoutedEventArgs e)
        {
            books_debt pr = (books_debt)grid.SelectedItem;
            if (pr != null)
            {

                string id_product = pr.idBook;
                string date_give_book = pr.DateGive;
                
                

                try
                {
                    DateTime CurrentDate = DateTime.Now;

                    string sqlexpression = "UPDATE GIVEBOOK SET DateGet = @today_date WHERE DateGive = '"+ date_give_book +"' and CodeBook = '"+ id_product +"' and CodeReader = '"+ id_reader +"' ";
                    SqlCommand command = new SqlCommand(sqlexpression, connection);

                    SqlParameter cur_date_par = new SqlParameter("@today_date", CurrentDate);
                    command.Parameters.Add(cur_date_par);

                    
                    command.ExecuteNonQuery();
                   
                }
                catch (SqlException er)
                {
                    MessageBox.Show(er.Message);
                }

                



            }

            update_window();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            parent.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
