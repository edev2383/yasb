﻿using System;
namespace StockBox.Base.Tokens
{
    public enum TokenType
    {
        eDeficient = 0,
        // single char tokens
        eLeftParen,
        eRightParen,
        eLeftBrace,
        eRightBrace,
        eComma,
        eDot,
        eMinus,
        ePlus,
        eSemicolon,
        eSlash,
        eStar,
        ePercent,

        // one, or two char related tokens
        eBang,
        eBangEqual,
        eEqual,
        eEqualEqual,
        eGreaterThan,
        eGreaterThanOrEqual,
        eLessThan,
        eLessThenOrEqual,
        eCrossOver,

        // literals
        eIdentifier,
        eString,
        eNumber,
        eQuoteSingle,
        eQuoteDouble,

        // Domain Keywords
        eHigh,
        eLow,
        eOpen,
        eClose,
        eVolume,
        eIndicator,
        eIndicatorIndices,
        eColumn,
        eEntryPoint,
        eAllTimeHigh,
        eAllTimeLow,
        e52WeekHigh,
        e52WeekLow,

        // Timeframe frequency keywords
        // All Day, Daily, Days, Weeks, etc.. are reduced to these tokens
        eWeekly,
        eMonthly,
        eDaily,

        eYesterday,
        eAgo,

        // Numeric keywords
        eZero,
        eOne,
        eTwo,
        eThree,
        eFour,
        eFive,
        eSix,
        eSeven,
        eEight,
        eNine,
        eTen,

        // Indicator keywords
        eSma,
        eSloSto,
        eEma,

        // keywords
        eAnd,
        eClass,
        eEsle,
        eFalse,
        eFun,
        eFor,
        eIf,
        eNull,
        eOr,
        ePrint,
        eReturn,
        eSuper,
        eThis,
        eTrue,
        eVar,
        eWhile,

        // SQL keywords
        eProcedure,
        eSelect,
        eInsert,
        eUpdate,
        eDelete,
        eGo,
        eCreate,
        eAlter,
        eDbo,
        eGrant,
        eExec,
        eOn,
        eAs,
        eIs,
        eNot,
        eReplace,
        eBegin,
        eEnd,

        //
        eLast,

        // file
        eEOF,
    }
}
