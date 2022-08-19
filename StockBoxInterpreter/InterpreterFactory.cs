using System;
using StockBox.Associations.Tokens;
using StockBox.Data.SbFrames;
using StockBox.Interpreter;


namespace StockBox.Interpreter
{

    public static class InterpreterFactory
    {

        public static SbInterpreter Create(TokenList tokens)
        {
            SbFrameList fl = new SbFrameList();
            return new SbInterpreter(fl);
        }
    }
}
