class UserCollection

    constructor: (user_id) ->
        @set_collection(user_id)

    set_collection: (user_id) ->
        @collection =
            links: [
                {"rel":"self", "name": "self", "href":"/user/#{user_id}"},
                {"rel":"rooms", "name": "rooms", "href": "/user/#{user_id}/rooms"}
            ]

    fetch_to: (callback) ->
        callback(@collection)

module.exports = UserCollection
