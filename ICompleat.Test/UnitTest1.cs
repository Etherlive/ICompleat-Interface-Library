using System.Linq;
using Xunit;

namespace ICompleat.Test;

public class UnitTest1
{
    #region Methods

    [Fact]
    public async void CustomFieldsWork()
    {
        var t = await Objects.CustomFields.GetCustomFieldsAsync(Auth.PMY);
        Assert.NotNull(t);

        await t[0].LoadFull(Auth.PMY);
        Assert.NotNull(t[0]);

        await t[0].AppendValues(new System.Collections.Generic.List<Objects.CustomFields.Field> { new Objects.CustomFields.Field() { Code = "j2", Name = "Another Another Job" } }, Auth.PMY);
    }

    [Fact]
    public async void SuppliersWork()
    {
        var t = await Objects.Supplier.GetSuppliersAsync(Auth.PMY);
        Assert.NotNull(t);
    }

    [Fact]
    public async void TransactionsWork()
    {
        var t = await Objects.Transaction.GetTransactionsUntillAllAsync(Auth.PMY);
        //var k = t.Where(x => !"APPR PEND DEL".Split(' ').Contains(x.json.GetProperty("Status").GetString()));
        var sps = t.Select(x => x.Status).Distinct();
        var tps = t.Select(x => x.Type).Distinct();
        Assert.NotNull(t);
    }

    #endregion Methods
}