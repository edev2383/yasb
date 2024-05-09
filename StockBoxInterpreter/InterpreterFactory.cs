using System;
using StockBox.Base.Tokens;
using StockBox.Data.SbFrames;
using StockBox.Interpreter;


namespace StockBox.Interpreter
{

    /// <summary>
    /// Real talk. I don't remember why I made this class...
    /// </summary>
    public static class InterpreterFactory
    {

        public static SbInterpreter Create(TokenList tokens)
        {
            SbFrameList fl = new SbFrameList();
            return new SbInterpreter(fl);
        }
    }
}
