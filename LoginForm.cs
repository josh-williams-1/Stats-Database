using System;
using System.Drawing;
using System.Windows.Forms;

namespace NBA_DataBase{   
    
    public partial class LoginForm : Form{

        private Button LoginButton = new Button();
        private TextBox LoginBox0 = new TextBox();
        private TextBox LoginBox1 = new TextBox();
        private TextBox LoginBox2 = new TextBox();
        private Label Label0 = new Label();
        private Label Label1 = new Label();
        private Label Label2 = new Label();
        

        // values in textboxes are used to setup database
        private void LoginSubmit(object sender, EventArgs e){
            
            ip_addr = LoginBox0.Text;
            user_name = LoginBox1.Text;
            password = LoginBox2.Text;

            //if the login is successful, this window will be cleared and run the main form
            DB_Setup s = new DB_Setup();
            if(s.Init_DB(ip_addr, user_name, password)){
                Controls.Clear();
                MainForm(ip_addr, user_name, password);
            }
            else{
                LoginBox2.Clear();
            }
        }

        // pressing enter in any of the boxes submits the data
        // just like pressing the submit button
        private void LoginEnterPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginSubmit(this, new EventArgs());
            }
        }
        public LoginForm(){

            Label0.Location = new Point(20, 30);
            Label0.Size = new Size(75, 30);
            Label0.Text = "IP Address:";

            Label1.Location = new Point(20, 80);
            Label1.Size = new Size(75, 30);
            Label1.Text = "User Name:";

            Label2.Location = new Point(20, 130);
            Label2.Size = new Size(75, 30);
            Label2.Text = "Password:";

            LoginBox0.Location = new Point(100, 30);
            LoginBox0.Name = "LoginBox0";
            LoginBox0.Size = new Size(120, 30);
            LoginBox0.TabIndex = 0;
            LoginBox0.Text = "localhost";
            LoginBox0.KeyDown += LoginEnterPress;

            LoginBox1.Location = new Point(100, 80);
            LoginBox1.Name = "LoginBox1";
            LoginBox1.Size = new Size(120, 30);
            LoginBox1.TabIndex = 1;
            LoginBox1.Text = "SA";
            LoginBox1.KeyDown += LoginEnterPress;

            LoginBox2.Location = new Point(100, 130);
            LoginBox2.Name = "LoginBox2";
            LoginBox2.Size = new Size(120, 30);
            LoginBox2.TabIndex = 2;
            LoginBox2.PasswordChar = '*';
            LoginBox2.MaxLength = 128;
            LoginBox2.KeyDown += LoginEnterPress;

            LoginButton.Location = new Point(100, 160);
            LoginButton.Name = "LoginButton";
			LoginButton.Size = new Size(50, 25);
			LoginButton.TabIndex = 3;
			LoginButton.Text = "Submit";
			LoginButton.TextAlign = ContentAlignment.MiddleCenter;
            LoginButton.Click += LoginSubmit;
        
            Text = "NBA DataBase Login";
            ClientSize = new Size(300, 300);
			Controls.AddRange(new Control[]{LoginBox0, LoginBox1, LoginBox2, LoginButton,
                Label0, Label1, Label2});
        }

        static public void Main(){
            
            Application.Run(new LoginForm());
        }
    }
}