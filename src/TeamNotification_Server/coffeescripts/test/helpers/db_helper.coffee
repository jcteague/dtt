Q = require('q')
_ = require('underscore')
async = require('async')

db_config = require('../../support/globals').db
pg_gateway = require('../../support/database/pg_gateway')

handle_actions = (steps...) ->
    async.series steps, () ->
        console.log 'finished'

clear = (tables...) ->
    return (callback) ->
        tasks = (deferred_clear table for table in tables)
        async.series tasks, () ->
            console.log 'Cleared', tables
            callback(null, null)

deferred_clear = (table) ->
    return (callback) ->
        pg_gateway.open_c db_config.test, (client) ->
            console.log "Dropping #{table}"
            client.query "drop table if exists #{table} cascade;", (err, result) ->
                if err
                    console.log err
                console.log 'dropped'
                callback(null, null)

create = (table_structures...) ->
    return (callback) ->
        tasks = (deferred_create structure for structure in table_structures)
        async.series tasks, () ->
            console.log 'Created', table_structures
            callback(null, null)

deferred_create = (table_structure) ->
    return (callback) ->
        pg_gateway.open_c db_config.test, (client) ->
            column_types = ("#{name} #{type}" for name, type of table_structure.columns).join(',')
            console.log "Creating #{table_structure.name}"
            client.query "CREATE TABLE #{table_structure.name}(#{column_types})", (err, result) ->
                if err
                    console.log err

                console.log 'created'
                callback(null, null)

save = (table_objects...) ->
    return (callback) ->
        tasks = (deferred_save obj for obj in table_objects)
        async.series tasks, () ->
            console.log 'Saved', table_objects
            callback(null, null)

deferred_save = (table_obj) ->
    return (callback) ->
        pg_gateway.open_c db_config.test, (client) ->
            tasks = []
            for entity in table_obj.entities
                columns = _.keys(entity).join(',')
                values = _.values(entity).join(',')
                tasks.push(make_save_query(table_obj.name, columns, values, client))

            async.series tasks, (err, results) ->
                console.log 'saved'
                callback(null, null)

make_save_query = (name, columns, values, client) ->
    return (callback) ->
        console.log "INSERT INTO #{name}(#{columns}) VALUES(#{values})"
        client.query "INSERT INTO #{name}(#{columns}) VALUES(#{values})", (err, result) ->
            if err
                console.log err
                return
            callback(null, null)

module.exports =
    #clear: deferred_clear
    #create: deferred_create
    #save: deferred_save
    clear: clear
    create: create
    save: save
    handle: handle_actions
