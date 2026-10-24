using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly EmployeeServiceBase service = new EmployeeService();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshEmployeeList();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            int age = (int)numAge.Value;

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Zəhmət olmasa bütün xanaları doldurun!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (age <= 0)
            {
                MessageBox.Show("Yaş düzgün daxil edilməlidir!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Employee newEmp = new Employee(code, firstName, lastName, age);
            bool isAdded = service.AddEmployee(newEmp);

            if (isAdded)
            {
                MessageBox.Show("İşçi uğurla əlavə edildi!", "Uğur", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearEmployeeInputs();
                RefreshEmployeeList();
            }
            else
            {
                MessageBox.Show("Bu koda malik işçi artıq mövcuddur!", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearEmployeeInputs()
        {
            txtCode.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            numAge.Value = 0;
        }

        private void RefreshEmployeeList()
        {
            lstEmployees.Items.Clear();
            cmbEmployees.Items.Clear();

            List<Employee> allEmployees = service.GetAllEmployees();
            foreach (var emp in allEmployees)
            {
                lstEmployees.Items.Add($"{emp.Code} - {emp.FirstName} {emp.LastName} ({emp.Age} yaş)");
                cmbEmployees.Items.Add($"{emp.Code} - {emp.FirstName} {emp.LastName}");
            }
        }

        private void cmbEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPaymentListBox();
        }

        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            if (cmbEmployees.SelectedIndex == -1)
            {
                MessageBox.Show("Zəhmət olmasa işçi seçin!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedText = cmbEmployees.SelectedItem.ToString();
            string empCode = selectedText.Split('-')[0].Trim();
            string title = txtPaymentTitle.Text.Trim();
            double amount = (double)numAmount.Value;

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Ödəniş adını daxil edin!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (amount <= 0)
            {
                MessageBox.Show("Məbləğ 0-dan böyük olmalıdır!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isPaymentAdded = service.AddPayment(empCode, title, amount);

            if (isPaymentAdded)
            {
                MessageBox.Show("Ödəniş uğurla əlavə olundu!", "Uğur", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPaymentTitle.Clear();
                numAmount.Value = 0;
                RefreshPaymentListBox();
            }
            else
            {
                MessageBox.Show("Ödəniş əlavə edilə bilmədi!", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshPaymentListBox()
        {
            lstEmployeePayments.Items.Clear();
            if (cmbEmployees.SelectedIndex != -1)
            {
                string selectedText = cmbEmployees.SelectedItem.ToString();
                string empCode = selectedText.Split('-')[0].Trim();
                List<Payment> payments = service.GetEmployeePayments(empCode);

                if (payments != null)
                {
                    foreach (var p in payments)
                    {
                        lstEmployeePayments.Items.Add(p.ToString());
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchCode = txtSearchCode.Text.Trim();
            lstStatPayments.Items.Clear();
            lblEmployeeDetails.Text = "";

            if (string.IsNullOrEmpty(searchCode))
            {
                MessageBox.Show("Axtarış üçün işçi kodunu daxil edin!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Employee emp = service.FindEmployee(searchCode);

            if (emp == null)
            {
                MessageBox.Show("Axtarılan koda uyğun işçi tapılmadı!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblEmployeeDetails.Text = $"Kod: {emp.Code} | Ad: {emp.FirstName} | Soyad: {emp.LastName} | Yaş: {emp.Age}";

            List<Payment> payments = service.GetEmployeePayments(searchCode);
            if (payments != null && payments.Count > 0)
            {
                foreach (var p in payments)
                {
                    lstStatPayments.Items.Add(p.ToString());
                }
            }
            else
            {
                lstStatPayments.Items.Add("Bu işçiyə aid heç bir ödəniş yoxdur.");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Proqramdan çıxmaq istədiyinizə əminsiniz?",
                "Çıxış Təsdiqi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void InitializeComponent()
        {
            this.Statistika = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstEmployeePayments = new System.Windows.Forms.TextBox();
            this.btnAddPayment = new System.Windows.Forms.TextBox();
            this.numAmount = new System.Windows.Forms.TextBox();
            this.txtPaymentTitle = new System.Windows.Forms.TextBox();
            this.cmbEmployees = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.numAge = new System.Windows.Forms.TextBox();
            this.lstStatPayments = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.TextBox();
            this.txtSearchCode = new System.Windows.Forms.TextBox();
            this.lblEmployeeDetails = new System.Windows.Forms.TextBox();
            this.Statistika.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Statistika
            // 
            this.Statistika.Controls.Add(this.tabPage1);
            this.Statistika.Controls.Add(this.tabPage2);
            this.Statistika.Controls.Add(this.tabPage3);
            this.Statistika.Location = new System.Drawing.Point(70, 7);
            this.Statistika.Name = "Statistika";
            this.Statistika.SelectedIndex = 0;
            this.Statistika.Size = new System.Drawing.Size(883, 356);
            this.Statistika.TabIndex = 0;
            this.Statistika.SelectedIndexChanged += new System.EventHandler(this.Statistika_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.AccessibleName = "İşçi əlavə et";
            this.tabPage1.Controls.Add(this.txtLastName);
            this.tabPage1.Controls.Add(this.txtFirstName);
            this.tabPage1.Controls.Add(this.txtCode);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(875, 327);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "İşçi əlavə et";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(190, 81);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(100, 22);
            this.txtLastName.TabIndex = 3;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(190, 27);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(100, 22);
            this.txtFirstName.TabIndex = 2;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(6, 6);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(100, 22);
            this.txtCode.TabIndex = 1;
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.AccessibleName = "Ödəniş yaz";
            this.tabPage2.Controls.Add(this.lstEmployeePayments);
            this.tabPage2.Controls.Add(this.btnAddPayment);
            this.tabPage2.Controls.Add(this.numAmount);
            this.tabPage2.Controls.Add(this.txtPaymentTitle);
            this.tabPage2.Controls.Add(this.cmbEmployees);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(875, 327);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstEmployeePayments
            // 
            this.lstEmployeePayments.Location = new System.Drawing.Point(403, 126);
            this.lstEmployeePayments.Name = "lstEmployeePayments";
            this.lstEmployeePayments.Size = new System.Drawing.Size(100, 22);
            this.lstEmployeePayments.TabIndex = 4;
            // 
            // btnAddPayment
            // 
            this.btnAddPayment.Location = new System.Drawing.Point(338, 249);
            this.btnAddPayment.Name = "btnAddPayment";
            this.btnAddPayment.Size = new System.Drawing.Size(100, 22);
            this.btnAddPayment.TabIndex = 3;
            // 
            // numAmount
            // 
            this.numAmount.Location = new System.Drawing.Point(535, 195);
            this.numAmount.Name = "numAmount";
            this.numAmount.Size = new System.Drawing.Size(100, 22);
            this.numAmount.TabIndex = 2;
            // 
            // txtPaymentTitle
            // 
            this.txtPaymentTitle.Location = new System.Drawing.Point(551, 48);
            this.txtPaymentTitle.Name = "txtPaymentTitle";
            this.txtPaymentTitle.Size = new System.Drawing.Size(100, 22);
            this.txtPaymentTitle.TabIndex = 1;
            this.txtPaymentTitle.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // cmbEmployees
            // 
            this.cmbEmployees.Location = new System.Drawing.Point(310, 31);
            this.cmbEmployees.Name = "cmbEmployees";
            this.cmbEmployees.Size = new System.Drawing.Size(100, 22);
            this.cmbEmployees.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.AccessibleName = "Statistika";
            this.tabPage3.Controls.Add(this.numAge);
            this.tabPage3.Controls.Add(this.lstStatPayments);
            this.tabPage3.Controls.Add(this.btnSearch);
            this.tabPage3.Controls.Add(this.txtSearchCode);
            this.tabPage3.Controls.Add(this.lblEmployeeDetails);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(875, 327);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // numAge
            // 
            this.numAge.Location = new System.Drawing.Point(90, 82);
            this.numAge.Name = "numAge";
            this.numAge.Size = new System.Drawing.Size(100, 22);
            this.numAge.TabIndex = 4;
            // 
            // lstStatPayments
            // 
            this.lstStatPayments.Location = new System.Drawing.Point(288, 203);
            this.lstStatPayments.Name = "lstStatPayments";
            this.lstStatPayments.Size = new System.Drawing.Size(100, 22);
            this.lstStatPayments.TabIndex = 3;
            this.lstStatPayments.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(477, 203);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 22);
            this.btnSearch.TabIndex = 2;
            // 
            // txtSearchCode
            // 
            this.txtSearchCode.Location = new System.Drawing.Point(569, 111);
            this.txtSearchCode.Name = "txtSearchCode";
            this.txtSearchCode.Size = new System.Drawing.Size(100, 22);
            this.txtSearchCode.TabIndex = 1;
            // 
            // lblEmployeeDetails
            // 
            this.lblEmployeeDetails.Location = new System.Drawing.Point(370, 96);
            this.lblEmployeeDetails.Name = "lblEmployeeDetails";
            this.lblEmployeeDetails.Size = new System.Drawing.Size(100, 22);
            this.lblEmployeeDetails.TabIndex = 0;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1281, 415);
            this.Controls.Add(this.Statistika);
            this.Name = "Form1";
            this.Statistika.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        private TabControl Statistika;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox txtCode;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtPaymentTitle;
        private TextBox cmbEmployees;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private TextBox lstEmployeePayments;
        private TextBox btnAddPayment;
        private TextBox numAmount;
        private TabPage tabPage3;
        private TextBox txtSearchCode;
        private TextBox lblEmployeeDetails;
        private TextBox lstStatPayments;
        private TextBox btnSearch;

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private TextBox numAge;

        private void Statistika_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}