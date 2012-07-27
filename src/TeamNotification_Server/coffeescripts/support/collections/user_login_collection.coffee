class UserLoginCollection

    constructor: (@data) ->

    to_json: ->
        return @data

module.exports = UserLoginCollection
