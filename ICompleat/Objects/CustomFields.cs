using System.Text.Json;

namespace ICompleat.Objects
{
    public class CustomFields : API_Whisperer.JsonObject
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

        public string? Name
        {
            get { return json.GetProperty("Name").GetString(); }
        }

        public List<Field> Values
        {
            get { return this.json.GetProperty("Values").EnumerateArray().Select(x => new Field() { Code = x.GetProperty("Code").GetString(), Name = x.GetProperty("Name").GetString() }).ToList(); }
        }

        #endregion Properties

        #region Methods

        public static async Task<CustomFields> GetCustomFieldAsync(string fieldid, Auth auth)
        {
            var req = new API_Whisperer.Request() { url = $"api/customfield/{auth.tenantId}/{auth.companyId}/{fieldid}" };
            var d = await req.Execute(auth);

            return new CustomFields(d.bodyAsJson.Value.GetProperty("CustomField"));
        }

        public static async Task<CustomFields[]> GetCustomFieldsAsync(Auth auth)
        {
            var req = new API_Whisperer.Request() { url = $"api/customfields/{auth.tenantId}/{auth.companyId}" };
            var d = await req.Execute(auth);

            List<CustomFields> s = new List<CustomFields>();
            foreach (JsonElement e in d.bodyAsJson.Value.GetProperty("CustomFields").EnumerateArray())
            {
                s.Add(new CustomFields(e));
            }
            return s.ToArray();
        }

        public async Task AppendValues(List<Field> Values, Auth auth)
        {
            await ReplaceValues(this.Values.Union(Values).ToList(), auth);
        }

        public async Task LoadFull(Auth auth)
        {
            if (fieldid?.Length > 0)
            {
                json = (await GetCustomFieldAsync(fieldid, auth)).json;
            }
            return;
        }

        public async Task ReplaceValues(List<Field> Values, Auth auth)
        {
            string continuationToken = "";

            for (int i = 0; i < Values.Count / 100.0f; i++)
            {
                var req = new API_Whisperer.Request()
                {
                    url = $"api/customfield/{auth.tenantId}/{auth.companyId}/{fieldid}",
                    method = "POST",
                    content = new { CustomFieldListItems = Values.Skip(100 * i).Take(100), ContinuationToken = continuationToken }
                };
                var d = await req.Execute(auth);
                continuationToken = d.bodyAsJson.Value.GetProperty("ContinuationToken").GetString();
            }
            await LoadFull(auth);
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