﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL;

namespace W3DT._3D
{
    public class Face
    {
        private List<Position> points;

        public int PointCount { get { return points.Count; } }

        public Face()
        {
            points = new List<Position>();
        }

        public void addPoint(Position point)
        {
            points.Add(point);
        }

        public void Draw(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);

            foreach (Position point in points)
                gl.Vertex(point.X, point.Y, point.Z);

            gl.End();
        }
    }
}