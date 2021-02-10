using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Finisar.SQLite;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace CVFinder
{
    public partial class Form1 : Form
    {
        SQLiteConnection sQLiteConnection;
        SQLiteCommand sQLiteCommand;


        public Form1()
        {
            InitializeComponent();
            GetDataList();
        }
        string path;

        private void button1_Click(object sender, EventArgs e)
        {
            sQLiteConnection = new SQLiteConnection("Data Source=myDatabase.db;Version=3");
            sQLiteConnection.Open();
            try
            {
                
                if (txtID.Text == "")
                {
                    MessageBox.Show("Please Insert ID", "Error", MessageBoxButtons.OK);
                }
                else if (txtName.Text == "")
                {
                    MessageBox.Show("Please Insert Name", "Error", MessageBoxButtons.OK);
                }
                else if (txtCNIC.Text == "")
                {
                    MessageBox.Show("Please Insert CNIC", "Error", MessageBoxButtons.OK);
                }
                else if (path == null)
                {
                    MessageBox.Show("Please Insert file", "Error", MessageBoxButtons.OK);
                }
                else
                {

                    sQLiteCommand = sQLiteConnection.CreateCommand();
                    sQLiteCommand.CommandText = "SELECT  COUNT(*) FROM CVFinder WHERE ID=" + txtID.Text + "";
                    int count = int.Parse(sQLiteCommand.ExecuteScalar().ToString());
                    if (count > 0)
                    {
                        sQLiteCommand.CommandText = "UPDATE CVFinder SET CNIC='" + txtCNIC.Text + "',Name='" + txtName.Text + "',Path='" + path + "' WHERE ID='" + txtID.Text + "'";
                        sQLiteCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        sQLiteCommand.CommandText = "Insert into CVFinder(ID,CNIC,Name,Path) Values('" + txtID.Text + "','" + txtCNIC.Text + "','" + txtName.Text + "','" + path + "')";
                        sQLiteCommand.ExecuteNonQuery();
                    }
                    MessageBox.Show("Data Has Been Inserted Successfully", "Information", MessageBoxButtons.OK);
                    GetDataList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please Refer to the Error" + ex, "Error", MessageBoxButtons.AbortRetryIgnore);
            }

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog slctfile = new OpenFileDialog();
            slctfile.Filter = @"All Files|*.txt;*.docx;*.doc;*.pdf*.xls;*.xlsx;*.pptx;*.ppt|Text File (.txt)|*.txt|Word File (.docx ,.doc)|*.docx;*.doc|PDF(.pdf)|*.pdf|Spreadsheet (.xls ,.xlsx)|  *.xls ;*.xlsx|Presentation (.pptx ,.ppt)|*.pptx;*.ppt";
            slctfile.Title = "Please Select a File";
            if (slctfile.ShowDialog() == DialogResult.OK)
            {
                FileInfo info = new FileInfo(slctfile.FileName);
                path = info.Name;

                string fileName = info.Name;
                string sourcePath = info.DirectoryName;
                string targetPath = @"C:\Docs";
                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string destFile = System.IO.Path.Combine(targetPath, fileName);

                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }
                System.IO.File.Copy(sourceFile, destFile, true);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {



        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory(@"C:\Docs");
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }


        public void GetDataList()
        {
            dataGridView2.Rows.Clear();
            sQLiteConnection = new SQLiteConnection("Data Source=myDatabase.db;Version=3");
            sQLiteConnection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand("Select * from CVFinder where (Name like '" + txSearch.Text + "%') or (ID like '" + txSearch.Text + "%') or (CNIC like '" + txSearch.Text + "%')", sQLiteConnection);
            using (SQLiteDataReader read = sQLiteCommand.ExecuteReader())
            {
                while (read.Read())
                {
                    dataGridView2.Rows.Add(new object[] {
                        read.GetValue(read.GetOrdinal("ID")),
                        read.GetValue(read.GetOrdinal("Name")),
                        read.GetValue(read.GetOrdinal("CNIC")),
                        read.GetValue(read.GetOrdinal("Path"))
                    });

                }
            }
        }
        private void txSearch_TextChanged(object sender, EventArgs e)
        {
            if (txSearch.Text == "")
            {
                dataGridView2.Rows.Clear();
                GetDataList();

            }
            else
            {
                GetDataList();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
            {
                if (dataGridView2.CurrentCell != null && dataGridView2.CurrentCell.Value != null)
                {
                    System.Diagnostics.Process.Start(@"C:\Docs\" + dataGridView2.CurrentCell.Value.ToString());

                    PrintDialog printDlg = new PrintDialog();
                    PrintDocument printDoc = new PrintDocument();

                    printDoc.DocumentName = @"C:\Docs\" + dataGridView2.CurrentCell.Value.ToString();
                    printDlg.Document = printDoc;

                    printDlg.AllowSelection = true;
                    printDlg.AllowSomePages = true;

                    //Call ShowDialog  
                    if (printDlg.ShowDialog() == DialogResult.OK) printDoc.Print();


                }
            }
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
