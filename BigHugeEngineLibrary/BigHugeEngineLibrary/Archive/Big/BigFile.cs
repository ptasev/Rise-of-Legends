namespace BigHugeEngineLibrary.Archive.Big
{
    using MiscUtil.Conversion;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BigFile
    {
        public List<BigEntry> Entries { get; set; }

        private UInt32 _version;
        private Byte _unknown;
        private UInt16 _unknown2;

        public event EventHandler<ProgressChangedEventArgs> ImportProgressChanged;
        public event EventHandler<ProgressChangedEventArgs> ExportProgressChanged;

        public BigFile()
        {
            _version = 11;
            _unknown = 1;
            _unknown2 = UInt16.MaxValue;
            this.Entries = new List<BigEntry>();
        }

        protected virtual void OnImportProgressChanged(ProgressChangedEventArgs e)
        {
            if (ImportProgressChanged != null)
            {
                ImportProgressChanged(this, e);
            }
        }
        protected virtual void OnExportProgressChanged(ProgressChangedEventArgs e)
        {
            if (ExportProgressChanged != null)
            {
                ExportProgressChanged(this, e);
            }
        }

        public void Read(Stream stream)
        {
            using (BigBinaryReader reader = new BigBinaryReader(EndianBitConverter.Little, stream))
            {
                this._version = reader.ReadUInt32();
                this._unknown = reader.ReadByte();

                string magic = reader.ReadString();
                if (magic != "WAR-BUILDER")
                {
                    throw new Exception("This is not a Big file!");
                }

                reader.ReadBytes(4); // padding
                int entryCount = reader.ReadInt32();
                reader.ReadInt32(); // fC2

                this._unknown2 = reader.ReadUInt16();
                reader.ReadByte(); // unk

                this.Entries = new List<BigEntry>(entryCount);
                for (int i = 0; i < entryCount; i++)
                {
                    BigEntry entry = new BigEntry();
                    entry.Read(reader);
                    this.Entries.Add(entry);
                }
            }
        }

        public void Write(Stream stream)
        {
            using (BigBinaryWriter writer = new BigBinaryWriter(EndianBitConverter.Little, stream))
            {
                writer.Write(this._version);
                writer.Write(this._unknown);
                writer.Write("WAR-BUILDER");

                writer.Write(new byte[4]);
                writer.Write(this.Entries.Count);
                writer.Write(this.Entries.Count);

                writer.Write(this._unknown2);
                writer.Write((byte)0);

                this.UpdateOffsets();
                foreach (BigEntry entry in this.Entries)
                {
                    entry.Write(writer);
                }

                foreach (BigEntry entry in this.Entries)
                {
                    writer.Write(entry.CompressedSize);
                    writer.Write(entry.Data);
                }
            }
        }

        private void UpdateOffsets()
        {
            Int32 offset = 46;
            for (int i = 0; i < this.Entries.Count; ++i)
            {
                offset += Encoding.Unicode.GetByteCount(this.Entries[i].FileName) +
                    Encoding.Unicode.GetByteCount(this.Entries[i].Type);
            }
            offset += this.Entries.Count * 34; // 34

            for (int i = 0; i < this.Entries.Count; ++i)
            {
                this.Entries[i].Offset = offset;
                offset += (Int32)this.Entries[i].CompressedSize + 4;
            }
        }

        public void Export(string folderPath)
        {
            ProgressChangedEventArgs evArgs;
            int progress = 0;
            bool failed;

            for (int i = 0; i < this.Entries.Count;)
            {
                try
                {
                    failed = false;
                    string outputFileName = Path.Combine(folderPath, this.Entries[i].FileName.Substring(2));
                    if (!string.IsNullOrEmpty(this.Entries[i].Type))
                    {
                        outputFileName += "." + this.Entries[i].Type;
                    }
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFileName));

                    evArgs = new ProgressChangedEventArgs(progress, "Exporting " + outputFileName + "... ");
                    this.OnImportProgressChanged(evArgs);

                    this.Entries[i].Export(File.Open(outputFileName, FileMode.Create, FileAccess.Write, FileShare.Read));
                }
                catch
                {
                    failed = true;
                }

                ++i;
                progress = i * 100 / this.Entries.Count;
                if (failed)
                {
                    evArgs = new ProgressChangedEventArgs(progress, "FAILED");
                    this.OnImportProgressChanged(evArgs);
                }
                else
                {
                    evArgs = new ProgressChangedEventArgs(progress, "SUCCESS");
                    this.OnImportProgressChanged(evArgs);
                }
            }
        }

        public void Import(string folderPath)
        {
            ProgressChangedEventArgs evArgs;
            int progress = 0;
            byte state; // 0 -- skipped, 1 -- success, 2 -- failed

            int succeeded = 0;
            int skipped = 0;
            int failed = 0;

            string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length;)
            {
                state = 0;
                evArgs = new ProgressChangedEventArgs(progress, "Importing " + files[i] + "... ");
                this.OnImportProgressChanged(evArgs);

                try
                {
                    foreach (BigEntry entry in this.Entries)
                    {
                        string eName = entry.FileName.Substring(2);

                        if (!string.IsNullOrEmpty(entry.Type))
                        {
                            eName += "." + entry.Type;
                        }

                        if (files[i].EndsWith(eName))
                        {
                            entry.Import(File.Open(files[i], FileMode.Open, FileAccess.Read, FileShare.Read));
                            state = 1;
                            break;
                        }
                    }
                }
                catch
                {
                    state = 2;
                }

                ++i;
                progress = i * 100 / files.Length;
                switch (state)
                {
                    case 0:
                        skipped++;
                        evArgs = new ProgressChangedEventArgs(progress, "SKIPPED" + Environment.NewLine);
                        this.OnImportProgressChanged(evArgs);
                        break;
                    case 1:
                        succeeded++;
                        evArgs = new ProgressChangedEventArgs(progress, "SUCCEEDED" + Environment.NewLine);
                        this.OnImportProgressChanged(evArgs);
                        break;
                    case 2:
                        failed++;
                        evArgs = new ProgressChangedEventArgs(progress, "FAILED" + Environment.NewLine);
                        this.OnImportProgressChanged(evArgs);
                        break;
                }
            }

            evArgs = new ProgressChangedEventArgs(100, string.Format("{0} Succeeded, {1} Skipped, {2} Failed", succeeded, skipped, failed));
            this.OnImportProgressChanged(evArgs);
        }
    }
}
