Repository = require('../repository')
strategy = (q) ->
     new Repository('User').find({email:q.email, password:q.password}).then (user) ->
        if user == null || typeof user == 'undefined'
            return {}
        else
            return user[0]
module.exports = strategy
