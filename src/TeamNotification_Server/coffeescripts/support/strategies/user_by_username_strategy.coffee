Repository = require('../repository')
strategy = (username) ->
     new Repository('User').find().then (users) ->
        if typeof(username) != 'undefined'
           return (user for user in users when user.name.indexOf(username)==0)
        else
            return users
        
module.exports = strategy
