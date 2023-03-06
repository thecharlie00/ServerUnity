using MySql.Data.MySqlClient;

public class Database_Manager
{
    const string databaseName = "patojuegoenti";
    const int port = 3306;
    const string uID = "charlie00";
    const string password = "Apolonio26";
    string connectionString = $"Server=db4free.net;Port={3306};database={databaseName};uID={uID};password={password};SSL Mode=None;connect timeout = 3600;";
    MySqlConnection connection;
    public void DatabaseManager()
    {
        ShowAllPlayer();
       
    }

    public void StartsService()
    {
        connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void ShowAllPlayer() {
        StartsService();
        MySqlCommand command = connection.CreateCommand();
        string query = $"Select * from Users;";

        command.CommandText = query;
        MySqlDataReader reader;
        try
        {
            reader = command.ExecuteReader(); ;
            while (reader.Read())
            {
                Console.WriteLine(reader["nick"].ToString() + "" + reader["password"].ToString() + " " );

            }
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        connection.Close();
    
    }

    
    public void Register(string nick, string password)
    {
        StartsService();
        MySqlCommand command = connection.CreateCommand();

        string query = $"Insert Into Users(nick, password)Values('{nick}','{password}');";
        command.CommandText = query;
        try
        {
            command.ExecuteNonQuery();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        connection.Close();
        ShowAllPlayer();
    }
    public bool LogInUser(string nick, string password, ref Client client)
    {
        StartsService();
        MySqlCommand command = connection.CreateCommand();
        string query = $"Select * from Users where nick='{nick}' and password='{password}';";
        command.CommandText = query;
        MySqlDataReader reader;
        try
        {
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                client.SetNick(reader["nick"].ToString());
                client.SetPassword(reader["password"].ToString());
                Console.WriteLine("Logueado");
                return reader["nick"].ToString() == nick && reader["password"].ToString() == password;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        connection.Close();
        return false;
    }

    public string GetUserIDByNick(string nick)
    {
        StartsService();
        MySqlCommand command = connection.CreateCommand();
        string query = $"Select id_user from Users where nick='{nick}';";
        command.CommandText = query;
        MySqlDataReader reader;
        string id = "";
        try
        {
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetString("id_user");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        connection.Close();
        return id;
    }
    
}


