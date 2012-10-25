entity_factory = require('./core').core.entity_factory
Q = require('q')

class Repository

    constructor: (@entity) ->


    get_by_id: (id) ->
        deferred = Q.defer()
        callback = @get_on_resolve_callback(deferred)
        entity_factory.get(@entity).get(id, callback)
        deferred.promise

    find: (query_args...) ->
        deferred = Q.defer()
        callback = @get_on_resolve_callback(deferred)
        entity_factory.get(@entity).find.apply(@, query_args.concat(callback))
        deferred.promise
        
    save: (entity_data) ->
        deferred = Q.defer()
        entity_factory.create(@entity, entity_data).save (err, saved_entity) ->
            deferred.resolve(saved_entity)
        deferred.promise

    update: (entity_data) ->
        deferred = Q.defer()
        @get_by_id(entity_data.id).then (entity) ->
            for key, value of entity_data
                entity[key] = value

            entity.save (err, updated_entity) ->
                deferred.resolve updated_entity

        deferred.promise

    get_on_resolve_callback: (deferred) ->
        return (entities) ->
            deferred.resolve(entities)

module.exports = Repository
