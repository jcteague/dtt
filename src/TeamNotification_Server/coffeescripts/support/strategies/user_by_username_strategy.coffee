Repository = require '../repository'
strategy = (username) ->

    #if typeof(username) == 'undefined'
    #    username = ''
    #l.find( { "name_rus ilike": data.name + "%"}, 4, function ( results ) {})

    #new Repository('User').find({'name ilike':username+'%'}).then (users) ->
     new Repository('User').find().then (users) ->
        if typeof(username) != 'undefined'
           return (user for user in users when user.name.indexOf(username)==0)
        else
        #console.log users
            return users
        
module.exports = strategy
