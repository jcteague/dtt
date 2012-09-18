Repository = require('../repository')
strategy = (email) ->
     new Repository('User').find().then (users) ->
        if email?
           (user for user in users when user.email.toLowerCase().indexOf(email.toLowerCase())==0)
        else
            users
        
module.exports = strategy
