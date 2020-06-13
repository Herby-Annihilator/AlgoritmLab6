using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerritoryAndBases
{
    [Serializable]
    public class Vertex
    {
        public int Index { get; set; }

        public int TerritorySize { get; set; }

        public int ControlDistance { get; set; }

        public Vertex(int index, int territorySize, int controlDistance)
        {
            Index = index;
            TerritorySize = territorySize;
            ControlDistance = controlDistance;
            Edges = new List<Edge>();
        }

        public List<Edge> Edges { get; set; }

        public bool RemoveEdge(Edge edge)
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                if (Edges[i].Equals(edge))
                {
                    Edges.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }
}
