using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;

public class Network_Manager
{
    private TcpListener serverListener;
    private List<Client> clients;
    private Mutex clietListMutex;
    private int lastTimePing;
    private List<Client> disconnectClients;
    private Database_Manager databaseManager;
    bool logIn = false;

    public Network_Manager() {
        this.clients = new List<Client>();
        this.serverListener = new TcpListener(IPAddress.Any, 6543);
        this.clietListMutex = new Mutex();
        this.lastTimePing = Environment.TickCount;
        this.disconnectClients = new List<Client>();
        this.databaseManager = new Database_Manager();

    }

    public void Start_Network_Service()
    {
        try
        {
            this.serverListener.Start();
            StartListening();
            

        }catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

   

    private void StartListening()
    {
        Console.WriteLine("Esperando nueva conexion");
        this.serverListener.BeginAcceptTcpClient(AcceptConnection, this.serverListener);
    }

    private void AcceptConnection(IAsyncResult ar)
    {
        Console.WriteLine("Recibo una conexion");
        
        TcpListener listener = (TcpListener)ar.AsyncState;
        this.clietListMutex.WaitOne();
        this.clients.Add(new Client(listener.EndAcceptTcpClient(ar)));
        this.clietListMutex.ReleaseMutex();
        StartListening();


    }

    public void CheckMessage()
    {
        clietListMutex.WaitOne();
        foreach(Client client in this.clients) { 
            NetworkStream netStream = client.GetTcpClient().GetStream();
            if (netStream.DataAvailable)
            {
                StreamReader reader = new StreamReader(netStream, true);
                string data = reader.ReadToEnd();
                if(data != null)
                {
                    ManageData(client, data);
                }
            }
        }
        clietListMutex.ReleaseMutex();




        
    }
    private void ManageData(Client client, string data)
    {
        
        string[] parameters = data.Split('/');

        switch (parameters[0])
        {           
            case "Login":
                client.SetNick(parameters[1]);
                client.SetPassword(parameters[2]);

                string idUser = databaseManager.GetUserIDByNick(parameters[1]);
                client.SetIdDatabase(int.Parse(idUser));

                Login(client);

                break;
            case "Register":
                Register(parameters[1], parameters[2]);
                break;
            case "1":     
                ReceivePing(client);
                break;
            
        }
    }
    bool Login(Client client)
    {
        
        if (databaseManager.LogInUser(client.GetNick(), client.GetPassword(), ref client))
        {

            return true;
        }
        else
        {
            
            SendInfoToUnityClient(client, 3);
            return false;
        }
    }

    public void Register(string nick, string password)
    {
        databaseManager.Register(nick, password);
    }



    private void SendPing(Client client)
    {
        try
        {
            StreamWriter writer = new StreamWriter(client.GetTcpClient().GetStream());

            writer.WriteLine("Ping");
            writer.Flush();
            writer.Close();

            client.SetWaitingPing(true);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);   
        }
    }

    public void CheckConnection()
    {
        if(Environment.TickCount - this.lastTimePing > 5000)
        {
            clietListMutex.WaitOne();
            foreach(Client client in this.clients)
            {
                if(client.GetWaitingPing() == true)
                {
                    disconnectClients.Add(client);
                }
                else
                {
                    SendPing(client);
                }
            }
            this.lastTimePing = Environment.TickCount;
            clietListMutex.ReleaseMutex();
        }
    }

    private void ReceivePing(Client client)
    {
        client.SetWaitingPing(false);
    }

    public void DisconnectClient()
    {
        clietListMutex.WaitOne();
        foreach(Client client in this.disconnectClients)
        {
            Console.WriteLine("Desconectando usuarios");
            client.GetTcpClient().Close();
            this.clients.Remove(client);
        }

        this.disconnectClients.Clear();
        clietListMutex.ReleaseMutex();
    }
    private void SendInfoToUnityClient(Client client, int code)
    {
        try
        {
            
            StreamWriter writer = new StreamWriter(client.GetTcpClient().GetStream());


            if (code == 2)
            {
                string clientString = code.ToString() + "/" + client.GetNick() + "/" + databaseManager.GetUserIDByNick(client.GetNick());
                writer.WriteLine(clientString);
            }
            else if (code == 3)
            {
                writer.WriteLine(code.ToString());
            }
     

            
            writer.Flush();

             
            client.SetWaitingPing(true);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message + " with client" + client.GetNick());
        }
    }

    
}
