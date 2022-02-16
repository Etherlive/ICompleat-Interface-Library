using Xunit;
using ICompleat;

namespace ICompleat.Test;

public class UnitTest1
{
    #region Methods

    [Fact]
    public async void RequestsWork()
    {
        var t = await Request.Execucte($"api/users/{Config._instance.tenantId}/{Config._instance.companyId}");
        Assert.NotNull(t);
    }

    #endregion Methods
}