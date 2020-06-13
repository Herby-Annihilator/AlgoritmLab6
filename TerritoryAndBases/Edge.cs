using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerritoryAndBases
{
    public class Edge
    {
        public Vertex FirstVertex { get; set; }
        public Vertex SecondVertex { get; set; }

        public int Weight { get; set; }
        public Edge(Vertex first, Vertex second)
        {
            FirstVertex = first;
            SecondVertex = second;
            Weight = (FirstVertex.ControlDistance + SecondVertex.ControlDistance) / 2;
        }
    }
}
