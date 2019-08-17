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

## Currently implemented functionality V0.2:
* Ready to use - configuration with advanced editor (see screenshots)
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

## To do
* Implement SSIS UI
* Refactor
* Release V1.0
* Implement default country code input

## Work done:
* Added Gac cmd script
* Wrote a lot of code
* Did tests in SSIS
* Signed libphonenumber-csharp and added assembly to project (v8.10.16) in order to use the library in SSIS
* Prepared the SSIS UI
* Implemented SSIS Shape
* Added Azure DevOps Pipeline build

## Screenshots V0.2

### Configuration
<p align="center">
  <img src="../master/Screenshots/1_Capture_V0.2-0.JPG" title="SSIS Dataflow with shape V0.2">
</p>

### Input Data
<p align="center">
  <img src="../master/Screenshots/1_Capture_V0.2-1.JPG" title="SSIS Dataflow with shape V0.2">
</p>

### SSIS libphonenumber Output Columns (1)
<p align="center">
  <img src="../master/Screenshots/1_Capture_V0.2-2.JPG" title="SSIS Dataflow with shape V0.2">
</p>

### SSIS libphonenumber Output Columns (2)
<p align="center">
  <img src="../master/Screenshots/1_Capture_V0.2-3.JPG" title="SSIS Dataflow with shape V0.2">
</p>

## Screenshots V0.3

### UI Integration
<p align="center">
  <img src="../master/Screenshots/1_Capture_V0.3_1.JPG" title="SSIS Dataflow with shape and UI V0.3">
</p>

## Install Instructions:
* Run gacinstall.com from bin/debug or bin/release --> Will install the shape and the signed phonelib assembly to GAC and adds the shape to SSIS
* Use advanced editor on shape to map phone number input column

## Licenses:
* From Goolgles libphonenumber and libphonenumber-csharp are preserved in git root as txt files

## Disclaimer / Impressum

* Published under the MIT license
* Use at your own risk

<a href="https://github.com/ChrisMayor/Impressum">Impressum / Imprint in German language to comply with German tele-media regulations.</a>
