using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using PhoneNumbers;

namespace SSISPhoneLibShape
{

    [DtsPipelineComponent(DisplayName = "PhoneLibShape",
    ComponentType = ComponentType.Transform,
    CurrentVersion = 1)]
    public class PhoneLibShape : PipelineComponent
    {

        private bool areInputColumnsValid = true;
        private int[] inputBufferColumnIndex;
        private int[] outputBufferColumnIndex;

        #region Design Time Methods

        //Design time - constructor
        public override void ProvideComponentProperties()
        {
            // Set component information
            ComponentMetaData.Name = "PhoneLibShape";
            ComponentMetaData.Description = "A SSIS Data Flow Transformation Component to provide funtionality from Google lib phonenumbers csharp port.";
            ComponentMetaData.ContactInfo = "ChrisMayor @ Github";


            // Reset the component.
            base.RemoveAllInputsOutputsAndCustomProperties();

            // Add input objects
            IDTSInput100 input = ComponentMetaData.InputCollection.New();
            input.Name = "Input";

            // Add output objects.
            IDTSOutput100 output = ComponentMetaData.OutputCollection.New();
            output.Name = "Output";
            output.SynchronousInputID = input.ID; //Synchronous transformation

            //Add error objects
            IDTSOutput100 errorOutput = ComponentMetaData.OutputCollection.New();
            errorOutput.Name = "Error";
            errorOutput.IsErrorOut = true;
        }

        //Design time - Metadata Validataor
        public override DTSValidationStatus Validate()
        {
            bool pbCancel = false;

            // Validate that there is only one input.
            if (ComponentMetaData.InputCollection.Count != 1)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Incorrect number of inputs.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            // Validate number of outputs.
            if (ComponentMetaData.OutputCollection.Count != 1)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Incorrect number of outputs.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            // Determine whether the metdada needs refresh
            IDTSInput100 input = ComponentMetaData.InputCollection[0];
            IDTSVirtualInput100 vInput = input.GetVirtualInput();

            bool cancel = false;

            foreach (IDTSInputColumn100 column in input.InputColumnCollection)
            {
                try
                {
                    IDTSVirtualInputColumn100 vColumn = vInput.VirtualInputColumnCollection.GetVirtualInputColumnByLineageID(column.LineageID);
                }
                catch
                {
                    ComponentMetaData.FireError(0, ComponentMetaData.Name, "The input column " + column.IdentificationString + " does not match a column in the upstream component.", "", 0, out cancel);
                    areInputColumnsValid = false;
                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                }

            }

            //validate input to be of type string/numeric only
            for (int x = 0; x < input.InputColumnCollection.Count; x++)
            {
                if (!(input.InputColumnCollection[x].DataType == DataType.DT_STR ||
                    input.InputColumnCollection[x].DataType == DataType.DT_WSTR ||
                    input.InputColumnCollection[x].DataType == DataType.DT_DECIMAL ||
                    input.InputColumnCollection[x].DataType == DataType.DT_NUMERIC))
                {
                    ComponentMetaData.FireError(0, ComponentMetaData.Name, "Invalid Data Type specified for " + input.InputColumnCollection[x].Name
                                                        + ". Supported Data Types are DT_STR,DT_WSTR,DT_NUMERIC and DT_NUMERIC", "", 0, out pbCancel);
                    return DTSValidationStatus.VS_ISCORRUPT;
                }
            }

            //create corresponding output columns dynamically
            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];

            foreach (IDTSInputColumn100 inputcolumn in input.InputColumnCollection)
            {
                bool IsExist = false;
                foreach (IDTSOutputColumn100 OutputColumn in output.OutputColumnCollection)
                {
                    if (OutputColumn.Name == "IsValidNumber " + inputcolumn.Name)
                    {
                        IsExist = true;
                    }
                }

                if (!IsExist)
                {
                    IDTSOutputColumn100 outputcol = output.OutputColumnCollection.New();
                    outputcol.Name = "IsValidNumber " + inputcolumn.Name;
                    outputcol.Description = "Indicates whether " + inputcolumn.Name + " is a Valid Phone Number";
                    outputcol.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
                }
            }

            //Remove redundant output columns that don't match input columns
            if (output.OutputColumnCollection.Count > input.InputColumnCollection.Count)
            {
                foreach (IDTSOutputColumn100 OutputColumn in output.OutputColumnCollection)
                {
                    Boolean IsRedundant = true;
                    foreach (IDTSInputColumn100 InputCoulmn in input.InputColumnCollection)
                    {
                        IsRedundant = OutputColumn.Name.Contains(InputCoulmn.Name) ? false : true;
                        if (!IsRedundant)
                            break;
                    }

                    if (IsRedundant)
                    {
                        output.OutputColumnCollection.RemoveObjectByID(OutputColumn.ID);
                    }
                }
            }

            return DTSValidationStatus.VS_ISVALID;
        }

        //Design Time - method to autocorrect VS_NEEDSNEWMETADATA error
        public override void ReinitializeMetaData()
        {
            if (!areInputColumnsValid)
            {
                IDTSInput100 input = ComponentMetaData.InputCollection[0];
                IDTSVirtualInput100 vInput = input.GetVirtualInput();

                foreach (IDTSInputColumn100 column in input.InputColumnCollection)
                {
                    IDTSVirtualInputColumn100 vColumn = vInput.VirtualInputColumnCollection.GetVirtualInputColumnByLineageID(column.LineageID);

                    if (vColumn == null)
                        input.InputColumnCollection.RemoveObjectByID(column.ID);
                }
                areInputColumnsValid = true;
            }

        }

        //Override InsertOutputColumnAt to prevent addition of new column from Advanced Editor
        public override IDTSOutputColumn100 InsertOutputColumnAt(
             int outputID,
             int outputColumnIndex,
             string name,
             string description)
        {
            bool cancel = true;
            ComponentMetaData.FireError(0, ComponentMetaData.Name, "Output columns cannot be added to " + ComponentMetaData.Name, "", 0, out cancel);
            //bubble-up the error to VS
            throw new Exception("Output columns cannot be added to " + ComponentMetaData.Name, null);
        }

        #endregion Design Time Methods

        #region Run Time Methods
        //Run Time - Pre Execute identifying the input columns in this component from Buffer Manager
        public override void PreExecute()
        {
            IDTSInput100 input = ComponentMetaData.InputCollection[0];
            inputBufferColumnIndex = new int[input.InputColumnCollection.Count];

            for (int x = 0; x < input.InputColumnCollection.Count; x++)
            {
                IDTSInputColumn100 column = input.InputColumnCollection[x];
                inputBufferColumnIndex[x] = BufferManager.FindColumnByLineageID(input.Buffer, column.LineageID);
            }

            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
            outputBufferColumnIndex = new int[output.OutputColumnCollection.Count];

            for (int x = 0; x < output.OutputColumnCollection.Count; x++)
            {
                IDTSOutputColumn100 outcol = output.OutputColumnCollection[x];
                //This is the key - synchronous output does not appear in output buffer, but in input buffer
                outputBufferColumnIndex[x] = BufferManager.FindColumnByLineageID(input.Buffer, outcol.LineageID);
            }

        }

        //Run Time - Validate Phone Number
        public override void ProcessInput(int inputID, PipelineBuffer buffer)
        {
            if (!buffer.EndOfRowset)
            {
                while (buffer.NextRow())
                {
                    for (int x = 0; x < inputBufferColumnIndex.Length; x++)
                    {
                        bool IsValid = false;
                        DataType BufferColDataType;

                        BufferColDataType = buffer.GetColumnInfo(inputBufferColumnIndex[x]).DataType;

                        if (BufferColDataType == DataType.DT_STR ||
                            BufferColDataType == DataType.DT_WSTR)
                        {
                            IsValid = IsPhoneNumberValid(buffer.GetString(inputBufferColumnIndex[x])).IsViablePhoneNumber.Value;
                        }
                        else if (BufferColDataType == DataType.DT_NUMERIC ||
                                BufferColDataType == DataType.DT_DECIMAL)
                        {

                            IsValid = IsPhoneNumberValid(buffer.GetDecimal(inputBufferColumnIndex[x]).ToString()).IsViablePhoneNumber.Value;
                        }



                        buffer.SetBoolean(outputBufferColumnIndex[x], IsValid);
                    }
                }
            }

        }

        #endregion Run Time Methods




        //phonelib calls
        public ParsedPhoneNumber IsPhoneNumberValid(string phoneNumber)
        {
            var parsedNumber = new ParsedPhoneNumber();

            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            parsedNumber.IsViablePhoneNumber = PhoneNumberUtil.IsViablePhoneNumber(phoneNumber);
            parsedNumber.ExtractPossibleNumber = PhoneNumberUtil.ExtractPossibleNumber(phoneNumber);
            parsedNumber.NormalizedNumber = PhoneNumberUtil.Normalize(phoneNumber);
            parsedNumber.NormalizedDigitsOnly = PhoneNumberUtil.NormalizeDigitsOnly(phoneNumber);
            if (parsedNumber.IsViablePhoneNumber.Value)
            {
                var numberObject = phoneNumberUtil.Parse(phoneNumber, "DE");
                parsedNumber.E164Format = phoneNumberUtil.Format(numberObject, PhoneNumberFormat.E164);
                parsedNumber.IntFormat = phoneNumberUtil.Format(numberObject, PhoneNumberFormat.INTERNATIONAL);
                parsedNumber.IsValidNumber = phoneNumberUtil.IsValidNumber(numberObject);
                parsedNumber.CountryCode = numberObject.CountryCode;
                parsedNumber.HasCountryCode = numberObject.HasCountryCode;
                parsedNumber.PreferredDomesticCarrierCode = numberObject.PreferredDomesticCarrierCode;
                var geocoder = PhoneNumbers.PhoneNumberOfflineGeocoder.GetInstance();
                parsedNumber.GeoCoderDescription = geocoder.GetDescriptionForNumber(numberObject, PhoneNumbers.Locale.English);
            }

            return parsedNumber;
        }




    }
}
