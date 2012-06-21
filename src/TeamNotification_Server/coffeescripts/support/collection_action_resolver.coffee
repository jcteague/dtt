StrategyGetter = require('./strategy_getter')
CollectionFactory = require('./collection_factory')
PromiseFactory = require('./promise_factory')

class CollectionActionResolver

    constructor: (@type) ->
        @strategy = new StrategyGetter().get_for(@type)
        @collection_class = new CollectionFactory().get_for(@type)
        @promise_factory = new PromiseFactory()

    for: (parameters) ->
        @promise_factory.get_for(@collection_class, @strategy(parameters))

module.exports = CollectionActionResolver
