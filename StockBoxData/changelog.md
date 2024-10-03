# StockboxData Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/).

## [1.0.2] - 2024-10-02

### Changed
- StreamFactory 
  - removed all functionality, marked for removal
- Supporting `Conqueror`
- Comments `PriceChannel`, `SimpleMovingAverage`, `SlowStochastic`
- Indicators return type updated to be custom per Indicator, rather than generic `object`
- Comments `DataPointList`
- FrameListFactory - convert to use async/await

### Removed
- DeedleToDataPointListAdapter [renamed to `DeedleToDataPointListYahooFinanceAdapter`]

### Added
- DeedleToDataPointListYahooFinanceAdapter [renamed from `DeedleToDataPointListAdapter`]
- Indicators/Conqueror.cs
- DataPointListFactory.cs
- AlphaVantage scraping integration

## [1.0.1] - 2024-05-09

### Changed
- BaseIndicator now uses generic type for `Payload` property and return type of `CalculateIndicator` method to allow children to define their payloads

### Added
- PriceChannel indicator

## [1.0.0] - 2024-05-08

### Added
- Added AverageTrueRange indicator