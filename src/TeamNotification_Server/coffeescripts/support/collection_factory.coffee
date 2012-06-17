mapping =
    'user_collection': require('./collections/user_collection')

class CollectionFactory

    constructor: (@type) ->

    for: (options) ->
        new mapping[@type](options)

module.exports = CollectionFactory
