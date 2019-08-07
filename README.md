** Currenty in work in progress - not yet ready to use **

# SSIS libphonenumber - A phone number parsing and normalization SSIS Pipeline Component
SSIS pipeline transformation shape, which provides some phone number parsing functionality by implementing the Google libphonenumber csharp port https://github.com/twcclegg/libphonenumber-csharp (v8.10.16)

## Highlights:
* SQL Server 2016/2017 data flow custom Shape
* Provides some functionality of Googles libphonenumber (using its libphonenumber-csharp port) for SQL Server 2016/2017
* SSIS Pipeline transformation shape
* Googles libphonenumber is great - runs on premise and also on your android phone (see https://github.com/google/libphonenumber)
* Transform your unformatted phone numbers to a normalized format (e.g. for your CRM system or for skype integration...)
* Tries to parse strings to phone numbers
* Can lookup the carrier code and offline geo location
* Tested with Visual Studio 2019 SSIS Extension

## Currently implemented functionality V0.01:
* Call to IsViablePhoneNumber to check if the phone number is viable

## Planned implementation of functionality from libphonenumber for V1.0:
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

## Work done:
* Added Gac cmd script
* Wrote a lot of code
* Did first Tests in SSIS
* Signed libphonenumber-csharp and added assembly to project (v8.10.16) in order to use the library in SSIS
* Prepared the SSIS UI

## Screenshot V0.1

<p align="center">
  <img src="../master/Screenshots/1_Capture_V0.1.jpg" title="SSIS Dataflow with shape V0.1">
</p>

## Install Instructions:
* Run gacinstall.com from bin/debug or bin/release --> Will install the shape and the signed phonelib assembly to GAC and adds the shape to SSIS

## Licenses:
* From Goolgle libphonenumber and libphonenumber-csharp are preserved in git root as txt file

## Disclaimer / Impressum

* Published under the MIT license
* Use at your own risk

<a href="https://github.com/ChrisMayor/Impressum">Impressum / Imprint in German language to comply with German tele-media regulations.</a>
