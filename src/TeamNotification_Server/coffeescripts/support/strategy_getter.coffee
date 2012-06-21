path = require('path')
strategies_map = require('./strategies/strategies_map')

class StrategyGetter

    get_for: (type) ->
        @require_strategy(strategies_map[type])

    require_strategy: (strategy_path) ->
        require(path.resolve(__dirname, 'strategies', strategy_path))

module.exports = StrategyGetter
