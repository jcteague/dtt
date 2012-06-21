path = require('path')
collections_map = require('./collections/collections_map')

class CollectionFactory

    get_for: (type) ->
        @require_collection(collections_map[type])

    require_collection: (collection_path) ->
        require(path.resolve(__dirname, 'collections', collection_path))

module.exports = CollectionFactory
