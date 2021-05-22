using System.Data.SqlClient;
using System.Windows;


namespace LIBRARY_PROGRAM
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        SqlConnection connection;
        Window parent;
        public Menu(SqlConnection connection, Window parent)
        {
            InitializeComponent();
            this.connection = connection;
            this.parent = parent;
            this.Show();
            parent.Visibility = Visibility.Hidden;
        }

        private void GiveBook_Click(object sender, RoutedEventArgs e)
        {
            new TakeBook(connection, this); //conect this
        }

        private void GetBook_Click(object sender, RoutedEventArgs e)
        {
            new ActGetBook(connection, this);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            parent.Visibility = Visibility.Visible;
            this.Close();
            
        }
    }
}
