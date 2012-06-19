mapping =
    user_collection: './collections/user_collection'
    user_rooms_collection: './collections/user_rooms_collection'

class CollectionFactory

    constructor: (@type) ->

    for: (options) ->
        collection = require(mapping[@type])
        new collection(options)

module.exports = CollectionFactory
