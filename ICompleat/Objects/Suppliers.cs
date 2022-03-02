using System.Text.Json;

namespace ICompleat.Objects
{
    public class Supplier : JsonObject
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

        public static async Task<Supplier[]> GetSuppliersAsync(int page = 1)
        {
            var d = await Execucte($"/api/suppliers/{Config._instance.tenantId}/{Config._instance.companyId}/{page}");

            List<Supplier> s = new List<Supplier>();
            foreach (JsonElement e in d.GetProperty("Suppliers").EnumerateArray())
            {
                s.Add(new Supplier() { json = e });
            }
            return s.ToArray();
        }

        public static async Task<Supplier[]> GetSuppliersUntillAllAsync()
        {
            List<Supplier> s = new List<Supplier>();

            int i = 1;
            while (s.Count % 100 == 0 || i == 1)
            {
                var d = await Execucte($"/api/suppliers/{Config._instance.tenantId}/{Config._instance.companyId}/{i}");

                var sup = d.GetProperty("Suppliers");
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