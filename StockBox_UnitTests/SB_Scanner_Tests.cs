﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Interpreter.Scanner;
using StockBox.Base.Tokens;
using StockBox_TestArtifacts.Helpers;
using static StockBox_TestArtifacts.Helpers.EFile;
using Antlr.Runtime;


namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Scanner_Tests
    {
        [TestMethod]
        public void SBC_01_CreatedTokenIsNotNullAndToStringReturnsExpectedValue()
        {
            var token = new Token(TokenType.eComma, ",", null, 0, 0);
            Assert.IsNotNull(token);
            Assert.AreEqual(token.ToString(), "eComma ,  / 0:0");
        }

        [TestMethod]
        public void SBC_02_CreatedScannerIsNotNull()
        {
            var scanner = new Scanner("");
            Assert.IsNotNull(scanner);
        }

        [TestMethod]
        public void SBC_03_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner("(");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eLeftParen);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_04_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner(")");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eRightParen);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_05_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner("{");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eLeftBrace);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_06_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner("}");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eRightBrace);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_07_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner(",");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eComma);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_08_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner(".");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eDot);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_09_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner("-");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eMinus);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_10_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner("+");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.ePlus);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_11_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner(";");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eSemicolon);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_12_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner("*");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eStar);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_13_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner("!");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eBang);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_14_ScannerReturnsExpectedResults_DoubleCharTokens()
        {
            var scanner = new Scanner("!=");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eBangEqual);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_15_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner("=");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eEqual);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_16_ScannerReturnsExpectedResults_DoubleCharTokens()
        {
            var scanner = new Scanner("==");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eEqualEqual);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_17_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner("<");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eLessThan);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_18_ScannerReturnsExpectedResults_DoubleCharTokens()
        {
            var scanner = new Scanner("<=");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eLessThenOrEqual);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_19_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner(">");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eGreaterThan);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_20_ScannerReturnsExpectedResults_DoubleCharTokens()
        {
            var scanner = new Scanner(">=");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eGreaterThanOrEqual);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_21_ScannerReturnsExpectedResults_SingleCharTokens()
        {
            var scanner = new Scanner("/");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eSlash);
            Assert.AreEqual(tokens[1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_22_TestReaderIsNotNullAndHasSourceTextValue()
        {
            var reader = new Reader().GetFileContents(EFile.eBasicString);
            Assert.IsNotNull(reader);
        }

        [TestMethod]
        public void SBC_23_ScannerReturnsExpectedResults_String()
        {
            var tokens = GetTokens(EFile.eBasicString);
            Assert.IsTrue(tokens.Count == 2);
            Assert.AreEqual(TokenType.eString, tokens[0].Type);
            Assert.AreEqual("T", tokens[0].Literal);
            Assert.AreEqual("\"T\"", tokens[0].Lexeme);
        }

        [TestMethod]
        public void SBC_24_ScannerReturnsExpectedResults_Int()
        {
            var tokens = GetTokens(eBasicInt);
            Assert.IsTrue(tokens.Count == 2);
            Assert.AreEqual(TokenType.eNumber, tokens[0].Type);
            Assert.AreEqual("1234", tokens[0].Lexeme);
            Assert.AreEqual(1234d, tokens[0].Literal);
        }

        [TestMethod]
        public void SBC_25_ScannerReturnsExpectedResults_Double()
        {
            var tokens = GetTokens(eBasicDouble);
            Assert.IsTrue(tokens.Count == 2);
            Assert.AreEqual(TokenType.eNumber, tokens[0].Type);
            Assert.AreEqual("12.34", tokens[0].Lexeme);
            Assert.AreEqual(12.34, tokens[0].Literal);
        }

        [TestMethod]
        public void SBC_26_ScannerCanRecognizeIdentifiers()
        {
            var scanner = new Scanner("and or this orchid banana");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 6);
            Assert.AreEqual(tokens[0].Lexeme, "and");
            Assert.AreEqual(tokens[0].Type, TokenType.eAnd);
            Assert.AreEqual(tokens[1].Lexeme, "or");
            Assert.AreEqual(tokens[1].Type, TokenType.eIdentifier);
            Assert.AreEqual(tokens[2].Lexeme, "this");
            Assert.AreEqual(tokens[2].Type, TokenType.eIdentifier);
            Assert.AreEqual(tokens[3].Lexeme, "orchid");
            Assert.AreEqual(tokens[3].Type, TokenType.eIdentifier);
            Assert.AreEqual(tokens[4].Lexeme, "banana");
            Assert.AreEqual(tokens[4].Type, TokenType.eIdentifier);
            Assert.AreEqual(tokens[5].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_27_ScannerCanRecognizeADomainExpression()
        {
            var scanner = new Scanner("2 days ago SMA(25)");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 5);
            Assert.AreEqual(tokens[0].Lexeme, "2");
            Assert.AreEqual(tokens[0].Type, TokenType.eNumber);
            Assert.AreEqual(tokens[1].Lexeme, "days");
            Assert.AreEqual(tokens[1].Type, TokenType.eDaily);
            Assert.AreEqual(tokens[2].Lexeme, "SMA");
            Assert.AreEqual(tokens[2].Type, TokenType.eIndicator);
            Assert.AreEqual(tokens[3].Literal, "25");
            Assert.AreEqual(tokens[3].Type, TokenType.eIndicatorIndices);
            Assert.AreEqual(tokens[4].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_28_ScannerCanRecognizeADomainExpression_ComplexIndicatorIndices()
        {
            var scanner = new Scanner("2 days ago SMA(14,3)");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 5);
            Assert.AreEqual(tokens[0].Lexeme, "2");
            Assert.AreEqual(tokens[0].Type, TokenType.eNumber);
            Assert.AreEqual(tokens[1].Lexeme, "days");
            Assert.AreEqual(tokens[1].Type, TokenType.eDaily);
            Assert.AreEqual(tokens[2].Lexeme, "SMA");
            Assert.AreEqual(tokens[2].Type, TokenType.eIndicator);
            Assert.AreEqual(tokens[3].Literal, "14,3");
            Assert.AreEqual(tokens[3].Type, TokenType.eIndicatorIndices);
            Assert.AreEqual(tokens[4].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_29_ScannerWillInjectUnsourcedTokensToOrphanedDomainItem_Columns()
        {
            var scanner = new Scanner("Close");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 4);
            Assert.AreEqual(tokens[0].Lexeme, "0");
            Assert.AreEqual(tokens[0].Type, TokenType.eNumber);
            Assert.AreEqual(tokens[1].Lexeme, "day");
            Assert.AreEqual(tokens[1].Type, TokenType.eDaily);
            Assert.AreEqual(tokens[2].Lexeme, "Close");
            Assert.AreEqual(tokens[2].Type, TokenType.eColumn);
            Assert.AreEqual(tokens[3].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_30_ScannerWillInjectUnsourcedTokensToOrphanedDomainItem_Indicator()
        {
            var scanner = new Scanner("SMA(25)");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 5);
            Assert.AreEqual(tokens[0].Lexeme, "0");
            Assert.AreEqual(tokens[0].Type, TokenType.eNumber);
            Assert.AreEqual(tokens[1].Lexeme, "day");
            Assert.AreEqual(tokens[1].Type, TokenType.eDaily);
            Assert.AreEqual(tokens[2].Lexeme, "SMA");
            Assert.AreEqual(tokens[2].Type, TokenType.eIndicator);
            Assert.AreEqual(tokens[3].Lexeme, "SMA(25)");
            Assert.AreEqual(tokens[3].Type, TokenType.eIndicatorIndices);
            Assert.AreEqual(tokens[4].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_31_ScannerWillInjectUnsourcedIndexFromZeroToDomainItemWithFrequencyIndex_Columns()
        {
            var scanner = new Scanner("Weekly Close");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 4);
            Assert.AreEqual(tokens[0].Lexeme, "0");
            Assert.AreEqual(tokens[0].Type, TokenType.eNumber);
            Assert.AreEqual(tokens[1].Lexeme, "Weekly");
            Assert.AreEqual(tokens[1].Type, TokenType.eWeekly);
            Assert.AreEqual(tokens[2].Lexeme, "Close");
            Assert.AreEqual(tokens[2].Type, TokenType.eColumn);
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_32_ScannerWillInjectUnsourcedIndexFromZeroToDomainItemWithFrequencyIndex_Indicators()
        {
            var scanner = new Scanner("Monthly SMA(25)");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 5);
            Assert.AreEqual(tokens[0].Lexeme, "0");
            Assert.AreEqual(tokens[0].Type, TokenType.eNumber);
            Assert.AreEqual(tokens[1].Lexeme, "Monthly");
            Assert.AreEqual(tokens[1].Type, TokenType.eMonthly);
            Assert.AreEqual(tokens[2].Lexeme, "SMA");
            Assert.AreEqual(tokens[2].Type, TokenType.eIndicator);
            Assert.AreEqual(tokens[3].Lexeme, "SMA(25)");
            Assert.AreEqual(tokens[3].Type, TokenType.eIndicatorIndices);
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_33_ScannerCanRecognizeBooleanTokens()
        {
            var scanner = new Scanner("True != FALSE");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 4);
            Assert.AreEqual(tokens[0].Lexeme, "True");
            Assert.AreEqual(tokens[0].Literal, true);
            Assert.AreEqual(tokens[1].Lexeme, "!=");
            Assert.AreEqual(tokens[2].Lexeme, "FALSE");
            Assert.AreEqual(tokens[2].Literal, false);
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_34_ScannerCanRecogizeKeywordLast()
        {
            var scanner = new Scanner("Last week close");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 4);
            Assert.AreEqual(tokens[0].Lexeme, "Last");
            Assert.AreEqual(tokens[0].Literal, 1);
            Assert.AreEqual(tokens[1].Type, TokenType.eWeekly);
            Assert.AreEqual(tokens[2].Type, TokenType.eColumn);
            Assert.AreEqual(tokens[2].Lexeme, "close");
            Assert.AreEqual(tokens[3].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_34_ScannerCanRecognizeCharPercent()
        {
            var scanner = new Scanner("20%");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 4);
            Assert.AreEqual(tokens[0].Lexeme, "20");
            Assert.AreEqual(tokens[0].Literal, 20d);
            Assert.AreEqual(tokens[0].Type, TokenType.eNumber);
            Assert.AreEqual(tokens[1].Type, TokenType.eSlash);
            Assert.AreEqual(tokens[2].Type, TokenType.eNumber);
            Assert.AreEqual(tokens[2].Literal, 100);
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_35_ScannerCanRecognizeYesterdayDomainKeyword()
        {
            var scanner = new Scanner("Yesterday Close");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 4);
            Assert.AreEqual(tokens[0].Lexeme, "1");
            Assert.AreEqual(tokens[0].Type, TokenType.eNumber);
            Assert.AreEqual(tokens[1].Lexeme, "day");
            Assert.AreEqual(tokens[1].Type, TokenType.eDaily);
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_36_ScannerCanRecognizeYesterdayDomainKeyword()
        {
            var scanner = new Scanner("High < Yesterday's High");
            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 8);
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_37_ScannerRecognizesAndIgnoresPossessiveContractions()
        {
            var scanner = new Scanner("yesterday's week's month's day's");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(9, tokens.Count);
            Assert.AreEqual(tokens[1].Type, TokenType.eDaily);
            Assert.AreEqual(tokens[3].Type, TokenType.eWeekly);
            Assert.AreEqual(tokens[5].Type, TokenType.eMonthly);
            Assert.AreEqual(tokens[7].Type, TokenType.eDaily);
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_38_ScannerRecognizesCrossoverOperator()
        {
            var scanner = new Scanner("x");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eCrossOver);
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_38_ScannerRecognizesCrossoverOperatorAsPartOfAnExpression()
        {
            var scanner = new Scanner("High x Yesterday's Close");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(8, tokens.Count);
            Assert.AreEqual(tokens[3].Type, TokenType.eCrossOver);
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_39_ScannerRecognizesEntryPointIdentifier()
        {
            var scanner = new Scanner("@Entry");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eEntryPoint);
            Assert.AreEqual(tokens[0].Lexeme, "@Entry");
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_40_ScannerRecognizes52WeekHighPointIdentifier()
        {
            var scanner = new Scanner("@52WeekHigh");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.e52WeekHigh);
            Assert.AreEqual(tokens[0].Lexeme, "@52WeekHigh");
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_41_ScannerRecognizes52WeekLowPointIdentifier()
        {
            var scanner = new Scanner("@52WeekLow");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.e52WeekLow);
            Assert.AreEqual(tokens[0].Lexeme, "@52WeekLow");
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_42_ScannerRecognizesAllTimeHighPointIdentifier()
        {
            var scanner = new Scanner("@ATH");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eAllTimeHigh);
            Assert.AreEqual(tokens[0].Lexeme, "@ATH");
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_43_ScannerRecognizesAllTimeLowPointIdentifier()
        {
            var scanner = new Scanner("@ATL");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eAllTimeLow);
            Assert.AreEqual(tokens[0].Lexeme, "@ATL");
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_43_ScannerDoesNotRecognizeEntryAsDomainKeywordWithoutSymbol()
        {
            // TODO - Error handling to show compile errors on the non symbol'd
            // Keyword 
            var scanner = new Scanner("Entry");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eIdentifier);
            Assert.AreEqual(tokens[0].Lexeme, "Entry");
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_44_ScannerRecognizesIndicators_SlowSto()
        {
            var scanner = new Scanner("SlowSto(14,3)");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(5, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eNumber);
            Assert.AreEqual(tokens[0].Literal, 0);
            Assert.AreEqual(tokens[1].Type, TokenType.eDaily);
            Assert.AreEqual(tokens[2].Type, TokenType.eIndicator);
            Assert.AreEqual(tokens[2].Lexeme, "SlowSto");
            Assert.AreEqual(tokens[3].Type, TokenType.eIndicatorIndices);
            Assert.AreEqual(tokens[3].Lexeme, "SlowSto(14,3)");
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }


        [TestMethod]
        public void SBC_45_ScannerRecognizesIndicators_RSI()
        {
            var scanner = new Scanner("RSI(14)");
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(5, tokens.Count);
            Assert.AreEqual(tokens[0].Type, TokenType.eNumber);
            Assert.AreEqual(tokens[0].Literal, 0);
            Assert.AreEqual(tokens[1].Type, TokenType.eDaily);
            Assert.AreEqual(tokens[2].Type, TokenType.eIndicator);
            Assert.AreEqual(tokens[2].Lexeme, "RSI");
            Assert.AreEqual(tokens[3].Type, TokenType.eIndicatorIndices);
            Assert.AreEqual(tokens[3].Lexeme, "RSI(14)");
            Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        [TestMethod]
        public void SBC_46_ScannerCanRecognizeSlopeTokens()
        {
            // need to further explore this to understand my intentions
            //var scanner = new Scanner("Slope[SMA(25)]");
            //var tokens = scanner.ScanTokens();

            //// expect eSlopeOf eLeftSquare 
            //Assert.AreEqual(5, tokens.Count);
            //Assert.AreEqual(tokens[0].Type, TokenType.eNumber);
            //Assert.AreEqual(tokens[0].Literal, 0);
            //Assert.AreEqual(tokens[1].Type, TokenType.eDaily);
            //Assert.AreEqual(tokens[2].Type, TokenType.eIndicator);
            //Assert.AreEqual(tokens[2].Lexeme, "RSI");
            //Assert.AreEqual(tokens[3].Type, TokenType.eIndicatorIndices);
            //Assert.AreEqual(tokens[3].Lexeme, "RSI(14)");
            //Assert.AreEqual(tokens[^1].Type, TokenType.eEOF);
        }

        private TokenList GetTokens(EFile target)
        {
            var contents = new Reader().GetFileContents(target);
            var scanner = new Scanner(contents);
            return scanner.ScanTokens();
        }

    }
}
