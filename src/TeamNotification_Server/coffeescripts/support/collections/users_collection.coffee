_ = require('underscore')
collection_json_mapper = require('./helpers/collection_json_mapper')

class UsersCollection

    constructor: (@users) ->

    to_json: ->
        ###
        links = ({"name": user.email, "rel": "User", "href": "/user/#{user.id}"} for user in @users)
        get_data_for = (user) ->
            return {
                "href": "/user/#{user.id}"
                "data": [
                    {"name": "id", "value": user.id}
                    {"name": "email", "value": user.email}
                ]
            }

        return {
            users: (get_data_for user for user in @users)
            links:  [{"name":"self", "rel": "Users", "href": "/users/query"}].concat(links)
        }
        ###

        extend = (user) ->
            user_clone = _.clone user
            user_clone.href = "/user/#{user_clone.id}"
            user_clone

        users =
            links: [{"name":"self", "rel": "Users", "href": "/users/query"}].concat({"name": user.email, "rel": "User", "href": "/user/#{user.id}"} for user in @users)
            users: (extend user for user in @users)

        collection = collection_json_mapper.map users

        collection.collection

    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = UsersCollection

