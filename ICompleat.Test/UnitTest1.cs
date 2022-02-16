using Xunit;
using ICompleat;

namespace ICompleat.Test;

public class UnitTest1
{
    #region Methods

    [Fact]
    public async void RequestsWork()
    {
        var t = await Objects.Supplier.GetSuppliersUntillAllAsync();
        Assert.NotNull(t);
    }

    #endregion Methods
}