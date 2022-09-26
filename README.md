# yasb

## Introduction
The _yasb_ project is essentially a stock trading bot. While there are many such items available, I strongly believe that anything available for purchase is __worthless__ out of the box, because if were any good, the author(s) would have little interest in selling, as a product, something that was consistently beating the market.

The original intent was to create a "simple" way to test different market patterns against previous years of market data, searching for patterns that were consistent winners. During development of v1, we realized that we could also test the patterns in real-time, potentially to allow the program to make trades for us. That is the ultimate goal -- automated, consistent market profits.

## Theory
We wanted a system that would allow for simple ways to test different stock market patterns on the fly, without having to hard-code different variables for different patterns. Versions 1 and 2 handled this with RegEx, but that proved to be buggy and sluggish. Version 3 has been rewritten as an run-time interpretter in C#, allowing for greater flexibility and better test coverage.

## Stock Market Patterns
Here is an example pattern that we are interested in trading. After the background of the chart pattern, we'll explain how this scenario will be handled within _yasb_.

In the below chart, we can see the stock prices reverse around July 1st in a pattern that repeats all over the markets. By watching two indicators, the [Slow Stochastic indicator](https://www.fidelity.com/learning-center/trading-investing/technical-analysis/technical-indicator-guide/slow-stochastic) and the [Relative Strength Index](https://www.investopedia.com/terms/r/rsi.asp).

The way to trade this would be to enter the position when the Slow Stochastic indicator (bottom black/red lines) crosses above the 20-value line, provided the Relative Strength Index is at or above 40 (top black line). 

![sc](https://user-images.githubusercontent.com/47084492/192391615-276eb632-87e0-4dce-8383-17d66fd28397.png)

Below is detail of the Slow Stochastic (bottom indicator) and the RSI (indicator on the very top), with the entry point highlighted in green.
![stockexample](https://user-images.githubusercontent.com/47084492/192392438-33ddc5ee-b7c0-4008-b082-5ce3ab91d6cd.png)

There are multiple places to leave this trade with a profit, as the full price move was about $25 from $75-100+. Typically, most people trading this pattern, or something similar that is Stochastic-based, will exit when the black line of the indicator crosses back below the top line (80%).

## Rules
The core of _yasb_ is the `Rule` class. We create Rules to describe these patterns. The entry signal for this particular pattern would be something like this

```c#

var rules = new RuleList() {
   // the `x` operator is the "CrossOver" operator, and denotes that
   // the lefthand side of the expression was previously less than
   // the right hand, but is now greater than
   new Rule("SlowSto(14,3) x 20"),
   new Rule("RSI(14) >= 38"),
}

```

That's it. Using plain English, we've described a simple stock market pattern that can then be evaluated against real-time data, and we use that evaluation to create actions, such as buying and selling. 

We chose to use simple strings as rules because it gives us the flexibilty to change the rules easily and test many patterns. Additionally, we wanted a simple interface that could apply to potential users. While the intent is to use this as a private application, there are some potential commercial uses. Many stock traders would pay for the opportunity to test their pattern ideas in a simple, comprehensive way. A potential `User` can easily figure out how to write quite complicated patterns with simple string descriptions and those rules can be easily stored in the data layer.  

## State
The majority of the heavy lifting within _yasb_ is managing the State of given stock Symbols, e.g., "MSFT" for Microsoft, "TSLA" for Tesla. For example, say we are watching the "AMD" stock from the chart above, and we have a setup watching for this exact pattern. You can see that there are multiple places on the chart where the SlowSto(14,3) indicator crosses over the 20, but only one where the action is actually profitable, so we don't want to just ALWAYS perform that same action. We need to manage the state of the stock. 

We have (2) user-defined states, "Watch", "Primed", and (3) application-defined states "Buy", "Buy Error", "Buy Active". We have a SymbolProfile for the "AMD" stock that is in the "Watch" state.
```c#

var symbolProfile = new SymbolProfile("AMD", new UserDefinedState("Watch"));

var rules = new RuleList() {
   new Rule("SlowSto(14,3) < 20"),
   new Rule("Close > 60"),
   new Rule("RSI(14) > 25"),
};

// SymbolProfiles and Setups are related by their state, in this case "Watch"
var setup = new Setup(rules, new UserDefinedState("Watch"));
setup.AddAction(new Move("Primed"));

```

When this Setup is run through the `DomainController` if all three of the Rules evaluate to true, Action provided to the setup is called. In this case, it's a simple `Move` action, which is essentially just a state change for the SymbolProfile, that is also updated in the Data Layer. The "AMD" stock is now in the "Primed" state.

Now that the stock is in the "Primed" state, we can run it against the first RuleList we created...

```c#

var rules = new RuleList() {
   // the `x` operator is the "CrossOver" operator, and denotes that
   // the lefthand side of the expression was previously less than
   // the right hand, but is now greater than
   new Rule("SlowSto(14,3) x 20"),
   new Rule("RSI(14) >= 38"),
}

var setup = new Setup(rules, new UserDefinedState("Primed"));
setup.AddAction(new Buy());

```
... and if this pattern has been executed, we'll see a `Buy` action. 

Assuming the `Buy` action was successful, our "AMD" stock is in the "BuyActive" state. We need to tell the application when to sell. 

```c#
var rules = new RuleList() {
   new Rule("80 x SlowSto(14,3)"),
};

var setup = new Setup(rules, new UserDefinedState("BuyActive"));
setup.AddAction(new Sell());
```
In this `Sell` example, we swap the left and right expressions to watch for the Slow Stochastic indicator to cross below the 80% threshold. 
