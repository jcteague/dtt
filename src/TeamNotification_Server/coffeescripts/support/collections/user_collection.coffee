class UserCollection

    constructor: (@user_id) ->

    to_json: ->
        return {
            links: [
                {"rel":"self", "name": "self", "href":"/user/#{@user_id}"},
                {"rel":"rooms", "name": "rooms", "href": "/user/#{@user_id}/rooms"}
            ]
        }

module.exports = UserCollection
