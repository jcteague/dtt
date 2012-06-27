Q = require('q')
_ = require('underscore')
async = require('async')

db_config = require('../../support/globals').db
pg_gateway = require('../../support/database/pg_gateway')

open_db = () ->
    defer = Q.defer()
    pg_gateway.open(db_config.test).then (client) ->
        defer.resolve client

    defer.promise

structure = 
    name: 'users'
    columns:
        id: 'integer'
        name: 'varchar(100)'
        email: 'varchar(100)'

users =
    name: 'users'
    entities: [
        {
            id: 1
            name: 'blah'
            email: 'foo@bar.com'
        },
        {
            id: 2
            name: 'ed'
            email: 'ed@es.com'
        }
    ]


open = (steps...) ->
    pg_gateway.open(db_config.test).then (client) ->
        #deferred_clear('users').then(deferred_create(structure)).then(deferred_save(users))

        result = Q.resolve client
        result.then step for step in steps
        result

handle_actions = (steps...) ->
    async.series steps, () ->
        console.log 'finished'

###
deferred_clear = (table) ->
    return (callback) ->
        pg_gateway.open(db_config.test).then (client) ->
            console.log "Dropping #{table}"
            client.query "drop table if exists #{table} cascade;", (err, result) ->
                if err
                    console.log err
                console.log 'dropped'
                callback(null, null)
###

deferred_clear = (table) ->
    return (callback) ->
        pg_gateway.open_c db_config.test, (client) ->
            console.log "Dropping #{table}"
            client.query "drop table if exists #{table} cascade;", (err, result) ->
                if err
                    console.log err
                console.log 'dropped'
                callback(null, null)

###
deferred_create = (table_structure) ->
    return (callback) ->
        pg_gateway.open(db_config.test).then (client) ->
            column_types = ("#{name} #{type}" for name, type of table_structure.columns).join(',')
            console.log "Creating #{table}"
            client.query "CREATE TABLE #{table_structure.name}(#{column_types})", (err, result) ->
                if err
                    console.log err

                console.log 'created'
                callback(null, null)
###

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

###
deferred_save = (table_obj) ->
    return (callback) ->
        pg_gateway.open(db_config.test).then (client) ->
            tasks = []
            for entity in table_obj.entities
                columns = _.keys(entity).join(',')
                values = _.values(entity).join(',')
                tasks.push(make_save_query(columns, values, client))

            async.series tasks, (err, results) ->
                console.log 'saved'
                callback(null, null)
###

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
    open: open_db
    open_all: open
    clear: deferred_clear
    create: deferred_create
    save: deferred_save
    handle: handle_actions
