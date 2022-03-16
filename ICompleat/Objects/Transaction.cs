using System.Text.Json;
using System.Linq;

namespace ICompleat.Objects
{
    public class Transaction : API_Whisperer.JsonObject
    {
        #region Properties

        public DateTime CreatedDate
        {
            get { return json.GetProperty("Dates").TryGetProperty("Created", out JsonElement e) && e.ValueKind != JsonValueKind.Undefined ? e.TryGetDateTime(out DateTime d) ? d : DateTime.Now : DateTime.Now; }
        }

        public DateTime DeliveryDate
        {
            get { return json.GetProperty("Dates").TryGetProperty("Delivery", out JsonElement e) && e.ValueKind != JsonValueKind.Undefined ? e.TryGetDateTime(out DateTime d) ? d : DateTime.Now : DateTime.Now; }
        }

        public string Id
        {
            get { return json.GetProperty("Id").GetString(); }
        }

        public string IdentifierReference
        {
            get { return json.GetProperty("References").GetProperty("Identifier").GetString(); }
        }

        public DateTime InvoiceDate
        {
            get { return json.GetProperty("Dates").TryGetProperty("Invoice", out JsonElement e) && e.ValueKind != JsonValueKind.Undefined ? e.TryGetDateTime(out DateTime d) ? d : DateTime.Now : DateTime.Now; }
        }

        public bool IsApproved
        {
            get { return Status == "APPR"; }
        }

        public bool IsCreditNote
        {
            get { return Type == "CRD"; }
        }

        public bool IsDeleted
        {
            get { return Status == "DEL"; }
        }

        public bool IsDenied
        {
            get { return Status == "DEN"; }
        }

        public bool IsInvoice
        {
            get { return Type == "INV"; }
        }

        public bool IsOrder
        {
            get { return Type == "ORD"; }
        }

        public bool IsPending
        {
            get { return Status == "PEND"; }
        }

        public bool IsSaved
        {
            get { return Status == "SAV"; }
        }

        public string JobId
        {
            get
            {
                if (json.TryGetProperty("Analysis", out JsonElement e))
                {
                    foreach (var layoutElements in e.EnumerateArray())
                    {
                        if (layoutElements.GetProperty("Name").GetString().Contains("Job"))
                        {
                            string id = layoutElements.GetProperty("Value").GetProperty("Code").GetString();
                            if (int.TryParse(id, out int i))
                            {
                                return i == 0 ? null : id;
                            }
                        }
                    }
                }
                return null;
            }
        }

        public TransactionLine[] lines
        {
            get { return json.TryGetProperty("Lines", out JsonElement e) ? e.EnumerateArray().Select(x => new TransactionLine() { json = x }).ToArray() : new TransactionLine[0]; }
        }

        public DateTime PaymentDueDate
        {
            get { return json.GetProperty("Dates").TryGetProperty("PaymentDue", out JsonElement e) && e.ValueKind != JsonValueKind.Undefined ? e.TryGetDateTime(out DateTime d) ? d : DateTime.Now : DateTime.Now; }
        }

        public string PurchaseOrderReference
        {
            get { return json.GetProperty("References").GetProperty("PurchaseOrder").GetString(); }
        }

        public string Status
        {
            get { return json.GetProperty("Status").GetString(); }
        }

        public string SupplierName
        {
            get { return json.TryGetProperty("Supplier", out JsonElement e) ? e.GetProperty("Name").GetString() : ""; }
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

        public static async Task<Transaction[]> GetTransactionsAsync(Auth auth, int page = 1)
        {
            ///api/transactions/{tenantId}/{pageNumber}/{companyId}/{userId}/{status}/{transactionType}
            var req = new API_Whisperer.Request() { url = $"/api/transactions/{auth.tenantId}/{page}/null/null/null/null" };
            var d = await req.Execute(auth);

            List<Transaction> s = new List<Transaction>();
            foreach (JsonElement e in d.bodyAsJson.Value.GetProperty("Transactions").EnumerateArray())
            {
                s.Add(new Transaction() { json = e });
            }
            return s.ToArray();
        }

        public static async Task<Transaction[]> GetTransactionsUntillAllAsync(Auth auth)
        {
            List<Transaction> s = new List<Transaction>();

            int i = 1;
            while (s.Count % 20 == 0 || i == 1)
            {
                var req = new API_Whisperer.Request() { url = $"/api/transactions/{auth.tenantId}/{i}/null/null/null/null" };
                var d = await req.Execute(auth);

                var sup = d.bodyAsJson.Value.GetProperty("Transactions");
                if (sup.GetArrayLength() == 0) break;

                foreach (JsonElement e in sup.EnumerateArray())
                {
                    s.Add(new Transaction() { json = e });
                }
                i++;
            }

            return s.ToArray();
        }

        public async Task<bool> LoadDetail(Auth auth)
        {
            var req = new API_Whisperer.Request() { url = $"/api/transaction/{auth.tenantId}/{Id}/null" };
            var d = await req.Execute(auth, false);
            if (d.isSuccess)
            {
                json = d.bodyAsJson.Value.GetProperty("Transaction");
                return true;
            }
            else
            {
                Console.WriteLine($"Load Detail Failed For Tx {Id} - {Title}");
                return false;
            }
        }

        #endregion Methods
    }
}