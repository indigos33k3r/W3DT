﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using W3DT._3D;

namespace W3DT.Formats
{
    public class WaveFrontWriter : WriterBase
    {
        private const string FORMAT = "0.00000000";
        private TextureManager texManager;
        private StreamWriter obj;
        private StreamWriter mtl;

        private List<Mesh> meshes;

        private string mtlFile;
        private string name;

        public WaveFrontWriter(string file, TextureManager texManager)
        {
            this.texManager = texManager;
            string mtlPath = Path.ChangeExtension(file, ".mtl");

            obj = new StreamWriter(file, false);
            mtl = new StreamWriter(mtlPath, false);

            mtlFile = Path.GetFileName(mtlPath);
            name = Path.GetFileNameWithoutExtension(file);

            meshes = new List<Mesh>();
        }

        public void addMesh(Mesh mesh)
        {
            meshes.Add(mesh);
        }

        public override void Write()
        {
            obj.WriteLine("# World of Warcraft WMO exported using W3DT");
            obj.WriteLine("# https://github.com/Kruithne/W3DT/");
            nl(obj);

            // Link material library.
            obj.WriteLine("mtllib " + mtlFile);
            nl(obj);

            obj.WriteLine("o " + name);
            nl(obj);

            List<string> texList = new List<string>();
            int faceOffset = 1;
            foreach (Mesh mesh in meshes)
            {
                // Group header
                obj.WriteLine("  g " + mesh.Name);
                obj.WriteLine("  s 1");
                nl(obj);
                
                // Vertices
                foreach (Position vert in mesh.Verts)
                    obj.WriteLine(string.Format("    v {0} {1} {2}", vert.X.ToString(FORMAT), vert.Y.ToString(FORMAT), vert.Z.ToString(FORMAT)));

                obj.WriteLine("    # " + mesh.VertCount + " verts");
                nl(obj);

                // UVs
                foreach (UV uv in mesh.UVs)
                    obj.WriteLine(string.Format("    vt {0} {1}", uv.u.ToString(FORMAT), uv.v.ToString(FORMAT)));

                obj.WriteLine("    # " + mesh.UVCount + " UVs");
                nl(obj);

                // ToDo: Normals

                // Faces
                uint previousTexID = 0xFF;
                foreach (Face face in mesh.Faces)
                {
                    if (face.TextureID != previousTexID)
                    {
                        string texPath = texManager.getFile(face.TextureID);
                        if (!texList.Contains(texPath))
                            texList.Add(texPath);

                        nl(obj);
                        obj.WriteLine("    usemtl " + Path.GetFileNameWithoutExtension(texPath));
                        previousTexID = face.TextureID;
                    }

                    int p1 = face.Offset[0] + faceOffset;
                    int p2 = face.Offset[1] + faceOffset;
                    int p3 = face.Offset[2] + faceOffset;

                    obj.WriteLine(string.Format("    f {0}/{0} {1}/{1} {2}/{2}", p1, p2, p3));
                }
                faceOffset += mesh.VertCount;
                obj.WriteLine("    # " + mesh.FaceCount + " faces");
                nl(obj);
            }

            foreach (string tex in texList)
            {
                string file = Path.GetFileNameWithoutExtension(tex);

                mtl.WriteLine("newmtl " + file);
                mtl.WriteLine("illum 2");
                mtl.WriteLine("Kd 1.0 1.0 1.0");
                mtl.WriteLine("Ka 0.250000 0.250000 0.250000");
                mtl.WriteLine("Ks 0.000000 0.000000 0.000000");
                mtl.WriteLine("Ke 0.000000 0.000000 0.000000");
                mtl.WriteLine("Ns 0.000000");
                mtl.WriteLine("map_Kd -s 1 -1 1 " + file + ".png");
                nl(mtl);
            }
        }

        public override void Close()
        {
            obj.Close();
            mtl.Close();
            meshes.Clear();
        }
    }
}
