mapping =
    user_collection: require('./collections/user_collection')
    user_rooms_collection: require('./collections/user_rooms_collection')

class CollectionFactory

    constructor: (@type) ->

    for: (options) ->
        new mapping[@type](options)

module.exports = CollectionFactory
