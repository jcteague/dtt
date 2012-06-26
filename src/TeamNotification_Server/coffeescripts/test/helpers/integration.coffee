db_config = require('../../support/globals').db
gateway = require('../../support/orm_gateway')
pg_gateway = require('../../support/database/pg_gateway')
Q = require('q')

# Needed because we shouldn't be clearing the db before the entities are defined
creation_promise = null

_entity = {}
create_db_tables = (entities_schema) ->
    gateway.open(db_config.test).then (db) ->
        for entity_name, values of entities_schema
            continue if entity_name is 'relations'
            _entity[entity_name] = db.define values.table, values.columns

        if entities_schema.relations
            relate relation for relation in entities_schema.relations

        sync entity for entity, values of _entity

relate = (relation) ->
    if relation.type is 'hasOne'
        _entity[relation.entity_name].hasOne(relation.relation_name, _entity[relation.related_entity], {autoFetch: true})
    if relation.type is 'hasMany'
        _entity[relation.entity_name].hasMany(relation.relation_name, _entity[relation.related_entity], relation.relation_column, {autoFetch: true})

sync = (entity) ->
    _entity[entity].sync()

insert_entity = (entity_name, values_obj) ->
    instance = new _entity[entity_name](values_obj)
    instance.save()

set_up_db = (entities_schema, entities) ->
    creation_promise = create_db_tables(entities_schema).then((db) ->
        for entity_name, values_obj of entities
            insert_entity(entity_name, values) for values in values_obj
    )

clean_up_db = (tables...) ->
    drop_callback = (err, result) ->
        if err
            console.log err
            return

    Q.when creation_promise, () ->
        pg_gateway.open(db_config.test).then (client) ->
            client.query("drop table if exists #{table} cascade;", drop_callback) for table in tables    

module.exports =
    set_up_db: set_up_db
    clean_up_db: clean_up_db
