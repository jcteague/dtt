Q = require('q')

class Promise

    constructor: (class_constructor, promise) ->
        @promised_class_instance = Q.when promise, (values) ->
            new class_constructor(values)

    fetch_to: (callback) ->
        @promised_class_instance.then callback

module.exports = Promise
