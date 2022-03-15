using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ICompleat.Objects
{
    public class TransactionLine : API_Whisperer.JsonObject
    {
        #region Properties

        public string Code
        {
            get { return json.GetProperty("Code").GetString(); }
        }

        public string Description
        {
            get { return json.GetProperty("Description").GetString(); }
        }

        public double Gross
        {
            get { return json.GetProperty("Gross").GetProperty("Local").GetProperty("Value").GetDouble(); }
        }

        public string Id
        {
            get { return json.GetProperty("Id").GetString(); }
        }

        public double Net
        {
            get { return json.GetProperty("Net").GetProperty("Local").GetProperty("Value").GetDouble(); }
        }

        public int Number
        {
            get { return json.GetProperty("Number").GetInt32(); }
        }

        public double Quantity
        {
            get { return json.GetProperty("Quantity").GetDouble(); }
        }

        public double UnitCost
        {
            get { return json.GetProperty("UnitCost").GetProperty("Local").GetProperty("Value").GetDouble(); }
        }

        public double Vat
        {
            get { return json.GetProperty("Vat").GetProperty("Local").GetProperty("Value").GetDouble(); }
        }

        #endregion Properties
    }
}