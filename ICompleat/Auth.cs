using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICompleat
{
    public class Auth : API_Whisperer.Authentication
    {
        #region Fields

        public string tenantId, companyId;

        #endregion Fields

        #region Constructors

        public Auth(string tenantId, string companyId, string key)
        {
            this.headers = new Dictionary<string, string>()
            {
                {"x-api-version", "1"},
                {"x-api-compleat-key", key}
            };

            this.tenantId = tenantId;
            this.companyId = companyId;
        }

        #endregion Constructors
    }
}