using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace CookBook.Prototyping
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = @"C:\Dev\CookBook\Data\FoodPics\1 (5).jpg";
            using var image = Image.Load(fileName);

            double maxWidth = 480;
            double maxHeight = 480;
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Max(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var x = (int)Math.Max(0, newWidth - maxWidth) / 2;
            var y = (int)Math.Max(0, newHeight - newHeight) / 2;
            var size = Math.Min(newWidth, newHeight);

            image.Mutate(img => img.
                Resize(newWidth, newHeight)
                .Crop(new Rectangle(x, y, size, size)));

            image.Save(string.Join(".test.", fileName.Split('.')));
        }

        private static void ScaleNetFramework(string fileName)
        {
            //using var fileStream = File.OpenRead(fileName);
            //var image = Image.FromStream(fileStream);
            //var scaled = ScaleImage(image, 480, 480);

            //using var saveStream = File.OpenWrite(string.Join(".test.", fileName.Split('.')));
            //scaled.Save(saveStream, ImageFormat.Jpeg);
        }

        //public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        //{
        //    var ratioX = (double)maxWidth / image.Width;
        //    var ratioY = (double)maxHeight / image.Height;
        //    var ratio = Math.Min(ratioX, ratioY);

        //    var newWidth = (int)(image.Width * ratio);
        //    var newHeight = (int)(image.Height * ratio);

        //    var newImage = new Bitmap(newWidth, newHeight);

        //    using (var graphics = Graphics.FromImage(newImage))
        //        graphics.DrawImage(image, 0, 0, newWidth, newHeight);

        //    return newImage;
        //}

        private static void RavenStuff()
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
