﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChatInterface
{
    public class Client
    {
        public  ClientConnection connection { get; set; }
        private string Login, Password;
        public ClientConnection GetConnection(string login , string password)
        {
            try
            {
                connection = ClientConnection.GetInstance(login, password);
            }
            catch (Exception e)
            {
                return null;
            }
            return connection;
        }
    }
}
