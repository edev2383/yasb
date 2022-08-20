using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Controllers;
using StockBox.Interpreter;
using StockBox.Interpreter.Scanner;
using StockBox.Services;
using StockBox.States;


namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Controllers_Tests
    {

        [TestMethod, Description("Create and assert the most simple DomainController possible is not null")]
        public void SB_Controllers_01_DomainControllerCanBeCreated()
        {

            var activeService = new ActiveService(new Scanner(), new Parser());
            var stateMachine = new StateMachine(new StateList(), new ActiveState());
            var controller = new DomainController(activeService, stateMachine);

            Assert.IsNotNull(controller);
        }

        [TestMethod, Description("")]
        public void SB_Controllers_02_()
        {

        }
    }
}
