using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MysqlClient;

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
		mySqlConnection.Close();
	}
}
