Repository = require('../repository')
strategy = (email) ->
    #TODO: Use the build in ilike function
     new Repository('User').find().then (users) ->#{'email ilike':email+'%'}, 5).then (users) ->
        if email?
            (user for user in users when user.email.toLowerCase().indexOf(email.toLowerCase())==0)
        else
            users
        
module.exports = strategy
