using System;
using StockBox.Data.Context;

namespace StockBox.Data.Managers
{
    public class SbManagerBase
    {
        protected CallContext Ctx { get { return _ctx; } }
        private CallContext _ctx;

        public SbManagerBase(CallContext ctx)
        {
            _ctx = ctx;
        }

        public bool Exec()
        {
            return false;
        }
    }
}
