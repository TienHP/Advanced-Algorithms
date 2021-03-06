﻿using Advanced.Algorithms.DataStructures.Graph.AdjacencyList;
using System;
using System.Collections.Generic;

namespace Advanced.Algorithms.GraphAlgorithms.Bridge
{
    public class Bridge<T>
    {
        public T vertexA { get; }
        public T vertexB { get; }

        public Bridge(T vertexA, T vertexB)
        {
            this.vertexA = vertexA;
            this.vertexB = vertexB;
        }
    }

    /// <summary>
    /// Bridge finder using Tarjan's algorithm
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TarjansBridgeFinder<T>
    {
        /// <summary>
        /// returns a list if Bridge points in this graph
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public List<Bridge<T>> FindBridges(Graph<T> graph)
        {
            int visitTime = 0;
            return DFS(graph.ReferenceVertex, new List<Bridge<T>>(),
                new Dictionary<T, int>(), new Dictionary<T, int>(),
                new Dictionary<T, T>(),
                ref visitTime);
        }

        /// <summary>
        /// Do a depth first search to find Bridge edges by keeping track of 
        /// discovery nodes and checking for back edges using low/discovery time maps
        /// </summary>
        /// <param name="currentVertex"></param>
        /// <param name="result"></param>
        /// <param name="discovery"></param>
        /// <param name="discoveryTimeMap"></param>
        /// <param name="lowTimeMap"></param>
        /// <param name="parent"></param>
        /// <param name="discoveryTime"></param>
        /// <returns></returns>
        private List<Bridge<T>> DFS(GraphVertex<T> currentVertex,
             List<Bridge<T>> result,
             Dictionary<T, int> discoveryTimeMap, Dictionary<T, int> lowTimeMap,
             Dictionary<T, T> parent, ref int discoveryTime)
        {

            discoveryTimeMap.Add(currentVertex.Value, discoveryTime);
            lowTimeMap.Add(currentVertex.Value, discoveryTime);

            //discovery childs in this iteration
            var discoveryChildCount = 0;

            foreach (var edge in currentVertex.Edges)
            {
                if (!discoveryTimeMap.ContainsKey(edge.Value))
                {
                    discoveryChildCount++;
                    parent.Add(edge.Value, currentVertex.Value);

                    discoveryTime++;
                    DFS(edge, result,
                                discoveryTimeMap, lowTimeMap, parent, ref discoveryTime);

                    //propogate lowTime index of neighbour so that ancestors can see check for back edge
                    lowTimeMap[currentVertex.Value] =
                        Math.Min(lowTimeMap[currentVertex.Value], lowTimeMap[edge.Value]);

                    //if neighbours lowTime is less than current
                    //then this is an Bridge point 
                    //because neighbour never had a chance to propogate any ancestors low value
                    //since this is an isolated componant
                    if (discoveryTimeMap[currentVertex.Value] < lowTimeMap[edge.Value])
                    {
                        result.Add(new Bridge<T>(currentVertex.Value, edge.Value));
                    }

                }
                else
                {   
                    //check if this edge target vertex is not in the current DFS path
                    //even if edge target vertex was already visisted
                    //update discovery so that ancestors can see it
                    if (parent.ContainsKey(currentVertex.Value) == false
                        || !edge.Value.Equals(parent[currentVertex.Value]))
                    {
                        lowTimeMap[currentVertex.Value] =
                            Math.Min(lowTimeMap[currentVertex.Value], discoveryTimeMap[edge.Value]);
                    }
                }
            }      

            return result;
        }
    }
}
