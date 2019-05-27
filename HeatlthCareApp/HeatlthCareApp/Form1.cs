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

namespace HeatlthCareApp
{
    public partial class Form1 : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost;Initial Catalog=master;Integrated Security=True");
        int PatientId = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {


            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                    if (btnInsert.Text =="Insert") {
                        SqlCommand sqlCmd = new SqlCommand("Insert", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@mode", "Add");
                        sqlCmd.Parameters.AddWithValue("@pid", txtPatientId.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@fname", txtFirstName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@lname", txtLastName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@age", txtAge.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@street", txtStreet.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@city", txtCity.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@zipcode", txtZipCode.Text.Trim());
                        sqlCmd.ExecuteNonQuery();
                        MessageBox.Show("Added,and Saved Suuccsessfully");
                    }
                    else
                    {
                        SqlCommand sqlCmd = new SqlCommand("Insert", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@mode", "Edit");
                        sqlCmd.Parameters.AddWithValue("@pid", PatientId);
                        sqlCmd.Parameters.AddWithValue("@fname", txtFirstName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@lname", txtLastName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@age", txtAge.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@street", txtStreet.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@city", txtCity.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@zipcode", txtZipCode.Text.Trim());
                        sqlCmd.ExecuteNonQuery();
                        MessageBox.Show("Updated Suuccsessfully");

                    }
                    

                }
                Reset();
                fillDataGridView();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error,Can't perform the requested Insert Function");

            }
            finally
            {
                sqlCon.Close();

            }


        }
        void fillDataGridView()
        {
            if (sqlCon.State == ConnectionState.Closed) {

                sqlCon.Open();

                SqlDataAdapter sqlData = new SqlDataAdapter("Search",sqlCon);

                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlData.SelectCommand.Parameters.AddWithValue("@fname", txtSearch.Text.Trim());
                DataTable dtbl = new DataTable();
                sqlData.Fill(dtbl);
                dgvPatientInfo.DataSource = dtbl;
                dgvPatientInfo.Columns[0].Visible = false;
                sqlCon.Close();
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Reset();
            fillDataGridView();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                fillDataGridView();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error,Unable to perform the Search");
            }

        }

        private void dgvPatientInfo_ReadOnlyChanged(object sender, EventArgs e)
        {

        }

        private void dgvPatientInfo_DoubleClick(object sender, EventArgs e)
        {
            if (dgvPatientInfo.CurrentRow.Index !=-1)
            {
                PatientId = Convert.ToInt32(dgvPatientInfo.CurrentRow.Cells[0].Value.ToString());
                txtFirstName.Text = dgvPatientInfo.CurrentRow.Cells[1].Value.ToString();
                txtLastName.Text = dgvPatientInfo.CurrentRow.Cells[2].Value.ToString();
                txtAge.Text = dgvPatientInfo.CurrentRow.Cells[3].Value.ToString();
                txtStreet.Text = dgvPatientInfo.CurrentRow.Cells[4].Value.ToString();
                txtCity.Text = dgvPatientInfo.CurrentRow.Cells[5].Value.ToString();
                txtZipCode.Text = dgvPatientInfo.CurrentRow.Cells[6].Value.ToString();
                btnInsert.Text = "Update";
                btnDelete.Enabled = true;


            }

        }
        void Reset()
        {
            txtFirstName.Text = txtLastName.Text = txtAge.Text = txtCity.Text = txtStreet.Text = txtZipCode.Text = "";
            btnInsert.Text = "Insert";
            PatientId = 0;
            btnDelete.Enabled = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                    SqlCommand sqlCmd = new SqlCommand("Delete", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@pid", PatientId);
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Delete Operation Was Suuccsessful");
                    Reset();
                    fillDataGridView();


                }
            }catch(Exception ex){
                MessageBox.Show(ex.Message,"Delete Operation Was Not Successful");

            }
        }
    }
}
