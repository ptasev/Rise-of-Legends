namespace BigHugeEngineLibrary.Archive.Big
{
    using MiscUtil.Conversion;
    using MiscUtil.IO;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BigBinaryWriter : EndianBinaryWriter
    {
        public BigBinaryWriter(EndianBitConverter bitConverter, System.IO.Stream stream)
            : base(bitConverter, stream)
        {
        }

        public new void Write(string str)
        {
            byte[] data = Encoding.Unicode.GetBytes(str);
            this.Write((Int32)str.Length);
            this.Write(data);
        }

        public void Write(DateTime dateTime)
        {
            this.Write((Int32)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds);
        }
    }
}
