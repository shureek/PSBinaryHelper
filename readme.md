# PSBinaryHelper

I need several PS Cmdlets for working with binary data. I need to get file content by offset, replace and reorder binary file parts. Plans:

 - `New-BinaryData` - create binary data with template or random
 - `Format-BinaryData` - for viewing binary data
 - `Write-BinaryData` - for writing binary data to file

All cmdlets should work with some binary data chunks and collections of them. So I need to create suitable class for it. Something like `DataChunk`.
