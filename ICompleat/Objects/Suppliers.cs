using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ICompleat.Objects
{
    public class Supplier : JsonObject
    {
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