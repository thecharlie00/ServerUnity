using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

public class Client
{
    private TcpClient tcp;
    public string nick;
    public string password;
    public string email;
    public int idDatabase;
    private bool waitingPing;

    public Client(TcpClient tcp)    {
        this.tcp = tcp;
        this.nick = "Guest";
        this.waitingPing = false;
    }

    public TcpClient GetTcpClient()
    {
        return this.tcp;
    }

    public bool GetWaitingPing()
    {
        return this.waitingPing;
    }

    public void SetWaitingPing(bool waitingPing)
    {
        this.waitingPing = waitingPing;
    }

    public String GetNick()
    {
        return this.nick;
    }

    public string GetPassword()
    {
        return this.password;
    }

   


   public void SetNick(string nick)
   {
        this.nick = nick;
   }

    public void SetPassword(string password)
    {
        this.password = password;
    }

   

    public void SetIdDatabase(int idDatabase)
    {
        this.idDatabase = idDatabase;
    }
}