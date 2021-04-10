using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Domain
{
    public class CheckpointDocument
    {
        public Position Position { get; set; }
    }
}
