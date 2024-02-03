using System;
using System.Windows.Forms;

namespace PhoneBookApp
{

    public partial class EntryForm : Form
    {
        public string fullname { get; private set; }
        public string phonenumber { get; private set; }
        public DateTime date { get; private set; }
        private DataGridViewRow _mySelectedRow;
        public DataGridViewRow MySelctedRow
        {
            get { return _mySelectedRow; }
            set { _mySelectedRow = value; }
        }
        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        private bool _okButtonClicked = false;

        public EntryForm()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            date = DateTime.Now;
            this.Name = _status;

        }

        private void EntryForm_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(this.EntryForm_FormClosing);
            FillFields();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            fullname = textBoxFullName.Text;
            phonenumber = textBoxPhoneNumber.Text;
            date = dateTimePicker1.Value;
            _okButtonClicked = true;
            this.Close();
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FillFields()
        {
            if (_mySelectedRow != null)
            {
                textBoxFullName.Text = _mySelectedRow.Cells["Full Name"].Value + string.Empty;
                textBoxPhoneNumber.Text = _mySelectedRow.Cells["Phone Number"].Value + string.Empty;
                dateTimePicker1.Value = (DateTime)_mySelectedRow.Cells["Date of birth"].Value;
            }
        }
        private void EntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_okButtonClicked)
                _status = "Cancel";
        }
    }
}
