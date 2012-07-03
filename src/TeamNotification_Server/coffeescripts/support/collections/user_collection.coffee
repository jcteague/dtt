class UserCollection

    constructor: (@user_id) ->

    to_json: ->
        self = "/user/#{@user_id}"
        return {
            href: self
            links: [
                {"rel":"User", "name": "self", "href": self}
                {"rel":"UserRooms", "name": "rooms", "href": "/user/#{@user_id}/rooms"}
                {"rel":"Room", "name": "Create Room", "href": "/room"}
            ]
        }

module.exports = UserCollection
