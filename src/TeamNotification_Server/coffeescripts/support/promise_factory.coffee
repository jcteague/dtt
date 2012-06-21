Promise = require('./promise')

class PromiseFactory

    get_for: (class_constructor, promise) ->
        new Promise(class_constructor, promise)

module.exports = PromiseFactory
