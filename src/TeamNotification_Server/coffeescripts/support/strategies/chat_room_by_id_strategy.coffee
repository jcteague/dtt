Repository = require('../repository')

strategy = (room_id) ->
    new Repository('ChatRoom').get_by_id(room_id)

module.exports = strategy
