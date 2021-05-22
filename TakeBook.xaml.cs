using System.Windows;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System;
using System.Windows.Media.Imaging;

namespace LIBRARY_PROGRAM
{
    public class books
    {
        public string idBook { get; set; }
        public string titleBook { get; set; }
        public string Year { get; set; }
        public byte[] photo { get; set; }
    }

    public partial class TakeBook : Window
    {
        SqlConnection connection;
        Window parent;
        public TakeBook(SqlConnection connection, Window parent) 
        {
            InitializeComponent();
            this.Show();
            parent.Visibility = Visibility.Hidden;
            this.connection = connection;
            this.parent = parent;
            try
            {
                List<books> book_list = new List<books>();
                string sqlexpression = "SELECT CodeBook, NameBook, YearPublic, Photo FROM dbo.BOOKS";
                SqlCommand command = new SqlCommand(sqlexpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        
                        books rec = new books(); 
                        rec.idBook = reader.GetValue(0).ToString();
                        rec.titleBook = reader.GetValue(1).ToString();
                        rec.Year = reader.GetValue(2).ToString();
                        rec.photo = reader[3] as byte[];

                        book_list.Add(rec);

                    }

                    reader.Close();
                    grid.ItemsSource = book_list;
                                                  
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            parent.Visibility = Visibility.Visible;
            this.Close();
        }

        private void next_step(object sender, RoutedEventArgs e)
        {
            books pr = (books)grid.SelectedItem;
            if (pr != null)
            {

                string id_product = pr.idBook;
                this.Visibility = Visibility.Hidden;
                
                new Student(connection, this, id_product);
            }
        }
    }
}

