using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;

namespace CookBook.Prototyping
{
    class Program
    {
        static void Main(string[] args)
        {
            var id = new Guid("9BFAF0B9EE9E4CC597730900DAC95F18");

            using (IDocumentStore store = new DocumentStore
            {
                Urls = new[]                        // URL to the Server,
                {                                   // or list of URLs 
                    "http://localhost:8080"  // to all Cluster Servers (Nodes)
                },
                Database = "cookbook",             // Default database that DocumentStore will interact with
                Conventions = { }                   // DocumentStore customizations
            })
            {
                store.Initialize();                 // Each DocumentStore needs to be initialized before use.
                                                    // This process establishes the connection with the Server
                                                    // and downloads various configurations
                                                    // e.g. cluster topology or client configuration

                SaveThing(store, id);

            }

        }

        private static void SaveThing(IDocumentStore store, Guid guid)
        {
            using (IDocumentSession session = store.OpenSession())  // Open a session for a default 'Database'
            {
                var thing = new Thing
                {
                    Id = guid.ToString(),
                    Count = 123,
                    Name = "Test thing - " + DateTime.UtcNow.ToString()
                };

                session.Store(thing);                             // Assign an 'Id' and collection (Products)
                                                                    // and start tracking an entity

                session.SaveChanges();                              // Send to the Server
                                                                    // one request processed in one transaction
            }
        }

        class Thing
        {
            public string Id { get; set; }
            public int Count { get; set; }
            public string Name { get; set; }
        }
    }
}
