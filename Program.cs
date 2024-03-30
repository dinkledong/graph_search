using System;
using System.Collections.Generic;

/*Дана матрица весов дуг. Определить и вывести все циклы в орграфе,
заданной длины х (вводится с клавиатуры)
Матрица
инцидентности*/

namespace Graph_Search
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Input number of verteces: ");
            int n = int.Parse(Console.ReadLine());

            Console.WriteLine("\nInput lines of weight matrix (separate numbers with ,)");
            Dictionary<int, List<int>> matrix = new Dictionary<int, List<int>>() { };
            for (int i = 0; i < n; i++)
            {
                matrix[i] = new List<int>();
                string[] s= Console.ReadLine().Split(",");
                for (int j = 0; j < s.Length; j++)
                {
                    matrix[i].Add(int.Parse(s[j]));
                }
            }

            Graph g = new Graph(matrix);

            Console.WriteLine("Incidence matrix:");
            g.PrintMatrix();

            foreach (Vertex v in g.V)
            {
                string s = "";
                int l = 0;
                g.dfs(v, s,l, v);
                g.ResetColors();
            }

            Console.WriteLine("\nFind cycles with length n");
            Console.WriteLine("Input n:");
            int length = int.Parse(Console.ReadLine());
            int c = 0;
            foreach (var item in g.catalogCycles)
            {
                if (item.length==length)
                {
                    Console.WriteLine($"cycle: {item.path}");
                    c++;
                } 
            }
            if (c==0)
            {
                Console.WriteLine("No cycles with such length");
            }
 
            Console.ReadLine();

        }
    }

    class Cycle
    {
        public string path;
        public int length;

        public Cycle(string p,int l)
        {
            path = p;
            length = l;
        }


    }
    class Vertex
    {
        public int index;
        public List<Vertex> neighbours;
        public int color;
        public bool flag = false;

        public Vertex(int i, int c = 0, bool f = false,List<Vertex> ne=null)
        {
            this.index = i;
            this.color = c;
            this.flag = f;
            this.neighbours = ne;
        }

    }

    class Edge
    {
        public int start;
        public int end;
        public int weight;

        public Edge(int start, int end,int w)
        {
            this.weight = w;
            this.start = start;
            this.end = end;
        }
        public void Print()
        {
            Console.WriteLine($"{this.start.ToString()} {this.end.ToString()} {this.weight.ToString()}");
        }
    }

    class Graph
    {
        public List<Vertex> V;
        public List<Edge> E;
        public List<Cycle> catalogCycles = new List<Cycle>();
        public Graph(List<Vertex> v, List<Edge> e)
        {
            V = v;
            E = e;
            ResetNeighbours();
        }
        public Graph()
        {
            V = new List<Vertex> { };
            E = new List<Edge> { };

        }

        public Graph(Dictionary<int,List<int>> matrix,bool incidence=false)
        {
            if (incidence)
            {
                catalogCycles = new List<Cycle>();
                V = new List<Vertex> { };
                E = new List<Edge> { };

                for (int i = 0; i < matrix.Count; i++)
                {
                    this.AddVertex();
                }

                for (int j = 0; j < matrix[0].Count; j++) //columns - edges
                {
                    int start = -1;
                    int end = -1;
                    int val = -1;
                    for (int i = 0; i < matrix.Count; i++)//lines - verteces
                    {

                        if (matrix[i][j] > 0)
                        {
                            start = i;
                            val = matrix[i][j];
                        }
                        if (matrix[i][j] < 0)
                        {
                            end = i;
                        }
                    }
                    this.AddEdge(start + 1, end + 1, val);
                }
                ResetNeighbours();
            }
            else
            {
                catalogCycles = new List<Cycle>();
                V = new List<Vertex> { };
                E = new List<Edge> { };

                for (int i = 0; i < matrix.Count; i++)
                {
                    AddVertex();
                    for (int j = 0; j < matrix[0].Count; j++)
                    {
                        if (matrix[i][j]!=0)
                        {
                            AddEdge(i+1, j+1, matrix[i][j]);
                        } 
                    }
                }
                ResetNeighbours();
            }
            
        }

        public Dictionary<int, List<int>> IncidenceMatrix()
        {
            Dictionary<int, List<int>> res = new Dictionary<int, List<int>>();
            int v = V.Count;
            int e = E.Count;

            for (int i = 0; i < v; i++)
            {
                res.Add(i, new List<int>() { });
                for (int j = 0; j < e; j++)
                {
                    //Console.WriteLine($"v{V[i].index}  e{E[j].index}");

                    if (this.E[j].start==this.V[i].index)
                    {
                        res[i].Add(1);
                    }
                    else if (this.E[j].end == this.V[i].index)
                    {
                        res[i].Add(-1);
                    }
                    else
                    {
                        res[i].Add(0);
                    }
                    //Console.WriteLine(res[i][j]);

                }
                //Console.WriteLine();
            }

            return res;
        }

        public void PrintMatrix()
        {
            Dictionary<int, List<int>> matrix = this.IncidenceMatrix();
            int v = V.Count;
            int e = E.Count;
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < e; j++)
                {
                    if (matrix[i][j]==-1)
                    {
                        Console.Write(" " + matrix[i][j] + "  ");
                    }
                    else
                    {
                        Console.Write("  " + matrix[i][j] + "  ");
                    }
                    
                }
                Console.WriteLine();
            }
        }
        public int First(Vertex v)
        {
            return v.neighbours[0].index;
        }
        public int Next(Vertex v,int i)
        {
            if (v.neighbours.Count>i+1)
            {
                return v.neighbours[i + 1].index;
            }
            return -1;
        }
        public Vertex GetVertex(Vertex v,int i)
        {
            if (v.neighbours.Count>i)
            {
                return v.neighbours[i];
            }
            return null;
        }
        public void AddVertex(int index,int color,bool flag)
        {
            this.V.Add(new Vertex(index,  color, flag));
        }
        public void AddVertex()
        {
            this.V.Add(new Vertex(this.V.Count + 1));
            //ResetNeighbours();
        }
        public void AddEdge(Edge e)
        {
            this.E.Add(e);
        }
        public void AddEdge(int start,int end ,int weight)
        {
            this.E.Add(new Edge(start,end,weight));
            //ResetNeighbours();
        }
        public void EditVertex(int index, int color, bool flag)
        {
            foreach (Vertex v in V)
            {
                if (v.index==index)
                {
                    v.color = color;
                    v.flag = flag;
                }
            }
        }
        public void EditEdge(int start, int end, int weight)
        {
            foreach (Edge e in E)
            {
                if (e.start == start && e.end==end)
                {
                    e.weight = weight;
                }
            }
        }

        public void ResetColors()
        {
            foreach (Vertex v in V)
            {
                v.color = 0;
            }
        }
        public void ResetNeighbours()
        {
            foreach (Vertex v in V)
            {
                v.neighbours = new List<Vertex>();
                //Console.WriteLine(v.index);
            }
            foreach (Edge e in E)
            {
                //Console.WriteLine($"{e.start}->{e.end}");
                //Console.WriteLine($"aa");
                V[e.start-1].neighbours.Add(V[e.end-1]);
            }
        }


        public void dfs(Vertex v,string path,int length,Vertex OGvertex)
        {
            path += v.index.ToString()+"-";
            v.color = 1; //gray
            int len = 0;

            foreach (var item in v.neighbours)
            {
                foreach (Edge e in this.E)
                {
                    if (e.start==v.index && e.end==item.index)
                    {
                        length += e.weight;
                        len = e.weight;
                    }
                }
                
                if (item.color==0)
                {
                    dfs(item,path,length,OGvertex);
                }
                if (item.color==1)
                {
                    length -= len;
                    if (item==OGvertex)
                    {
                        this.catalogCycles.Add(new Cycle(path.Substring(0, path.Length - 1),length+len));
                    }
                }
                if (item.color==2)
                {
                    item.color = 0;
                    length -= len;
                }

            }
            v.color = 2; //black
            path = path.Substring(0, path.Length - 2);
            length -= len;
        }

    }
}