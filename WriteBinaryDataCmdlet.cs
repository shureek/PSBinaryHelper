using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Data.SqlTypes;

namespace PSBinaryHelper
{
    [Cmdlet("Write", "BinaryData")]
    public class WriteBinaryDataCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string FileName { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public object Data { get; set; }

        bool useForce = false;
        [Parameter()]
        public SwitchParameter Force
        {
            get { return useForce; }
            set { useForce = value; }
        }

        bool noClobber = false;
        [Parameter()]
        public SwitchParameter NoClobber
        {
            get { return noClobber; }
            set { noClobber = value; }
        }

        bool append = false;
        [Parameter()]
        public SwitchParameter Append
        {
            get { return append; }
            set { append = value; }
        }

        FileStream stream = null;

        protected override void BeginProcessing()
        {
            FileMode mode;
            if (append)
                mode = FileMode.Append;
            else if (noClobber)
                mode = FileMode.CreateNew;
            else
                mode = FileMode.Create;
            stream = new FileStream(FileName, mode, FileAccess.Write, FileShare.Read);
        }

        protected override void ProcessRecord()
        {
            ProcessData(Data);
        }

        void ProcessData(object data)
        {
            if (data is PSObject obj)
                ProcessData(obj.BaseObject);
            else if (data is DataChunk chunk)
                WriteBytes(chunk);
            else if (data is byte[] bytes)
                WriteBytes(bytes);
            else
            {
                var ex = new NotSupportedException($"Data type {data.GetType()} is not supported");
                var error = new ErrorRecord(ex, "DataTypeNotSupported", ErrorCategory.InvalidData, Data);
                WriteError(error);
            }
        }

        void WriteBytes(byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        void ProcessData(byte[] data)
        {

        }

        protected override void EndProcessing()
        {
            stream.Flush(true);
            stream.Close();
        }
    }
}
