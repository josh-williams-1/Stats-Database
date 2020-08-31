using System;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NBA_DataBase{
    
    public partial class LoginForm : Form{
        public const int WIN_WIDTH = 860;
        public const int WIN_HEIGHT = 600;
        public const int MAX_NAME_LENGTH = 36;
        private string ip_addr;
        private string user_name;
        private string password;
        private Button button0 = new Button();
        private TextBox textbox0 = new TextBox();
        private DataGridView data_grid = new DataGridView();
        private BindingSource binding = new BindingSource();
        private DataTable d_table = new DataTable();
        private Panel search_bar = new Panel();

        public void MainForm(string ip_addr_in, string user_name_in, string password_in){

            ip_addr = ip_addr_in;
            user_name = user_name_in;
            password = password_in;
     
            button0.Location = new Point(600, 10);
            button0.Name = "button0";
			button0.Size = new Size(50, 25);
			button0.TabIndex = 2;
			button0.Text = "Submit";
			button0.TextAlign = ContentAlignment.MiddleCenter;
            button0.Click += new EventHandler(this.OnButtonClick);

            textbox0.Location = new Point(10, 10);
            textbox0.Name = "textbox0";
            textbox0.Size = new Size(240, 30);
            textbox0.TabIndex = 1;
            textbox0.Text = "Enter Player Name";
            textbox0.KeyDown += OnEnterPress;

            data_grid.Name = "data_grid";
            data_grid.ReadOnly = true;
            data_grid.AllowUserToResizeColumns = false;
            data_grid.AllowUserToResizeRows = false;
            data_grid.RowHeadersVisible = false;
            data_grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            data_grid.MultiSelect = false;
            data_grid.Dock = DockStyle.Fill;
            data_grid.AllowUserToAddRows = false;
            //initialize datagrid with all player stats visible
            binding.DataSource = GetQueryResults("");
            data_grid.DataSource = binding;

            search_bar.Controls.Add(button0);
            search_bar.Controls.Add(textbox0);
            search_bar.Height = 50;
            search_bar.Dock = DockStyle.Bottom;

            Text = "NBA Database";
            ClientSize = new Size(WIN_WIDTH, WIN_HEIGHT);
			Controls.AddRange(new Control[]{data_grid, search_bar});

            //formatting the DataGridView
            //which must be done *after* being added to Controls

            //abbreviated column names
            string[] column_names = new string[] {"ID", "Player Name", "Team", "Pos", "GP", "MPG", 
                "FTA", "FT%", "2PA", "2P%", "3PA", "3P%", "PPG", "RPG", "APG", "SPG", "BPG", "TOs"};
            int index = 0;
            foreach(DataGridViewColumn col in data_grid.Columns){
                col.Name = column_names[index++];
                col.Width = 40;
            }
            data_grid.Columns[1].Width = 140; // the Name column needs to be wider than the rest
        }

        private void OnButtonClick(object sender, EventArgs e){

            binding.DataSource = GetQueryResults(Sanitize(textbox0.Text));
        }

        // Pressing Enter in the textbox does the same thing as pressing the button
        private void OnEnterPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OnButtonClick(this, new EventArgs());
            }
        }

        private DataTable GetQueryResults(string player_name){
            
            //clear table before repopulating
             d_table.Clear();

            //string connection_string;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = ip_addr;
            builder.InitialCatalog = "NBA";
            builder.UserID = user_name;
            builder.Password = password;
            SqlConnection conn = new SqlConnection(builder.ConnectionString);
            conn.Open();

            string sql_query = "SELECT * FROM Player_Season WHERE Name LIKE '%' + @NAME + '%'";
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            command = new SqlCommand(sql_query, conn);
            command.Parameters.AddWithValue("@NAME", player_name);
            adapter.SelectCommand = command;

            adapter.Fill(d_table);

            return d_table;
        }

        // removes all characters that are not letters or spaces
        private string Sanitize(string input){

            string ret_string = "";
            foreach(char c in input){
                if(Char.IsLetter(c) || c == ' '){
                    ret_string += c;
                }
            }

            return ret_string;
        }
    }
}
