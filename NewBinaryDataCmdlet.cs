using System;
using System.IO;
using System.Management.Automation;

namespace PSBinaryHelper
{
    [Cmdlet(VerbsCommon.New, "BinaryData")]
    [CmdletBinding(DefaultParameterSetName = "ByChunks")]
    [OutputType(typeof(byte[]))]
    public class NewBinaryDataCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public long Length { get; set; }

        bool entirely;
        [Parameter(ParameterSetName = "Entirely", Mandatory = true)]
        public SwitchParameter Entirely
        {
            get { return entirely; }
            set { entirely = value; }
        }

        [Parameter(ParameterSetName = "ByChunks")]
        public int ChunkSize { get; set; } = 1024 * 1024;

        bool isRandom = true;
        [Parameter()]
        public SwitchParameter Random
        {
            get { return isRandom; }
            set { isRandom = value; }
        }

        protected override void EndProcessing()
        {
            if (entirely)
            {
                WriteObject(GenerateChunk((int)Length), false);
            }
            else
            {
                long written = 0L;
                while (written < Length)
                {
                    long size = Length - written;
                    if (size > ChunkSize)
                        size = ChunkSize;
                    WriteObject(GenerateChunk((int)size), false);
                    written += size;
                }
            }
        }

        static Random rnd = null;

        static DataChunk GenerateChunk(int length)
        {
            var data = new byte[length];
            if (rnd == null)
                rnd = new Random();
            rnd.NextBytes(data);
            return new DataChunk(data);
        }
    }
}