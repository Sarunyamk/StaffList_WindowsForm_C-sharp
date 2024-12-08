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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;

namespace WindowsFormsApp220424
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }




        public void SetFormbyID(string staffID)
        {
            string server = "DESKTOP-JG4CG59\\SQLEXPRESS";
            string database = "Mink";
            string username = "sa";
            string password = "passw0rd";

            string connectionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password};";

            string query = "select * from TbStaff";
            query += " where ID = " + staffID;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //// เพิ่มพารามิเตอร์และกำหนดค่า
                    //command.Parameters.AddWithValue("@SearchValue", searchValue);
                    connection.Open();
                    // สร้าง DataReader เพื่อดึงข้อมูล
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // ตรวจสอบว่ามีข้อมูลหรือไม่
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // ดึงข้อมูลจากแต่ละคอลัมน์
                                //int id = reader.GetInt32(reader.GetOrdinal("Id"));
                                //string name = reader.GetString(reader.GetOrdinal("Name"));
                                lblStaffId.Text = staffID;
                                comboPre.Text = reader.GetString(reader.GetOrdinal("PreName"));
                                txtName.Text = reader.GetString(reader.GetOrdinal("FName"));
                                txtLName.Text = reader.GetString(reader.GetOrdinal("LName"));
                                txtIDnumber.Text = reader.GetString(reader.GetOrdinal("IDNumber"));
                                txtUsername.Text = reader.GetString(reader.GetOrdinal("Username"));
                                txtPassword.Text = reader.GetString(reader.GetOrdinal("Password"));

                                int telOrdinal = reader.GetOrdinal("Tel");

                                if (!reader.IsDBNull(telOrdinal))
                                {
                                    txtTel.Text = reader.GetString(telOrdinal);
                                }
                                else
                                {
                                    txtTel.Text = ""; // หรือค่าที่คุณต้องการให้แสดงเมื่อเป็น NULL
                                }


                                int birthDayOrdinal = reader.GetOrdinal("Birthday");
                                if (!reader.IsDBNull(birthDayOrdinal))
                                {
                                    string bdString = reader.GetString(birthDayOrdinal);
                                    dtBD.Value = DateTime.Parse(bdString);
                                }
                                else
                                {
                                    // จัดการกรณีเป็น NULL
                                }

                               

                               


                                //// ทำสิ่งที่ต้องการกับข้อมูลที่ได้ เช่น แสดงผลหรือเก็บไว้ในรายการ
                                //Console.WriteLine($"ID: {id}, Name: {name}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data found.");
                        }
                    }
                }

                connection.Close();
            }
        }


        private void insert()
        {
            string server = "DESKTOP-JG4CG59\\SQLEXPRESS";
            string database = "Mink";
            string username = "sa";
            string password = "passw0rd";

            string connectionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password};";


            string insertQuery = @"
    INSERT INTO [dbo].[TbStaff]
           ([PreName]
           ,[FName]
           ,[LName]
           ,[IDNumber]
           ,[StartDate_STR]
           ,[StartDate_DT]
           ,[Username]
           ,[Password]
           ,[IsActive]
           ,[DepartmentID]
           ,[BirthDay]
           ,[Tel])
     VALUES
           (@PreName
           ,@FName
           ,@LName
           ,@IDNumber
           ,@StartDate_STR
           ,@StartDate_DT
           ,@Username
           ,@Password
           ,@IsActive
           ,@DepartmentID
           ,@BirthDay
           ,@Tel)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@PreName", comboPre.Text);
                command.Parameters.AddWithValue("@FName", txtName.Text); // ใส่ค่าจาก text box สำหรับชื่อ
                command.Parameters.AddWithValue("@LName", txtLName.Text); // ใส่ค่าจาก text box สำหรับนามสกุล
                command.Parameters.AddWithValue("@IDNumber", txtIDnumber.Text);
                command.Parameters.AddWithValue("@StartDate_STR", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                command.Parameters.AddWithValue("@StartDate_DT", DateTime.Now); // ใส่ค่าวันที่/เวลาปัจจุบัน
                command.Parameters.AddWithValue("@Username", txtUsername.Text);
                command.Parameters.AddWithValue("@Password", txtPassword.Text);
                command.Parameters.AddWithValue("@IsActive", "Y");
                command.Parameters.AddWithValue("@DepartmentID", 1); // ใส่ค่า DepartmentID ตามที่ต้องการ
                command.Parameters.AddWithValue("@BirthDay", dtBD.Value.ToString("yyyy/MM/dd"));
                command.Parameters.AddWithValue("@Tel", txtTel.Text);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("เพิ่มข้อมูลเรียบร้อยแล้ว!");
                    }
                    else
                    {
                        MessageBox.Show("เพิ่มข้อมูลล้มเหลว");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }


        }

        private Boolean isValidateDataCompleted()


        {
            string selectedValue = comboPre.SelectedItem?.ToString(); // ดึงค่าที่เลือกจาก ComboBox

            if (string.IsNullOrEmpty(selectedValue) || selectedValue == "โปรดเลือก")
            {
                MessageBox.Show("กรุณาเลือกข้อมูลใน ComboBox");
                return false; // หยุดการดำเนินการต่อ
            }
            if (txtName.Text.Trim().Length == 0 || txtLName.Text.Trim().Length == 0 || txtIDnumber.Text.Trim().Length == 0 || txtTel.Text.Trim().Length == 0 || txtUsername.Text.Trim().Length == 0 || txtPassword.Text.Trim().Length == 0)
            {
                MessageBox.Show("กรุณากรอกข้อมูล");
                return false;

            }


            return true;

        }

        private void Updateform()
        {
            string server = "DESKTOP-JG4CG59\\SQLEXPRESS";
            string database = "Mink";
            string username = "sa";
            string password = "passw0rd";

            string connectionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password};";

            string sql = "update TbStaff";
            sql += " set Prename = '" + comboPre.Text + "' , Fname = '" + txtName.Text + "' , Lname = '" + txtLName.Text +"'";
            sql += " where ID = " + lblStaffId.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);             

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("แก้ไขข้อมูลเรียบร้อยแล้ว!");
                    }
                    else
                    {
                        MessageBox.Show("แก้ไขข้อมูลล้มเหลว");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            

            if (isValidateDataCompleted() == true) 
            {

                if (lblStaffId.Text == "lblStaffId")
                {
                    insert();
                    clear();
                }
                else
                {
                   Updateform();
                    //clear();
                }
                        
                
                

            }
            

        }
        private void clear()
        { 
        comboPre.Items.Clear();
            txtName.Text = "";
            txtLName.Text = "";
            txtIDnumber.Text = "";
            txtTel.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            dtBD.Text = "";

        }

        private void txtIDnumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // ยกเลิกการป้อนตัวอักษร
            }
            
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            string input = txtUsername.Text;
            string pattern = @"^[a-zA-Z0-9]*$"; // Regular Expression สำหรับตรวจสอบภาษาอังกฤษและตัวเลขเท่านั้น

            if (!Regex.IsMatch(input, pattern))
            {
                MessageBox.Show("โปรดป้อนเฉพาะภาษาอังกฤษและตัวเลขเท่านั้น", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // ลบอักขระที่ไม่ถูกต้องออกจาก TextBox
                txtUsername.Text = Regex.Replace(input, "[^a-zA-Z0-9]", "");
                txtUsername.Select(txtUsername.Text.Length, 0); // นำเคอร์เซอร์ไปที่จุดสุดท้ายของข้อความ
            }
        }
    }
}
