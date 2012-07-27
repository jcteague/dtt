Repository = require('../repository')
strategy = (q) ->
     new Repository('User').find({email:q.email, password:q.pass}).then (user) ->
        return user
        #if typeof(username) != 'undefined'
        #   return (user for user in users when user.first_name.toLowerCase().indexOf(username.toLowerCase())==0)
        #else
        #    return users
        
module.exports = strategy
