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
using System.IO;
using System.Drawing.Imaging;

namespace SchoolProject
{
    public partial class Form1 : Form
    {
        OleDbConnection oleDb;
        Panel activepanel;
        int SidePanelExpanded = 275, SidePanelMin = 45, expanded= 0;
        bool IsExpandable;
        public Form1()
        {
            InitializeComponent();
            oleDb = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = school.mdb");
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            SettingActivePanel("LoginPanel");
            RegistUsertext.Leave += RegistUsertext_Leave;
            RegistPassText.Enter += RegistUsertext_Leave;
            RegistPassText.Leave += RegistPassText_Leave;
            RegistConfirmText.Enter += RegistPassText_Leave;
            RegistConfirmText.Leave += RegistConfirmText_Leave;
        }

        #region User-Created-Functions

        private void inputerrorclean(object sender)
        {
            Control control = sender as Control;
            Control ParentControl = control.Parent;
            if (ParentControl.Controls.ContainsKey("inputerrorLabel"))
            {
                ParentControl.Controls.RemoveByKey("inputerrorLabel");
            }
        }

        private void inputerrorshow(object sender)
        {
            Control control = sender as Control;
            Control control1 = control.Parent;
            Panel panel = null;
            foreach(Control control2 in control1.Controls)
            {
                if(control2 is Panel)
                {
                    foreach(Control control3 in control2.Controls)
                    {
                        if(control3 is TextBox)
                        {
                            TextBox textBox = control3 as TextBox;
                            if(textBox.Multiline == true)
                            {
                                panel = (Panel)textBox.Parent;
                            }
                        }
                    }
                }
            }
            Label inputerrorLabel = new Label();
            inputerrorLabel.AutoSize = true;
            inputerrorLabel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            inputerrorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            inputerrorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(69)))), ((int)(((byte)(2)))));
            inputerrorLabel.Text = "Please Fill all Fields Required";
            inputerrorLabel.Location = new Point(panel.Left + panel.Width + 5 - inputerrorLabel.Width, (panel.Top) - inputerrorLabel.Height);
            inputerrorLabel.Name = "inputerrorLabel";
            inputerrorLabel.Size = new System.Drawing.Size(166, 16);
            inputerrorLabel.TabIndex = 52;
            inputerrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            control1.Controls.Add(inputerrorLabel);
        }

        private void phone_error(object sender)
        {
            Control control = sender as Control;
            Control control1 = control.Parent;
            Control control2 = control1.Parent;
            Label ErrorLabel = new Label();
            ErrorLabel.AutoSize = true;
            ErrorLabel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            ErrorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ErrorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(69)))), ((int)(((byte)(2)))));
            ErrorLabel.Location = new Point(control1.Left + control1.Width + 5, (control1.Top - control1.Height/2)+ErrorLabel.Height/2);
            ErrorLabel.Name = "ErrorLabel";
            ErrorLabel.Size = new System.Drawing.Size(166, 16);
            ErrorLabel.TabIndex = 52;
            ErrorLabel.Text = "Please Enter Only Number";
            ErrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            control2.Controls.Add(ErrorLabel);
        }

        private bool checkinput(object sender)
        {
            int flag = 0;
            Control control = sender as Control;
            Control control1 = control.Parent;
            foreach(Control control2 in control1.Controls)
            {
                if(control2 is TextBox)
                {
                    TextBox textBox = control2 as TextBox;
                    if(textBox.Text is "")
                    {
                        flag = 1;
                        break;
                    }
                }else if(control2 is ComboBox)
                {
                    ComboBox box = control2 as ComboBox;
                    if(box.Text == "")
                    {
                        flag = 1;
                        break;
                    }
                }else if(control2 is PictureBox)
                {
                    PictureBox picture = control2 as PictureBox;
                    if(picture.Image == null)
                    {
                        flag = 1;
                        break;
                    }
                }
            }
            if(flag == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Panel panelchecknull(object sender)
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
            return panel;
        }
        private void studentSidePanelShow()
        {
            ExpStudentDataPanel.Visible = true;
            ExpStudentDataPanel.Parent.Controls.SetChildIndex(ExpStudentDataPanel, 2);
        }

        private void studentsidePanelHide()
        {
            ExpStudentDataPanel.Visible = false;
            ExpStudentDataPanel.Parent.Controls.SetChildIndex(ExpStudentDataPanel, 2);
        }
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

                if (panel.Name.Substring(0, 3) == "Exp")
                {
                    if (activepanel.Name.Substring(activepanel.Name.Length - 5) != "Panel")
                    {
                        foreach (Panel panel1 in panel.Controls)
                        {
                            if (panel1.Name.Substring(4) == activepanel.Name)
                            {
                                panel1.BackColor = Color.FromArgb(255, 215, 184);
                                label19.Text = "expran";
                            }
                            else
                            {
                                panel1.BackColor = Color.FromArgb(254, 171, 109);
                            }
                        }
                    }
                }
                if (panel.Name.Substring(4) == activepanel.Name || panel.Name.Substring(4, 3) == activepanel.Name.Substring(0,3))
                {
                    panel.BackColor = Color.FromArgb(254, 171, 109);
                    
                }
                else
                {
                    if(panel.Name.Substring(0,4) == "Side") { panel.BackColor = Color.FromArgb(255, 215, 184); }
                }
            }
        }

        public void SettingActivePanel(string str)
        {
            if (activepanel != null)
            {
                if(activepanel.Name != str)
                {
                    expanded = 0;
                    
                    
                }
            }
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
                        if (activepanel.Name.Substring(activepanel.Name.Length - 5) == "Panel" && expanded == 0)
                        {
                            studentsidePanelHide();
                        }
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

        private void SettingStudentDataPage()
        {
            int vh = panel4.Height / 100;
            int vw = panel4.Width / 100;
            label31.Top = 4 * vh - getCenterHeight(label31);
            centerwidth(label31);
            panel3.Top = 7 * vh;
            panel3.Width = 90 * vw;
            centerwidth(panel3);
            panel23.Left = 20 * vw - panel24.Width / 2;
            panel23.Top = panel24.Top = panel25.Top = 80 * vh - getCenterHeight(panel23);
            panel24.Left = 52 * vw - panel24.Width / 2;
            panel25.Left = 85 * vw - panel25.Width / 2;
        }

        private void Studentidsuggest(string str,object sender)
        {
            TextBox text = sender as TextBox;
            oleDb.Open();
            string sql = "Select * from Student";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, oleDb);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            for(int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                if (dataSet.Tables[0].Rows[i]["Student_id"].ToString().Substring(0, text.Text.Length) == text.Text)
                {

                } 
            }
        }

        private void SettingUpdatePage()
        {
            int vh = panel4.Height / 100;
            int vw = panel4.Width / 100;
            label53.Top = 3 * vh;
            centerwidth(label53);
            panel35.Left = panel36.Left = panel37.Left = panel39.Left = panel34.Left = panel33.Left = 7 * vw;
            panel35.Top = 10 * vh;
            SuggestionPanel.Top = panel35.Top + panel35.Height;
            SuggestionPanel.Left = panel35.Left;
            label47.Left = label48.Left = label49.Left = label52.Left = label46.Left = label45.Left = 7 * vw;
            label47.Top = panel35.Top - label47.Height;
            panel36.Top = 25 * vh - getCenterHeight(panel36);
            label48.Top = panel36.Top - label48.Height;
            panel37.Top = 40 * vh - getCenterHeight(panel37);
            label49.Top = panel37.Top - label49.Height;
            panel39.Top = 55 * vh - getCenterHeight(panel39);
            label52.Top = panel39.Top - label52.Height;
            panel34.Top = 70 * vh - getCenterHeight(panel34);
            label46.Top = panel34.Top - label46.Height;
            panel33.Top = 88 * vh - getCenterHeight(panel33);
            label45.Top = panel33.Top - label45.Height;
            pictureBox23.Top = 12 * vh;
            pictureBox23.Left = UpdatePageButton.Left = 80 * vw;
            label50.Top = pictureBox23.Top + pictureBox23.Height;
            UpdatePageButton.Top = 50 * vh;
            label44.Left = (pictureBox23.Left + pictureBox23.Width / 2) - label44.Width / 2;
            panel38.Left = comboBox4.Left = comboBox3.Left = label50.Left = label51.Left = label43.Left = 45 * vw;
            panel38.Top = 10 * vh;
            label50.Top = panel38.Top - label50.Height;
            comboBox4.Top = 20 * vh;
            label51.Top = comboBox4.Top - label51.Height;
            comboBox3.Top = 30 * vh;
            label43.Top = comboBox2.Top - label43.Height;
        }
        
        private void SettingInsertPage()
        {
            oleDb.Open();
            string sql = "Select * from Student";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql,oleDb);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            int students = ds.Tables[0].Rows.Count + 1;
            DateTime dateTime = DateTime.Today;
            int year = dateTime.Year;
            textBox1.Text = students.ToString() + year.ToString();
            oleDb.Close();
            int vh = panel4.Height / 100;
            int vw = panel4.Width / 100;
            label32.Top = 3 * vh;
            centerwidth(label32);
            panel27.Left = panel26.Left = panel28.Left = panel29.Left = panel30.Left = panel31.Left = 7 * vw;
            panel27.Top = 10 * vh;
            label33.Left = label34.Left = label35.Left = label36.Left = label37.Left = label38.Left = 7 * vw;
            label33.Top = panel27.Top - label33.Height;
            panel28.Top = 25 * vh - getCenterHeight(panel28);
            label34.Top = panel28.Top - label34.Height;
            panel26.Top = 40 * vh - getCenterHeight(panel26);
            label35.Top = panel26.Top - label35.Height;
            panel29.Top = 55 * vh - getCenterHeight(panel29);
            label36.Top = panel29.Top - label36.Height;
            panel30.Top = 70 * vh - getCenterHeight(panel30);
            label37.Top = panel30.Top - label37.Height;
            panel31.Top = 88 * vh - getCenterHeight(panel31);
            label38.Top = panel31.Top - label38.Height;
            pictureBox22.Top = 12 * vh;
            pictureBox22.Left = InsertPageNewRecordButton.Left = 80 * vw;
            label41.Top = pictureBox22.Top + pictureBox22.Height;
            InsertPageNewRecordButton.Top = 50 * vh;
            label41.Left = (pictureBox22.Left + pictureBox22.Width / 2) - label41.Width / 2;
            panel32.Left = comboBox1.Left = comboBox2.Left = label39.Left = label40.Left = label42.Left = 45 * vw;
            panel32.Top = 10 * vh;
            label39.Top = panel32.Top - label39.Height;
            comboBox1.Top = 20 * vh;
            label40.Top = comboBox1.Top - label40.Height;
            comboBox2.Top = 30 * vh;
            label42.Top = comboBox2.Top - label42.Height;
        }

        private void SettingDashboardPage()
        {
            int viewheight = panel4.Height / 100;
            int viewwidth = panel4.Width / 100;
            label26.Top = 2 * viewheight - getCenterHeight(label26);
            centerwidth(label26);
            StudentContainerDashboardPanel.Width = 90 * viewwidth;
            StudentContainerDashboardPanel.Top = 5 * viewheight;
            centerwidth(StudentContainerDashboardPanel);
            panel14.Left = 5 * viewwidth;
            panel14.Width = 50 * viewwidth;
            centerheight(panel14);
            panel16.Left = 65 * viewwidth;
            centerheight(panel16);
            panel18.Top = panel20.Top = panel22.Top = 70 * viewheight - getCenterHeight(panel20);
            panel18.Left = 8 * viewwidth;
            panel20.Left = 42 * viewwidth;
            panel22.Left = 74 * viewwidth;
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

        #region SidePanel

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
            //label19.Text = sendername.Substring(4);
            SettingActivePanel(sendername.Substring(4));
        }

        private void SidePanel_DoubleClick(object sender, EventArgs e)
        {
            studentsidePanelHide();
            if(activepanel.Name.Substring(activepanel.Name.Length - 5) != "Panel")
            {
                studentSidePanelShow();
            }
            if (SidePanel.Width != SidePanelExpanded)
            {
                SidePanel.Width = SidePanelExpanded;
            }
            else
            {
            SidePanel.Width = SidePanelMin;
                studentsidePanelHide();
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
            if(panel.Name.Substring(4) != activepanel.Name && panel.Name.Substring(4, 3) != activepanel.Name.Substring(0, 3)) { panel.BackColor = Color.FromArgb(254, 171, 109); }
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
            if (panel.Name.Substring(4) != activepanel.Name && panel.Name.Substring(4, 3) != activepanel.Name.Substring(0, 3)) { panel.BackColor = Color.FromArgb(255, 215, 184); }
        }

        #endregion

        #region Login and RegisterPanel
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
                oleDb.Open();
                string sql = "Select * from Login where UserId= '" + RegistUsertext.Text + "'";
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, oleDb);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    errorshow("Username Already Exists", RegistUsertext);
                }
                oleDb.Close();
            }
        }

        private void RegistPassText_Focus(object sender, EventArgs e)
        {
            if (RegistUsertext.Text == "")
            {
                errorshow("Enter Username First", RegistUsertext);
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
                SideLoginPanel.Visible = false;
                SideRegistrationPanel.Visible = false;
                SideBusDataPanel.Visible = true;
                SideLibraryDataPanel.Visible = true;
                SideStudentDataPanel.Visible = true;
                SideDashboardPanel.Visible = true;
                SettingActivePanel("DashboardPanel");
                label21.Visible = false;
            }
            else
            {
                LoginErrorShow();
            }
            oleDb.Close();
        }

        private void Registbtn_Click(object sender, EventArgs e)
        {
            if(RegistUsertext.Text == ""  && RegistPassText.Text == "" && RegistConfirmText.Text == "" && label20.Visible == false)
            {
                errorshow("Please Fill Appropriate Fields", RegistUsertext);
            }
            else if(label20.Visible == true)
            {

            }
            else
            {
                oleDb.Open();
                string sql = "Insert into Login Values('" + RegistUsertext.Text + "','" + RegistPassText.Text + "'," + AdminBox.Checked + "," + false + ")";
                OleDbCommand dbCommand = new OleDbCommand(sql, oleDb);
                dbCommand.ExecuteNonQuery();
                SettingActivePanel("LoginPanel");
            }
        }

        private void RegistConfirmText_TextChanged(object sender, EventArgs e)
        {
            if(RegistConfirmText.Text == RegistPassText.Text)
            {
                label20.Visible = false;
            }
        }

        
        #endregion

        private void StudentSidePanel_Expand_DoubleClick(object sender, EventArgs e)
        {
            Panel panel = panelchecknull(sender);
            if (SidePanel.Width == SidePanelExpanded && panel.Name.Substring(4) == activepanel.Name)
            {
                if(expanded >= 2) {
                    expanded = 0;
                    SidePanel.Width = SidePanelMin;
                }
                else
                {
                    if (ExpStudentDataPanel.Visible == false)
                    {
                        studentSidePanelShow();
                        expanded = 1;
                    }
                    else
                    {
                        studentsidePanelHide();
                        expanded = 2;
                    }
                }

            }
            else
            {
                SidePanel.Width = SidePanelExpanded;
            }
        }

        private void DashboardPanel_SizeChanged(object sender, EventArgs e)
        {
            SettingDashboardPage();
        }

        private void StudentDataPanel_SizeChanged(object sender, EventArgs e)
        {
            SettingStudentDataPage();
        }

        private void StudentInsertData_SizeChanged(object sender, EventArgs e)
        {
            SettingInsertPage();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] arr = { "F", "P", "A", "S" };
            comboBox2.Items.Clear();
            comboBox2.Text = "";
            for(int i =1;i < 6; i++)
            {
                comboBox2.Items.Add(arr[comboBox1.SelectedIndex] + "0" + i);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            studentSidePanelShow();
            SettingActivePanel("StudentInsertData");

        }

        private void InsertPageNewRecordButton_Click(object sender, EventArgs e)
        {
            if (checkinput(sender))
            {
                inputerrorclean(sender);
                oleDb.Open();
                string classstring = comboBox1.GetItemText(comboBox1.SelectedItem);
                string secstring = comboBox2.GetItemText(comboBox2.SelectedItem);
                string sql = "Insert into Student values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox6.Text + "','" + textBox5.Text + "','" + textBox4.Text + "','" + classstring + "','" + textBox7.Text + "','" + secstring + "','" + null + "','" + null + "', @Picture)"; 
                OleDbCommand dbCommand = new OleDbCommand(sql,oleDb);
                using(MemoryStream stream = new MemoryStream())
                {
                    pictureBox22.Image.Save(stream, ImageFormat.Jpeg);
                    byte[] insertPicture = new byte[stream.Length];
                    stream.Position = 0;
                    stream.Read(insertPicture, 0, insertPicture.Length);
                    dbCommand.Parameters.AddWithValue("@Picture", insertPicture);
                }
                dbCommand.ExecuteNonQuery();
                label19.Text = "Record Inserted Successfully";
            }
            else
            {
                inputerrorshow(sender);
            }
        }

        private void ExpPanel_Mouse_Enter(object sender,EventArgs e)
        {
            Panel panel = panelchecknull(sender);

            if (panel.Name.Substring(4) != activepanel.Name) { panel.BackColor = Color.FromArgb(255, 215, 184); }
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "jpg";
            openFileDialog1.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png";
            openFileDialog1.FilterIndex = 1;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox22.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rollcount = 1;
            oleDb.Open();
            string sql = "Select * from Student";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql,oleDb);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if(ds.Tables[0].Rows[i]["Roll_no"].ToString().Substring(0,3) == comboBox2.GetItemText(comboBox2.SelectedItem))
                {
                    rollcount++;
                } 
            }
            oleDb.Close();
            DateTime dateTime = DateTime.Today;
            int year = dateTime.Year;
            string batchstring = comboBox2.GetItemText(comboBox2.SelectedItem);
            textBox7.Text = batchstring + year.ToString() + rollcount.ToString();
        }

        private void StudentUpdateData_SizeChanged(object sender, EventArgs e)
        {
            SettingUpdatePage();
        }

        private void textBox10_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                oleDb.Open();
                string sql = "Select * from Student where Student_id = '" + textBox10.Text + "'";
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, oleDb);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                if(ds.Tables[0].Rows.Count > 0)
                {
                    textBox10.ReadOnly = true;
                    textBox11.Text= ds.Tables[0].Rows[0]["Student_name"].ToString();
                    textBox12.Text = ds.Tables[0].Rows[0]["Father_name"].ToString();
                    textBox14.Text = ds.Tables[0].Rows[0]["Voter_id"].ToString();
                    textBox9.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                    textBox8.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    textBox13.Text = ds.Tables[0].Rows[0]["Roll_no"].ToString();
                    comboBox4.Text = ds.Tables[0].Rows[0]["Class"].ToString();
                    comboBox3.Text = ds.Tables[0].Rows[0]["Section"].ToString();
                    pictureBox23.Image = Image.FromStream(new MemoryStream(ds.Tables[0].Rows[0]["picnew"] as byte[]));
                }
                oleDb.Close();
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] arr = { "F", "P", "A", "S" };
            comboBox3.Items.Clear();
            comboBox3.Text = "";
            for (int i = 1; i < 6; i++)
            {
                comboBox3.Items.Add(arr[comboBox4.SelectedIndex] + "0" + i);
            }
        }

        private void textBox15_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                oleDb.Open();
                string sql = "Select * from Student where Student_id = '" + textBox15.Text + "'";
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, oleDb);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Panel picturepanel1 = new Panel();
                    picturepanel1.Dock = System.Windows.Forms.DockStyle.Left;
                    picturepanel1.Location = new System.Drawing.Point(0, 0);
                    picturepanel1.Name = "picturepanel1";
                    picturepanel1.Size = new System.Drawing.Size(80, 66);
                    picturepanel1.TabIndex = 0;
                    picturepanel1.Paint += title_btn_picture_set;


                    PictureBox pictureBox1img = new PictureBox();
                    pictureBox1img.Location = new System.Drawing.Point(9, 8);
                    pictureBox1img.Name = "pictureBox24";
                    pictureBox1img.Size = new System.Drawing.Size(55, 45);
                    pictureBox1img.TabIndex = 0;
                    pictureBox1img.TabStop = false;
                    pictureBox1img.Image = Image.FromStream(new MemoryStream(ds.Tables[0].Rows[0]["picnew"] as byte[]));
                    picturepanel1.Controls.Add(pictureBox1img);
                    pictureBox1img.SizeMode = PictureBoxSizeMode.StretchImage;
                    //panel44.Controls.Add(picturepanel1);

                    Label namelabel = new Label();
                    namelabel.Dock = System.Windows.Forms.DockStyle.Left;
                    namelabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    namelabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(69)))), ((int)(((byte)(2)))));
                    namelabel.Location = new System.Drawing.Point(0, 0);
                    namelabel.Name = "namelabel";
                    namelabel.Size = new System.Drawing.Size(126, 66);
                    namelabel.TabIndex = 76;
                    namelabel.Text = ds.Tables[0].Rows[0]["Student_name"].ToString();
                    namelabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    //panel44.Controls.Add(namelabel);


                    Label Fathernamelabel = new Label();
                    Fathernamelabel.Dock = System.Windows.Forms.DockStyle.Left;
                    Fathernamelabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    Fathernamelabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(69)))), ((int)(((byte)(2)))));
                    Fathernamelabel.Location = new System.Drawing.Point(0, 0);
                    Fathernamelabel.Name = "namelabel";
                    Fathernamelabel.Size = new System.Drawing.Size(126, 66);
                    Fathernamelabel.TabIndex = 76;
                    Fathernamelabel.Text = ds.Tables[0].Rows[0]["Father_name"].ToString();
                    Fathernamelabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                    Label AddressNameLabel = new Label();
                    AddressNameLabel.Dock = System.Windows.Forms.DockStyle.Left;
                    AddressNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    AddressNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(69)))), ((int)(((byte)(2)))));
                    AddressNameLabel.Location = new System.Drawing.Point(0, 0);
                    AddressNameLabel.Name = "namelabel";
                    AddressNameLabel.Size = new System.Drawing.Size(126, 66);
                    AddressNameLabel.TabIndex = 76;
                    AddressNameLabel.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    AddressNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                    Label PhoneNameLabel = new Label();
                    PhoneNameLabel.Dock = System.Windows.Forms.DockStyle.Left;
                    PhoneNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    PhoneNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(69)))), ((int)(((byte)(2)))));
                    PhoneNameLabel.Location = new System.Drawing.Point(0, 0);
                    PhoneNameLabel.Name = "namelabel";
                    PhoneNameLabel.Size = new System.Drawing.Size(126, 66);
                    PhoneNameLabel.TabIndex = 76;
                    PhoneNameLabel.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                    PhoneNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                    Label VoterIdLabel = new Label();
                    VoterIdLabel.Dock = System.Windows.Forms.DockStyle.Left;
                    VoterIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    VoterIdLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(69)))), ((int)(((byte)(2)))));
                    VoterIdLabel.Location = new System.Drawing.Point(0, 0);
                    VoterIdLabel.Name = "namelabel";
                    VoterIdLabel.Size = new System.Drawing.Size(126, 66);
                    VoterIdLabel.TabIndex = 76;
                    VoterIdLabel.Text = ds.Tables[0].Rows[0]["Voter_id"].ToString();
                    VoterIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                    Label ClassIdLabel = new Label();
                    ClassIdLabel.Dock = System.Windows.Forms.DockStyle.Left;
                    ClassIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    ClassIdLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(69)))), ((int)(((byte)(2)))));
                    ClassIdLabel.Location = new System.Drawing.Point(0, 0);
                    ClassIdLabel.Name = "namelabel";
                    ClassIdLabel.Size = new System.Drawing.Size(126, 66);
                    ClassIdLabel.TabIndex = 76;
                    ClassIdLabel.Text = ds.Tables[0].Rows[0]["Class"].ToString();
                    ClassIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                    Label RollIdLabel = new Label();
                    RollIdLabel.Dock = System.Windows.Forms.DockStyle.Left;
                    RollIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    RollIdLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(69)))), ((int)(((byte)(2)))));
                    RollIdLabel.Location = new System.Drawing.Point(0, 0);
                    RollIdLabel.Name = "namelabel";
                    RollIdLabel.Size = new System.Drawing.Size(126, 66);
                    RollIdLabel.TabIndex = 76;
                    RollIdLabel.Text = ds.Tables[0].Rows[0]["Roll_no"].ToString();
                    RollIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                    Label SectionIdLabel = new Label();
                    SectionIdLabel.Dock = System.Windows.Forms.DockStyle.Left;
                    SectionIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    SectionIdLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(69)))), ((int)(((byte)(2)))));
                    SectionIdLabel.Location = new System.Drawing.Point(0, 0);
                    SectionIdLabel.Name = "namelabel";
                    SectionIdLabel.Size = new System.Drawing.Size(126, 66);
                    SectionIdLabel.TabIndex = 76;
                    SectionIdLabel.Text = ds.Tables[0].Rows[0]["Section"].ToString();
                    SectionIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;


                    panel44.Controls.Add(SectionIdLabel);
                    panel44.Controls.Add(RollIdLabel);
                    panel44.Controls.Add(ClassIdLabel);
                    panel44.Controls.Add(VoterIdLabel);
                    panel44.Controls.Add(PhoneNameLabel);
                    panel44.Controls.Add(AddressNameLabel);
                    panel44.Controls.Add(Fathernamelabel);
                    panel44.Controls.Add(namelabel);
                    panel44.Controls.Add(picturepanel1);

                }   
                oleDb.Close();
            }
        }

        //private void UpdatePageButton_Click(object sender, EventArgs e)
        //{
        //    if (checkinput(sender))
        //    {
        //        inputerrorclean(sender);
        //        oleDb.Open();
        //        string classstring = comboBox1.GetItemText(comboBox1.SelectedItem);
        //        string secstring = comboBox2.GetItemText(comboBox2.SelectedItem);
        //        string sql = "Update Student Set Student_id = '" + textBox2.Text + "','" + textBox3.Text + "','" + textBox6.Text + "','" + textBox5.Text + "','" + textBox4.Text + "','" + classstring + "','" + textBox7.Text + "','" + secstring + "','" + null + "','" + null + "', @Picture)";
        //        OleDbCommand dbCommand = new OleDbCommand(sql, oleDb);
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            pictureBox22.Image.Save(stream, ImageFormat.Jpeg);
        //            byte[] insertPicture = new byte[stream.Length];
        //            stream.Position = 0;
        //            stream.Read(insertPicture, 0, insertPicture.Length);
        //            dbCommand.Parameters.AddWithValue("@Picture", insertPicture);
        //        }
        //        dbCommand.ExecuteNonQuery();
        //        label19.Text = "Record Inserted Successfully";
        //    }
        //    else
        //    {
        //        inputerrorshow(sender);
        //    }
        //}

        private void ExpPanel_Mouse_Leave(object sender, EventArgs e)
        {
            Panel panel = panelchecknull(sender);

            if (panel.Name.Substring(4) != activepanel.Name) { panel.BackColor = Color.FromArgb(254, 171, 109); }
        }

        private void Phone_Number(object sender, KeyEventArgs e)
        {

            TextBox text = sender as TextBox;
            try
            {
                char keypressed = (char)e.KeyCode;
                string handledstring = Char.ToString(keypressed);
                int pelican = Int32.Parse(handledstring);
                if (text.Parent.Parent.Controls.ContainsKey("ErrorLabel"))
                {
                    text.Parent.Parent.Controls.RemoveByKey("ErrorLabel");
                }
            }
            catch
            {
                
                if (text.Text.Length != 0 && e.KeyCode != Keys.Back)
                {
                    phone_error(sender);
                    text.Text = text.Text.Substring(0, text.Text.Length - 1);
                }
                label19.Text = "It can't be backspace";
                text.SelectionStart = text.Text.Length;
                text.SelectionLength = 0;
            }
        }
    }
}
