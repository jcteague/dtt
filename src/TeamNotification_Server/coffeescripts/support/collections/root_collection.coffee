class RootCollection

    constructor: (@user_id) ->


    to_json: ->
        return {
            links: [
                {"name": "self", "rel": "self", "href": "/" },
                {"name": "user", "rel": "User", "href": "/user/#{@user_id}" }
            ]
        }

module.exports = RootCollection
