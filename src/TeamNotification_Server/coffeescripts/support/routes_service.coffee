CollectionFactory = require('./collection_factory')
build = (collection_type) ->
    new CollectionFactory(collection_type)

module.exports =
    build: build
