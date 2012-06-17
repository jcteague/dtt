entity_factory = require('support').core.entity_factory
Q = require('q')

class Repository

    constructor: (@entity) ->


    find: (query_args...) ->
        deferred = Q.defer()
        callback = @get_on_resolve_callback(deferred)
        entity_factory.get(@entity).find.apply(@, query_args.concat(callback))
        deferred.promise

    get_on_resolve_callback: (deferred) ->
        return (entities) ->
            deferred.resolve(entities)

module.exports = Repository
