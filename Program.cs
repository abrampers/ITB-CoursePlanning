using System;
using System.Collections.Generic;

namespace CoursePlanning
{
    class Program
    {
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

			P.topologicalSort(matrix);



			// for(int i = 0; i < matrix.Length; i++) {
			// 	for(int j = 0; j < matrix[i].Count; j++) {
			// 		Console.Write("{0} ", matrix[i][j]);
			// 	}
			// 	Console.Write("\n");
			// }
		}

		void topologicalSort(List<string>[] matrix) 
		{
			Program P = new Program();

			int[,] graph = new int[matrix.Length, matrix.Length];
			graph = generateGraph(matrix);

			for(int i = 0; i < matrix.Length; i++) {
				for(int j = 0; j < matrix.Length; j++) {
					Console.Write("{0} ", graph[i, j]);
				}
				Console.Write("\n");
			}

			P.DFS(graph);

			P.BFS(graph);
		}

		int[,] generateGraph(List<string>[] matrix)
		{
			SortedDictionary<string, int> dict = new SortedDictionary<string, int>();

			for(int i = 0; i < matrix.Length; i++) {
				dict[matrix[i][0]] = i;
			}

			int[,] graph = new int[matrix.Length, matrix.Length]; // Initialize matrix to zeros
			for(int i = 0; i < matrix.Length; i++) {
				for(int j = 1; j < matrix[i].Count; j++) {
					graph[i, dict[matrix[i][j]]] = 1;
				}
			}
			return graph;
		}

		void DFS(int[,] graph)
		{

		}

		void BFS(int[,] graph)
		{

		}    
	}
}
