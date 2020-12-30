using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace SchoolProject
{
    public partial class Form1 : Form
    {
        OleDbConnection oleDb;
        Panel activepanel;
        int SidePanelExpanded = 275, SidePanelMin = 45;
        public Form1()
        {
            InitializeComponent();
            oleDb = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = school.mdb");
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            SettingActivePanel("LoginPanel");
            RegistUsertext.Leave += RegistUsertext_Leave;
            RegistPassText.Leave += RegistPassText_Leave;
            RegistConfirmText.Leave += RegistConfirmText_Leave;
        }

        #region User-Created-Functions

        private void LoginErrorShow()
        {
            label21.Text = "Wrong UserName or Password";
            label21.Visible = true;
            label21.Top = LoginPassPanel.Top + LoginPassPanel.Height;
            label21.Left = LoginPassPanel.Left + LoginPassPanel.Width - label21.Width;
        }

        private void errorshow(string str, Control control)
        {
            label20.Visible = true;
            label20.Text = str;
            label20.Left = control.Parent.Left + control.Parent.Width - label20.Width;
            label20.Top = control.Parent.Top + control.Parent.Height;
            
        }

        public void sidepanelbtnreset()
        {
            foreach(Panel panel in SidePanel.Controls)
            {
                if(panel.Name.Substring(4) == activepanel.Name)
                {
                    panel.BackColor = Color.FromArgb(254, 171, 109);
                }
                else
                {
                    panel.BackColor = Color.FromArgb(255, 215, 184);
                }
            }
        }

        public void SettingActivePanel(string str)
        {
            Panel panel = panel4;
            foreach (Control control in panel.Controls)
            {
                if (control is Panel)
                {
                    if(control.Name == str)
                    {
                        activepanel = control as Panel;
                        control.Visible = true;
                        control.Dock = DockStyle.Fill;
                        activepanel = (Panel)control;
                    }
                    else
                    {
                        control.Visible = false;
                        control.Dock = DockStyle.None;
                    }
                }
            }
            sidepanelbtnreset();
        }

        public void centerwidth(Control control)
        {
            control.Left = control.Parent.Width / 2 - control.Width / 2;
        }

        public int getCenterHeight(Control control)
        {
            return control.Height / 2;
        }

        public void SettingRegistrationPage()
        {
            int RegistPanelViewWidth = RegistrationPanel.Width / 100;
            int RegistPanelViewHeight = RegistrationPanel.Height / 100;
            label17.Top = 5 * RegistPanelViewHeight - getCenterHeight(label17);
            centerwidth(label17);
            label16.Top = 13 * RegistPanelViewHeight - getCenterHeight(label16);
            centerwidth(label16);
            RegistUserPanel.Top = 40 * RegistPanelViewHeight - getCenterHeight(RegistUserPanel);
            centerwidth(RegistUserPanel);
            label15.Top = 34 * RegistPanelViewHeight - getCenterHeight(label15);
            label15.Left = RegistUserPanel.Left;
            RegistPassPanel.Top = 55 * RegistPanelViewHeight - getCenterHeight(RegistPassPanel);
            centerwidth(RegistPassPanel);
            label14.Top = 49 * RegistPanelViewHeight - getCenterHeight(label14);
            label14.Left = RegistPassPanel.Left;
            RegistConfirmPassPanel.Top = 70 * RegistPanelViewHeight - getCenterHeight(RegistConfirmPassPanel);
            centerwidth(RegistConfirmPassPanel);
            label18.Top = 64 * RegistPanelViewHeight - getCenterHeight(label18);
            label18.Left = RegistConfirmPassPanel.Left;
            AdminBox.Top = 80 * RegistPanelViewHeight - getCenterHeight(AdminBox);
            centerwidth(AdminBox);
            Registbtn.Top = 90 * RegistPanelViewHeight - getCenterHeight(Registbtn);
            centerwidth(Registbtn);
        }

        public void SettingLoginPage()
        {
            int LoginPanelViewWidth = LoginPanel.Width / 100;
            int LoginPanelViewHeight = LoginPanel.Height / 100;
            label10.Top = 5 * LoginPanelViewHeight - getCenterHeight(label10);
            centerwidth(label10);
            label11.Top = 13 * LoginPanelViewHeight - getCenterHeight(label11);
            centerwidth(label11);
            LoginUserPanel.Top = 40 * LoginPanelViewHeight - getCenterHeight(LoginUserPanel);
            centerwidth(LoginUserPanel);
            label12.Top = 34 * LoginPanelViewHeight - getCenterHeight(label12);
            label12.Left = LoginUserPanel.Left;
            LoginPassPanel.Top = 55 * LoginPanelViewHeight - getCenterHeight(LoginPassPanel);
            centerwidth(LoginPassPanel);
            label13.Left = LoginPassPanel.Left;
            label13.Top = 49 * LoginPanelViewHeight - getCenterHeight(label13);
            LoginBtn.Top = 70 * LoginPanelViewHeight - getCenterHeight(LoginBtn);
            centerwidth(LoginBtn);
            RegistPageBtn.Top = 80 * LoginPanelViewHeight - getCenterHeight(RegistPageBtn);
            centerwidth(RegistPageBtn);
            centerheight(LoginUserText);
            centerheight(LoginPassText);
        }

        public void centerheight(Control control)
        {
            control.Top = control.Parent.Height / 2 - control.Height / 2;
        }

        #endregion

        #region TitleBar
        private void title_btn_picture_set(object sender,PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            foreach (PictureBox picture in panel.Controls)
            {
                picture.Location = new Point(panel.Width / 2 - picture.Width / 2, panel.Height / 2 - picture.Height / 2);
            }
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Maximized)
            {
                pictureBox3.Image = global::SchoolProject.Properties.Resources.squares_min;
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                pictureBox3.Image = global::SchoolProject.Properties.Resources.squares;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void title_btn_entered(object sender, EventArgs e)
        {
            Panel temppanel = sender as Panel;
            temppanel.BackColor = Color.FromArgb(244, 152, 82);
        }
        private void title_btn_left(object sender, EventArgs e)
        {
            Panel temppanel = sender as Panel;
            temppanel.BackColor = Color.FromArgb(255, 235, 220);
        }

        private void picture_title_btn_entered(object sender,EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.Parent.BackColor = Color.FromArgb(244, 152, 82);
        }

        private void picture_title_btn_left(object sender,EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.Parent.BackColor = Color.FromArgb(255, 235, 220);
        }
        #endregion

        #region StatusBar
        private void statusbar_text_enter(object sender, EventArgs e)
        {
            Label label = sender as Label;
            label.Font = new Font(label.Font, FontStyle.Underline | FontStyle.Bold);
        }

        private void statusbar_text_left(object sender,EventArgs e)
        {
            Label label = sender as Label;
            label.Font = new Font(label.Font, FontStyle.Bold);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/dcornade");
        }
        #endregion


        private void SidePanel_DoubleClick(object sender, EventArgs e)
        {
            if(SidePanel.Width != SidePanelExpanded)
            {
                SidePanel.Width = SidePanelExpanded;
            }
            else
            {
                SidePanel.Width = SidePanelMin;
            }
        }

        private void SidePanel_Hover_Enter(object sender,EventArgs e)
        {
            Panel panel = sender as Panel;
            if(panel == null)
            {
                Control control = sender as Control;
                if(control.Parent.Name.Substring(0, 5) == "panel")
                {
                    panel = (Panel)control.Parent.Parent;
                }
                else
                {
                    panel = (Panel)control.Parent;
                }
            }
            if(panel.Name.Substring(0,5) == "panel")
            {
                panel = (Panel)panel.Parent;
            }
            if(panel.Name.Substring(4) != activepanel.Name) { panel.BackColor = Color.FromArgb(254, 171, 109); }
        }
        private void SidePanel_Hover_Left(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null)
            {
                Control control = sender as Control;
                if (control.Parent.Name.Substring(0, 5) == "panel")
                {
                    panel = (Panel)control.Parent.Parent;
                }
                else
                {
                    panel = (Panel)control.Parent;
                }
            }
            if (panel.Name.Substring(0, 5) == "panel")
            {
                panel = (Panel)panel.Parent;
            }
            if (panel.Name.Substring(4) != activepanel.Name) { panel.BackColor = Color.FromArgb(255, 215, 184); }
        }

        private void LoginPanel_SizeChanged(object sender, EventArgs e)
        {
            SettingLoginPage();
        }

        private void Text_Panel_Click(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            if(panel == null)
            {
                TextBox textBox = sender as TextBox;
                panel = (Panel)textBox.Parent;
            }
            foreach(Control control in panel.Controls)
            {
                if(control is TextBox)
                {
                    control.Focus();
                }
                if(control is PictureBox)
                {
                    control.Visible = true;
                }
            }
        }

        private void Text_Panel_FocusLost(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Panel panel = (Panel)textBox.Parent;
            foreach (Control control in panel.Controls)
            {
                if (control is PictureBox)
                {
                    control.Visible = false;
                }
            }
        }

        private void Password_Show_Click(object sender,EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            Panel panel = (Panel)pictureBox.Parent;
            foreach(Control control in panel.Controls)
            {
                if(control is TextBox)
                {
                    TextBox textBox = control as TextBox;
                    if(textBox.UseSystemPasswordChar == true)
                    {
                        textBox.UseSystemPasswordChar = false;
                        pictureBox.Image = global::SchoolProject.Properties.Resources.invisible;
                    }
                    else
                    {
                        textBox.UseSystemPasswordChar = true;
                        pictureBox.Image = global::SchoolProject.Properties.Resources.eye;
                    }
                }
            }
        }

        private void RegistrationPanel_SizeChanged(object sender, EventArgs e)
        {
            SettingRegistrationPage();
        }

        private void RegistPageBtn_Click(object sender, EventArgs e)
        {
            SettingActivePanel("RegistrationPanel");
        }

        private void RegistUsertext_TextChanged(object sender, EventArgs e)
        {
            if (RegistUsertext.Text.Length > 3)
            {
                label20.Visible = false;
            }
        }

        private void RegistPassText_Leave(object sender,EventArgs e)
        {
            if(RegistPassText.Text == "")
            {
                errorshow("Please Insert Appropriate Password", RegistPassText);
                RegistPassText.Focus();
            }
        }

        private void RegistConfirmText_Leave(object sender, EventArgs e)
        {
            if(RegistPassText.Text != RegistConfirmText.Text)
            {
                errorshow("Password Doesn't Match", RegistConfirmText);
                RegistConfirmText.Focus();
            }
        }

        private void RegistUsertext_Leave(object sender, EventArgs e)
        {
            if(label20.Visible == true)
            {
                RegistUsertext.Focus();
            }
            else if(RegistUsertext.Text.Length < 3)
            {
                errorshow("Username Must be more than three letters", RegistUsertext);
                RegistUsertext.Focus();
            }
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            oleDb.Open();
            string sql = "Select * from Login where UserId='" + LoginUserText.Text + "' and passw='" + LoginPassText.Text + "'";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, oleDb);
            DataSet set = new DataSet();
            adapter.Fill(set);
            if(set.Tables[0].Rows.Count != 0)
            {
                label21.Visible = false;
            }
            else
            {
                LoginErrorShow();
            }
            oleDb.Close();
        }

        private void SidePanel_Click(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;

            if (panel == null)
            {
                Control control = sender as Control;
                if (control.Parent.Name.Substring(0, 5) == "panel")
                {
                    panel = (Panel)control.Parent.Parent;
                }
                else
                {
                    panel = (Panel)control.Parent;
                }
            }
            if (panel.Name.Substring(0, 5) == "panel")
            {
                panel = (Panel)panel.Parent;
            }
            string sendername = panel.Name;
            label19.Text = sendername.Substring(4);
            SettingActivePanel(sendername.Substring(4));
        }
    }
}
