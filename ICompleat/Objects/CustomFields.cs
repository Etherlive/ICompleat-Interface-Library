using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ICompleat.Objects
{
    public class CustomFields : JsonObject
    {
        #region Properties

        public string? fieldid
        {
            get { return json.GetProperty("Id").GetString(); }
        }

        #endregion Properties

        #region Methods

        public static async Task<CustomFields> GetCustomFieldAsync(string fieldid)
        {
            var d = await Execucte($"api/customfield/{Config._instance.tenantId}/{Config._instance.companyId}/{fieldid}");

            return new CustomFields() { json = d.GetProperty("CustomField") };
        }

        public static async Task<CustomFields[]> GetCustomFieldsAsync()
        {
            var d = await Execucte($"api/customfields/{Config._instance.tenantId}/{Config._instance.companyId}");

            List<CustomFields> s = new List<CustomFields>();
            foreach (JsonElement e in d.GetProperty("CustomFields").EnumerateArray())
            {
                s.Add(new CustomFields() { json = e });
            }
            return s.ToArray();
        }

        public async Task LoadFull()
        {
            if (fieldid?.Length > 0)
            {
                json = (await GetCustomFieldAsync(fieldid)).json;
            }
            return;
        }

        #endregion Methods
    }
}