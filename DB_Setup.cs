using System;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NBA_DataBase{

    public class DB_Setup{
                
        public const string P_NAME = "NBA_DB";

        public bool Init_DB(string ip_addr, string user_id, string password){

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = ip_addr;
            builder.UserID = user_id;
            builder.Password = password;
            SqlConnection conn;
            try{
                conn = new SqlConnection(builder.ConnectionString);
                conn.Open();
            }
            catch(SqlException e){
                Logger log = new Logger(P_NAME, e.ToString());
                MessageBox.Show("Login Failed");
                return false;
            }
            catch(System.ArgumentException e){
                Logger log = new Logger(P_NAME, e.ToString());
                MessageBox.Show("Login Failed");
                return false;
            }

            SqlCommand command;
            SqlDataReader data_reader;
            command = new SqlCommand("IF DB_ID('NBA') IS NOT NULL SELECT 1 ELSE SELECT 0", conn);

            data_reader = command.ExecuteReader();
            data_reader.Read();
            // if database 'NBA' does not exist, create it
            if(data_reader.GetInt32(0) == 0){
                data_reader.Close();
                Console.WriteLine("DataBase does not exist, Creating...");
                command = new SqlCommand("CREATE DATABASE NBA", conn);
                command.ExecuteNonQuery();
            }
            data_reader.Close(); //needs to be closed whether database exists or not

            // switch context to newly created database
            conn.ChangeDatabase("NBA");

            command = new SqlCommand("IF OBJECT_ID('Player_Season') IS NOT NULL SELECT 1 ELSE SELECT 0", conn);
            data_reader = command.ExecuteReader(); 
            data_reader.Read();
            //if table 'Player_Season' does not exist, create it
            if(data_reader.GetInt32(0) == 0){
                data_reader.Close();
                Console.WriteLine("Creating table Player_Season...");
                string table_def = 
                "CREATE TABLE Player_SEASON (ID int PRIMARY KEY, Name varchar(32), Team varchar(4), Position varchar(4), " +
                "GamesPlayed INT, MinutesPG DECIMAL(4,1), FreeThrowAtt INT, FreeThrowPer DECIMAL(4,3), TwoPointAtt INT, " +
                "TwoPointPer DECIMAL(4,3), ThreePointAtt INT, ThreePointPer DECIMAL(4,3), PointsPG DECIMAL(5,2), " +
                "ReboundsPG DECIMAL(5,2), AssistsPG DECIMAL(5,2), StealsPG DECIMAL(5,2), BlocksPG DECIMAL(5,2), TurnOverPG DECIMAL(5,2))";
                command = new SqlCommand(table_def, conn);
                command.ExecuteNonQuery();
            }
            data_reader.Close();
            
            // Insert data from given csv file
            StreamReader sr = new StreamReader("Player_Season_Stats.csv");
            string line;
            int i = 1; // this is the value of the ID field, it is not in the csv file
            string errors = "";

            Console.WriteLine("Inserting stats into table Player_Season...");
            while((line = sr.ReadLine()) != null){
                // if the entry already exists, it is skipped
                string sql = "IF NOT EXISTS (SELECT * FROM Player_Season WHERE ID = " + i +
                ") BEGIN INSERT INTO Player_Season VALUES ("+ i.ToString() + ", " + line + ") END";

                command = new SqlCommand(sql, conn);
                try{ command.ExecuteNonQuery();}
                catch(SqlException e){
                    // the only exceptions should come from changing the csv file to have bad data
                    Logger log = new Logger(P_NAME, e.ToString());
                    errors += " " + i;
                }
                Console.Write("\r" + i.ToString() + " records inserted...");
                i++;
            }
            if(errors != ""){
                Console.WriteLine("Errors occurred on lines:" + errors + ". Check formatting");
            }

            Console.WriteLine("\nDone.");
            sr.Close();
            return true;

        }
    }
}