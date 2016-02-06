namespace BigHugeEngineLibrary.Archive.Big
{
    using Ionic.Zlib;
    using MiscUtil.Conversion;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BigEntry
    {
        public string FileName { get; set; }
        public UInt32 Size { get; set; }
        public UInt32 CompressedSize { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Type { get; set; }
        public UInt16 Type2 { get; set; }
        public Byte[] Data
        {
            get
            {
                return this._data;
            }
            private set
            {
                this._data = value;
            }
        }

        public string Name
        {
            get
            {
                return Path.GetFileName(this.FileName);
            }
        }
        internal Int32 Offset
        {
            set { this._offset = value; }
        }

        private Byte[] _data;
        private Int32 _offset;

        public BigEntry()
        {
        }

        public void Read(BigBinaryReader reader)
        {
            this.FileName = reader.ReadString();

            reader.Seek(8, SeekOrigin.Current); // skip padding?

            this._offset = reader.ReadInt32();
            this.Size = reader.ReadUInt32();
            reader.Seek(4, SeekOrigin.Current); // skip padding?
            this.TimeStamp = reader.ReadDateTime();

            this.Type = reader.ReadString();
            this.Type2 = reader.ReadUInt16();

            int pos = (int)reader.BaseStream.Position;
            reader.Seek(this._offset, SeekOrigin.Begin);
            this.CompressedSize = reader.ReadUInt32();
            this.Data = reader.ReadBytes((int)this.CompressedSize);
            reader.Seek(pos, SeekOrigin.Begin);
        }

        public void Write(BigBinaryWriter writer)
        {
            writer.Write(this.FileName);
            writer.Write((UInt64)0);

            writer.Write(this._offset);
            writer.Write(this.Size);
            writer.Write((UInt32)0);
            writer.Write(this.TimeStamp);

            writer.Write(this.Type);
            writer.Write(this.Type2);
        }

        public void Export(Stream stream)
        {
            using (BigBinaryWriter writer = new BigBinaryWriter(EndianBitConverter.Little, stream))
            {
                writer.Write(this.GetDataArray(true));
            }
        }

        public byte[] GetDataArray(bool decompress)
        {
            byte[] data;
            if (decompress)
            {
                data = ZlibStream.UncompressBuffer(this.Data);
            }
            else
            {
                data = this.Data;
            }

            return data;
        }


        public void Import(Stream stream)
        {
            using (BigBinaryReader reader = new BigBinaryReader(EndianBitConverter.Little, stream))
            {
                this.SetData(reader.ReadBytes((int)reader.BaseStream.Length), true);
            }
        }

        public void SetData(byte[] data, bool compress)
        {
            if (compress)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    using (ZlibStream compressor = new ZlibStream(ms, CompressionMode.Compress, CompressionLevel.Default))
                    {
                        compressor.Write(data, 0, data.Length);
                    }

                    this.Data = ms.ToArray();
                }
            }
            else
            {
                this.Data = data;
            }

            this.Size = (UInt32)data.Length;
            this.CompressedSize = (UInt32)this._data.Length;
        }
    }
}
