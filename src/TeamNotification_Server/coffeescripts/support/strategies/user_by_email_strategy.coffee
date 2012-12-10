Repository = require('../repository')
strategy = (email) ->
     new Repository('User').find(email:email).then (users) ->
        if email?
            users[0]
           #(user for user in users when user.email.toLowerCase().indexOf(email.toLowerCase())==0)
        else
            users
        
module.exports = strategy
