using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab8
{
    public partial class Form2 : Form
    {
        string connectionString;
        public Form2(string str)
        {
            InitializeComponent();
            connectionString = str;

            LoadAuthors();
            LoadGenres();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = Application.OpenForms[0];
            form.Close();
        }

        private void LoadAuthors()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT AuthorID, CONCAT(FirstName, ' ', LastName) AS FullName FROM Authors";
                var adapter = new SqlDataAdapter(query, connection);
                var table = new DataTable();
                adapter.Fill(table);
                comboBox2.DataSource = table;
                comboBox2.DisplayMember = "FullName";
                comboBox2.ValueMember = "AuthorID";
            }
        }

        private void LoadGenres()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT GenreID, GenreName FROM Genres";
                var adapter = new SqlDataAdapter(query, connection);
                var table = new DataTable();
                adapter.Fill(table);
                comboBox1.DataSource = table;
                comboBox1.DisplayMember = "GenreName";
                comboBox1.ValueMember = "GenreID";
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO Books (Title, AuthorID, GenreID, Price) VALUES (@Title, @AuthorID, @GenreID, @Price); ";

                    command.Parameters.Add("@Title", SqlDbType.VarChar, 200);
                    command.Parameters.Add("@AuthorID", SqlDbType.Int);
                    command.Parameters.Add("@GenreID", SqlDbType.Int);
                    command.Parameters.Add("@Price", SqlDbType.Decimal);

                    command.Parameters["@Title"].Value = textBox1.Text;
                    command.Parameters["@AuthorID"].Value = comboBox2.SelectedValue;
                    command.Parameters["@GenreID"].Value = comboBox1.SelectedValue;
                    command.Parameters["@Price"].Value = decimal.Parse(textBox2.Text); 
 
                    command.ExecuteNonQuery();
                    MessageBox.Show("Книга додана успішно!");
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не вдалося додати книгу до БД: " + exception.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Будь ласка, введіть ID книги для пошуку.");
                return;
            }

            int bookId;
            if (!int.TryParse(textBox3.Text, out bookId))
            {
                MessageBox.Show("Введіть дійсне значення ID.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT * FROM Books WHERE BookID = @BookID";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@BookID", bookId);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string title = reader.GetString(reader.GetOrdinal("Title"));
                        int authorId = reader.GetInt32(reader.GetOrdinal("AuthorID"));
                        int genreId = reader.GetInt32(reader.GetOrdinal("GenreID"));
                        decimal price = reader.GetDecimal(reader.GetOrdinal("Price"));

                        textBox1.Text = title;
                        comboBox2.SelectedValue = authorId;
                        comboBox1.SelectedValue = genreId;
                        textBox2.Text = price.ToString("0.00");

                        MessageBox.Show("Книга знайдена!");
                    }
                    else
                    {
                        MessageBox.Show("Книга з таким ID не знайдена.");
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не вдалося знайти книгу в БД: " + exception.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Будь ласка, введіть ID книги для оновлення.");
                return;
            }

            int bookId;
            if (!int.TryParse(textBox3.Text, out bookId))
            {
                MessageBox.Show("Введіть дійсне значення ID.");
                return;
            }

            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;
                        command.CommandText = @" UPDATE Books SET Title = @Title, AuthorID = @AuthorID, 
                                                GenreID = @GenreID, Price = @Price WHERE BookID = @BookID";

                        command.Parameters.Add("@BookID", SqlDbType.Int);
                        command.Parameters.Add("@Title", SqlDbType.VarChar, 200);
                        command.Parameters.Add("@AuthorID", SqlDbType.Int);
                        command.Parameters.Add("@GenreID", SqlDbType.Int);
                        command.Parameters.Add("@Price", SqlDbType.Decimal);

                        command.Parameters["@BookID"].Value = bookId; 
                        command.Parameters["@Title"].Value = textBox1.Text; 
                        command.Parameters["@AuthorID"].Value = comboBox2.SelectedValue; 
                        command.Parameters["@GenreID"].Value = comboBox1.SelectedValue;
                        command.Parameters["@Price"].Value = decimal.Parse(textBox2.Text); 

                        command.ExecuteNonQuery();

                        MessageBox.Show("Дані книги оновлені успішно!");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Не вдалося оновити дані книги в БД: " + exception.Message);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Будь ласка, введіть ID книги для видалення.");
                return;
            }

            int bookId;
            if (!int.TryParse(textBox3.Text, out bookId))
            {
                MessageBox.Show("Введіть дійсне значення ID.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"DELETE FROM Books WHERE BookID = @BookID";

                    command.Parameters.Add("@BookID", SqlDbType.Int);
                    command.Parameters["@BookID"].Value = bookId;

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Книга успішно видалена!");
                        textBox1.Clear(); 
                        textBox2.Clear();
                        textBox3.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Книгу з таким ID не знайдено.");
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не вдалося видалити книгу з БД: " + exception.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля!");
                return;
            }

            int bookId;
            if (!int.TryParse(textBox3.Text, out bookId))
            {
                MessageBox.Show("Введіть дійсний ID книги.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = @"UPDATE Books SET Title = @Title, GenreID = @GenreID, Price = @Price WHERE BookID = @BookID";
                    command.Parameters.AddWithValue("@BookID", bookId);
                    command.Parameters.AddWithValue("@Title", textBox1.Text);
                    command.Parameters.AddWithValue("@GenreID", int.Parse(comboBox1.SelectedValue.ToString()));
                    command.Parameters.AddWithValue("@AuthorID", int.Parse(comboBox2.SelectedValue.ToString()));
                    command.Parameters.AddWithValue("@Price", decimal.Parse(textBox2.Text));

                    command.ExecuteNonQuery();

                    command.Parameters.Clear();

                    command.CommandText = @"INSERT INTO Genres (GenreName) VALUES (@GenreName)";
                    command.Parameters.AddWithValue("@GenreName", textBox4.Text);

                    command.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Книга оновлена, а жанр доданий успішно!");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Помилка виконання транзакції: " + ex.Message);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = @"SELECT 
                                        CONVERT(DATE, o.OrderDate) AS OrderDate,
                                        g.GenreName,
                                        SUM(od.Quantity) AS TotalBooksOrdered
                                    FROM Orders o
                                    JOIN OrderDetails od ON o.OrderID = od.OrderID
                                    JOIN Books b ON od.BookID = b.BookID
                                    JOIN Genres g ON b.GenreID = g.GenreID
                                    GROUP BY 
                                        CONVERT(DATE, o.OrderDate), 
                                        g.GenreName
                                    HAVING 
                                        SUM(od.Quantity) > 1
                                    ORDER BY 
                                        OrderDate,
                                        g.GenreName;";

                    SqlCommand command = new SqlCommand(sql, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder result = new StringBuilder();
                    while (reader.Read())
                    {
                        DateTime orderDate = reader.GetDateTime(0);
                        string genreName = reader.GetString(1);
                        int totalBooksOrdered = reader.GetInt32(2);

                        result.AppendLine($"Дата: {orderDate.ToShortDateString()}, Жанр: {genreName}, Кількість книг: {totalBooksOrdered}");
                    }

                    reader.Close();

                    if (result.Length > 0)
                    {
                        MessageBox.Show(result.ToString(), "Результати запиту");
                    }
                    else
                    {
                        MessageBox.Show("Даних для відображення немає.", "Результати запиту");
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось выполнить запрос: " + exception.Message);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string sqlExpression = "AddBook";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Title", textBox1.Text));
                    command.Parameters.Add(new SqlParameter("@GenreID", int.Parse(comboBox1.SelectedValue.ToString())));
                    command.Parameters.Add(new SqlParameter("@Price", decimal.Parse(textBox2.Text)));
                    command.Parameters.Add(new SqlParameter("@AuthorID", int.Parse(comboBox2.SelectedValue.ToString())));

                    var result = command.ExecuteScalar();

                    Console.WriteLine("ID доданої книги: {0}", result);
                    MessageBox.Show("Книга успішно додана! ID: " + result);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не вдалося виконати процедуру додавання книги. " + exception.Message);
                }
            }
        }
    }
}
