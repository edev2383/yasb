# StockBox Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 20241001

### Changed
- Adapters 
  - Incorporate change to interface, `Position` arg to `PerformAction` method
- Helpers
  - enums changed names from `e` prefix
- `Alert`, `Buy`, `Sell`, etc.
  - update above enum calls
  - add `Position` to `PerformAction` methods
- Controllers
  - Add async await interface
- Setup
  - Add `Position` argument to `PerformActions` method

### Added
- Order.cs [renamed from `Transaction`]
- OrderList.cs [renamed from `TransactionList`]

### Removed
- Transaction.cs [renamed to `Order`]
- TransactionList.cs [renamed to `OrderList`]