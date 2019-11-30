using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterPlayground
{
    abstract public class Enemy : Entity
    {
        public Attack Weapon { get; protected set; }
    }
}
