Repository = require('../repository')
strategy = (username) ->
     new Repository('User').find().then (users) ->
        if typeof(username) != 'undefined'
           return (user for user in users when user.first_name.toLowerCase().indexOf(username.toLowerCase())==0)
        else
            return users
        
module.exports = strategy
