using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ICompleat.Objects
{
    public class CustomFields : JsonObject
    {
        #region Constructors

        public CustomFields(JsonElement json)
        {
            this.json = json;
        }

        #endregion Constructors

        #region Properties

        public string? fieldid
        {
            get { return json.GetProperty("Id").GetString(); }
        }

        public List<Field> Values
        {
            get { return this.json.GetProperty("Values").EnumerateArray().Select(x => new Field() { Code = x.GetProperty("Code").GetString(), Name = x.GetProperty("Name").GetString() }).ToList(); }
        }

        #endregion Properties

        #region Methods

        public static async Task<CustomFields> GetCustomFieldAsync(string fieldid)
        {
            var d = await Execucte($"api/customfield/{Config._instance.tenantId}/{Config._instance.companyId}/{fieldid}");

            return new CustomFields(d.GetProperty("CustomField"));
        }

        public static async Task<CustomFields[]> GetCustomFieldsAsync()
        {
            var d = await Execucte($"api/customfields/{Config._instance.tenantId}/{Config._instance.companyId}");

            List<CustomFields> s = new List<CustomFields>();
            foreach (JsonElement e in d.GetProperty("CustomFields").EnumerateArray())
            {
                s.Add(new CustomFields(e));
            }
            return s.ToArray();
        }

        public async Task AppendValues(List<Field> Values)
        {
            await ReplaceValues(this.Values.Union(Values).ToList());
        }

        public async Task LoadFull()
        {
            if (fieldid?.Length > 0)
            {
                json = (await GetCustomFieldAsync(fieldid)).json;
            }
            return;
        }

        public async Task ReplaceValues(List<Field> Values)
        {
            var d = await Execucte($"api/customfield/{Config._instance.tenantId}/{Config._instance.companyId}/{fieldid}", "POST", new { CustomFieldListItems = Values });
            await LoadFull();
        }

        #endregion Methods

        #region Classes

        public class Field
        {
            #region Properties

            public string Code { get; set; }
            public string Name { get; set; }

            #endregion Properties
        }

        #endregion Classes
    }
}