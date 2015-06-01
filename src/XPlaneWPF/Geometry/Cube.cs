using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace XPlaneWPF.Geometry
{
    public class Cube
    {
        public MeshGeometry3D Mesh { get; private set; }

        public Cube()
        {
            Mesh = new MeshGeometry3D();
        }

        public Cube(double magnitude) : this()
        {
            // Define Vertices
            Mesh.Positions.Add(new Point3D(-1, -1, 1));
            Mesh.Positions.Add(new Point3D(1, -1, 1));
            Mesh.Positions.Add(new Point3D(1, 1, 1));
            Mesh.Positions.Add(new Point3D(-1, 1, 1));
            Mesh.Positions.Add(new Point3D(-1, -1, -1));
            Mesh.Positions.Add(new Point3D(1, -1, -1));
            Mesh.Positions.Add(new Point3D(1, 1, -1));
            Mesh.Positions.Add(new Point3D(-1, 1, -1));

            // Define Faces

            // Front
            Mesh.TriangleIndices.Add(0);
            Mesh.TriangleIndices.Add(1);
            Mesh.TriangleIndices.Add(2);           
            Mesh.TriangleIndices.Add(2);
            Mesh.TriangleIndices.Add(3);
            Mesh.TriangleIndices.Add(0);

            // Back
            Mesh.TriangleIndices.Add(6);
            Mesh.TriangleIndices.Add(5);
            Mesh.TriangleIndices.Add(4);
            Mesh.TriangleIndices.Add(4);
            Mesh.TriangleIndices.Add(7);
            Mesh.TriangleIndices.Add(6);

            // Right
            // Left
            // Top
            // Bottom
        }
    }
}
