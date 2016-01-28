﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace W3DT.Formats
{
    public class StringBlock
    {
        private int blockOffset;
        private Dictionary<int, string> data;

        public StringBlock(FormatBase input, int readLimit = -1)
        {
            data = new Dictionary<int, string>();
            blockOffset = input.getSeek();

            int i = 0; // Relative offset.
            while ((readLimit == -1 || i < readLimit) && input.isEndOfStream())
            {
                i += input.seekNonZero();

                if (input.isOutOfBounds(i))
                    break;

                string line = input.readString();
                data.Add(i, line);

                i += line.Length + 1;
            }
        }

        public string get(int offset, bool relative = true)
        {
            if (!relative)
                offset -= blockOffset;

            return data.ContainsKey(offset) ? data[offset] : null;
        }

        public IEnumerable<string> all()
        {
            return data.Values;
        }
    }
}