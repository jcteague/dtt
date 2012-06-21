Q = require('q')

class Promise

    constructor: (class_constructor, promise) ->
        
        @promised_class_instance = Q.when promise, (values) ->
            new class_constructor(values)

    fetch_to: (callback) ->
        console.log @promised_class_instance
        @promised_class_instance.then (instance) -> 
            console.log 'Instance:'
            console.log instance 
            console.log instance.to_json()
            callback(instance.to_json())

module.exports = Promise
