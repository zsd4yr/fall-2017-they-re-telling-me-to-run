﻿/* This class converts the object produced by the MapGeneration system for consumption by systems that have no knowledge
of the MapGeneration system. */

using CaveGeneration.MapGeneration;
using CaveGeneration.MeshGeneration;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaveGeneration
{
    /// <summary>
    /// Converts the output of the MapGenerator into other objects.
    /// </summary>
    static class MapConverter
    {
        /// <summary>
        /// Converts a map for consumption by the mesh generation system.
        /// </summary>
        public static WallGrid MapToWallGrid(Map mapChunk, int scale, Coord index)
        {
            Assert.IsNotNull(mapChunk);
            Assert.IsTrue(scale > 0, "Scale must be positive");

            Vector3 position = IndexToPosition(index, scale);
            return new WallGrid(mapChunk.ToByteArray(), position, scale);
        }

        public static CollisionTester MapToCollisionTester(Map map, int scale)
        {
            Assert.IsNotNull(map);
            Assert.IsTrue(scale > 0, "Scale must be positive");
            /*
             This is a bit round-about, but it's a result of passing through multiple systems. 
             CollisionTester is a class written in CaveGeneration to consume and build upon FloorTester.
             FloorTester is a class in MeshGeneration, offering fast primitives for testing for floors
             in a mesh generated by a given WallGrid. 
             WallGrid is how a grid of walls is represented in the MeshGeneration system, which knows nothing
             about Map objects.
             Map is how a grid of walls is represented in the MapGeneration system. MeshGeneration and 
             MapGeneration have no knowledge of each other to avoid unnecessary dependencies. */
            return new CollisionTester(new FloorTester(ToWallGrid(map, scale)));
        }

        static WallGrid ToWallGrid(Map map, int scale)
        {
            return new WallGrid(map.ToByteArray(), Vector3.zero, scale);
        }

        static Vector3 IndexToPosition(Coord index, int scale)
        {
            return new Vector3(index.x, 0f, index.y) * scale * MapSplitter.CHUNK_SIZE;
        }
    } 
}
