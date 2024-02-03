using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PhoneBookApp
{
    public partial class PhonebookForm : Form
    {
        string _connectionStringName = "PhoneBookAppDB";
        SqlConnection _connection;
        public PhonebookForm()
        {
            InitializeComponent();
        }

        private static string GetConnectionString(string name)
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[name];
            if (connectionStringSettings == null)
            {
                throw new InvalidOperationException($"No connection string found with the name '{name}'");
            }
            return connectionStringSettings.ConnectionString;
        }

        private void PhonebookForm_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(this.PhonebookForm_FormClosing);
            try
            {
                string connectionString = GetConnectionString(_connectionStringName);
                _connection = new SqlConnection(connectionString);
                LoadDataIntoDataGridView();
                dataGridView1.Columns[0].Name = "Id";
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Name = "Full Name";
                dataGridView1.Columns[1].HeaderText = "Full Name";
                dataGridView1.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.Columns[2].Name = "Phone Number";
                dataGridView1.Columns[2].HeaderText = "Phone Number";
                dataGridView1.Columns[3].Name = "Date of birth";
                dataGridView1.Columns[3].HeaderText = "Date of birth";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDataIntoDataGridView()
        {
            try
            {
                if (_connection != null && _connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                using (SqlCommand command = new SqlCommand("CustomSelect", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    List<Contact> CantactList = new List<Contact>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var contact = new Contact()
                        {
                            Id = (int)row["Id"],
                            FullName = (string)row["fullname"],
                            PhoneNumber = (string)row["phonenumber"],
                            Birthdate = (DateTime)row["birthdate"]
                        };
                        CantactList.Add(contact);
                    }
                    dataGridView1.DataSource = CantactList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            EntryForm entryForm = new EntryForm();
            entryForm.Text = "Add";
            entryForm.Status = "Add";
            entryForm.ShowDialog();
            if (entryForm.Status == "Add")
            {
                AddContact(entryForm.fullname, entryForm.phonenumber, entryForm.date);
            }
            LoadDataIntoDataGridView();
        }

        private void AddContact(string contactName, string phoneNumber, DateTime birthDate)
        {
            try
            {
                if (_connection != null && _connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                using (SqlCommand command = new SqlCommand("AddContact", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", contactName);
                    command.Parameters.AddWithValue("@Number", phoneNumber);
                    command.Parameters.AddWithValue("@Date", birthDate);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                EntryForm entryForm = new EntryForm();
                entryForm.MySelctedRow = dataGridView1.SelectedRows[0];
                entryForm.Text = "Edit";
                entryForm.Status = "Edit";
                entryForm.ShowDialog();
                if (entryForm.Status == "Edit")
                {
                    EditContact(entryForm.fullname, entryForm.phonenumber, entryForm.date);
                }
                LoadDataIntoDataGridView();
            }
            else
            {
                MessageBox.Show("Please select a row first.");
            }
        }

        private void EditContact(string newContactName, string newPhoneNumber, DateTime newBirthdate)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                using (SqlCommand command = new SqlCommand("UpdateContact", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", dataGridView1.SelectedRows[0].Cells["Id"].Value);
                    command.Parameters.AddWithValue("@newName", newContactName);
                    command.Parameters.AddWithValue("@newNumber", newPhoneNumber);
                    command.Parameters.AddWithValue("@newDate", newBirthdate);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                DeleteContact();
                LoadDataIntoDataGridView();
            }
            else
            {
                MessageBox.Show("Please select a row first.");
            }

        }

        private void DeleteContact()
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                using (SqlCommand command = new SqlCommand("DeleteContact", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", dataGridView1.SelectedRows[0].Cells["Id"].Value);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PhonebookForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_connection != null)
                _connection.Close();
        }
    }
}
