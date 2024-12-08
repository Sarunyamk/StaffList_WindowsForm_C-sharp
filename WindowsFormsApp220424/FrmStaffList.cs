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

namespace WindowsFormsApp220424
{
    public partial class FrmStaffList : Form
    {
        public FrmStaffList()
        {
            InitializeComponent();
        }

        private void FrmStaffList_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            string server = "DESKTOP-JG4CG59\\SQLEXPRESS";
            string database = "Mink";
            string username = "sa";
            string password = "passw0rd";

            string connectionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password};";


            string query = "SELECT * FROM TbStaff";


            if (txtSearch.Text.Length != 0)
            {
                query += " Where Fname like '%" + txtSearch.Text + "%' or Lname like '%" + txtSearch.Text + "%' or IDnumber like '%" + txtSearch.Text + "%' or username like '%" + txtSearch.Text + "%' ";
            }

            query += " order by Fname,Lname ";


            // Create a DataTable to hold the data
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a SqlCommand to execute the query
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Create a SqlDataAdapter to fill the DataTable
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            // Fill the DataTable with the results of the query
                            adapter.Fill(dataTable);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            dgv.DataSource = dataTable;


        }

            private void btnCreate_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
                        form1.ShowDialog(); 
        }

        private void btnCreate1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // ตรวจสอบว่าเกิดการดับเบิลคลิกบนข้อมูลในแถว
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // ดึงข้อมูลจากแถวที่ดับเบิลคลิก
                DataGridViewRow selectedRow = dgv.Rows[e.RowIndex];
                string id = selectedRow.Cells["ID"].Value.ToString();
                

                // แสดงข้อความเพื่อตรวจสอบการทำงาน (สามารถเปลี่ยนโค้ดตรงนี้เป็นการปรับแต่งข้อมูลตามที่ต้องการ)
                MessageBox.Show($"Double-clicked row: ID = {id}");

                Form1 form1 = new Form1();
                form1.SetFormbyID(id);
                form1.ShowDialog();
            }
        }
    }
}
