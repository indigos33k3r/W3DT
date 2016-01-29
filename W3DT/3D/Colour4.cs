﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using W3DT.Formats;

namespace W3DT._3D
{
    public class Colour4
    {
        public byte r { get; set; }
        public byte g { get; set; }
        public byte b { get; set; }
        public byte a { get; set; }

        public Colour4(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public override string ToString()
        {
            return string.Format("R: [{0}] G: [{1}] B: [{2}] A: [{3}]", r, g, b, a);
        }

        public static Colour4 Read(FormatBase input)
        {
            return new Colour4(input.readUInt8(), input.readUInt8(), input.readUInt8(), input.readUInt8());
        }
    }
}
