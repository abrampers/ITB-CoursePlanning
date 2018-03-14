using System;
using System.Collections.Generic;

namespace CoursePlanning
{
    class Program
    {
		static void Main()
		{
			Graph g = new Graph("input.txt");

			g.topologicalSort();
		}
	}

	class Graph
	{
		private int[,] graph;
    	private bool[] visited;
    	private int no_of_vertex;
    	private SortedDictionary<string, int> dictStoI;
    	private SortedDictionary<int, string> dictItoS;

    	public Graph(string file) 
    	{
    		// Read input.txt and store to matrix
			string input = System.IO.File.ReadAllText(file);
			string[] lines = input.Split('\n');
			List<string>[] matrix = new List<string>[lines.Length];

			// Initialize all the list in matrix
			for(int i = 0; i < lines.Length; i++) {
				matrix[i] = new List<string>();
			}

			// Add each element to list
			for(int i = 0; i < lines.Length; i++) {
				string[] temp = lines[i].Remove(lines[i].Length - 1).Split(',');
				foreach(string s in temp) {
					matrix[i].Add(s);
				}
			}

			no_of_vertex = matrix.Length;

			dictStoI = new SortedDictionary<string, int>();
			dictItoS = new SortedDictionary<int, string>();

			for(int i = 0; i < no_of_vertex; i++) {
				dictStoI[matrix[i][0]] = i;
				dictItoS[i] = matrix[i][0];
			}

			graph = new int[no_of_vertex, no_of_vertex]; // Initialize matrix to zeros
			for(int i = 0; i < no_of_vertex; i++) {
				for(int j = 1; j < matrix[i].Count; j++) {
					graph[dictStoI[matrix[i][j]], i] = 1;
				}
			}
    	}

    	private bool visitedAllVertex() {
    		bool visited_all = true;
    		for(int i = 0; i < no_of_vertex; i++) {
    			if(!visited[i]) {
    				visited_all = false;
    				break;
    			}
    		}
    		return visited_all;
    	}

		private int compareTuple(Tuple<int, int> x, Tuple<int, int> y) {
		    if (x.Item1 == y.Item1) {
		    	return x.Item2 < y.Item2 ? -1 : 1;
		    } else {
		    	return x.Item1 > y.Item1 ? -1 : 1;
		    }
		}

		private int vertexNoIn() {
			int i = 0;
			bool found = false;
			while(i < no_of_vertex && !found) {
				if(noInEdge(i)) {
					found = true;
				} else {
					i++;
				}
			}
			if(found) {
				return i;
			} else {
				return -1;
			}
		}

		private bool noOutEdge(int v) {
			bool no_out_edge = true;
			for(int i = 0; (i < no_of_vertex); i++) {
				if(graph[v, i] != 0) {
					no_out_edge = false;
					break;
				}
			}
			return no_out_edge;
		}

		private bool noInEdge(int v) {
			bool no_in_edge = true;
			for(int i = 0; (i < no_of_vertex); i++) {
				if(graph[i, v] != 0) {
					no_in_edge = false;
					break;
				}
			}
			return no_in_edge;
		}

		private void DFS(int vertex, ref List<Tuple<int, int> > list, ref int timestamp)
		{
			visited[vertex] = true;
			// Start timestamp
			list.Add(Tuple.Create(timestamp, vertex));
			timestamp++;
			for(int i = 0; i < no_of_vertex; i++) {
				if(graph[vertex, i] == 1 && !visited[i]) {
					DFS(i, ref list, ref timestamp);
				}
			}
			// Finished timestamp
			list.Add(Tuple.Create(timestamp, vertex));
			timestamp++;
		}

		private void BFS(int vertex, ref List<int> list)
		{
			Queue<int> q = new Queue<int>();
			q.Enqueue(vertex);
			while((q.Count != 0) && !visitedAllVertex()) {
				int v = q.Dequeue();
				if(noInEdge(v) && !visited[v]) {
					visited[v] = true;
					for(int i = 0; i < no_of_vertex; i++) {
						if(graph[v, i] == 1 && !visited[i]) {
							graph[v, i] = 0;
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
			for(int i = 0; i < no_of_vertex; i++) {
				for(int j = 0; j < no_of_vertex; j++) {
					Console.Write("{0} ", graph[i, j]);
				}
				Console.Write("\n");
			}
			Console.WriteLine();

			// Tuple of timestamp and vertex
			List<Tuple<int, int> > listDFS = new List<Tuple<int, int> >();
			visited = new bool[no_of_vertex];
			int timestamp = 1;
			DFS(vertexNoIn(), ref listDFS, ref timestamp);
			listDFS.Sort(compareTuple);

			// Print all ordered
			Console.Write("DFS: ");
			bool[] printed = new bool[no_of_vertex];
			for(int i = 0; i < listDFS.Count; i++) {
				if(!printed[listDFS[i].Item2]) {
					Console.Write("{0} ", dictItoS[listDFS[i].Item2]);
					printed[listDFS[i].Item2] = true;
				}
			}
			Console.WriteLine();
			Console.WriteLine();

			List<int> listBFS = new List<int>();
			visited = new bool[no_of_vertex];
			BFS(vertexNoIn(), ref listBFS);

			// Print all Ordered
			Console.Write("BFS: ");
			for(int i = 0; i < listBFS.Count; i++) {
				Console.Write("{0} ", dictItoS[listBFS[i]]);
			}
			Console.WriteLine();
			Console.WriteLine();
		}
	}
}
