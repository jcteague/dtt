class RootCollection

    #constructor: (@user_id) ->
    constructor: () ->


    to_json: ->
        return {
            links: [
                {"name": "self", "rel": "self", "href": "/" },
                #{"name": "user", "rel": "User", "href": "/user/#{@user_id}" },
                {"name": "Login", "rel": "login", "href": "/user/login" },
                {"name": "Registration", "rel": "registration", "href": "/registration" }
            ]
        }

module.exports = RootCollection
