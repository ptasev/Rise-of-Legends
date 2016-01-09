namespace BigHugeEngineLibrary.Archive.Big
{
    using MiscUtil.Conversion;
    using MiscUtil.IO;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BigBinaryReader : EndianBinaryReader
    {
        public BigBinaryReader(EndianBitConverter bitConverter, System.IO.Stream stream)
            : base(bitConverter, stream)
        {
        }

        public new string ReadString()
        {
            UInt32 count = this.ReadUInt32();
            return Encoding.Unicode.GetString(this.ReadBytes((int)count*2));
        }

        public DateTime ReadDateTime()
        {
            UInt32 t = this.ReadUInt32();
            return new System.DateTime(1970, 1, 1).AddSeconds(t); 
        }
    }
}
