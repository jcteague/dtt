class RootCollection

    constructor: ->


    to_json: ->
        return {
            links: [
                {"name": "self", "rel": "self", "href": "/" },
                {"name": "user", "rel": "User", "href": "/user" }
            ]
        }

module.exports = RootCollection
