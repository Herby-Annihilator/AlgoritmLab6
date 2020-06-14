using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorGraph;
using LibraryForAlgLab;
using System.IO;

namespace TerritoryAndBases
{
    [Serializable]
    public class Graph
    {
        public Graph()
        {
            Vertices = new List<Vertex>();
            Edges = new List<Edge>();
        }
        public List<Vertex> Vertices { get; set; }
        public List<Edge> Edges { get; set; }
        /// <summary>
        /// Соединяет вершины с указанным индексом (свойство Index)
        /// </summary>
        /// <param name="vertexIndex1">Vertices[i].Index</param>
        /// <param name="vertexIndex2">Vertices[i].Index</param>
        /// <returns></returns>
        public bool ConnectVerties(int vertexIndex1, int vertexIndex2)
        {
            Vertex first = null, second = null;
            bool firstConnected = false, secondConnected = false;
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i].Index == vertexIndex1)
                {
                    first = Vertices[i];
                    firstConnected = true;
                }
                else if (Vertices[i].Index == vertexIndex2)
                {
                    second = Vertices[i];
                    secondConnected = true;
                }
                if (firstConnected && secondConnected)
                {
                    break;
                }
            }
            if (first == null || second == null)
            {
                return false;
            }
            else
            {
                Edges.Add(new Edge(first, second));
                first.Edges.Add(Edges[Edges.Count - 1]);
                second.Edges.Add(Edges[Edges.Count - 1]);
                return true;
            }
        }
        /// <summary>
        /// Разъединяет вершины по нужному мне признаку
        /// </summary>
        /// <returns></returns>
        public bool DisconnectVerties()
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                if (Edges[i].FirstVertex.ControlDistance < Edges[i].Weight && Edges[i].SecondVertex.ControlDistance < Edges[i].Weight)
                {
                    if (!Edges[i].FirstVertex.RemoveEdge(Edges[i]))
                    {
                        return false;
                    }
                    if (!Edges[i].SecondVertex.RemoveEdge(Edges[i]))
                    {
                        return false;
                    }
                    Edges.RemoveAt(i);
                    i--;
                }
            }
            return true;
        }
        /// <summary>
        /// Получает те регионы, в которых нужно установить базы. Сначала нужно подготовить граф,
        /// то есть просто соединить все соседние регионы.
        /// </summary>
        /// <returns></returns>
        public List<int> GetRegionsToBuiltBases()
        {
            if (!DisconnectVerties())
            {
                return null;
            }
            List<int> regions = new List<int>();
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i].Edges.Count == 0)
                {
                    regions.Add(Vertices[i].Index);
                }
            }
            GraphHelper.SaveMatrixToFile("output.dat", CreateAdjacencyMatrix());

            ColorGraph.ColorGraph colorGraph = new ColorGraph.ColorGraph(CreateAdjacencyMatrix(), null);
            List<int> list;
            //
            // Получаем все независимые множества
            //
            for (int i = 0; i < colorGraph.VertexCount; i++)
            {
                list = colorGraph.GetNonAdjacentPeaks(i);
                if (list != null)
                {
                    list = colorGraph.CorrectNonAdjacentPeaks(list);
                    colorGraph.IndependentSets.Add(list);
                }
            }
            //
            // Находим максимальное
            //
            int maxSetIndex = 0;
            for (int i = 0; i < colorGraph.IndependentSets.Count; i++)
            {
                if (colorGraph.IndependentSets[i].Count > colorGraph.IndependentSets[maxSetIndex].Count)
                {
                    maxSetIndex = i;
                }
            }
            //
            // Вершины этого множества нужно исключить из графа, а остальные запомнить
            //
            bool needToAdd;
            for (int i = 0; i < Vertices.Count; i++)
            {
                needToAdd = true;
                for (int j = 0; j < colorGraph.IndependentSets[maxSetIndex].Count; j++)
                {
                    if (Vertices[i].Index == colorGraph.IndependentSets[maxSetIndex][j])
                    {
                        needToAdd = false;
                        break;
                    }
                }
                if (needToAdd)
                {
                    regions.Add(Vertices[i].Index);
                }
            }
            return regions;            
        }

        private int[][] CreateAdjacencyMatrix()
        {
            int[][] matrix = new int[Vertices.Count][];
            for (int i = 0; i < Vertices.Count; i++)
            {
                matrix[i] = new int[Vertices.Count];
            }
            for (int i = 0; i < Edges.Count; i++)
            {
                matrix[Edges[i].FirstVertex.Index][Edges[i].SecondVertex.Index] = Edges[i].Weight;
                matrix[Edges[i].SecondVertex.Index][Edges[i].FirstVertex.Index] = Edges[i].Weight;
            }
            return matrix;
        }
        public void UpdateEdgesData()
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                Edges[i].Weight = (Edges[i].FirstVertex.TerritorySize + Edges[i].SecondVertex.TerritorySize) / 2;
            }
        }
    }
}
