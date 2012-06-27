Q = require('q')
_ = require('underscore')
async = require('async')
pg_gateway = require('../../support/database/pg_gateway')

handle_actions = (steps..., done) ->
    async.series steps, () ->
        done()

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
            client.query "CREATE TABLE #{table_structure.name}(#{column_types})", (err, result) ->
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
    handle: handle_actions
