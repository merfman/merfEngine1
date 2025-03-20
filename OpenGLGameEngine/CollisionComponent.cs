using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine;
internal class CollisionComponent : Component
{
    private int[][] collisionPoints {  get; set; }
    public CollisionComponent(GameObject parent) : base(parent)
    {
        
    }
}
