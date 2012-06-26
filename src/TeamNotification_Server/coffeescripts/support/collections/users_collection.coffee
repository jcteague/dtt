Q = require('q')
_ = require('underscore')

Repository = require('../repository')

class UsersCollection
    constructor: (users) ->
        #console.log users
        @users = users
        #_.bindAll @
        #@collection = @repository.find().then(@set_collection)
        
    to_json: () ->
        users = users: ({"href": "/user/#{user.id}", "data": [{"name": "name", "value": user.name }, {"name": "id", "value": user.id}]} for user in @users)
module.exports = UsersCollection
