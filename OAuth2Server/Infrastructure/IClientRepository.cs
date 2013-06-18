using System;
using System.Collections.Generic;

namespace OAuth2Server.Infrastructure
{
    public interface IClientRepository
    {
        void Save(Client client);
        Client Get(string id);
    }

    public class InMemoryClientRepository : IClientRepository
    {
        readonly IDictionary<string, Client> _clients = new Dictionary<string, Client>();

        public void Save(Client client)
        {
            if (_clients.ContainsKey(client.Id))
                _clients[client.Id] = client;
            else
                _clients.Add(client.Id, client);
        }

        public Client Get(string id)
        {
            return _clients.ContainsKey(id) ? _clients[id] : null;
        }
    }

    public class Client
    {
        public string Id { get; set; }
        public string SecretKey { get; set; }

        public Uri CallbackUrl { get; set; }
    }
}
