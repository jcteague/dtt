Repository = require('../repository')

strategy = (owner_id) ->
    new Repository('ChatRoom').find({owner_id: owner_id})

module.exports = strategy
