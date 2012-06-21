StrategyGetter = require('./strategy_getter')
CollectionFactory = require('./collection_factory')

class CollectionActionResolver

    constructor: (@type) ->
        @strategy = new StrategyGetter().get_for(@type)
        @collection_class = new CollectionFactory().get_for(@type)

    for: (parameters) ->
        @strategy(parameters)

module.exports = CollectionActionResolver
