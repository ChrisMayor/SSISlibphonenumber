<img src ="https://dev.azure.com/ich0166/SSIS%20libphonenumber/_apis/build/status/ChrisMayor.SSISlibphonenumber?branchName=master"></img>
# SSIS libphonenumber - A phone number parsing and normalization SSIS Pipeline Component
SSIS pipeline transformation shape, which provides phone number parsing functionality by implementing the Google libphonenumber csharp port https://github.com/twcclegg/libphonenumber-csharp (v8.10.16)

See Google libphonenumber web demo <a href="https://libphonenumber.appspot.com/">Web demo (external link)</a> for demonstration.

## Highlights:
* SQL Server 2016/2017 data flow custom Shape
* Transform, normalize, validate and geo locate your phone numbers in SSIS
* Provides functionality of Googles libphonenumber (using its libphonenumber-csharp port) for SQL Server 2016/2017
* SSIS Pipeline transformation shape
* Googles libphonenumber is great - runs on premise and also on your android phone (see https://github.com/google/libphonenumber)
* Transform your unformatted phone numbers to a normalized format (e.g. for your CRM system or for skype integration...)
* Tries to parse strings to phone numbers for national and international numbers
* Can lookup the carrier code and offline geo location
* Tested with Visual Studio 2019 SSIS Extension

## Currently implemented functionality V1:
* Ready to use - configuration with shape UI
* Call to IsViablePhoneNumber to check if the phone number is viable
* ExtractPossibleNumber
* NormalizedNumber
* NormalizedDigitsOnly
* Format PhoneNumberFormat.E164
* Format PhoneNumberFormat.INTERNATIONAL
* IsValidNumber
* CountryCode
* HasCountryCode
* PreferredDomesticCarrierCode
* GeoCoderDescription (GeoCoder)

## Screenshots V1 (Visual Studio 2019 / SSIS Extension)

### SSIS Toolbox (Data Flow)
<p align="center">
  <img src="../master/Screenshots/1_Capture_V1_0.JPG" title="SSIS Dataflow with shape and UI V1">
</p>

### SSIS Data Flow sample
<p align="center">
  <img src="../master/Screenshots/1_Capture_V1_1.JPG" title="SSIS Dataflow with shape and UI V1">
</p>

### Shape UI Configuration
<p align="center">
  <img src="../master/Screenshots/1_Capture_V1_2.JPG" title="SSIS Dataflow with shape and UI V1">
</p>

### Shape Input
<p align="center">
  <img src="../master/Screenshots/1_Capture_V1_3.JPG" title="SSIS Dataflow with shape and UI V1">
</p>

### Shape Output (1)
<p align="center">
  <img src="../master/Screenshots/1_Capture_V1_4.JPG" title="SSIS Dataflow with shape and UI V1">
</p>

### Shape Output (2)
<p align="center">
  <img src="../master/Screenshots/1_Capture_V1_5.JPG" title="SSIS Dataflow with shape and UI V1">
</p>


## Install Instructions:
* Run gacinstall.com from bin/debug or bin/release --> Will install the shape and the signed phonelib assembly to GAC and adds the shape to SSIS
* Create/Open data flow
* Drag SSIS libphonenumber shape from toolbox to data flow
* Connect input and output
* Use UI of shape for configuration (double-click on shape)
* Run

## Licenses:
* From Goolgles libphonenumber and libphonenumber-csharp are preserved in git root as txt files

## Disclaimer / Impressum

* Published under the MIT license
* Use at your own risk

<a href="https://github.com/ChrisMayor/Impressum">Impressum / Imprint in German language to comply with German tele-media regulations.</a>
