using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ICompleat.Objects
{
    public class Transaction : JsonObject
    {
        #region Properties

        public bool IsApproved
        {
            get { return Status == "APPR"; }
        }

        public bool IsDenied
        {
            get { return Status == "DEN"; }
        }
        public bool IsPending
        {
            get { return Status == "PEND"; }
        }

        public bool IsSaved
        {
            get { return Status == "SAV"; }
        }


        public bool IsCreditNote
        {
            get { return Type == "CRD"; }
        }

        public bool IsDeleted
        {
            get { return Status == "DEL"; }
        }

        public bool IsInvoice
        {
            get { return Type == "INV"; }
        }

        public bool IsOrder
        {
            get { return Type == "ORD"; }
        }

        public string Status
        {
            get { return json.GetProperty("Status").GetString(); }
        }

        public string SupplierName
        {
            get { return json.GetProperty("SupplierName").GetString(); }
        }

        public string Title
        {
            get { return json.GetProperty("Title").GetString(); }
        }

        public string Type
        {
            get { return json.GetProperty("Type").GetString(); }
        }

        #endregion Properties

        #region Methods

        public static async Task<Transaction[]> GetTransactionsAsync(int page = 1)
        {
            ///api/transactions/{tenantId}/{pageNumber}/{companyId}/{userId}/{status}/{transactionType}
            var d = await Execucte($"/api/transactions/{Config._instance.tenantId}/{page}/null/null/null/null");

            List<Transaction> s = new List<Transaction>();
            foreach (JsonElement e in d.GetProperty("Transactions").EnumerateArray())
            {
                s.Add(new Transaction() { json = e });
            }
            return s.ToArray();
        }

        public static async Task<Transaction[]> GetTransactionsUntillAllAsync()
        {
            List<Transaction> s = new List<Transaction>();

            int i = 1;
            while (s.Count % 20 == 0 || i == 1)
            {
                var d = await Execucte($"/api/transactions/{Config._instance.tenantId}/{i}/null/null/null/null");

                var sup = d.GetProperty("Transactions");
                if (sup.GetArrayLength() == 0) break;

                foreach (JsonElement e in sup.EnumerateArray())
                {
                    s.Add(new Transaction() { json = e });
                }
                i++;
            }

            return s.ToArray();
        }

        #endregion Methods
    }
}