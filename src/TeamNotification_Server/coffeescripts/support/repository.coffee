entity_factory = require('./core').core.entity_factory
Q = require('q')

class Repository

    constructor: (@entity) ->


    get_by_id: (id) ->
        deferred = Q.defer()
        callback = @get_on_resolve_callback(deferred, id)
        entity_factory.get(@entity).get(id, callback)
        deferred.promise

    find: (query_args...) ->
        deferred = Q.defer()
        callback = @get_on_resolve_callback(deferred, query_args)
        entity_factory.get(@entity).find.apply(@, query_args.concat(callback))
        deferred.promise

    get_on_resolve_callback: (deferred, args...) ->
        return (entities) ->
            if entities?
                return deferred.resolve(entities)
            deferred.resolve 
                is_empty: true
                args: args

module.exports = Repository
