using System.Collections.Generic;

namespace iTunesSmartParser
{
    public class Playlist
    {
        public string Name { get; set; }

        public string LineString
        {
            get
            {
                return (IsSmart ? "⚙ " : "🖹 ") + Name;
            }
        }

        public int ID { get; set; }

        public bool IsSmart { get; set; }

        public string Filter { get; set; }

        public List<int> Items { get; set; }

        public Playlist()
        {

        }
    }
}
