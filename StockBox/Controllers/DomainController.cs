using System;
using StockBox.Services;
using StockBox.States;

namespace StockBox.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public class DomainController
    {

        private readonly ISbService _service;
        private readonly StateMachine _stateMachine;

        public DomainController(ISbService service, StateMachine stateMachine)
        {
            _service = service;
            _stateMachine = stateMachine;
        }
    }
}
