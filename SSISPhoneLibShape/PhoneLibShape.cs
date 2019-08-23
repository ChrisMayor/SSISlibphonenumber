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
using System.Linq;

namespace SSISPhoneLibShape
{

    [DtsPipelineComponent(DisplayName = "SSIS libphonenumber",
    ComponentType = ComponentType.Transform,
    IconResource = "SSISPhoneLibShape.PhoneLibIcon.ico",
    UITypeName = "SSISPhoneLibShape.SSISPhoneLibUi, SSISPhoneLibShape, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a232ba076cd66dc8")]
    public class PhoneLibShape : PipelineComponent
    {

        private bool areInputColumnsValid = true;
        private int[] inputBufferColumnIndex;
        private int[] outputBufferColumnIndex;
        private int _outputId;
        private int _errorOutId;
        private List<OutputColumn> _outputColumnList;
        private int _phonenumberLinage;
        private int _phoneNumberIsoLinage;
        private string _phoneNumberIsoCode;
        private PhonenumberColumnInfo _phneNumberColumnInfo;

        #region Design Time Methods

        //Design time - constructor
        public override void ProvideComponentProperties()
        {

            ComponentMetaData.Name = "SSIS libphonenumber";
            ComponentMetaData.Description = "A SSIS Data Flow Transformation Component to provide funtionality from Google lib phonenumbers csharp port.";
            ComponentMetaData.ContactInfo = "ChrisMayor at Github";

            base.RemoveAllInputsOutputsAndCustomProperties();

            // Add a single input
            var input = this.ComponentMetaData.InputCollection.New();
            input.Name = "Input";
            input.Description = "Input for the PhoneLib transformation";
            input.HasSideEffects = false; // Determines if component is left in data flow when run in OptimizedMode and output is not connected
            input.ErrorRowDisposition = DTSRowDisposition.RD_RedirectRow;
            input.ErrorOrTruncationOperation = "ValidationFailure";

            // Add a synchronous output
            var output = this.ComponentMetaData.OutputCollection.New();
            output.Name = "Output";
            output.Description = "Output for the PhoneLib transformation";
            output.HasSideEffects = false; // Determines if component is left in data flow when run in OptimizedMode and output is not connected
            output.SynchronousInputID = input.ID;
            output.ExclusionGroup = 1;

            // Add an error output
            var errorOutput = this.ComponentMetaData.OutputCollection.New();
            errorOutput.IsErrorOut = true;
            errorOutput.Name = "Error Output";
            errorOutput.Description = "Error Output for the PhoneLib transformation";
            errorOutput.HasSideEffects = false; // Determines if component is left in data flow when run in OptimizedMode and output is not connected
            errorOutput.SynchronousInputID = input.ID;
            errorOutput.ExclusionGroup = 1;

            //Add Componentproperties

            var iso2CountryText = ComponentMetaData.CustomPropertyCollection.New();
            iso2CountryText.Name = Constants.PhoneNumberIsoCodeColumn;
            iso2CountryText.Description = "Default Iso 2 Country Code";
            iso2CountryText.Value = null;

            var phoneNumberLinage = ComponentMetaData.CustomPropertyCollection.New();
            phoneNumberLinage.Name = Constants.PhoneNumberLinageColumn;
            phoneNumberLinage.Description = "Phone number column lineage";
            phoneNumberLinage.Value = -1;

            var phoneNumberIsoCodeLinage = ComponentMetaData.CustomPropertyCollection.New();
            phoneNumberIsoCodeLinage.Name = Constants.PhoneNumberIsoLinageColumn;
            phoneNumberIsoCodeLinage.Description = "Phone number iso column lineage";
            phoneNumberIsoCodeLinage.Value = -1;
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

            _phonenumberLinage = GetCustomPropertyValue<int>(Constants.PhoneNumberLinageColumn);
            _phoneNumberIsoLinage = GetCustomPropertyValue<int>(Constants.PhoneNumberIsoLinageColumn);
            _phoneNumberIsoCode = GetCustomPropertyValue<string>(Constants.PhoneNumberIsoCodeColumn);

            // check configuration
            if (-1 == _phonenumberLinage || (-1 == _phoneNumberIsoLinage && string.IsNullOrEmpty(_phoneNumberIsoCode)))
            {
                return DTSValidationStatus.VS_NEEDSNEWMETADATA;
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
                if (inputcolumn.LineageID == _phonenumberLinage)
                {
                    GenerateOutputColumns(output, inputcolumn);
                }
            }

            var outputColumnToRemove = new List<int>();
            foreach (IDTSOutputColumn100 outputColumn in output.OutputColumnCollection)
            {
                if (!_outputColumnList.Any(c => c.ColumnName == outputColumn.Name))
                {
                    outputColumnToRemove.Add(outputColumn.ID);

                }
            }
            outputColumnToRemove.ForEach(o => output.OutputColumnCollection.RemoveObjectByID(o));

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
            _phneNumberColumnInfo = new PhonenumberColumnInfo();
            _phneNumberColumnInfo.UsesIsoAsString = _phoneNumberIsoLinage == default;
            _phneNumberColumnInfo.PhoneNumberIsoString = _phoneNumberIsoCode;

            for (int x = 0; x < input.InputColumnCollection.Count; x++)
            {
                IDTSInputColumn100 column = input.InputColumnCollection[x];
                inputBufferColumnIndex[x] = BufferManager.FindColumnByLineageID(input.Buffer, column.LineageID);

                if (column.LineageID == _phonenumberLinage)
                {
                    _phneNumberColumnInfo.PhoneNumberBufferIndex = x;
                }
                else if (column.LineageID == _phoneNumberIsoLinage)
                {
                    _phneNumberColumnInfo.PhoneNumberIsoBufferIndex = x;
                }
            }

            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
            outputBufferColumnIndex = new int[output.OutputColumnCollection.Count];

            for (int x = 0; x < output.OutputColumnCollection.Count; x++)
            {
                IDTSOutputColumn100 outcol = output.OutputColumnCollection[x];
                var colreference = _outputColumnList.FirstOrDefault(c => c.ColumnName == outcol.Name);
                if (colreference != null)
                {
                    colreference.ColumnIndex = BufferManager.FindColumnByLineageID(input.Buffer, outcol.LineageID);
                }
                //This is the key - synchronous output does not appear in output buffer, but in input buffer
                outputBufferColumnIndex[x] = BufferManager.FindColumnByLineageID(input.Buffer, outcol.LineageID);
            }

            _outputId = output.ID;

            foreach (IDTSOutput100 outputcoll in ComponentMetaData.OutputCollection)
            {
                if (outputcoll.IsErrorOut)
                {
                    _errorOutId = outputcoll.ID;
                    break;
                }
            }

        }

        //Run Time - Validate Phone Number
        public override void ProcessInput(int inputID, PipelineBuffer buffer)
        {
            if (!buffer.EndOfRowset)
            {
                while (buffer.NextRow())
                {
                    try
                    {
                        var phoneNumberColumnIndex = _phneNumberColumnInfo.PhoneNumberBufferIndex;
                        var bufferColDataType = buffer.GetColumnInfo(inputBufferColumnIndex[_phneNumberColumnInfo.PhoneNumberBufferIndex]).DataType;
                        var parsedPhoneNumberResult = GetParsedPhoneNumberResult(buffer, phoneNumberColumnIndex, bufferColDataType);
                        SetPhoneNumberResultValuesToOutput(buffer, phoneNumberColumnIndex, parsedPhoneNumberResult);

                        buffer.DirectRow(_outputId);
                    }
                    catch (Exception e)
                    {
                        buffer.DirectErrorRow(_errorOutId, 100, _phonenumberLinage);
                    }

                }
            }

        }

        private void SetPhoneNumberResultValuesToOutput(PipelineBuffer buffer, int x, ParsedPhoneNumber parsedPhoneNumberResult)
        {
            foreach (var outputColumn in _outputColumnList)
            {
                var colindex = outputColumn.ColumnIndex;
                switch (outputColumn.Name)
                {
                    case PhoneLibMethodConstants.CountryCode:
                        buffer.SetInt32(colindex, parsedPhoneNumberResult.CountryCode);
                        break;
                    case PhoneLibMethodConstants.E164Format:
                        buffer.SetString(colindex, parsedPhoneNumberResult.E164Format);
                        break;
                    case PhoneLibMethodConstants.ExtractPossibleNumber:
                        buffer.SetString(colindex, parsedPhoneNumberResult.ExtractPossibleNumber);
                        break;
                    case PhoneLibMethodConstants.GeoCoderDescription:
                        buffer.SetString(colindex, parsedPhoneNumberResult.GeoCoderDescription);
                        break;
                    case PhoneLibMethodConstants.HasCountryCode:
                        buffer.SetBoolean(colindex, parsedPhoneNumberResult.HasCountryCode);
                        break;
                    case PhoneLibMethodConstants.IntFormat:
                        buffer.SetString(colindex, parsedPhoneNumberResult.IntFormat);
                        break;
                    case PhoneLibMethodConstants.IsValidNumber:
                        buffer.SetBoolean(colindex, parsedPhoneNumberResult.IsValidNumber);
                        break;
                    case PhoneLibMethodConstants.IsViablePhoneNumber:
                        if (parsedPhoneNumberResult.IsViablePhoneNumber.HasValue)
                        {
                            buffer.SetBoolean(colindex, parsedPhoneNumberResult.IsViablePhoneNumber.Value);
                        }
                        else
                        {
                            buffer.SetBoolean(colindex, false);
                        }
                        break;
                    case PhoneLibMethodConstants.NormalizedDigitsOnly:
                        buffer.SetString(colindex, parsedPhoneNumberResult.NormalizedDigitsOnly);
                        break;
                    case PhoneLibMethodConstants.NormalizedNumber:
                        buffer.SetString(colindex, parsedPhoneNumberResult.NormalizedNumber);
                        break;
                    case PhoneLibMethodConstants.PreferredDomesticCarrierCode:
                        buffer.SetString(colindex, parsedPhoneNumberResult.PreferredDomesticCarrierCode);
                        break;
                    case PhoneLibMethodConstants.NumberType:
                        buffer.SetString(colindex, parsedPhoneNumberResult.NumberType);
                        break;
                }

            }
        }

        private ParsedPhoneNumber GetParsedPhoneNumberResult(PipelineBuffer buffer, int phonenumberColumnIndex, DataType BufferColDataType)
        {
            var defaultIsoCode = string.Empty;
            if (_phneNumberColumnInfo.UsesIsoAsString)
            {
                defaultIsoCode = _phneNumberColumnInfo.PhoneNumberIsoString;
            }
            else
            {
                defaultIsoCode = buffer.GetString(inputBufferColumnIndex[_phneNumberColumnInfo.PhoneNumberIsoBufferIndex]);
                // Fallback
                if (string.IsNullOrEmpty(defaultIsoCode))
                {
                    defaultIsoCode = _phneNumberColumnInfo.PhoneNumberIsoString;
                }
            }

            if (BufferColDataType == DataType.DT_STR ||
                BufferColDataType == DataType.DT_WSTR)
            {
                return IsPhoneNumberValid(buffer.GetString(inputBufferColumnIndex[phonenumberColumnIndex]), defaultIsoCode);
            }
            else if (BufferColDataType == DataType.DT_NUMERIC ||
                    BufferColDataType == DataType.DT_DECIMAL)
            {

                return IsPhoneNumberValid(buffer.GetDecimal(inputBufferColumnIndex[phonenumberColumnIndex]).ToString(), defaultIsoCode);
            }

            throw new Exception("Invaid DataType");

        }

        #endregion Run Time Methods

        private void GenerateOutputColumns(IDTSOutput100 output, IDTSInputColumn100 inputcolumn)
        {
            GeneratePhoneNumbersOutputList(inputcolumn);
            foreach (var outputColumn in _outputColumnList)
            {
                GenerateOutputColumn(output, inputcolumn, outputColumn);
            }
        }

        private static void GenerateOutputColumn(IDTSOutput100 output, IDTSInputColumn100 inputcolumn, OutputColumn outputColumn)
        {
            bool IsExist = false;
            foreach (IDTSOutputColumn100 OutputColumn in output.OutputColumnCollection)
            {
                if (OutputColumn.Name == outputColumn.ColumnName)
                {
                    IsExist = true;
                }
            }

            if (!IsExist)
            {
                IDTSOutputColumn100 outputcol = output.OutputColumnCollection.New();
                outputcol.Name = outputColumn.ColumnName;
                outputcol.Description = $"PhoneLib call to {outputColumn.Name}";
                outputcol.SetDataTypeProperties(outputColumn.DataType, outputColumn.DataTypeLength, 0, 0, 0);
            }
        }

        private void GeneratePhoneNumbersOutputList(IDTSInputColumn100 inputcolumn)
        {
            _outputColumnList = new List<OutputColumn>();
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_IsViablePhoneNumber " + inputcolumn.Name, DataType = DataType.DT_BOOL, DataTypeLength = 0, Name = PhoneLibMethodConstants.IsViablePhoneNumber });
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_ExtractPossibleNumber " + inputcolumn.Name, DataType = DataType.DT_WSTR, DataTypeLength = 255, Name = PhoneLibMethodConstants.ExtractPossibleNumber });
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_NormalizedNumber " + inputcolumn.Name, DataType = DataType.DT_WSTR, DataTypeLength = 255, Name = PhoneLibMethodConstants.NormalizedNumber });
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_NormalizedDigitsOnly " + inputcolumn.Name, DataType = DataType.DT_WSTR, DataTypeLength = 255, Name = PhoneLibMethodConstants.NormalizedDigitsOnly });
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_E164Format " + inputcolumn.Name, DataType = DataType.DT_WSTR, DataTypeLength = 255, Name = PhoneLibMethodConstants.E164Format });
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_IntFormat " + inputcolumn.Name, DataType = DataType.DT_WSTR, DataTypeLength = 255, Name = PhoneLibMethodConstants.IntFormat });
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_IsValidNumber " + inputcolumn.Name, DataType = DataType.DT_BOOL, DataTypeLength = 0, Name = PhoneLibMethodConstants.IsValidNumber });
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_CountryCode " + inputcolumn.Name, DataType = DataType.DT_I4, DataTypeLength = 0, Name = PhoneLibMethodConstants.CountryCode });
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_HasCountryCode " + inputcolumn.Name, DataType = DataType.DT_BOOL, DataTypeLength = 0, Name = PhoneLibMethodConstants.HasCountryCode });
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_PreferredDomesticCarrierCode " + inputcolumn.Name, DataType = DataType.DT_WSTR, DataTypeLength = 255, Name = PhoneLibMethodConstants.PreferredDomesticCarrierCode });
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_GeoCoderDescription " + inputcolumn.Name, DataType = DataType.DT_WSTR, DataTypeLength = 255, Name = PhoneLibMethodConstants.GeoCoderDescription });
            _outputColumnList.Add(new OutputColumn { ColumnName = $"PhoneLib_NumberType " + inputcolumn.Name, DataType = DataType.DT_WSTR, DataTypeLength = 255, Name = PhoneLibMethodConstants.NumberType });
        }

        //phonelib calls
        public ParsedPhoneNumber IsPhoneNumberValid(string phoneNumber, string defaultIsoCode)
        {
            var parsedNumber = new ParsedPhoneNumber();
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                parsedNumber.IsViablePhoneNumber = PhoneNumberUtil.IsViablePhoneNumber(phoneNumber);
                parsedNumber.ExtractPossibleNumber = PhoneNumberUtil.ExtractPossibleNumber(phoneNumber);
                parsedNumber.NormalizedNumber = PhoneNumberUtil.Normalize(phoneNumber);
                parsedNumber.NormalizedDigitsOnly = PhoneNumberUtil.NormalizeDigitsOnly(phoneNumber);
                if (parsedNumber.IsViablePhoneNumber.Value)
                {
                    var numberObject = phoneNumberUtil.Parse(phoneNumber, defaultIsoCode);
                    parsedNumber.E164Format = phoneNumberUtil.Format(numberObject, PhoneNumberFormat.E164);
                    parsedNumber.IntFormat = phoneNumberUtil.Format(numberObject, PhoneNumberFormat.INTERNATIONAL);
                    parsedNumber.IsValidNumber = phoneNumberUtil.IsValidNumber(numberObject);
                    parsedNumber.CountryCode = numberObject.CountryCode;
                    parsedNumber.HasCountryCode = numberObject.HasCountryCode;
                    parsedNumber.PreferredDomesticCarrierCode = numberObject.PreferredDomesticCarrierCode;
                    var geocoder = PhoneNumbers.PhoneNumberOfflineGeocoder.GetInstance();
                    parsedNumber.GeoCoderDescription = geocoder.GetDescriptionForNumber(numberObject, PhoneNumbers.Locale.English);
                    parsedNumber.NumberType = phoneNumberUtil.GetNumberType(numberObject).ToString();
                }
            }

            return parsedNumber;
        }

        private T GetCustomPropertyValue<T>(string propertyName)
        {
            if (ComponentMetaData.CustomPropertyCollection[propertyName] != null && ComponentMetaData.CustomPropertyCollection[propertyName].Value != null)
            {
                return (T)ComponentMetaData.CustomPropertyCollection[propertyName].Value;
            }
            else
            {
                return default(T);
            }
        }





    }
}
