Repository = require('../repository')

strategy = (username) ->
    new Repository('User').find().then (users) ->
        (user for user in users when user.name.indexOf(username) != -1)

module.exports = strategy

