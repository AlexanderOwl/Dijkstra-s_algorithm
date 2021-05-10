using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra_s_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new Graph();

            //додавання вершин
            g.AddVertex("s");
            g.AddVertex("a");
            g.AddVertex("b");
            g.AddVertex("c");
            g.AddVertex("d");
            g.AddVertex("t");
            

            //додавання ребер
            g.AddEdge("s", "a", 4);
            g.AddEdge("s", "b", 7);
            g.AddEdge("s", "c", 3);
            g.AddEdge("a", "d", 2);
            g.AddEdge("a", "b", 3);
            g.AddEdge("b", "t", 2);
            g.AddEdge("d", "t", 2);
            g.AddEdge("c", "d", 3);
          

            var dijkstra = new Dijkstra(g);
            var path = dijkstra.FindShortestPath("s", "t");
            Console.WriteLine(path);
            Console.ReadLine();
        }
    }

    /// <summary>
    /// Ребро графа
    /// </summary>
    public class GraphEdge
    {
        /// <summary>
        /// Звязана вершина
        /// </summary>
        public GraphVertex ConnectedVertex { get; }

        /// <summary>
        /// Вага ребра
        /// </summary>
        public int EdgeWeight { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectedVertex">Звязана вершина</param>
        /// <param name="weight">Вага ребра</param>
        public GraphEdge(GraphVertex connectedVertex, int weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }
    }

    /// <summary>
    /// Вершина графа
    /// </summary>
    public class GraphVertex
    {
        /// <summary>
        /// Назва вершини
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Список ребр
        /// </summary>
        public List<GraphEdge> Edges { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="vertexName">Назва вершини</param>
        public GraphVertex(string vertexName)
        {
            Name = vertexName;
            Edges = new List<GraphEdge>();
        }

        /// <summary>
        /// Додати ребро
        /// </summary>
        /// <param name="newEdge">Ребро</param>
        public void AddEdge(GraphEdge newEdge)
        {
            Edges.Add(newEdge);
        }

        /// <summary>
        /// Додати ребро
        /// </summary>
        /// <param name="vertex">Вершина</param>
        /// <param name="edgeWeight">Вага</param>
        public void AddEdge(GraphVertex vertex, int edgeWeight)
        {
            AddEdge(new GraphEdge(vertex, edgeWeight));
        }

        /// <summary>
        /// Перетворення в рядок
        /// </summary>
        /// <returns>Назва вершини</returns>
        public override string ToString() => Name;
    }

    /// <summary>
    /// Граф
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Список вершин графа
        /// </summary>
        public List<GraphVertex> Vertices { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Graph()
        {
            Vertices = new List<GraphVertex>();
        }

        /// <summary>
        /// Додавання вершини
        /// </summary>
        /// <param name="vertexName">Ім'я вершины</param>
        public void AddVertex(string vertexName)
        {
            Vertices.Add(new GraphVertex(vertexName));
        }

        /// <summary>
        /// Пошук вершини
        /// </summary>
        /// <param name="vertexName">Назва вершини</param>
        /// <returns>Знайдена вершина</returns>
        public GraphVertex FindVertex(string vertexName)
        {
            foreach (var v in Vertices)
            {
                if (v.Name.Equals(vertexName))
                {
                    return v;
                }
            }

            return null;
        }

        /// <summary>
        /// Додавання ребра
        /// </summary>
        /// <param name="firstName">Ім'я першої вершини</param>
        /// <param name="secondName">Ім'я другої вершини</param>
        /// <param name="weight">Вага ребра з'єднуючого вершини</param>
        public void AddEdge(string firstName, string secondName, int weight)
        {
            var v1 = FindVertex(firstName);
            var v2 = FindVertex(secondName);
            if (v2 != null && v1 != null)
            {
                v1.AddEdge(v2, weight);
                v2.AddEdge(v1, weight);
            }
        }
    }

    /// <summary>
    /// Інформація про вершину
    /// </summary>
    public class GraphVertexInfo
    {
        /// <summary>
        /// Вершина
        /// </summary>
        public GraphVertex Vertex { get; set; }

        /// <summary>
        /// Не відвідана вершина
        /// </summary>
        public bool IsUnvisited { get; set; }

        /// <summary>
        /// Сума ваги ребер
        /// </summary>
        public int EdgesWeightSum { get; set; }

        /// <summary>
        /// Попередня вершина
        /// </summary>
        public GraphVertex PreviousVertex { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="vertex">Вершина</param>
        public GraphVertexInfo(GraphVertex vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = int.MaxValue;
            PreviousVertex = null;
        }
    }

    /// <summary>
    /// Алгоритм Дейкстри
    /// </summary>
    public class Dijkstra
    {
        Graph graph;

        List<GraphVertexInfo> infos;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="graph">Граф</param>
        public Dijkstra(Graph graph)
        {
            this.graph = graph;
        }

        /// <summary>
        /// Ініціалізація інформація
        /// </summary>
        void InitInfo()
        {
            infos = new List<GraphVertexInfo>();
            foreach (var v in graph.Vertices)
            {
                infos.Add(new GraphVertexInfo(v));
            }
        }

        /// <summary>
        /// Отримання інформації про вершину
        /// </summary>
        /// <param name="v">Вершина</param>
        /// <returns>Інформаія про вершину</returns>
        GraphVertexInfo GetVertexInfo(GraphVertex v)
        {
            foreach (var i in infos)
            {
                if (i.Vertex.Equals(v))
                {
                    return i;
                }
            }

            return null;
        }

        /// <summary>
        /// Пошук невідвіданої вершини з мінімальним значенням суми
        /// </summary>
        /// <returns>Інформація про вершину</returns>
        public GraphVertexInfo FindUnvisitedVertexWithMinSum()
        {
            var minValue = int.MaxValue;
            GraphVertexInfo minVertexInfo = null;
            foreach (var i in infos)
            {
                if (i.IsUnvisited && i.EdgesWeightSum < minValue)
                {
                    minVertexInfo = i;
                    minValue = i.EdgesWeightSum;
                }
            }
            return minVertexInfo;
        }

        /// <summary>
        /// Пошук найкоротшого шляху за назвами вершин
        /// </summary>
        /// <param name="startName">Назва початкової вершини</param>
        /// <param name="finishName">Назва кінцевої вершини</param>
        /// <returns>Найкоротший шлях</returns>
        public string FindShortestPath(string startName, string finishName)
        {
            return FindShortestPath(graph.FindVertex(startName), graph.FindVertex(finishName));
        }

        /// <summary>
        /// Пошук найкоротшого шляху по вершинах
        /// </summary>
        /// <param name="startVertex">Стартова вершина</param>
        /// <param name="finishVertex">Фінішна вершина</param>
        /// <returns>Кратчайший путь</returns>
        public string FindShortestPath(GraphVertex startVertex, GraphVertex finishVertex)
        {
            InitInfo();
            var first = GetVertexInfo(startVertex);
            first.EdgesWeightSum = 0;
            while (true)
            {
                var current = FindUnvisitedVertexWithMinSum();
                if (current == null)
                {
                    break;
                }

                SetSumToNextVertex(current);
            }

            return GetPath(startVertex, finishVertex);
        }

        /// <summary>
        /// Обчислення суми ваг ребер для наступної вершини
        /// </summary>
        /// <param name="info">Інформація про поточну вершину</param>
        void SetSumToNextVertex(GraphVertexInfo info)
        {
            info.IsUnvisited = false;
            foreach (var e in info.Vertex.Edges)
            {
                var nextInfo = GetVertexInfo(e.ConnectedVertex);
                var sum = info.EdgesWeightSum + e.EdgeWeight;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }
            }
        }

        /// <summary>
        /// Генерація шляху
        /// </summary>
        /// <param name="startVertex">Початкова вершина</param>
        /// <param name="endVertex">Кінцева вершина</param>
        /// <returns>Шлях</returns>
        string GetPath(GraphVertex startVertex, GraphVertex endVertex)
        {
            var path = endVertex.ToString();
            while (startVertex != endVertex)
            {
                endVertex = GetVertexInfo(endVertex).PreviousVertex;
                path = endVertex.ToString() + path;
            }

            return path;
        }
    }
}