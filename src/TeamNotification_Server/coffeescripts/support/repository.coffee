 entity_factory = require('./core').core.entity_factory
Q = require('q')

class Repository

    constructor: (@entity) ->


    get_by_id: (id) ->
        deferred = Q.defer()
        callback = @get_on_resolve_callback(deferred)
        # entity_factory.get(@entity).get(id, callback)
        deferred.promise

    find: (query_args...) ->
        deferred = Q.defer()
        callback = @get_on_resolve_callback(deferred)
        # entity_factory.get(@entity).find.apply(@, query_args.concat(callback))
        deferred.promise

    get_on_resolve_callback: (deferred) ->
        return (entities) ->
            deferred.resolve(entities)

module.exports = Repository
