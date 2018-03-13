using System;
using System.Collections.Generic;

namespace CoursePlanning
{
    class Program
    {
    	int[,] graph;
    	bool[] visited;
    	int no_of_vertex;
    	SortedDictionary<string, int> dictStoI;
    	SortedDictionary<int, string> dictItoS;

		int compareTuple(Tuple<int, int> x, Tuple<int, int> y) {
		    if (x.Item1 == y.Item1) {
		    	return x.Item2 < y.Item2 ? -1 : 1;
		    } else {
		    	return x.Item1 > y.Item1 ? -1 : 1;
		    }
		}

		int vertexNoIn() {
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

		bool noOutEdge(int v) {
			bool no_out_edge = true;
			for(int i = 0; (i < no_of_vertex); i++) {
				if(graph[v, i] != 0) {
					no_out_edge = false;
					break;
				}
			}
			return no_out_edge;
		}

		bool noInEdge(int v) {
			bool no_in_edge = true;
			for(int i = 0; (i < no_of_vertex); i++) {
				if(graph[i, v] != 0) {
					no_in_edge = false;
					break;
				}
			}
			return no_in_edge;
		}

		void generateGraph(List<string>[] matrix)
		{
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
			// return graph;
		}

		void DFS(int vertex, ref List<Tuple<int, int> > list, ref int timestamp)
		{
			visited[vertex] = true;
			// Start timestamp
			list.Add(Tuple.Create(timestamp, vertex));
			timestamp++;
			for(int i = 0; i < no_of_vertex; i++) {
				if(graph[vertex, i] == 1 && !visited[i]) {
					graph[vertex, i] = 0;
					DFS(i, ref list, ref timestamp);
				}
			}
			// Finished timestamp
			list.Add(Tuple.Create(timestamp, vertex));
			timestamp++;
		}

		void BFS(int vertex, ref List<Tuple<int, int> > list, ref int timestamp)
		{

		}

		void topologicalSort(List<string>[] matrix) 
		{
			generateGraph(matrix);

			for(int i = 0; i < no_of_vertex; i++) {
				for(int j = 0; j < no_of_vertex; j++) {
					Console.Write("{0} ", graph[i, j]);
				}
				Console.Write("\n");
			}

			// Tuple of timestamp and vertex
			List<Tuple<int, int> > list = new List<Tuple<int, int> >();
			visited = new bool[no_of_vertex];
			int timestamp = 1;
			int start_vertex = vertexNoIn();
			DFS(start_vertex, ref list, ref timestamp);
			list.Sort(compareTuple);
			// for(int i = 0; i < list.Count; i++) {
			// 	Console.WriteLine("{0} {1}", list[i].Item1, dictItoS[list[i].Item2]);
			// }

			// Print all ordered
			bool[] printed = new bool[no_of_vertex];
			for(int i = 0; i < list.Count; i++) {
				if(!printed[list[i].Item2]) {
					Console.Write("{0} ", dictItoS[list[i].Item2]);
					printed[list[i].Item2] = true;
				}
			}
			Console.WriteLine();

			// list.Clear();
			// visited = new bool[no_of_vertex];
			// timestamp = 1;
			// BFS(vertexNoIn(), ref list, ref timestamp);
			// list.Sort(compareTuple);

			// Print all Ordered
		}

		static void Main()
		{
			Program P = new Program();

			// Read input.txt and store to matrix
			string input = System.IO.File.ReadAllText("input.txt");
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

			P.no_of_vertex = matrix.Length;

			P.topologicalSort(matrix);
		}
	}
}
