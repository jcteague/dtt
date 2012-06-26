class UsersCollection

    constructor: (@users) ->

    to_json: ->
        links = ({"name": user.name, "rel": "User", "href": "/user/#{user.id}"} for user in @users)
        return {
            links:  links
        }

    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = UsersCollection

