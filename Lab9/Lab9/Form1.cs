using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab9
{
    public partial class Form1 : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["BookStoreOrders"].ConnectionString;
        SqlDataAdapter adapter;
        DataSet ds;

        public Form1()
        {
            InitializeComponent();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = true;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(GetSql(), connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                ds = new DataSet();
                adapter.Fill(ds, "Books");
                dataGridView1.DataSource = ds.Tables["Books"];

                dataGridView1.Columns["BookID"].ReadOnly = true;

                textBox1.Enabled = false;
                textBox1.DataBindings.Add("Text", ds.Tables["Books"], "BookID");
                textBox2.DataBindings.Add("Text", ds.Tables["Books"], "Title", true);
                textBox3.DataBindings.Add("Text", ds.Tables["Books"], "Price", true);

                adapter = new SqlDataAdapter("SELECT AuthorID, + FirstName + ' ' + LastName AS FullName FROM Authors;", connection);
                adapter.Fill(ds, "Authors");
                comboBox1.DataSource = ds.Tables["Authors"];
                comboBox1.DisplayMember = "FullName";
                comboBox1.ValueMember = "AuthorID";
                comboBox1.DataBindings.Add("SelectedValue", ds.Tables["Books"], "AuthorID", true);

                adapter = new SqlDataAdapter("SELECT GenreID, GenreName FROM Genres;", connection);
                adapter.Fill(ds, "Genres");
                comboBox2.DataSource = ds.Tables["Genres"];
                comboBox2.DisplayMember = "GenreName";
                comboBox2.ValueMember = "GenreID";
                comboBox2.DataBindings.Add("SelectedValue", ds.Tables["Books"], "GenreID", true);
            }
        }

        private string GetSql()
        {
            return "SELECT BookID, Title, Price, AuthorID, GenreID FROM Books ORDER BY BookID";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataRow newRow = ds.Tables["Books"].NewRow();
            ds.Tables["Books"].Rows.Add(newRow);

            dataGridView1.ClearSelection();
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (!row.IsNewRow)
                    dataGridView1.Rows.Remove(row);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BindingContext[ds, "Books"].EndCurrentEdit();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(GetSql(), connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                adapter.InsertCommand = new SqlCommand("AddBook", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;

                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Title", SqlDbType.VarChar, 200, "Title"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@GenreID", SqlDbType.Int, 0, "GenreID"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal, 0, "Price"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@AuthorID", SqlDbType.Int, 0, "AuthorID"));

                adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                adapter.DeleteCommand = commandBuilder.GetDeleteCommand();

                adapter.Update(ds, "Books");
            }

            MessageBox.Show("Зміни успішно збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.booksBindingSource.EndEdit();
            this.booksTableAdapter.Update(bookStoreOrdersDataSet.Books);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'bookStoreOrdersDataSet.Genres' table. You can move, or remove it, as needed.
            this.genresTableAdapter.Fill(this.bookStoreOrdersDataSet.Genres);
            // TODO: This line of code loads data into the 'bookStoreOrdersDataSet.Books' table. You can move, or remove it, as needed.
            this.booksTableAdapter.Fill(this.bookStoreOrdersDataSet.Books);
            // TODO: This line of code loads data into the 'bookStoreOrdersDataSet.Authors' table. You can move, or remove it, as needed.
            this.authorsTableAdapter.Fill(this.bookStoreOrdersDataSet.Authors);

        }

        BookStoreOrdersEntities db = new BookStoreOrdersEntities();
        private void button5_Click(object sender, EventArgs e)
        {
            var bookAverageOrders =
                from b in db.Books
                join od in db.OrderDetails on b.BookID equals od.BookID
                group od by new { b.BookID, b.Title, b.GenreID } into bookGroup
                select new
                {
                    BookID = bookGroup.Key.BookID,
                    BookTitle = bookGroup.Key.Title,
                    GenreID = bookGroup.Key.GenreID,
                    AvgBookOrders = bookGroup.Average(od => (float)od.Quantity)
                };

            var genreAverageOrders =
                from b in db.Books
                join od in db.OrderDetails on b.BookID equals od.BookID
                group od by b.GenreID into genreGroup
                select new
                {
                    GenreID = genreGroup.Key,
                    AvgGenreOrders = genreGroup.Average(od => (float)od.Quantity)
                };

            var query = from bao in bookAverageOrders
                        join gao in genreAverageOrders on bao.GenreID equals gao.GenreID
                        join g in db.Genres on bao.GenreID equals g.GenreID
                        where bao.AvgBookOrders > gao.AvgGenreOrders
                        orderby bao.AvgBookOrders descending
                        select new
                        {
                            BookTitle = bao.BookTitle,
                            GenreName = g.GenreName,
                            AverageBookOrders = bao.AvgBookOrders,
                            AverageGenreOrdersPerGenre = gao.AvgGenreOrders
                        };

            dataGridView3.DataSource = query.ToList();
        }
    }
}
