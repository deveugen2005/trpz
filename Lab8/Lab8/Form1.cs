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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder["Data Source"] = @"(localdb)\MSSQLLocalDB";
            connectionStringBuilder["Initial Catalog"] = "BookStoreOrders";
            connectionStringBuilder["User ID"] = textBox1.Text;
            connectionStringBuilder["Password"] = textBox2.Text;
            using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    //Вывод на экран информации о подключении к базе данных
                    MessageBox.Show("Вдалось успішно підключитись до БД " + connection.Database);

                    Form2 form = new Form2(connectionStringBuilder.ConnectionString);
                    form.Show();
                    this.Hide();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }

        }
    }
}
