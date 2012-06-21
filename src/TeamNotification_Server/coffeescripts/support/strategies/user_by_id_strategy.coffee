Repository = require('../repository')

strategy = (user_id) ->
    new Repository('User').get_by_id(user_id)

module.exports = strategy
