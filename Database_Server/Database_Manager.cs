using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

class Database_Manager
{
	static void Main(string[] args)
	{
		//Parametros de conexion
		const string connectionString = "Server=db4free.net;Port=3306;database=patojuegoenti;Uid=charlie00;password=Apolonio26;SSL Mode=None;connect timeout=3600;default command timeout=3600;";

		//Instancio clase Mysql
		MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

		try
		{
			mySqlConnection.Open();

		}catch(Exception ex)
		{
			Console.WriteLine(ex.ToString());	
		}

		//SelectExample(mySqlConnection);
		//InsertExample(mySqlConnection);
		DeleteExample(mySqlConnection);
		void SelectExample(MySqlConnection mySqlConnection)
		{
			MySqlDataReader reader;

			MySqlCommand command = mySqlConnection.CreateCommand();
			command.CommandText = "Select * from Race";
			try
			{
				reader = command.ExecuteReader();
				while(reader.Read())
				{
					Console.WriteLine(reader["id_user"].ToString());
				}
			}catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		void InsertExample(MySqlConnection mySqlConnection)
		{
			MySqlCommand command = mySqlConnection.CreateCommand();
			command.CommandText = "INSERT INTO Race VALUES (1,'Pato', 100,120,250,300);";
			try
			{
				command.ExecuteNonQuery();
			}catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		void DeleteExample(MySqlConnection mySqlConnection)
		{
			MySqlCommand command = mySqlConnection.CreateCommand();
			command.CommandText = "DELETE from Users WHERE User.id_user = 1;";

			try
			{
				command.ExecuteNonQuery();
			}catch(Exception ex)
			{
				Console.WriteLine(ex.Message);	
			}

		}
		
		mySqlConnection.Close();

	}
}
