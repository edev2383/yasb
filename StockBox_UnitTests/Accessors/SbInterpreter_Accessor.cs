using System;
using StockBox.Interpreter;
using sbenv = StockBox.Interpreter.Environments;

namespace StockBox_UnitTests.Accessors
{
    public class SbInterpreter_Accessor : SbInterpreter
    {
        public SbInterpreter_Accessor()
        {
        }

        public sbenv.Environment Access_GetEnvironment()
        {
            return Environment;
        }
    }
}
