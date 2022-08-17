using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.Context;


namespace StockBox_IntegrationTests
{
    [TestClass]
    public class SB_CallContext_Tests
    {
        [TestMethod]
        public void SB_CallContext_01_CanCreateCallContext()
        {
            var cs = "Host=localhost;Username=postgres;Password=admin;Database=postgres";
            var ctx = new CallContext(cs);
        }
    }
}
