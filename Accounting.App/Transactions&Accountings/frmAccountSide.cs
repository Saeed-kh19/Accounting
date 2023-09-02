using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.DataLayer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.App
{
    public partial class frmAccountSide : Form
    {
        public int userId;
        public frmAccountSide()
        {
            InitializeComponent();
        }

        private void frmAccountSide_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db1 = new UnitOfWork())
            {
                if (db1.LoginRepository.GetUserById(userId).Role == 1)
                {
                    this.txtSearch.AutoSize = false;
                    BindGrid();
                }
                else
                {
                    MessageBox.Show("Access Denied!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        public void BindGrid()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgPersons.AutoGenerateColumns = false;
                dgPersons.DataSource = db.AccountingRepository.GetAllPersons();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void btnRemovePerson_Click(object sender, EventArgs e)
        {
            if (dgPersons.CurrentRow != null)
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    string name = dgPersons.CurrentRow.Cells[1].Value.ToString();
                    if (MessageBox.Show($"Are you sure you want to delete '{name}'?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        int PersonId = int.Parse(dgPersons.CurrentRow.Cells[0].Value.ToString());
                        db.AccountingRepository.DeletePerson(PersonId);
                        db.Save();
                        BindGrid();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a person to remove");
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddOrEditPersons frmAddOrEditPersons = new frmAddOrEditPersons();
            if (frmAddOrEditPersons.ShowDialog() == DialogResult.OK)
            {
                BindGrid();
            }
        }

        private void btnEditPerson_Click(object sender, EventArgs e)
        {
            frmAddOrEditPersons frmAddOrEditPersons = new frmAddOrEditPersons();
            frmAddOrEditPersons.personId = int.Parse(dgPersons.CurrentRow.Cells[0].Value.ToString());
            if (frmAddOrEditPersons.ShowDialog() == DialogResult.OK)
            {
                BindGrid();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgPersons.DataSource = db.AccountingRepository.PersonFilter(txtSearch.Text);
            }
        }
    }
}
