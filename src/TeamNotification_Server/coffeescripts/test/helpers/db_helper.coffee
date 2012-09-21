Q = require('q')
_ = require('underscore')
async = require('async')
pg_gateway = require('../../support/database/pg_gateway')
redis_cli = require("../../support/redis/redis_gateway").open()

redis =
    save: (table_object) ->
        table_name = table_object.name
        for entity in table_object.entities
            redis_cli.zadd(table_name,new Date().getTime(), JSON.stringify(entity))
    clear: (tables) ->
        for table_name in tables
            redis_cli.del(table_name)
    end: ->
        redis_cli.end()

clear = (tables...) ->
    return (callback) ->
        tasks = (deferred_clear table for table in tables)
        async.series tasks, () ->
            callback(null, null)

deferred_clear = (table) ->
    return (callback) ->
        pg_gateway.open (client) ->
            client.query "drop table if exists #{table} cascade;", (err, result) ->
                if err
                    console.log err
                callback(null, null)

create = (table_structures...) ->
    return (callback) ->
        tasks = (deferred_create structure for structure in table_structures)
        async.series tasks, () ->
            callback(null, null)

deferred_create = (table_structure) ->
    return (callback) ->
        pg_gateway.open (client) ->
            column_types = ("#{name} #{type}" for name, type of table_structure.columns).join(',')
            has_id_column = (column for column, type of table_structure.columns when column is 'id').length > 0
            if has_id_column
                create_statement = "CREATE TABLE #{table_structure.name}(#{column_types}, PRIMARY KEY(id))"
            else
                create_statement = "CREATE TABLE #{table_structure.name}(#{column_types})"

            client.query create_statement, (err, result) ->
                if err
                    console.log err

                callback(null, null)

save = (table_objects...) ->
    return (callback) ->
        tasks = (deferred_save obj for obj in table_objects)
        async.series tasks, () ->
            callback(null, null)

deferred_save = (table_obj) ->
    return (callback) ->
        pg_gateway.open (client) ->
            tasks = []
            for entity in table_obj.entities
                columns = _.keys(entity).join(',')
                values = _.values(entity).join(',')
                tasks.push(make_save_query(table_obj.name, columns, values, client))

            async.series tasks, (err, results) ->
                callback(null, null)

make_save_query = (name, columns, values, client) ->
    return (callback) ->
        client.query "INSERT INTO #{name}(#{columns}) VALUES(#{values})", (err, result) ->
            if err
                console.log err
                return
            callback(null, null)

module.exports =
    clear: clear
    create: create
    save: save
    redis: redis
