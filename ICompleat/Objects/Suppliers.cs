using System.Text.Json;
using API_Whisperer;

namespace ICompleat.Objects
{
    public class Supplier : API_Whisperer.JsonObject
    {
        #region Properties

        public string AddressLine1
        {
            get { return json.TryGetProperty("AddressLine1", out JsonElement e) ? e.GetString() : ""; }
        }

        public string AddressLine2
        {
            get { return json.TryGetProperty("AddressLine2", out JsonElement e) ? e.GetString() : ""; }
        }

        public string BankAccountNumber
        {
            get { return json.TryGetProperty("Name", out JsonElement e) ? e.GetString() : ""; }
        }

        public string BankName
        {
            get { return json.TryGetProperty("Name", out JsonElement e) ? e.GetString() : ""; }
        }

        public string BankSortCode
        {
            get { return json.TryGetProperty("Name", out JsonElement e) ? e.GetString() : ""; }
        }

        public string Code
        {
            get { return json.TryGetProperty("Code", out JsonElement e) ? e.GetString() : ""; }
        }

        public string Country
        {
            get { return json.TryGetProperty("Country", out JsonElement e) ? e.GetString() : ""; }
        }

        public string Email
        {
            get { return json.TryGetProperty("Name", out JsonElement e) ? e.GetString() : ""; }
        }

        public string IsoCode
        {
            get { return json.TryGetProperty("IsoCode", out JsonElement e) ? e.GetString() : ""; }
        }

        public string Name
        {
            get { return json.TryGetProperty("Name", out JsonElement e) ? e.GetString() : ""; }
        }

        public string PaymentDueDays
        {
            get { return json.TryGetProperty("Name", out JsonElement e) ? e.GetString() : ""; }
        }

        public string PaymentTermsType
        {
            get { return json.TryGetProperty("Name", out JsonElement e) ? e.GetString() : ""; }
        }

        public string PostcodeOrZip
        {
            get { return json.TryGetProperty("PostcodeOrZip", out JsonElement e) ? e.GetString() : ""; }
        }

        public string StateOrCounty
        {
            get { return json.TryGetProperty("StateOrCounty", out JsonElement e) ? e.GetString() : ""; }
        }

        public string Telephone
        {
            get { return json.TryGetProperty("Telephone", out JsonElement e) ? e.GetString() : ""; }
        }

        #endregion Properties

        #region Methods

        public static async Task<Supplier[]> GetSuppliersAsync(Auth auth, int page = 1)
        {
            var req = new API_Whisperer.Request() { url = $"/api/suppliers/{auth.tenantId}/{auth.companyId}/{page}" };
            var d = await req.Execute(auth);

            List<Supplier> s = new List<Supplier>();
            foreach (JsonElement e in d.bodyAsJson.Value.GetProperty("Suppliers").EnumerateArray())
            {
                s.Add(new Supplier() { json = e });
            }
            return s.ToArray();
        }

        public static async Task<Supplier[]> GetSuppliersUntillAllAsync(Auth auth)
        {
            List<Supplier> s = new List<Supplier>();

            int i = 1;
            while (s.Count % 100 == 0 || i == 1)
            {
                var req = new Request() { url = $"/api/suppliers/{auth.tenantId}/{auth.companyId}/{i}" };
                var d = await req.Execute(auth);

                var sup = d.bodyAsJson.Value.GetProperty("Suppliers");
                if (sup.GetArrayLength() == 0) break;

                foreach (JsonElement e in sup.EnumerateArray())
                {
                    s.Add(new Supplier() { json = e });
                }
                i++;
            }

            return s.ToArray();
        }

        #endregion Methods
    }
}