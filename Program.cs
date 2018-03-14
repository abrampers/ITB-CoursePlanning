/*
	TUGAS BESAR STRATEGI ALGORITMA
	Abram Perdanaputra 		/ 13516083
	Faza Fahleraz			/ 13516095
	Nicholas Rianto Putra 	/ 13516020
*/

using System;
using System.Collections.Generic;

namespace CoursePlanning
{
    class Program
    {
		static void Main()
		{
			Graph g = new Graph("./input/input.txt");

			g.topologicalSort();
		}
	}

	class Graph
	{
		private Dictionary<string, HashSet<string>> graph;
    	private Dictionary<string, bool> visited = new Dictionary<string, bool>();
    	private int no_of_vertex;

    	public Graph(string file) 
    	{
    		// Read input.txt and store to matrix
			string input = System.IO.File.ReadAllText(file);
			string[] lines = input.Split('\n');
			List<string>[] matrix = new List<string>[lines.Length];

			// Initialize all the list in matrix
			for(int i = 0; i < lines.Length; i++) 
			{
				matrix[i] = new List<string>();
			}

			// Add each element to list
			for(int i = 0; i < lines.Length; i++) 
			{
				string[] temp = lines[i].Remove(lines[i].Length - 1).Split(',');
				foreach(string s in temp) 
				{
					matrix[i].Add(s);
				}
			}

			no_of_vertex = matrix.Length;

			graph = new Dictionary<string, HashSet<string>>(); // Initialize matrix to zeros
			for(int i = 0; i < no_of_vertex; i++) {
				graph[matrix[i][0]] = new HashSet<string>();
				for(int j = 1; j < matrix[i].Count; j++) 
				{
					graph[matrix[i][0]].Add(matrix[i][j]);
				}
			}
    	}

    	private void initializeVisited() 
    	{
    		foreach(string s in graph.Keys) 
    		{
    			visited[s] = false;
    		}
    	}

    	private bool isAdjacent(string src, string dest) 
    	{
    		return graph[dest].Contains(src);
    	}

    	private void deleteEdge(string src, string dest) 
    	{
    		graph[dest].Remove(src);
    	}

    	private bool visitedAllVertex() 
    	{
    		bool visited_all = true;
    		foreach(string i in graph.Keys) 
    		{
    			if(!visited[i]) 
    			{
    				visited_all = false;
    				break;
    			}
    		}
    		return visited_all;
    	}

		private int compareTuple(Tuple<int, string> x, Tuple<int, string> y) 
		{
		    return x.Item1 > y.Item1 ? -1 : 1;
		}

		private string vertexNoIn() 
		{
			string res = "\0";
			foreach(KeyValuePair<string, HashSet<string>> kvp in graph) 
			{
				if(kvp.Value.Count == 0) 
				{
					res = kvp.Key;
					break;
				}
			}
			return res;
		}

		private bool noOutEdge(string v) 
		{
			bool no_out_edge = true;
			foreach(string i in graph.Keys) 
			{
				if(isAdjacent(v, i)) 
				{
					no_out_edge = false;
					break;
				}
			}
			return no_out_edge;
		}

		private bool noInEdge(string v) 
		{
			bool no_in_edge = true;
			foreach(string i in graph.Keys) 
			{
				if(isAdjacent(i, v)) 
				{
					no_in_edge = false;
					break;
				}
			}
			return no_in_edge;
		}

		private void DFS(string vertex, ref List<Tuple<int, string> > list, ref int timestamp)
		{
			visited[vertex] = true;
			// Start timestamp
			list.Add(Tuple.Create(timestamp, vertex));
			timestamp++;
			foreach(string i in graph.Keys) {
				if(isAdjacent(vertex, i) && !visited[i]) 
				{
					DFS(i, ref list, ref timestamp);
				}
			}
			// Finished timestamp
			list.Add(Tuple.Create(timestamp, vertex));
			timestamp++;
		}

		private void BFS(string vertex, ref List<string> list)
		{
			Queue<string> q = new Queue<string>();
			q.Enqueue(vertex);
			while((q.Count != 0) && !visitedAllVertex()) {
				string v = q.Dequeue();
				if(noInEdge(v) && !visited[v]) 
				{
					visited[v] = true;
					foreach(string i in graph.Keys) 
					{
						if(isAdjacent(v, i) && !visited[i]) 
						{
							deleteEdge(v, i);
							q.Enqueue(i);
						}
					}
					list.Add(v);
				}
			}
		}

		public void topologicalSort() 
		{
			Console.WriteLine("Graph : ");
			foreach(KeyValuePair<string, HashSet<string>> kvp in graph) 
			{
				Console.Write("{0} ", kvp.Key);
				foreach(string j in kvp.Value) 
				{
					Console.Write("{0} ", j);
				}
				Console.Write("\n");
			}
			Console.WriteLine();

			// Tuple of timestamp and vertex
			List<Tuple<int, string> > listDFS = new List<Tuple<int, string> >();
			initializeVisited();
			int timestamp = 1;
			DFS(vertexNoIn(), ref listDFS, ref timestamp);
			listDFS.Sort(compareTuple);

			// Print all ordered
			Console.Write("DFS: ");
			HashSet<string> printed = new HashSet<string>();
			for(int i = 0; i < listDFS.Count; i++) 
			{
				if(!printed.Contains(listDFS[i].Item2)) 
				{
					Console.Write("{0} ", listDFS[i].Item2);
					printed.Add(listDFS[i].Item2);
				}
			}
			Console.WriteLine();
			Console.WriteLine();

			List<string> listBFS = new List<string>();
			initializeVisited();
			BFS(vertexNoIn(), ref listBFS);

			// Print all Ordered
			Console.Write("BFS: ");
			for(int i = 0; i < listBFS.Count; i++) 
			{
				Console.Write("{0} ", listBFS[i]);
			}
			Console.WriteLine();
			Console.WriteLine();
		}
	}
}
