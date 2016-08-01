﻿using System;

namespace Algorithm.Sandbox.DataStructures
{
    public class AsBMaxHeapNode<I, V> where I : IComparable
    {
        public I Identifier { get; set; }
        public V Value { get; set; }

        public AsBMaxHeap<I, V> Left { get; set; }
        public AsBMaxHeap<I, V> Right { get; set; }

        public bool IsLeaf => Left == null && Right == null;

        public AsBMaxHeapNode(I identifier, V value)
        {
            this.Identifier = identifier;
            this.Value = value;
        }

    }

    public class AsBMaxHeap<I, V> where I : IComparable
    {
        public AsBMaxHeapNode<I, V> Root { get; set; }

    }
}
