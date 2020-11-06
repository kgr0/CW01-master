using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW01
{
    public class Location
    {
        public string name;
        public List<NonPlayerCharacter> npc_list;
        public bool IsUnlocked;

        public Location(string name, bool isUnlocked)
        {
            this.name = name;
            npc_list = new List<NonPlayerCharacter>();
            this.IsUnlocked = isUnlocked;
        } 

        public void Add_npc(NonPlayerCharacter npc)
        {
            npc_list.Add(npc);
        }
    }
}
