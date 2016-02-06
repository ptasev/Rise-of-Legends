namespace BigHugeEngineLibrary.Archive.Big
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BigDependency
    {
        public string FileName { get; set; }
        public UInt32 Hash { get; set; }

        public BigDependency()
        {

        }

        public void Read(BigBinaryReader reader)
        {
            this.FileName = reader.ReadString();
            this.Hash = reader.ReadUInt32();
        }

        public void Write(BigBinaryWriter writer)
        {
            writer.Write(this.FileName);
            writer.Write(this.Hash);
        }
    }
}
