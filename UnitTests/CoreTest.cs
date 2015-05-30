using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core.Atoms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CoreTest
    {
        [TestMethod]
        public void TestCreateEmptyGraph()
        {
            var graph = new Graph();
            Assert.AreEqual((ulong)0, graph.GetMaxId());
        }

        [TestMethod]
        public void TestCreateGraphWithBlocks()
        {
            var graph = new Graph(new List<Block>(){new Block {Content = "Test",Id = 1}});
            Assert.AreEqual((ulong)1, graph.GetMaxId());
        }

        [TestMethod]
        public void TestGetNewId()
        {
            var graph = new Graph(new List<Block>() { new Block { Content = "Test", Id = 1 } });
            Assert.AreEqual((ulong)2, graph.GetNewId());
        }

        [TestMethod]
        public void TestCreateGraphWithLinks()
        {
            var graph = new Graph(new List<Link>() { new Link { From = 1, To = 2 } });
            Assert.AreEqual((ulong)0, graph.GetMaxId());
        }

        [TestMethod]
        public void TestCreateGraphWithLinksAndBlocks()
        {
            var graph = new Graph(new List<Block>() {new Block {Content = "Test", Id = 1}},
                new List<Link>() {new Link {From = 1, To = 2}});
            Assert.AreEqual((ulong)1, graph.GetMaxId());
        }

        [TestMethod]
        public void TestAddBlockByContent()
        {
            var graph = new Graph();
            Assert.AreEqual(graph.AddBlock("Content"), graph.GetMaxId());
        }

        [TestMethod]
        public void TestAddBlockByNullBlock()
        {
            var graph = new Graph();
            var id = graph.AddBlock("Content");
            graph.AddBlock(ref id);
            Assert.AreEqual(id, graph.GetMaxId());
        }

        [TestMethod]
        public void TestAddBlockByBlock()
        {
            var graph = new Graph();
            var id = graph.AddBlock("Content");
            graph.AddBlock(ref id, new Block());
            Assert.AreEqual(id, graph.GetMaxId());
        }

        [TestMethod]
        public void TestGetMatrix()
        {
            var graph = new Graph(new List<Block>()
            {
                new Block {Content = string.Empty, Id=1},
                new Block {Content = string.Empty, Id=2},
                new Block {Content = string.Empty, Id=3},
                new Block {Content = string.Empty, Id=4},
                new Block {Content = string.Empty, Id=5}
            },
            new List<Link>()
            {
                new Link {From = 1, To = 3},
                new Link {From = 4, To = 1},
                new Link {From = 4, To = 2},
                new Link {From = 5, To = 2},
                new Link {From = 5, To = 3},
                new Link {From = 5, To = 5}
            });
            var matrix = graph.GetMatrix();
            Assert.AreEqual(graph.GetMaxId(),(ulong)matrix.Count);
            Assert.AreEqual(matrix[1][2]-1,matrix[2][1]);
        }
    }


}
