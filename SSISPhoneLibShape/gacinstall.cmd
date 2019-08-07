"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\gacutil" -u SSISPhoneLibShape

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\gacutil" -i SSISPhoneLibShape.dll

copy SSISPhoneLibShape.dll "C:\Program Files (x86)\Microsoft SQL Server\140\DTS\PipelineComponents"
copy SSISPhoneLibShape.dll "C:\Program Files\Microsoft SQL Server\140\DTS\PipelineComponents"

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\gacutil" -i .\..\..\ext-Assembly\PhoneNumbers.dll