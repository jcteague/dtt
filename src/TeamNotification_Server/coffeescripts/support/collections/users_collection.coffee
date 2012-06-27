class UsersCollection

    constructor: (@users) ->

    to_json: ->
        links = ({"name": user.name, "rel": "User", "href": "/user/#{user.id}"} for user in @users)
        get_data_for = (user) ->
            return {
                "href": "/user/#{user.id}"
                "data": [
                    {"name": "id", "value": user.id}
                    {"name": "name", "value": user.name}
                ]
            }

        return {
            users: (get_data_for user for user in @users)
            links:  [{"name":"self", "rel": "Users", "href": "/users/query"}].concat(links)
        }

    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = UsersCollection

