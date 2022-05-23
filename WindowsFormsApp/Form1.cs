using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=test;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            build_grid();
        }
        private void build_grid()
        {
            try
            {

                using (SqlDataAdapter adapter = new SqlDataAdapter("sel_dep_im", con))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = comboBox1.SelectedIndex;
                    adapter.SelectCommand.Parameters.Add("@ch", SqlDbType.Int).Value = checkBox1.Checked ? "1" : "0";
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.Columns.Clear();
                    foreach (DataColumn col in table.Columns)
                    {
                        DataGridViewColumn dcol = new DataGridViewTextBoxColumn();
                        dcol.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dcol.DataPropertyName = col.ColumnName;
                        dcol.HeaderText = col.ColumnName;
                        dcol.Name = col.ColumnName;

                        if (col.DataType == typeof(double))
                        {
                            dcol.DefaultCellStyle.Format = "N2";
                        }

                        dataGridView1.Columns.Add(dcol);
                    }

                    dataGridView1.DataSource = table;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message + " " + ex.InnerException);
            }
        }

        //закрытие приложения
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
        }



        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
                checkBox1.Visible = false;
            else
                checkBox1.Visible = true;
            build_grid();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            build_grid();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }
    }
}
